namespace Core.Response
{
    public class UseCaseResponse<T> where T: class
    {
        public UseCaseResponseKind Status { get; set; }
        public T Result { get; set; }
        public IEnumerable<String> Errors { get; set; }
        private int StatusCode => (int)Status;
        public bool IsSuccessful => StatusCode is > 0 and < 300;

        public UseCaseResponse<T> Ok(T result) => new UseCaseResponse<T>()
        {
            Status = UseCaseResponseKind.Ok,
            Result = result,
        };

        public UseCaseResponse<T> Created(T result) => new UseCaseResponse<T>()
        {
            Status = UseCaseResponseKind.Created,
            Result = result,
        };

        public UseCaseResponse<T> Accepted(T result) => new UseCaseResponse<T>()
        {
            Status = UseCaseResponseKind.Accepted,
            Result = result,
        };

        public UseCaseResponse<T> BadRequest(string message) => new UseCaseResponse<T>()
        {
            Status = UseCaseResponseKind.BadRequest,
            Result = null!,
            Errors = new[] { message }
        };

        public UseCaseResponse<T> BadRequest(List<string> message) => new UseCaseResponse<T>()
        {
            Status = UseCaseResponseKind.BadRequest,
            Result = null!,
            Errors = message
        };

        public UseCaseResponse<T> Unauthorized(T result, string message) => new UseCaseResponse<T>()
        {
            Status = UseCaseResponseKind.Unauthorized,
            Result = result,
            Errors = new[] { message }
        };

        public UseCaseResponse<T> Forbidden(T result) => new UseCaseResponse<T>()
        {
            Status = UseCaseResponseKind.Forbidden,
            Result = result,
        };

        public UseCaseResponse<T> NotFound(string message = "") => new UseCaseResponse<T>()
        {
            Status = UseCaseResponseKind.NotFound,
            Result = null,
            Errors = new[] { String.IsNullOrWhiteSpace(message) ? $"Record not found" : message }
        };
    }
}
