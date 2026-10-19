namespace TenantHub.Dtos{
    public class LoginRequest{
        public string Email {get;set;}=string.Empty;
        public string Password {get;set;}=string.Empty;
        public string CompanySlug {get;set;}=string.Empty;

    }
}