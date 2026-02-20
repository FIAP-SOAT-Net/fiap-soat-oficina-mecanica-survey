# GitHub Actions Workflows

Este diretório contém os workflows de CI/CD do projeto.

## 📋 Workflows Disponíveis

### 1. webdeploy.yml - Web Deploy (Manual)

**Objetivo**: Build e publicação da imagem Docker do frontend no Docker Hub

**Tipo**: Manual (workflow_dispatch)

**Uso**:
1. Vá para Actions → Web Deploy
2. Clique em "Run workflow"
3. Configure os parâmetros:
   - **tag**: Versão da imagem (ex: `v1.0.0`)
   - **push_latest**: Também fazer push como `:latest`
4. Execute

**Quando usar**:
- Para publicar uma nova versão do frontend
- Após merge de features na main
- Para criar releases

**Saída**:
- Imagem Docker publicada no Docker Hub
- Tags: especificada + `:latest` (opcional)
- Suporte multi-plataforma (amd64 + arm64)

📖 [Guia completo de deploy](../../CICD.md)

### 2. dotnet.yml - .NET CI (Desabilitado)

**Status**: ⚠️ Desabilitado - projeto web não usa .NET

**Nota**: Este workflow foi mantido para referência de projetos futuros de API.

---

## 🔧 Configuração Necessária

### Variables (Settings → Secrets and variables → Actions → Variables)
- `DOCKERHUB_USERNAME`: Username do Docker Hub

### Secrets (Settings → Secrets and variables → Actions → Secrets)
- `DOCKERHUB_TOKEN`: Token de acesso do Docker Hub

## 📚 Recursos

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Workflow Syntax](https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions)
- [Docker Build Push Action](https://github.com/docker/build-push-action)
