
# Motorcycle Rental Challenge

Projeto desenvolvimento em .NET Core 8, utilizando arquitetura Clean Architecture. Este serviço utiliza, como dependências, serviços docker em Container, aos quais contamos com: 

- Postgres;
- Kafka;
- Zookeeper;

## Requisitos

Antes de começar, certifique-se de ter as seguintes ferramentas instaladas:

- [Docker](https://docs.docker.com/get-docker/)
- [Docker Compose](https://docs.docker.com/compose/install/)
- [.NET SDK](https://dotnet.microsoft.com/download)

## Estrutura do Projeto

Do ponto de vista da Solução, o projeto está dividido na seguinte estrutura: 

 - `Core/` - Nucleo da aplicação, contendo basicamente as entidades, padrão de resposta, enums e extensões
- `Infrastructure/` - Camada de acesso a dados da aplicação, contem a implementação dos repositórios e da unidade de trabalho (Unit of Work)
- `Application/` - Camada de serviço da aplicação, onde se encontra a lógica das regras de negócios
 - `Tests/` - Projeto contendo os testes da aplicação
- `Api/` - Projeto endpoint que irá receber as solicitações externas

Arquivos que requerem atenção: 
- `docker-compose.yml` - Arquivo de configuração para o Docker Compose
- `Api/Dockerfile` - Arquivo de configuração para construir a imagem Docker da aplicação
- `Api/appsettings.json` - Arquivo de configuração da aplicação

## Configuração do Ambiente

### 1. Clonando o Repositório

```bash
git clone https://github.com/gustavosg/mrc-challenge.git
cd mrc-challenge
```

### 2. Configurando Variáveis de Ambiente

A aplicação tem uma configuração padrão, conforme a seguinte configuração: 

```bash
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AppSettings": {
    "Database": {
      "ConnectionString": "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=Password2024!"
    },
    "JwtOptions": {
      "Secret": "zwduHCtbF1r4sJcDsXYJiPnlRH5A2POh"

    },
    "Kafka": {
      "Server": "localhost:9092",
      "Topic": "APP_TOPIC",
      "DefaultLimit": 1000
    }
  },
  "AllowedHosts": "*"
}

```

A configuração existente pode ser modificada dentro do arquivo `docker-compose.yml`, ou, se preferir, crie um arquivo `.env` na raiz do projeto, e passe a configuração conforme desejar: 

```bash
POSTGRES_USER=seu-user
POSTGRES_PASSWORD=sua-senha
POSTGRES_DB=seu-banco
```

### 3. Executando o Projeto com Docker

Execute, no terminal:

```bash
docker-compose up -d
```

Este comando irá iniciar os contêineres do Kafka, PostgreSQL e do Zookeeper. Caso precise desligar os containers, execute: 

```bash
docker-compose down
```

### 4. Executando a Aplicação

A aplicação estará acessível em [https://localhost:5001/swagger](https://localhost:5001/swagger). Para executar a aplicação, executamos no terminal: 

```bash
# entre no diretório Api
cd Api
# restaure todos os pacotes necessários: 
dotnet restore .\Api.csproj
# inicie a aplicação
dotnet run
```

Aguarde a aplicação terminar de carregar, e no browser, entre em [https://localhost:5001/swagger](https://localhost:5001/swagger). Os endpoints estarão disponíveis para serem executados.

#### 4.1 Endpoins de autenticação:

Os endpoints de autenticação, necessários para executar qualquer comando autenticação, são os endpoints: 

- [/api/v1/user/register-admin](https://localhost:5001/api/v1/user/register-admin): Responsável por registrar um Administrador
- [/api/v1/user/register-delivery-person](https://localhost:5001/api/v1/user/register-delivery-person): Responsável por registrar um Entregador

Em ambos os endpoints, uma vez concluído o registro, será retornado um token. Guarde este token, e no módulo de autenticação do Swagger, autentique informando o seguinte conteúdo:

```bash
Bearer <SEU TOKEN AQUI>
```

Por exemplo: 
```bash
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJjYzUzNTc5Ni1iZmQwLTQ1NzgtYjMxNi0xNGUyNWE3MTllYmMiLCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzI0MzM2MTYwLCJleHAiOjE3MjQzNzIxNTksImlhdCI6MTcyNDMzNjE2MH0.hY10cACONZI5Y4ClPT4rlO_xMiyds05v9Qz5EwjL8Dc
```

Com isso, poderá executar chamadas que precisem de autenticação. 

### 5. Parando e Removendo os Contêineres

Para parar e remover os contêineres, execute:

```bash
docker-compose down
```

## Testes

Para executar os testes, é preciso executar algumas etapas para iniciar:

#### 1. Docker Compose

Certifique-se de que o docker-compose esteja em execução: 

```bash
docker-compose ls
```

Se não aparecer alguma imagem em execução com o nome _mrc-challenge_ então, precisamos subir.

```bash
docker-compose up -d
```

#### 2. Build

Entre na raiz do projeto, e execute o seguinte comando: 

```bash
# para compilar a alteração
> dotnet build .\Api\Api.csproj
# para realizar os testes
> dotnet test .\Tests\Tests.csproj
```

Aguarde, e a aplicação irá trazer o resultado. Atualmente há 4 testes implementados, o resultado será similar à : 

**Aprovado!  – Com falha:     0, Aprovado:     4, Ignorado:     0, Total:     4, Duração: 4 s - Tests.dll (net8.0)**
