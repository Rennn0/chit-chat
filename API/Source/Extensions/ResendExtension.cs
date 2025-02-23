using Resend;

public static class ResendExtension
{
    public static async Task SendTwoFactorToken(this IResend client,string to, string token)
    {
        await client.EmailSendAsync(new EmailMessage()
        {
            From = "guardian@resend.dev",
            To = to,
            Subject = "Two Factor Authentication",
            TextBody = token
        });
    }
}