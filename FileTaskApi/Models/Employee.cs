using System.ComponentModel.DataAnnotations;

namespace FileTaskApi.Models
{
    public class Employee
    {
        [Key]
        [Required]
        
        public int Id { get; set; }
        public string employeeName { get; set; }
        public string mobileNumber { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string netSalary { get; set; }
        public string grossSalary { get; set; }
        public string gender { get; set; }
    }
}
