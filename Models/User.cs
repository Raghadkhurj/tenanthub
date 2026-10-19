namespace TenantHub.Models;

    public class User{
       public int Id{get;set;}

        public int TenantId{get;set;}
        public Tenant Tenant { get; set; } = null!; 

        public string Email{get;set;}=string.Empty;
        public string PasswordHash{get;set;}=string.Empty;
         public DateTime CreatedDate{get;set;}= DateTime.UtcNow;

    }
