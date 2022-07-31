using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement_WebAPI.Data.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

