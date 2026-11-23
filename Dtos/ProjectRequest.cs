using TenantHub.Models;
using System.ComponentModel.DataAnnotations;
namespace TenantHub.Dtos
{
    public class ProjectRequest
    {
         [Required,MaxLength(100)]
         public string Name  {get;set;}=string.Empty;
        [MaxLength(500)]
        public string? Description{get;set;}
        public  ProjectStatus Status{get;set;}
    }
       
    }
