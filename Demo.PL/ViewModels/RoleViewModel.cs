using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class RoleViewModel
    {
        public string RoleId { get; set; }

        [Required(ErrorMessage = "RoleName is Required")]
        public string RoleName { get; set; }

        public RoleViewModel()
        {
            RoleId = Guid.NewGuid().ToString();
        }

    }
}
