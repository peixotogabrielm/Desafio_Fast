# Desafio Fast

## Backend

## Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **ASP.NET Core Web API** - Para criação da API REST
- **Entity Framework Core** - ORM para acesso a dados
- **SQL Server** - Banco de dados
- **JWT (JSON Web Tokens)** - Autenticação e autorização
- **Swagger/OpenAPI** - Documentação da API


## Como executar

1. Clone o repositório
2. Navegue até a pasta do projeto: `cd Desafio_Fast\Desafio_Fast`
3. Execute: `dotnet run`
4. Acesse o Swagger: `https://localhost:7XXX/swagger`

API REST para gerenciamento de Workshops, Atas e Colaboradores.

## Autenticação
JWT via endpoint de login. Enviar `Authorization: Bearer <token>` nos endpoints protegidos.

### Login
POST `api/auth/login`  
Body: `{ "username": "<admin>", "password": "<senha>" }`  
Resposta: `{ token, expiresAt }`

## Endpoints

### Atas (`api/atas`)
- POST `api/atas` cria ata
- PUT `api/atas/{ataId}/colaboradores/{colaboradorId}` adiciona colaborador à ata
- DELETE `api/atas/{ataId}/colaboradores/{colaboradorId}` remove colaborador da ata
- GET `api/atas?workshopNome={nome}&data={yyyy-MM-dd}` lista atas (filtros opcionais)

### Colaboradores (`api/colaboradores`)
- POST `api/colaboradores` cria colaborador
- GET `api/colaboradores` lista colaboradores

### Workshops (`api/workshops`)
- POST `api/workshops` cria workshop
- GET `api/workshops/{id}` obtém workshop

## Códigos de Resposta Principais
201 Created, 200 OK, 204 No Content, 401 Unauthorized, 404 Not Found

## Observações
Todos os endpoints (exceto login) exigem autenticação JWT.

## Endpoints e Fluxo

Auth
- POST /api/auth/login → AuthController.Login

Atas
- POST /api/atas → AtasController.CreateAta → AtaService.CreateAtaAsync
- PUT /api/atas/{ataId}/colaboradores/{colaboradorId} → AtasController.AddColaborador → AtaService.AddColaboradorAsync
- DELETE /api/atas/{ataId}/colaboradores/{colaboradorId} → AtasController.RemoveColaborador → AtaService.RemoveColaboradorAsync
- GET /api/atas → AtasController.GetAtas → AtaService.GetAtasAsync

Colaboradores
- POST /api/colaboradores → ColaboradoresController.CreateColaborador → ColaboradorService.CreateColaboradorAsync
- GET /api/colaboradores → ColaboradoresController.GetColaboradores → ColaboradorService.GetColaboradoresAsync

Workshops
- POST /api/workshops → WorkshopsController.CreateWorkshop → WorkshopService.CreateWorkshopAsync
- GET /api/workshops/{id} → WorkshopsController.GetWorkshop → WorkshopService.GetWorkshopAsync

# Frontend

## Requisitos
- Node.js (recomendado: >= 16.x ou 18.x LTS)
- npm (vem com Node)
- Angular CLI 15.x (opcional instalar globalmente)

## O que foi implementado:
Verifique versão Node:
```bash
node -v
```
>>>>>>> Stashed changes

Se precisar instalar Angular CLI global (opcional, você pode usar npx):
```bash
npm install -g @angular/cli@15.2.6
```

## Instalação
Dentro da pasta do projeto (`workshop-atas-app`):
```bash
npm install
```
Isto instalará todas as dependências listadas em `package.json`.

## Executando em Desenvolvimento
```bash
npm start
```
Depois acesse: http://localhost:4200/

Caso a porta esteja ocupada, você pode alterar:
```bash
ng serve --port 4300
```
ou
```bash
npm start -- --port=4300
```


