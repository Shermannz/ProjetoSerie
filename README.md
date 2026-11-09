# ProjetoSerie

API REST em .NET para cadastro de séries, episódios e categorias, com upload de imagem de capa e paginação de resultados.

## Tecnologias

- C# / .NET 9 (ASP.NET Web API)
- Dapper (acesso a dados com SQL)
- SQLite
- Cloudinary (armazenamento das imagens de capa)
- dotenv.net (variáveis de ambiente)

## Funcionalidades

- CRUD de séries, com relacionamento muitos-para-muitos com categorias (gravado em transação)
- CRUD de episódios, vinculados a uma série
- CRUD de categorias
- Upload da imagem de capa da série para o Cloudinary
- Listagem de séries paginada
- Arquitetura em camadas: Controller → Service → Repository

## Estrutura

```
Backend/
├── Controllers/    # endpoints da API
├── Services/       # regras de negócio
├── Repositories/   # acesso ao banco com Dapper
└── Models/         # entidades e DTOs
```

## Como rodar

**Pré-requisitos:** [.NET 9 SDK](https://dotnet.microsoft.com/download) e uma conta gratuita no [Cloudinary](https://cloudinary.com/).

1. Clone o repositório:

   ```bash
   git clone https://github.com/Shermannz/ProjetoSerie.git
   cd ProjetoSerie/Backend
   ```

2. Crie um arquivo `.env` na pasta `Backend` com as credenciais do Cloudinary:

   ```
   CLOUDINARY_CLOUD_NAME=seu_cloud_name
   CLOUDINARY_API_KEY=sua_api_key
   CLOUDINARY_API_SECRET=seu_api_secret
   ```

3. Compile o projeto para criar a pasta de saída:

   ```bash
   dotnet build
   ```

4. Crie o banco SQLite em `bin/Debug/net9.0/MeuBanco.db` com as tabelas abaixo (por exemplo, com o `sqlite3` ou o DB Browser for SQLite):

   ```sql
   CREATE TABLE Categoria (
       id_categoria INTEGER PRIMARY KEY AUTOINCREMENT,
       nome TEXT NOT NULL
   );

   CREATE TABLE Serie (
       id_serie INTEGER PRIMARY KEY AUTOINCREMENT,
       nome TEXT NOT NULL,
       sinopse TEXT,
       estudio TEXT,
       ano INTEGER,
       lancamento INTEGER,
       foto TEXT
   );

   CREATE TABLE SerieCategoria (
       id_serie INTEGER NOT NULL,
       id_categoria INTEGER NOT NULL,
       PRIMARY KEY (id_serie, id_categoria),
       FOREIGN KEY (id_serie) REFERENCES Serie (id_serie),
       FOREIGN KEY (id_categoria) REFERENCES Categoria (id_categoria)
   );

   CREATE TABLE Episodio (
       id_episodio INTEGER PRIMARY KEY AUTOINCREMENT,
       nome TEXT NOT NULL,
       video TEXT,
       id_serie INTEGER NOT NULL,
       FOREIGN KEY (id_serie) REFERENCES Serie (id_serie)
   );
   ```

5. Execute a API:

   ```bash
   dotnet run --launch-profile http
   ```

A API sobe em `http://localhost:5256`. No ambiente de desenvolvimento, a especificação OpenAPI fica em `/openapi/v1.json`.

## Endpoints

### Séries (`/serie`)

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/serie?pagina=1&tamanhoPagina=10` | Lista as séries com paginação |
| GET | `/serie/{id}` | Busca uma série por id, com categorias e episódios |
| POST | `/serie` | Cria uma série (`multipart/form-data`: dados, `file` com a capa e `categorias` com os ids) |
| PUT | `/serie/{id}` | Atualiza uma série e suas categorias |
| PUT | `/serie/atualizar-imagem/{id}` | Atualiza apenas a imagem de capa |
| DELETE | `/serie/{id}` | Exclui uma série |

### Episódios (`/episodio`)

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/episodio` | Lista os episódios |
| GET | `/episodio/{id}` | Busca um episódio por id |
| POST | `/episodio` | Cria um episódio |
| PUT | `/episodio/{id}` | Atualiza um episódio |
| DELETE | `/episodio/{id}` | Exclui um episódio |

### Categorias (`/categoria`)

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/categoria` | Lista as categorias |
| POST | `/categoria` | Cria uma categoria |
| PUT | `/categoria/{id}` | Atualiza uma categoria |
| DELETE | `/categoria/{id}` | Exclui uma categoria |

## Próximos passos

- Filtro de séries por nome e categoria
- Mover a string de conexão do banco para o `appsettings.json`
- Autenticação com JWT
- Frontend em React com TypeScript

## Autor

Robson Cardoso Faraco — [LinkedIn](https://linkedin.com/in/robson-cardoso-faraco) · [GitHub](https://github.com/Shermannz)
