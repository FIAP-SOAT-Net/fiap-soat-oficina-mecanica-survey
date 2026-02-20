# Smart Mechanical Workshop - Survey API

API backend para gerenciamento de pesquisas de satisfação de clientes da oficina mecânica. Construída com .NET 10 Minimal API, MongoDB para armazenamento e RabbitMQ para processamento assíncrono de envio de emails.

## 📋 Índice

- [Arquitetura](#arquitetura)
- [Tecnologias](#tecnologias)
- [Endpoints](#endpoints)
- [MongoDB Schema](#mongodb-schema)
- [RabbitMQ](#rabbitmq)
- [Configuração](#configuração)
- [Desenvolvimento Local](#desenvolvimento-local)
- [Docker](#docker)

## 🏗️ Arquitetura

```
┌─────────────┐      ┌─────────────┐      ┌─────────────┐
│   Web App   │─────▶│     API     │─────▶│   MongoDB   │
│  (Frontend) │      │  (Backend)  │      │  (Database) │
└─────────────┘      └─────────────┘      └─────────────┘
                            │
                            │
                            ▼
                     ┌─────────────┐
                     │  RabbitMQ   │
                     │  (Queue)    │
                     └─────────────┘
                            │
                            │
                            ▼
                     ┌─────────────┐
                     │    Email    │
                     │   Service   │
                     └─────────────┘
```

### Componentes

1. **API REST**: Recebe e armazena respostas de pesquisas
2. **MongoDB Repository**: Gerencia persistência de dados
3. **RabbitMQ Consumer**: Serviço em background que consome mensagens
4. **Email Service**: Envia convites de pesquisa via SMTP

## 🚀 Tecnologias

- **.NET 10.0** - Framework principal
- **Minimal API** - Arquitetura de API simplificada
- **MongoDB 7.0** - Banco de dados NoSQL
- **RabbitMQ 3.13** - Message broker
- **MailKit 4.3.0** - Cliente SMTP para envio de emails
- **Docker** - Containerização

### Pacotes NuGet

```xml
<PackageReference Include="MongoDB.Driver" Version="2.28.0" />
<PackageReference Include="RabbitMQ.Client" Version="6.8.1" />
<PackageReference Include="MailKit" Version="4.3.0" />
```

## 📡 Endpoints

### POST /api/survey

Cria uma nova resposta de pesquisa.

**Request Body:**
```json
{
  "customerName": "João Silva",
  "customerEmail": "joao.silva@email.com",
  "vehicleModel": "Honda Civic 2020",
  "serviceName": "Revisão Geral",
  "question1": 5,
  "question2": 4,
  "question3": 5,
  "question4": 5,
  "question5": 4,
  "comments": "Excelente atendimento!"
}
```

**Response:** `201 Created`
```json
{
  "id": "65abc123def456789",
  "customerName": "João Silva",
  "customerEmail": "joao.silva@email.com",
  "vehicleModel": "Honda Civic 2020",
  "serviceName": "Revisão Geral",
  "question1": 5,
  "question2": 4,
  "question3": 5,
  "question4": 5,
  "question5": 4,
  "comments": "Excelente atendimento!",
  "submittedAt": "2024-01-20T15:30:00Z"
}
```

### GET /api/survey/{id}

Recupera uma pesquisa específica por ID.

**Response:** `200 OK`
```json
{
  "id": "65abc123def456789",
  "customerName": "João Silva",
  ...
}
```

### GET /api/survey?page={page}&pageSize={pageSize}

Lista pesquisas com paginação (ordenadas por data de submissão decrescente).

**Query Parameters:**
- `page` (default: 1)
- `pageSize` (default: 10)

**Response:** `200 OK`
```json
[
  {
    "id": "65abc123def456789",
    "customerName": "João Silva",
    ...
  },
  ...
]
```

### GET /health

Health check endpoint.

**Response:** `200 OK`
```json
{
  "status": "Healthy"
}
```

## 🗄️ MongoDB Schema

**Collection:** `SurveyResponses`

**Document Structure:**
```javascript
{
  "_id": ObjectId("65abc123def456789"),
  "CustomerName": "João Silva",
  "CustomerEmail": "joao.silva@email.com",
  "VehicleModel": "Honda Civic 2020",
  "ServiceName": "Revisão Geral",
  "Question1": 5,
  "Question2": 4,
  "Question3": 5,
  "Question4": 5,
  "Question5": 4,
  "Comments": "Excelente atendimento!",
  "SubmittedAt": ISODate("2024-01-20T15:30:00Z")
}
```

**Indexes:**
- `_id` (default)
- `SubmittedAt` (descending) - para ordenação eficiente

## 🐰 RabbitMQ

### Queue Configuration

- **Queue Name:** `survey-requests`
- **Exchange:** Default (direct)
- **Durability:** Durable (sobrevive a reinicializações)
- **Auto-delete:** false
- **Exclusive:** false

### Message Format

```json
{
  "CustomerName": "João Silva",
  "CustomerEmail": "joao.silva@email.com",
  "VehicleModel": "Honda Civic 2020",
  "ServiceName": "Revisão Geral"
}
```

### Consumer Behavior

O `SurveyRequestConsumerService` roda como um `BackgroundService` e:

1. **Conecta ao RabbitMQ** ao iniciar a aplicação
2. **Consome mensagens** da fila `survey-requests`
3. **Processa cada mensagem** chamando o `EmailService`
4. **ACK positivo** em caso de sucesso
5. **NACK** em caso de erro (mensagem volta para a fila)
6. **Reconexão automática** em caso de falha de conexão

## ⚙️ Configuração

### appsettings.json

```json
{
  "MongoDb": {
    "ConnectionString": "mongodb://localhost:27017",
    "Database": "SmartMechanicalWorkshop",
    "CollectionName": "SurveyResponses",
    "UserName": "",
    "Password": ""
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "QueueName": "survey-requests"
  },
  "Email": {
    "SmtpHost": "localhost",
    "SmtpPort": 1025,
    "FromEmail": "noreply@oficinamecanica.com",
    "FromName": "Oficina Mecânica",
    "UseSsl": false
  },
  "Survey": {
    "WebAppUrl": "http://localhost:8080"
  }
}
```

### Variáveis de Ambiente

Todas as configurações podem ser sobrescritas via variáveis de ambiente:

```bash
# MongoDB
MongoDB__ConnectionString=mongodb://mongodb:27017
MongoDB__UserName=root
MongoDB__Password=example

# RabbitMQ
RabbitMQ__HostName=rabbitmq
RabbitMQ__UserName=guest
RabbitMQ__Password=guest

# Email
Email__SmtpHost=mailhog
Email__SmtpPort=1025
Email__FromEmail=noreply@oficinamecanica.com

# Survey
Survey__WebAppUrl=http://localhost
```

## 💻 Desenvolvimento Local

### Pré-requisitos

- .NET 10 SDK
- MongoDB 7.0+
- RabbitMQ 3.13+
- MailHog (para testes de email)

### Instalação

1. **Clone o repositório:**
```bash
git clone <repository-url>
cd src/api
```

2. **Restaure as dependências:**
```bash
dotnet restore
```

3. **Configure o appsettings.Development.json:**
```json
{
  "MongoDb": {
    "ConnectionString": "mongodb://localhost:27017"
  },
  "RabbitMQ": {
    "HostName": "localhost"
  },
  "Email": {
    "SmtpHost": "localhost",
    "SmtpPort": 1025
  }
}
```

4. **Execute a aplicação:**
```bash
dotnet run
```

A API estará disponível em `http://localhost:5000`.

### Testando com cURL

**Criar pesquisa:**
```bash
curl -X POST http://localhost:5000/api/survey \
  -H "Content-Type: application/json" \
  -d '{
    "customerName": "João Silva",
    "customerEmail": "joao.silva@email.com",
    "vehicleModel": "Honda Civic 2020",
    "serviceName": "Revisão Geral",
    "question1": 5,
    "question2": 4,
    "question3": 5,
    "question4": 5,
    "question5": 4,
    "comments": "Excelente!"
  }'
```

**Listar pesquisas:**
```bash
curl http://localhost:5000/api/survey?page=1&pageSize=10
```

**Buscar pesquisa por ID:**
```bash
curl http://localhost:5000/api/survey/65abc123def456789
```

## 🐳 Docker

### Build

```bash
docker build -t smart-mechanical-workshop-survey-api:latest .
```

### Run

```bash
docker run -d \
  --name survey-api \
  -p 3000:8080 \
  -e MongoDB__ConnectionString=mongodb://mongodb:27017 \
  -e RabbitMQ__HostName=rabbitmq \
  -e Email__SmtpHost=mailhog \
  smart-mechanical-workshop-survey-api:latest
```

### Docker Compose (Development)

```bash
# Iniciar todos os serviços
docker-compose -f ../../docker-compose.dev.yml up

# Acessar:
# - API: http://localhost:3000
# - MongoDB: localhost:27017
# - RabbitMQ Management: http://localhost:15672
# - MailHog UI: http://localhost:8025
```

### Docker Compose (Production)

```bash
# Iniciar stack de produção
docker-compose up -d

# Logs
docker-compose logs -f api

# Parar
docker-compose down
```

## 📊 Monitoramento

### Health Check

```bash
curl http://localhost:3000/health
```

### RabbitMQ Management

Acesse `http://localhost:15672` (usuário: `guest`, senha: `guest`)

- Visualizar filas
- Monitorar consumidores
- Inspecionar mensagens

### MailHog UI

Acesse `http://localhost:8025` para visualizar emails enviados durante o desenvolvimento.

## 🧪 Testes

### Fluxo Completo de Teste

1. **Enviar mensagem para RabbitMQ** (simula serviço de criação de ordem de serviço):
```bash
# Publicar mensagem via RabbitMQ Management UI ou usar script
```

2. **Consumer processa mensagem** automaticamente

3. **Email enviado** para MailHog

4. **Cliente acessa link** no email

5. **Cliente preenche pesquisa** na web

6. **POST para API** salva resposta

7. **Consultar dados** via GET /api/survey

## 🔐 Segurança

### Produção

Para ambiente de produção, considere:

1. **Autenticação/Autorização**: Adicionar JWT ou OAuth2
2. **Rate Limiting**: Limitar requisições por IP
3. **CORS**: Configurar origens permitidas
4. **HTTPS**: Usar certificados SSL/TLS
5. **Secrets**: Usar Azure Key Vault, AWS Secrets Manager, etc.
6. **MongoDB**: Habilitar autenticação e usar usuários específicos
7. **RabbitMQ**: Criar usuários com permissões limitadas

## 📝 Licença

Este projeto faz parte do trabalho acadêmico da FIAP - SOAT.
