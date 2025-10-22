# Workshop Atas App

Aplicação frontend em Angular para visualização de workshops e suas respectivas atas contendo participantes (colaboradores). Este README consolida os tópicos solicitados para apresentação em entrevista.

## 1. Metodologia Utilizada
A abordagem seguiu princípios de desenvolvimento incremental e orientação a componentes:
- **Feature First**: Pastas por domínio (`components/atas-list`, `components/workshop-details`, `models`, `services`).
- **Separação de responsabilidades**: Interfaces de dados em `models`, lógica de obtenção/filtro em `MockDataService`, apresentação nas camadas de componentes.
- **Mock de dados**: Uso de um service local para simular backend, permitindo foco em UI/UX e modelagem antes de integrar API real.
- **RxJS + Observables**: Para representar fluxos assíncronos futuros (facilitando troca por chamadas HTTP).
- **Escopo mínimo viável (MVP)**: Entregar listagem e detalhes antes de enriquecer com filtros específicos.

## 2. Passo a Passo da Codificação
1. Inicialização do projeto com Angular CLI (`ng new`).
2. Criação das interfaces de modelo: `Workshop`, `Colaborador`, `Ata` em `src/app/models` para tipagem forte.
3. Implementação do `index.ts` (barrel) para export centralizado dos modelos.
4. Criação do `MockDataService` com coleções em memória e métodos de acesso/filtragem (por nome de workshop, nome de colaborador, data e id).  
5. Implementação do componente `atas-list` para listar atas e permitir (potencial) filtros usando métodos do service.
6. Implementação do componente `workshop-details` para exibir detalhes de um workshop selecionado (via rota/id).
7. Configuração de rotas em `app-routing.module.ts` (ex.: rota raiz listagem e rota de detalhes `workshop/:id`).
8. Estilização básica com `SCSS` mantendo escopo por componente.
9. Ajustes de template para bind de dados e iteração *ngFor sobre coleções.
10. Testes manuais em desenvolvimento (`npm start`) verificando fluxo de navegação e rendering.

## 3. Dificuldades e Como Foram Superadas
| Dificuldade | Abordagem de Superação |
|-------------|------------------------|
| Definição clara de relações entre Ata/Workshop/Colaborador | Uso de composição direta (Ata contém `workshop: Workshop` e `colaboradores: Colaborador[]`). |
| Simulação de filtros sem backend | Implementação de métodos de filtragem local no service com `Array.filter` e `String.includes` case-insensitive. |
| Garantir tipagem coerente para datas | Armazenar `dataRealizacao` como `Date` real e converter apenas na exibição (format pipes). |
| Evitar acoplamento entre componentes | Comunicação via rota e injeção do service; nenhum compartilhamento de estado global complexo. |
| Preparar para futura integração HTTP | Uso de `Observable` desde o início, reduzindo refator posterior (basta trocar `of()` por `http.get()`). |

## 4. Percepção Sobre o Desafio
- **Nível de dificuldade**: Moderado-baixo em termos de estrutura Angular; foco maior em modelagem clara e filtragem de dados.
- **Clareza**: Requisitos objetivos (listar workshops, atas e participantes) permitiram modelagem direta.
- **Escalabilidade**: Estrutura atual facilita expansão (ex.: adicionar CRUD, paginação ou autenticação) sem reescrever base.

## 5. Estrutura do Código-Fonte
```
src/
  app/
    app.module.ts                # Módulo raiz
    app-routing.module.ts         # Definição de rotas
    components/
      atas-list/                  # Listagem de atas
        atas-list.component.*
      workshop-details/           # Detalhes de workshop
        workshop-details.component.*
    models/                       # Modelagem de dados (interfaces)
      ata.model.ts
      colaborador.model.ts
      workshop.model.ts
      index.ts                    # Barrel exports
    services/
      mock-data.service.ts        # Fonte de dados (mock + filtros)
  assets/                         # Recursos estáticos
  main.ts                         # Bootstrap Angular
  styles.scss                     # Estilos globais
```
**Racional**: Componentes coesos, service isolado, modelos declarativos, rotas explícitas e fácil substituição do mock por backend real.

## 6. Modelagem de Dados
Interfaces em TypeScript fornecem contrato estável:
```ts
export interface Workshop {
  id: number;
  nome: string;
  dataRealizacao: Date;
  descricao: string;
}

export interface Colaborador {
  id: number;
  nome: string;
}

export interface Ata {
  id: number;
  workshop: Workshop;
  colaboradores: Colaborador[];
}
```
- **Relações**: `Ata` encapsula o contexto do workshop e os colaboradores presentes.
- **Normalização vs. Composição**: Optou-se por composição simples (sem IDs referenciais separados) dada a ausência de grande volume ou necessidade de lazy loading. Facilita leitura e prototipagem inicial.
- **Extensibilidade**: Fácil adicionar campos como `notas: string` em `Ata` ou `cargo` em `Colaborador` com impacto mínimo.

## 7. Navegação Entre Telas e Funcionalidades
- **Roteamento**: `app-routing.module.ts` define rotas principais (ex.: `/` para lista de atas; `/workshop/:id` para detalhes).
- **Listagem de Atas**: Renderiza cada ata exibindo nome do workshop e participantes.
- **Detalhes de Workshop**: Consulta via ID e exibe descrição e data em destaque.
- **Filtragem (Service)**: Métodos disponíveis para futuras integrações de UI:
  - `obterAtasPorWorkshopNome(nome: string)`
  - `obterAtasPorColaboradorNome(nome: string)`
  - `obterAtasPorData(data: Date)`
- **Observables**: Permitem futuros loaders/spinners e tratamento de erros (ex.: `catchError`).

## 8. Execução do Projeto
### Pré-requisitos
- Node.js LTS (≥ 16) e NPM
- Angular CLI (global opcional): `npm install -g @angular/cli`

### Scripts
- `npm start` – Servidor de desenvolvimento (default `http://localhost:4200`).
- `npm test` – Executa testes unitários Karma/Jasmine (estrutura pronta para expansão).
- `npm run build` – Build de produção usando Angular CLI.

### Passos
```powershell
# Instalar dependências
npm install

# Executar em desenvolvimento
npm start

# Rodar testes
npm test

# Gerar build de produção
npm run build
```

## 9. Decisões de Design
| Decisão | Justificativa |
|---------|---------------|
| Service isolado | Facilita troca para HTTP sem alterar componentes. |
| Interfaces simples | Menor custo cognitivo em MVP. |
| Observables desde início | Pronta adaptação a assincronismo real. |
| Componentes específicos (list vs details) | Single Responsibility e rotas claras. |
| SCSS por componente | Escopo e manutenção localizada. |

## 10. Possíveis Evoluções
- Integração com backend real (REST ou GraphQL).
- Adição de formulário para criação/edição de atas.
- Paginação e busca com debounce para filtros.
- Estado global (NgRx ou Signals) se volume crescer.
- Internacionalização (i18n) para múltiplos idiomas.
- Testes unitários e de integração abrangendo service e componentes.

## 11. Lições Aprendidas
- Começar com modelo de dados claro acelera decisões de UI.
- Observables antecipam necessidades de assincronismo sem overengineering.
- Separar mock de dados em service evita espalhar lógica de filtro nos componentes.

## 12. Resumo para Entrevista
Este projeto demonstra: modelagem de dados explícita, arquitetura modular em Angular, uso de service para abstração de dados, potencial de ampliação para filtros avançados e integração futura. Foca em clareza, separação de responsabilidades e preparo para evolução incremental.

---
Qualquer dúvida ou necessidade de expansão, abra uma issue ou proponha PR.
