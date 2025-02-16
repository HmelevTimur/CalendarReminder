using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace CalendarReminder.Application.Services.Implementations;

public class FirebaseNotificationService
{
    public FirebaseNotificationService(IConfiguration configuration)
    {
        var credentialPath = configuration["Firebase:CredentialPath"];

        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(credentialPath)
            });
        }
    }

    public async Task<string?> SendPushNotificationAsync(string deviceToken, string title, string body)
    {
        var message = new Message()
        {
            Token = deviceToken,
            Notification = new Notification()
            {
                Title = title,
                Body = body
            }
        };

        try
        {
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            Console.WriteLine($"Уведомление отправлено: {response}");
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отправке уведомления: {ex.Message}");
            return null;
        }
    }
}