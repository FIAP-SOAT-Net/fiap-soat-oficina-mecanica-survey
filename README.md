# Smart Mechanical Workshop Survey

Sistema de pesquisa de satisfação para oficina mecânica.

## 🏗️ Arquitetura

```
┌─────────────────┐
│   Web (Nginx)   │  ← Frontend estático (Pesquisa)
└────────┬────────┘
         │
         ↓
┌─────────────────┐
│   API           │  ← Backend (A ser implementado)
└─────────────────┘
```

## 🚀 Quick Start

### Desenvolvimento Local

```bash
# Subir apenas o frontend
docker-compose -f docker-compose.dev.yml up web

# Acessar: http://localhost:8080/index.html?nome=João Silva&email=joao@email.com
```

### Produção

```bash
# Build da imagem
docker build -t smart-mechanical-workshop-survey-web .

# Subir todos os serviços
docker-compose up -d
```

## 📁 Estrutura do Projeto

```
.
├── docker/                    # Configurações Docker
│   ├── nginx.conf            # Configuração do Nginx
│   └── docker-entrypoint.sh  # Script de inicialização
├── src/
│   ├── web/                  # Frontend (Pesquisa)
│   src/
│   ├── web/                  # Frontend (Pesquisa)
│   │   ├── docker/           # Configurações Docker do Web
│   │   │   ├── nginx.conf
│   │   │   └── docker-entrypoint.sh
│   │   ├── Dockerfile        # Build do container web
│   │   ├── .dockerignore
│   │   ├── index.html
│   │   ├── styles.css
│   │   ├── script.js
│   │   ├── env-config.js
│   │   └── README.md
│   └── api/                  # Backend (A ser implementado)
## 🐳 Docker

### Construir Imagem
# A partir da pasta web
cd src/web
docker build -t smart-mechanical-workshop-survey-web .

# Ou da raiz do projeto
docker build -t smart-mechanical-workshop-survey-web -f src/web/Dockerfile src/web
```bash
docker build -t smart-mechanical-workshop-survey-web .
```

### Executar Container

```bash
docker run -d \
  -p 8080:80 \
  -e API_URL=http://api.exemplo.com/survey \
  smart-mechanical-workshop-survey-web
```

### Docker Compose - Desenvolvimento

```bash
# Subir
docker-compose -f docker-compose.dev.yml up

# Subir em background
docker-compose -f docker-compose.dev.yml up -d

# Ver logs
docker-compose -f docker-compose.dev.yml logs -f web

# Parar
docker-compose -f docker-compose.dev.yml down
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

**Formato da URL:**
```
http://localhost:8080/index.html?nome=[NOME]&email=[EMAIL]
```

**Exemplo:**
```
http://localhost:8080/index.html?nome=Maria%20Santos&email=maria@email.com
```

### Health Check

```bash
curl http://localhost:8080/health
|----------|-----------|--------|-------------|
| `API_URL` | URL do endpoint da API | `/api/survey` | Não |
| `NODE_ENV` | Ambiente da aplicação | `production` | Não |
| `DATABASE_URL` | String de conexão PostgreSQL | - | Sim (API) |
| `POSTGRES_USER` | Usuário do banco | `user` | Sim (DB) |
| `POSTGRES_PASSWORD` | Senha do banco | `password` | Sim (DB) |
| `POSTGRES_DB` | Nome do banco | `survey_db` | Sim (DB) |

Veja [.env.example](.env.example) para mais detalhes.

## 📦 Publicar Imagem no Docker Hub

```bash
# Login no Docker Hub
docker login

# Tag da imagem
docker tag smart-mechanical-workshop-survey-web:latest \
  smartmechanicalworkshop/smart-mechanical-workshop-survey-web:latest

# Push
docker push smartmechanicalworkshop/smart-mechanical-workshop-survey-web:latest

# Com versão específica
docker tag smart-mechanical-workshop-survey-web:latest \
  smartmechanicalworkshop/smart-mechanical-workshop-survey-web:1.0.0
docker push smartmechanicalworkshop/smart-mechanical-workshop-survey-web:1.0.0
```

## 🧪 Testando

### Testar Web Container

```bash
# Build e run
docker build -t survey-web-test .
docker run -d -p 8080:80 --name test-web survey-web-test

# Teste básico
curl http://localhost:8080/health
curl http://localhost:8080/env-config.js

# Cleanup
docker stop test-web && docker rm test-web
```

### Testar com Custom API URL

```bash
docker run -d -p 8080:80 \
  -e API_URL=https://minha-api.com/v1/survey \
  smart-mechanical-workshop-survey-web

# Verificar configuração
curl http://localhost:8080/env-config.js
# Deve retornar: window.ENV_CONFIG = { API_URL: 'https://minha-api.com/v1/survey' };
```

## 🛠️ Desenvolvimento

### Desenvolvimento Local (Sem Docker)

```bash
cd src/web
python3 -m http.server 8000
```

Acesse: `http://localhost:8000/index.html?nome=Teste&email=teste@email.com`

### Hot Reload no Docker

O `docker-compose.dev.yml` monta os arquivos locais como volume:

```bash
docker-compose -f docker-compose.dev.yml up

# Edite os arquivos em src/web/
# As mudanças são refletidas imediatamente
```

## 📚 Documentação

- [Web Frontend](src/web/README.md) - Documentação detalhada do frontend
- [Variables Guide](.env.example) - Guia de variáveis de ambiente

## 🔐 Segurança

- ✅ Headers de segurança configurados no Nginx
- ✅ Prevenção de XSS
- ✅ Validação de email
- ⚠️ **IMPORTANTE**: Alterar senhas padrão em produção!

## 📝 TODO
Implementar API backend
- [ ] Adicionar autenticação
- [ ] Dashboard de analytics
- [ ] Testes automatizados
- [ ] CI/CD pipeline

## 📄 Licença

FIAP SOAT - Projeto Acadêmico
