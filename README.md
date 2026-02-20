# Smart Mechanical Workshop Survey

[![Web Deploy](https://github.com/FIAP-SOAT-Net/fiap-soat-oficina-mecanica-survey/actions/workflows/webdeploy.yml/badge.svg)](https://github.com/FIAP-SOAT-Net/fiap-soat-oficina-mecanica-survey/actions/workflows/webdeploy.yml)
[![API Deploy](https://github.com/FIAP-SOAT-Net/fiap-soat-oficina-mecanica-survey/actions/workflows/apideploy.yml/badge.svg)](https://github.com/FIAP-SOAT-Net/fiap-soat-oficina-mecanica-survey/actions/workflows/apideploy.yml)

Sistema de pesquisa de satisfação para oficina mecânica. Solução completa com frontend web, API backend em .NET 10, MongoDB, RabbitMQ para envio assíncrono de emails e MailHog para testes.

## 🏗️ Arquitetura

```
┌─────────────┐      ┌─────────────┐      ┌─────────────┐
│   Web App   │─────▶│  .NET API   │─────▶│   MongoDB   │
│  (Nginx)    │      │ (Min. API)  │      │     7.0     │
└─────────────┘      └─────────────┘      └─────────────┘
                            │
                            │
                            ▼
                     ┌─────────────┐
                     │  RabbitMQ   │
                     │   3.13      │
                     └─────────────┘
                            │
                            │
                            ▼
                     ┌─────────────┐
                     │   MailKit   │
                     │  (Email)    │
                     └─────────────┘
```

## 🚀 Quick Start

### Desenvolvimento Local (Docker Compose)

```bash
# Subir todo o stack de desenvolvimento
docker-compose -f docker-compose.dev.yml up

# Serviços disponíveis:
# - Web: http://localhost:8080
# - API: http://localhost:3000
# - RabbitMQ Management: http://localhost:15672 (guest/guest)
# - MailHog UI: http://localhost:8025
# - MongoDB: localhost:27017
```

### Produção

```bash
# Copiar arquivo de configuração
cp .env.example .env

# Editar variáveis de ambiente
nano .env

# Subir stack de produção
docker-compose up -d
```

## 📁 Estrutura do Projeto

```
.
├── .github/
│   └── workflows/
│       ├── webdeploy.yml       # Deploy do frontend
│       └── apideploy.yml       # Deploy da API
├── src/
│   ├── web/                    # Frontend (HTML/CSS/JS)
│   │   ├── docker/
│   │   │   ├── nginx.conf
│   │   │   └── docker-entrypoint.sh
│   │   ├── Dockerfile
│   │   ├── index.html
│   │   ├── styles.css
│   │   ├── script.js
│   │   └── README.md
│   └── api/                    # Backend (.NET 10)
│       ├── Models/
│       │   ├── SurveyResponse.cs
│       │   └── SurveyRequest.cs
│       ├── Configuration/
│       │   ├── MongoDbConfiguration.cs
│       │   ├── RabbitMqConfiguration.cs
│       │   ├── EmailConfiguration.cs
│       │   └── SurveyConfiguration.cs
│       ├── Repositories/
│       │   └── SurveyRepository.cs
│       ├── Services/
│       │   ├── EmailService.cs
│       │   └── SurveyRequestConsumerService.cs
│       ├── Dockerfile
│       ├── Program.cs
│       ├── appsettings.json
│       └── README.md
├── docker-compose.yml          # Produção
├── docker-compose.dev.yml      # Desenvolvimento
├── .env.example                # Variáveis de ambiente
└── README.md

## 🚢 Tecnologias

### Frontend
- **HTML5** - Estrutura da aplicação
- **CSS3** - Estilização responsiva
- **JavaScript** (Vanilla) - Lógica cliente
- **Nginx Alpine** - Servidor web

### Backend
- **.NET 10.0** - Framework principal
- **Minimal API** - Arquitetura simplificada
- **MongoDB 7.0** - Banco de dados NoSQL
- **RabbitMQ 3.13** - Message broker
- **MailKit 4.3.0** - Cliente SMTP

### DevOps
- **Docker** - Containerização
- **Docker Compose** - Orquestração
- **GitHub Actions** - CI/CD

## 🐳 Docker

### Construir Imagens

#### Web
```bash
cd src/web
docker build -t smart-mechanical-workshop-survey-web .
```

#### API
```bash
cd src/api
docker build -t smart-mechanical-workshop-survey-api .
```

### Executar Containers Individuais

#### Web Container
```bash
docker run -d \
  -p 8080:80 \
  -e API_URL=http://localhost:3000/api/survey \
  smart-mechanical-workshop-survey-web
```

#### API Container
```bash
docker run -d \
  -p 3000:8080 \
  -e MongoDB__ConnectionString=mongodb://mongodb:27017 \
  -e RabbitMQ__HostName=rabbitmq \
  -e Email__SmtpHost=mailhog \
  smart-mechanical-workshop-survey-api
```

### Docker Compose - Desenvolvimento

```bash
# Subir todos os serviços
docker-compose -f docker-compose.dev.yml up

# Subir em background
docker-compose -f docker-compose.dev.yml up -d

# Ver logs
docker-compose -f docker-compose.dev.yml logs -f

# Ver logs de um serviço específico
docker-compose -f docker-compose.dev.yml logs -f api

# Parar
docker-compose -f docker-compose.dev.yml down

# Parar e remover volumes
docker-compose -f docker-compose.dev.yml down -v
```

### Docker Compose - Produção

```bash
# Subir stack completo
docker-compose up -d

# Ver status
docker-compose ps

# Parar
docker-compose down
```


## 🌐 Acessando a Aplicação

### Frontend (Web)

**URL com Parâmetros:**
```
http://localhost:8080/index.html?nome=[NOME]&email=[EMAIL]
```

**Exemplo:**
```
http://localhost:8080/index.html?nome=Maria%20Santos&email=maria@email.com
```

### API Endpoints

#### Health Check
```bash
curl http://localhost:3000/health
```

#### Criar Pesquisa
```bash
curl -X POST http://localhost:3000/api/survey \
  -H "Content-Type: application/json" \
  -d '{
    "customerName": "João Silva",
    "customerEmail": "joao@email.com",
    "vehicleModel": "Honda Civic 2020",
    "serviceName": "Revisão",
    "question1": 5,
    "question2": 4,
    "question3": 5,
    "question4": 5,
    "question5": 4,
    "comments": "Ótimo!"
  }'
```

#### Listar Pesquisas
```bash
curl http://localhost:3000/api/survey?page=1&pageSize=10
```

### Serviços de Desenvolvimento

- **RabbitMQ Management**: http://localhost:15672 (guest/guest)
- **MailHog UI**: http://localhost:8025

## 🔧 Variáveis de Ambiente

| Variável | Descrição | Padrão | Obrigatório |
|----------|-----------|--------|-------------|
| `API_URL` | URL da API para o frontend | `http://api:8080/api/survey` | Não |
| `WEB_APP_URL` | URL do frontend | `http://localhost` | Não |
| `ASPNETCORE_ENVIRONMENT` | Ambiente .NET | `Production` | Não |
| `MONGODB_USER` | Usuário MongoDB | `root` | Sim (Prod) |
| `MONGODB_PASSWORD` | Senha MongoDB | `example` | Sim (Prod) |
| `RABBITMQ_USER` | Usuário RabbitMQ | `guest` | Não |
| `RABBITMQ_PASSWORD` | Senha RabbitMQ | `guest` | Não |
| `SMTP_HOST` | Servidor SMTP | `mailhog` | Sim |
| `SMTP_PORT` | Porta SMTP | `1025` | Não |
| `FROM_EMAIL` | Email remetente | `noreply@oficinamecanica.com` | Não |
| `FROM_NAME` | Nome remetente | `Oficina Mecânica` | Não |

Veja [.env.example](.env.example) para configuração completa.


## 📦 Publicar Imagens no Docker Hub

### Manual (Via Terminal)

#### Web
```bash
# Login no Docker Hub
docker login

# Build
cd src/web
docker build -t smart-mechanical-workshop-survey-web .

# Tag
docker tag smart-mechanical-workshop-survey-web:latest \
  smartmechanicalworkshop/smart-mechanical-workshop-survey-web:latest

# Push
docker push smartmechanicalworkshop/smart-mechanical-workshop-survey-web:latest
```

#### API
```bash
# Build
cd src/api
docker build -t smart-mechanical-workshop-survey-api .

# Tag
docker tag smart-mechanical-workshop-survey-api:latest \
  smartmechanicalworkshop/smart-mechanical-workshop-survey-api:latest

# Push
docker push smartmechanicalworkshop/smart-mechanical-workshop-survey-api:latest
```

### Automático (Via GitHub Actions)

#### Web Deploy
1. Acesse: **Actions → Web Deploy**
2. Clique em **Run workflow**
3. Configure a tag desejada (ex: `v1.0.0`)
4. Execute o workflow

#### API Deploy
1. Acesse: **Actions → API Deploy**
2. Clique em **Run workflow**
3. Configure a tag desejada (ex: `v1.0.0`)
4. Execute o workflow

Os workflows fazem build multi-arquitetura (amd64/arm64) e publicam no Docker Hub.


## 🧪 Testando

### Stack Completo

```bash
# Subir todos os serviços
docker-compose -f docker-compose.dev.yml up -d

# Aguardar todos os serviços ficarem saudáveis
docker-compose -f docker-compose.dev.yml ps

# Testar Web
curl http://localhost:8080/health

# Testar API
curl http://localhost:3000/health

# Simular envio de pesquisa
curl -X POST http://localhost:3000/api/survey \
  -H "Content-Type: application/json" \
  -d '{
    "customerName": "Teste",
    "customerEmail": "teste@email.com",
    "vehicleModel": "Test Car",
    "serviceName": "Test Service",
    "question1": 5,
    "question2": 5,
    "question3": 5,
    "question4": 5,
    "question5": 5
  }'

# Ver emails no MailHog
open http://localhost:8025

# Cleanup
docker-compose -f docker-compose.dev.yml down
```

### Testar Containers Individuais

#### Web
```bash
docker build -t survey-web-test ./src/web
docker run -d -p 8080:80 --name test-web survey-web-test

# Testes
curl http://localhost:8080/health
curl http://localhost:8080/env-config.js

# Cleanup
docker stop test-web && docker rm test-web
```

#### API
```bash
docker build -t survey-api-test ./src/api
docker run -d -p 3000:8080 \
  --name test-api \
  -e MongoDB__ConnectionString=mongodb://host.docker.internal:27017 \
  -e RabbitMQ__HostName=host.docker.internal \
  survey-api-test

# Testes
curl http://localhost:3000/health

# Cleanup
docker stop test-api && docker rm test-api
```

### Testar RabbitMQ Message

1. Acesse RabbitMQ Management UI: http://localhost:15672
2. Login: `guest` / `guest`
3. Vá em **Queues** → `survey-requests`
4. Publique uma mensagem:
```json
{
  "CustomerName": "João Silva",
  "CustomerEmail": "joao@email.com",
  "VehicleModel": "Honda Civic",
  "ServiceName": "Revisão"
}
```
5. Verifique o email no MailHog: http://localhost:8025

## 🛠️ Desenvolvimento

### Frontend (Sem Docker)

```bash
cd src/web
python3 -m http.server 8000

# Acessar: http://localhost:8000/index.html?nome=Teste&email=teste@email.com
```

### API (.NET)

```bash
cd src/api
dotnet restore
dotnet run

# API rodará em: http://localhost:5000
```

### Hot Reload com Docker Compose

O `docker-compose.dev.yml` monta volumes para hot reload:

```bash
docker-compose -f docker-compose.dev.yml up

# Para Web: edite arquivos em src/web/ (mudanças instantâneas)
# Para API: edite arquivos em src/api/ e reconstrua: docker-compose -f docker-compose.dev.yml up --build api
```


## 📚 Documentação

- [Web Frontend](src/web/README.md) - Documentação detalhada do frontend
- [API Backend](src/api/README.md) - Documentação completa da API, endpoints, MongoDB, RabbitMQ
- [Environment Variables](.env.example) - Guia de variáveis de ambiente

## 🔐 Segurança

### Implementado
- ✅ Headers de segurança no Nginx (CSP, X-Frame-Options, etc.)
- ✅ Sanitização de inputs no frontend
- ✅ Validação de email
- ✅ Prevenção de XSS
- ✅ Health checks em todos os serviços
- ✅ Network isolation no Docker

### Recomendações para Produção
- 🔒 **Autenticação**: Implementar JWT ou OAuth2 na API
- 🔒 **HTTPS**: Usar certificados SSL/TLS
- 🔒 **Rate Limiting**: Limitar requisições por IP
- 🔒 **CORS**: Configurar origens permitidas
- 🔒 **Secrets**: Usar gerenciadores de secrets (Azure Key Vault, AWS Secrets Manager)
- 🔒 **MongoDB Auth**: Habilitar autenticação com usuários específicos
- 🔒 **RabbitMQ Auth**: Criar usuários com permissões limitadas
- 🔒 **SMTP Real**: Substituir MailHog por serviço SMTP de produção (SendGrid, AWS SES)

## 🚀 CI/CD

O projeto usa GitHub Actions para deploy automatizado:

- **Web Deploy**: `.github/workflows/webdeploy.yml`
- **API Deploy**: `.github/workflows/apideploy.yml`

Ambos workflows:
- ✅ Build multi-arquitetura (amd64/arm64)
- ✅ Push para Docker Hub
- ✅ Cache de layers para builds rápidos
- ✅ Trigger manual com versionamento

## 📄 Licença

Este projeto faz parte do trabalho acadêmico da FIAP - SOAT.

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📞 Suporte

Para dúvidas ou problemas, abra uma [issue](https://github.com/FIAP-SOAT-Net/fiap-soat-oficina-mecanica-survey/issues).
