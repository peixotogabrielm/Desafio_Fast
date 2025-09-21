# Desafio Fast API

# Backend
API para gerenciamento de workshops, colaboradores e atas de presença.

## Como executar

1. Clone o repositório
2. Navegue até a pasta do projeto: `cd Desafio_Fast\Desafio_Fast`
3. Execute: `dotnet run`
4. Acesse o Swagger: `https://localhost:7XXX/swagger`

## Autenticação

A API usa JWT Bearer Token. Todos os endpoints (exceto registro e login) requerem autenticação.

# Frontend


## ✅ O que foi implementado:

### 1. **Modelos de Dados** (seguindo sua especificação)
- `Colaborador`: { id: number, nome: string }
- `Workshop`: { id: number, nome: string, dataRealizacao: Date, descricao: string }
- `Ata`: { id: number, workshop: Workshop, colaboradores: Colaborador[] }

### 2. **Interface de Visualização de Atas**
- Lista todas as atas por padrão
- Mostra data, nome, descrição e colaboradores de cada workshop
- Filtro por nome do colaborador
- Filtro por nome do workshop
- Filtro por data
- Design responsivo baseado no seu mock

### 3. **Detalhes do Workshop**
- Clique no nome do workshop para ver detalhes
- Lista completa de colaboradores presentes
- Informações detalhadas do workshop

### 4. **Integração com API C#**
- Serviço completo para todos os endpoints da sua API na porta 7163
- Implementação de todos os métodos HTTP necessários
- Dados mock para desenvolvimento

## 🚀 Como executar:

1. **Abra o terminal na pasta do projeto:**
   ```bash
   cd [CAMINHO DO PROJETO]
   ```

2. **Instale as dependências (se necessário):**
   ```bash
   npm install
   ```

3. **Execute a aplicação:**
   ```bash
   ng serve
   ```

4. **Abra no navegador:**
   `http://localhost:4200`


