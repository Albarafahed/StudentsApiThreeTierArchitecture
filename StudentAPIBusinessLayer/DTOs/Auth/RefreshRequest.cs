using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAPIBusinessLayer.DTOs.Auth
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }=null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
