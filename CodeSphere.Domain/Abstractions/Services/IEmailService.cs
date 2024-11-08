namespace CodeSphere.Domain.Abstractions.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string email, string message, string? reason);

    }
}
