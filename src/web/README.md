# Site Estático - Pesquisa de Satisfação

## 📋 Descrição

Site estático de pesquisa de satisfação para clientes da oficina mecânica que acabaram de receber seus veículos após serviço prestado.

## ✨ Funcionalidades

### Implementadas
- ✅ 5 perguntas obrigatórias com avaliação de 0 a 5
- ✅ Recebe nome e email do cliente via parâmetros URL
- ✅ Validação de todas as perguntas obrigatórias
- ✅ Chamada HTTP POST para endpoint `/api/survey`
- ✅ Campo de comentários opcional (até 500 caracteres)
- ✅ Indicador de progresso visual
- ✅ Design responsivo (mobile-first)
- ✅ Feedback visual ao usuário (loading, sucesso, erro)
- ✅ Validação de formato de email
- ✅ Sanitização de inputs para prevenir XSS
- ✅ Timeout de 10 segundos para requisições
- ✅ Prevenção de múltiplos envios

### Perguntas da Pesquisa
1. 🔧 Qualidade do serviço realizado no veículo
2. 👥 Atendimento e cordialidade da equipe
3. ⏱️ Prazo de entrega do veículo
4. 💰 Relação custo-benefício do serviço
5. ✨ Limpeza e cuidado com o veículo

## 🚀 Como Usar

### Acessar o Site

O site deve ser acessado com parâmetros na URL:

```
index.html?nome=João Silva&email=joao@email.com
```

**Parâmetros obrigatórios:**
- `nome`: Nome do cliente
- `email`: Email do cliente

### Exemplo de URL Completa

```
http://localhost/index.html?nome=Maria%20Santos&email=maria.santos@exemplo.com.br
```

### Testar Localmente

1. Navegue até a pasta `src/web`:
   ```bash
   cd src/web
   ```

2. Inicie um servidor HTTP simples:
   ```bash
   # Python 3
   python3 -m http.server 8000

   # ou Python 2
   python -m SimpleHTTPServer 8000

   # ou Node.js (se tiver http-server instalado)
   npx http-server -p 8000
   ```

3. Acesse no navegador:
   ```
   http://localhost:8000/index.html?nome=Teste&email=teste@email.com
   ```

## 📡 API Endpoint

### Endpoint Configurado

- **URL**: `/api/survey`
- **Método**: `POST`
- **Content-Type**: `application/json`
- **Timeout**: 10 segundos

### Estrutura do Payload

```json
{
  "customerName": "João Silva",
  "customerEmail": "joao@email.com",
  "responses": {
    "question1": 5,
    "question2": 4,
    "question3": 5,
    "question4": 4,
    "question5": 5
  },
  "comments": "Excelente atendimento!",
  "submittedAt": "2026-02-20T10:30:00.000Z"
}
```

### Campos do Payload

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `customerName` | string | Nome do cliente (da URL) |
| `customerEmail` | string | Email do cliente (da URL) |
| `responses` | object | Objeto com as respostas (question1 a question5) |
| `responses.question[1-5]` | number | Avaliação de 0 a 5 |
| `comments` | string | Comentários opcionais (máx. 500 caracteres) |
| `submittedAt` | string | Data/hora do envio (ISO 8601) |

### Alterar o Endpoint

Para alterar o endpoint da API, edite o arquivo `script.js`:

```javascript
const CONFIG = {
    API_ENDPOINT: 'https://sua-api.com/survey', // Altere aqui
    API_TIMEOUT: 10000,
    TOTAL_QUESTIONS: 5
};
```

## 🎨 Design

### Cores Principais
- **Primary**: #2563eb (Azul)
- **Success**: #10b981 (Verde)
- **Error**: #ef4444 (Vermelho)
- **Warning**: #f59e0b (Laranja)

### Responsividade
- **Desktop**: > 768px
- **Tablet**: 481px - 768px
- **Mobile**: ≤ 480px

### Acessibilidade
- Labels adequadas para screen readers
- Navegação por teclado
- Contraste de cores WCAG AA
- Mensagens de erro descritivas

## 🔒 Segurança

### Implementado
- Sanitização de inputs da URL (prevenção XSS)
- Validação de formato de email
- Prevenção de múltiplos envios
- Timeout de requisições
- Tratamento de erros robusto

## 📁 Estrutura de Arquivos

```
src/web/
├── index.html      # Estrutura HTML da pesquisa
├── styles.css      # Estilos e design responsivo
├── script.js       # Lógica e interações
└── README.md       # Esta documentação
```

## 🧪 Testando sem API

O site funciona normalmente mesmo sem a API implementada. Ao enviar o formulário:

1. **Se a API não existir**: Mostrará mensagem de erro com opção de retry
2. **Console do navegador**: Mostrará os dados que seriam enviados
   ```javascript
   console.log('Survey submitted successfully:', result);
   ```

## 🔧 Personalização

### Adicionar/Remover Perguntas

1. Edite `index.html` para adicionar novos blocos de pergunta
2. Atualize `CONFIG.TOTAL_QUESTIONS` em `script.js`
3. Ajuste o loop de validação conforme necessário

### Alterar Estilos

Edite as variáveis CSS em `styles.css`:

```css
:root {
    --primary-color: #2563eb;
    --primary-dark: #1e40af;
    /* ... outras variáveis */
}
```

## 📱 Compatibilidade

### Navegadores Suportados
- ✅ Chrome/Edge (últimas 2 versões)
- ✅ Firefox (últimas 2 versões)
- ✅ Safari (últimas 2 versões)
- ✅ Opera (últimas 2 versões)

### Tecnologias Utilizadas
- HTML5
- CSS3 (Flexbox, Grid, CSS Variables)
- JavaScript ES6+ (Promises, Async/Await, Fetch API)

## 🚨 Tratamento de Erros

O site trata os seguintes cenários:

1. **Parâmetros ausentes na URL**: Exibe mensagem informativa
2. **Email inválido**: Validação de formato
3. **Perguntas não respondidas**: Destaque visual e scroll
4. **Erro na requisição**: Mensagem específica com botão de retry
5. **Timeout**: Aviso de tempo excedido
6. **Sem conexão**: Alerta de problema de conexão

## 📝 Próximos Passos

- [ ] Implementar a API backend em `/api`
- [ ] Configurar CORS na API
- [ ] Adicionar analytics/tracking
- [ ] Implementar testes automatizados
- [ ] Adicionar suporte a múltiplos idiomas

## 📄 Licença

Este projeto faz parte do FIAP SOAT.
