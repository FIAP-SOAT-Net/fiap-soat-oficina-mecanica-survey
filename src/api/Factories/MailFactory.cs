namespace Fiap.Soat.SmartMechanicalWorkshop.Survey.Api.Factories;

public static class MailFactory
{
    public static EmailDetails Create(string email, string clientName, string title, string surveyUrl)
    {
        var bodyHtml = BuildEmailHtml(clientName, surveyUrl, email);
        return new EmailDetails(email, $"{title} | Pesquisa de Satisfação", bodyHtml);
    }

    private static string BuildEmailHtml(string clientName, string surveyUrl, string email)
    {
        return $@"
<!DOCTYPE html>
<html lang='pt-BR'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Pesquisa de Satisfação</title>
</head>
<body style='margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;'>
    <table role='presentation' style='width: 100%; border-collapse: collapse;'>
        <tr>
            <td style='padding: 40px 20px;'>
                <table role='presentation' style='max-width: 600px; margin: 0 auto; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border-radius: 10px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);'>
                    <!-- Header -->
                    <tr>
                        <td style='padding: 40px 30px; text-align: center; color: white;'>
                            <h1 style='margin: 0; font-size: 28px; font-weight: bold;'>🔧 Oficina Mecânica</h1>
                            <p style='margin: 10px 0 0 0; font-size: 16px; opacity: 0.9;'>Smart Mechanical Workshop</p>
                        </td>
                    </tr>

                    <!-- Content -->
                    <tr>
                        <td style='background-color: white; padding: 40px 30px; border-radius: 0 0 10px 10px;'>
                            <h2 style='margin: 0 0 20px 0; color: #333; font-size: 24px;'>Olá, {clientName}! 👋</h2>

                            <p style='margin: 0 0 20px 0; color: #555; font-size: 16px; line-height: 1.6;'>
                                Esperamos que esteja satisfeito(a) com o serviço realizado em seu veículo!
                            </p>

                            <p style='margin: 0 0 20px 0; color: #555; font-size: 16px; line-height: 1.6;'>
                                Sua opinião é muito importante para nós e nos ajuda a melhorar continuamente
                                a qualidade dos nossos serviços. Por isso, gostaríamos de convidá-lo(a) a
                                responder uma breve pesquisa de satisfação.
                            </p>

                            <p style='margin: 0 0 30px 0; color: #555; font-size: 16px; line-height: 1.6;'>
                                <strong>Leva apenas 2 minutos!</strong> ⏱️
                            </p>

                            <!-- Button -->
                            <table role='presentation' style='margin: 0 auto;'>
                                <tr>
                                    <td style='border-radius: 5px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);'>
                                        <a href='{surveyUrl}'
                                           style='display: inline-block; padding: 15px 40px; color: white; text-decoration: none; font-size: 16px; font-weight: bold; border-radius: 5px;'>
                                            📋 Responder Pesquisa
                                        </a>
                                    </td>
                                </tr>
                            </table>

                            <p style='margin: 30px 0 0 0; color: #888; font-size: 14px; line-height: 1.6;'>
                                Caso o botão acima não funcione, copie e cole este link no seu navegador:<br>
                                <a href='{surveyUrl}' style='color: #667eea; word-break: break-all;'>{surveyUrl}/index.html?nome={clientName}&email={email}</a>
                            </p>
                        </td>
                    </tr>

                    <!-- Footer -->
                    <tr>
                        <td style='padding: 20px 30px; text-align: center; color: white; font-size: 12px;'>
                            <p style='margin: 0; opacity: 0.8;'>
                                Obrigado por confiar em nossos serviços! 🚗✨
                            </p>
                            <p style='margin: 10px 0 0 0; opacity: 0.7;'>
                                Oficina Mecânica - Excelência em manutenção automotiva
                            </p>
                        </td>
                    </tr>
                </table>

                <!-- Disclaimer -->
                <table role='presentation' style='max-width: 600px; margin: 20px auto 0;'>
                    <tr>
                        <td style='text-align: center; color: #666; font-size: 12px; line-height: 1.5;'>
                            <p style='margin: 0;'>
                                Este é um e-mail automático. Por favor, não responda esta mensagem.
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
    }
}

public record EmailDetails(string To, string Subject, string BodyHtml);
