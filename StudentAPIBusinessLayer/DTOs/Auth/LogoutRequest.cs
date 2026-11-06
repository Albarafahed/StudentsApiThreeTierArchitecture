using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAPIBusinessLayer.DTOs.Auth
{
    public class LogoutRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}
