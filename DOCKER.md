# 🚀 Guia Rápido - Docker

## Comandos Úteis

### 🏗️ Build

```bash
# Build da imagem web (a partir da raiz)
docker build -t smart-mechanical-workshop-survey-web -f src/web/Dockerfile src/web

# Ou a partir da pasta web
cd src/web
docker build -t smart-mechanical-workshop-survey-web .

# Build com tag específica
docker build -t smart-mechanical-workshop-survey-web:1.0.0 -f src/web/Dockerfile src/web
```

### 🐳 Desenvolvimento

```bash
# Subir apenas web (desenvolvimento)
docker-compose -f docker-compose.dev.yml up web

# Subir em background
docker-compose -f docker-compose.dev.yml up -d

# Ver logs em tempo real
docker-compose -f docker-compose.dev.yml logs -f web

# Rebuild e subir
docker-compose -f docker-compose.dev.yml up --build

# Parar tudo
docker-compose -f docker-compose.dev.yml down

# Parar e remover volumes
docker-compose -f docker-compose.dev.yml down -v
```

### 🏭 Produção

```bash
# Subir stack completo
docker-compose up -d

# Ver status dos containers
docker-compose ps

# Ver logs
docker-compose logs -f

# Restart serviço específico
docker-compose restart web

# Parar tudo
docker-compose down
```

### 🔍 Debugging

```bash
# Entrar no container
docker exec -it smart-mechanical-workshop-survey-web-dev sh

# Ver logs do nginx
docker exec smart-mechanical-workshop-survey-web-dev cat /var/log/nginx/access.log
docker exec smart-mechanical-workshop-survey-web-dev cat /var/log/nginx/error.log

# Verificar env-config.js gerado
docker exec smart-mechanical-workshop-survey-web-dev cat /usr/share/nginx/html/env-config.js

# Testar health check
curl http://localhost:8080/health

# Ver configuração do nginx
docker exec smart-mechanical-workshop-survey-web-dev cat /etc/nginx/conf.d/default.conf

# Ver estrutura de arquivos no container
docker exec smart-mechanical-workshop-survey-web-dev ls -la /usr/share/nginx/html
```

### 🧪 Testes

```bash
# Teste rápido local
cd src/web
python3 -m http.server 8000
# Acesse: http://localhost:8000/index.html?nome=Teste&email=teste@email.com

# Teste com Docker
docker run -d -p 8080:80 \
  -e API_URL=http://localhost:3000/api/survey \
  --name test-web \
  smart-mechanical-workshop-survey-web

# Verificar configuração
curl http://localhost:8080/env-config.js

# Cleanup
docker stop test-web && docker rm test-web
```

### 📦 Docker Hub

```bash
# Login
docker login

# Tag para Docker Hub
docker tag smart-mechanical-workshop-survey-web:latest \
  smartmechanicalworkshop/smart-mechanical-workshop-survey-web:latest

docker tag smart-mechanical-workshop-survey-web:latest \
  smartmechanicalworkshop/smart-mechanical-workshop-survey-web:1.0.0

# Push
docker push smartmechanicalworkshop/smart-mechanical-workshop-survey-web:latest
docker push smartmechanicalworkshop/smart-mechanical-workshop-survey-web:1.0.0

# Pull (para testar)
docker pull smartmechanicalworkshop/smart-mechanical-workshop-survey-web:latest
```

### 🧹 Limpeza

```bash
# Remover containers parados
docker container prune

# Remover imagens não utilizadas
docker image prune

# Remover tudo (cuidado!)
docker system prune -a

# Remover volumes não utilizados
docker volume prune
```

### 🔧 Troubleshooting

```bash
# Container não inicia?
docker-compose -f docker-compose.dev.yml logs web

# Porta já em uso?
lsof -i :8080
# ou mude a porta no docker-compose:
# ports:
#   - "9090:80"

# Rebuild forçado
docker-compose -f docker-compose.dev.yml build --no-cache web
docker-compose -f docker-compose.dev.yml up -d

# Ver uso de recursos
docker stats

# Inspecionar container
docker inspect smart-mechanical-workshop-survey-web-dev
```

### ⚙️ Variáveis de Ambiente

```bash
# Passar variável no comando
docker run -p 8080:80 \
  -e API_URL=https://api.exemplo.com/survey \
  smart-mechanical-workshop-survey-web

# Com docker-compose
API_URL=https://api.exemplo.com/survey docker-compose up

# Usando arquivo .env
cp .env.example .env
# Edite o .env com suas configurações
docker-compose up
```

## 📋 Configurar `API_URL` correta
- [ ] Build da imagem: `docker build -t smart-mechanical-workshop-survey-web -f src/web/Dockerfile src/web
- [ ] Alterar senhas padrão do banco de dados
- [ ] Configurar `API_URL` correta
- [ ] Build da imagem: `docker build -t smart-mechanical-workshop-survey-web .`
- [ ] Tag da imagem: `docker tag ... smartmechanicalworkshop/...`
- [ ] Push para Docker Hub: `docker push ...`
- [ ] No servidor, criar `.env` de produção
- [ ] Pull da imagem: `docker-compose pull`
- [ ] Subir stack: `docker-compose up -d`
- [ ] Verificar health: `curl http://localhost/health`
- [ ] Testar aplicação web
- [ ] Verificar logs: `docker-compose logs`

## 🌐 URLs de Acesso

### Desenvolvimento
- Web: http://localhost:8080
- Health: http://localhost:8080/health
- Env Config: http://localhost:8080/env-config.js
- Exemplo: http://localhost:8080/index.html?nome=João&email=joao@email.com

### Produção
- Web: http://seu-dominio.com
- Health: http://seu-dominio.com/health
- Exemplo: http://seu-dominio.com/index.html?nome=João&email=joao@email.com

## 💡 Dicas

1. **Hot Reload**: Em dev, os arquivos são montados como volume. Edite e recarregue o navegador.

2. **Logs em Tempo Real**: Use `-f` para seguir os logs:
   ```bash
   docker-compose logs -f web
   ```

3. **Múltiplos Ambientes**: Use arquivos compose diferentes:
   ```bash
   docker-compose -f docker-compose.dev.yml up      # Dev
   docker-compose -f docker-compose.staging.yml up  # Staging
   docker-compose up                                 # Prod
   ```

4. **Backup do Banco** (quando implementado):
   ```uild Local**: Para testar o build:
   ```bash
   cd src/web
   docker build -t test-web .
   docker run -d -p 8080:80 test-web
