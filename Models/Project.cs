namespace TenantHub.Models
{
    public class Project
    {
        public int Id{get;set;}

        public int TenantId{get;set;}
         public Tenant Tenant { get; set; } = null!; 

        public string Name {get;set;}=string.Empty;
        public string? Description{get;set;}
        public int CreatedByUserId{get;set;}
         public DateTime CreatedDate{get;set;}= DateTime.UtcNow;
        public  ProjectStatus Status{get;set;}
    }
        public enum ProjectStatus
        {
            Planned, 
            Active, 
            Completed
        }


    }
