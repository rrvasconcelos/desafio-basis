# BookStore - Aplicação de Gerenciamento de Livros

Um sistema completo para gerenciamento de livros, autores e assuntos, desenvolvido com arquitetura limpa e modernas tecnologias web.

## 🛠️ Tecnologias Utilizadas

### Backend

* .NET 9
* Clean Architecture
* Entity Framework Core
* PostgreSQL
* CQRS com MediatR
* Pattern Result
* Docker

### Frontend

* Angular 19
* Angular Material
* TypeScript
* RxJS
* Docker

## 🚀 Como Executar a Aplicação

### Pré-requisitos

* [Docker](https://www.docker.com/products/docker-desktop/) instalado
* [Docker Compose](https://docs.docker.com/compose/install/) instalado

### Passos para Execução

1. **Clone o repositório**
   ```bash
   git clone https://github.com/seu-usuario/desafio-basis.git
   cd desafio-basis
   ```
2. **Execute a aplicação com Docker Compose**
   ```bash
   docker-compose up --build
   ```
3. **Acesse as aplicações**
   * Frontend: [http://localhost:4200](http://localhost:4200/)
   * API: [http://localhost:8080](http://localhost:8080/)
   * Swagger: [http://localhost:8080/swagger](http://localhost:8080/swagger)
4. **Para parar a aplicação**
   ```bash
   docker-compose down
   ```

## 📊 Conectando ao Banco de Dados

Para conectar ao banco de dados PostgreSQL usando um cliente como DBeaver:

* **Host** : localhost
* **Porta** : 5433
* **Banco de dados** : bookStoreDb
* **Usuário** : postgres
* **Senha** : postgres

## 🧩 Estrutura do Projeto

```
desafio-basis/
├── src/
│   ├── BookStore.Api/           # API REST
│   ├── BookStore.Application/   # Regras de negócio e casos de uso
│   ├── BookStore.Domain/        # Entidades e regras de domínio
│   ├── BookStore.Infrastructure/# Implementações de persistência
│   ├── BookStore.SharedKernel/  # Componentes compartilhados
│   └── BookStore.Web/           # Frontend Angular
└── docker-compose.yml           # Configuração Docker
```

**Copiar**

## 🔍 Desenvolvimento Local

### Backend (.NET)

```bash
cd src/BookStore.Api
dotnet run
```

**Copiar**

### Frontend (Angular)

```bash
cd src/BookStore.Web
npm install
ng serve
```

## 📄 Licença

Este projeto está licenciado sob a licença MIT - veja o arquivo [LICENSE](https://sai-library.saiapplications.com/saiappgen/LICENSE) para detalhes.

---

Desenvolvido como parte do desafio técnico para a empresa Basis.
