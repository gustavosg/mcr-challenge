using System.IdentityModel.Tokens.Jwt;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Application.DTO.Configuration;
using Application.Interfaces;

namespace Api.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate next;
        private readonly JwtOptionsSettings jwtOptions;

        public JwtMiddleware(RequestDelegate next, IOptions<JwtOptionsSettings> jwtOptions)
        {
            this.next = next;
            this.jwtOptions = jwtOptions.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await attachUserToContext(context, userService, token);

            await next(context);
        }

        private async Task attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(this.jwtOptions.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                Guid userId = Guid.Parse(jwtToken.Claims.First(x => x.Type.Equals("nameid")).Value);

                context.Items["User"] = (await userService.GetById(userId)).Result;
            }
            catch
            {
                // Não executar nada de validação JWT falhar
                // usuário não está anexado no contexto então a requisição não irá acessar rotas seguras
            }
        }
    }
}

