using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Email;
//Todo: create an interface
public class EmailSender
{
    private readonly IConfiguration _config;

    public EmailSender(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailSendGridAsync(string userEmail, string emailSubject, string msg)
    {
        var client = new SendGridClient("SendGrid:Key");
        var message = new SendGridMessage
        {
            From = new EmailAddress("luiscordobahoyos@hotmail.com","SendGrid:User"),
            Subject = emailSubject,
            PlainTextContent = msg,
            HtmlContent = msg,
        };
        message.AddTo(new EmailAddress(userEmail));
        message.SetClickTracking(false, false);
        
        var response = await client.SendEmailAsync(message);
        if (!response.IsSuccessStatusCode)
            throw new Exception(response.Body.ToString());

    }

    public async Task SendEmailSendInBlueAsync(string userEmail, string name, string emailSubject, string msg)
    {
        if(!Configuration.Default.ApiKey.ContainsKey("api-key"))
            Configuration.Default.ApiKey.Add("api-key",_config["SendInBlue:Key"]);

        var apiInstance = new TransactionalEmailsApi();
        var sendSmtpEmail = new SendSmtpEmail
        {
            To = new List<SendSmtpEmailTo>
            {
                new SendSmtpEmailTo(userEmail, name)
            },
            HtmlContent = msg,
            TextContent = msg,
            Subject = emailSubject,
            Sender = new SendSmtpEmailSender("Reactivities", "luiscordobahoyos@gmail.com")
        };
        try
        {
            CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}