using Demo.DAL.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(50, ErrorMessage = "Max length is 50 chars.")]
        [MinLength(5, ErrorMessage = "Min length is 5 chars.")]
        public string Name { get; set; }

        [Range(22, 35, ErrorMessage = "Age must be in range from 22 to 35")]
        public int? Age { get; set; }

        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}",
            ErrorMessage = "Address must be like 123-Streat-City-Country")]
        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        [InverseProperty("Employees")]
        public Department Department { get; set; }
    }
}
