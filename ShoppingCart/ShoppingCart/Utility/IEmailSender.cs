namespace ShoppingCart.Utility
{
    public interface IEmailSender
    {
        void SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
