# E-commerce API — convenções de organização

A API está organizada por responsabilidades em pastas dentro de um único projeto .NET (`EcommercePiorApi`). Esta etapa prepara a estrutura; não pretende implementar todos os serviços e repositórios agora.

## Estrutura

```text
EcommercePiorApi/
├── Api/
│   ├── Controllers/       # Endpoints HTTP; recebem dependências pelo construtor
│   └── Middleware/        # Tratamento transversal de requisições e erros
├── Application/
│   ├── Interfaces/        # Contratos de serviços e repositórios
│   ├── Services/           # Casos de uso e coordenação da aplicação
│   └── DTOs/
│       ├── Requests/      # Modelos de entrada da API
│       └── Responses/     # Modelos de saída da API
├── Domain/
│   ├── Entities/          # Entidades e conceitos centrais do negócio
│   ├── Enums/             # Estados e opções representados por tipos
│   └── Exceptions/        # Exceções específicas do domínio
├── Infrastructure/
│   ├── Persistence/       # EF Core, DbContext e persistência
│   └── Integrations/      # Integrações externas e implementações mockadas
├── Program.cs             # Composição da aplicação e registro de dependências
└── appsettings.json       # Configuração não secreta, incluindo connection string local
```

## Convenções do grupo

- **Controllers:** ficam em `Api/Controllers`. Devem lidar com HTTP e delegar o trabalho às dependências; não devem criar manualmente o `DbContext`.
- **Entidades:** ficam em `Domain/Entities`; não misturar DTOs de requisição/resposta com entidades.
- **DTOs:** ficam em `Application/DTOs/Requests` ou `Application/DTOs/Responses`, conforme a direção dos dados.
- **Interfaces e serviços:** contratos em `Application/Interfaces`; casos de uso em `Application/Services`.
- **Persistência:** `AppDbContext` e configurações relacionadas ao EF Core em `Infrastructure/Persistence`.
- **Integrações:** implementações de serviços externos e mocks em `Infrastructure/Integrations`.
- **Injeção de dependência:** registrar implementações no `Program.cs` (ou em extensões de registro quando o projeto crescer) e receber dependências por construtor.
- **Configuração:** ler valores de configuração com `builder.Configuration`; não fixar connection strings no código.
- **Namespaces:** acompanhar a estrutura de pastas, usando `EcommercePiorApi` como namespace raiz.
- **Mudanças incrementais:** cada issue deve manter a aplicação compilável e validar os critérios de aceite relacionados ao seu escopo.

## SQLite e inicialização

A connection string `Ecommerce` está em `appsettings.json`. O `AppDbContext` recebe `DbContextOptions<AppDbContext>` pelo construtor e é registrado por `AddDbContext` no `Program.cs`. A inicialização usa um escopo de DI para criar o banco e cadastrar os quatro produtos de exemplo apenas quando a tabela de produtos está vazia.

> Nesta etapa, alguns diretórios são apenas pontos de extensão para issues futuras. As regras de negócio, DTOs completos de resposta, repositórios, middleware e serviços serão implementados nas issues correspondentes.
