using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDataAccessLayer
{
    public class StudentLoginDTO
    {
        public StudentLoginDTO(int id, string name, int age, int grade, string email, string passwordHash, string role, string refreshTokenHash, DateTime? refreshTokenExpiresAt, DateTime? refreshTokenRevokedAt)
        {
            this.Id = id;
            this.Name = name;
            this.Age = age;
            this.Grade = grade;
            this.Email = email;
            this.PasswordHash = passwordHash;
            this.Role = role;
            this.RefreshTokenHash = refreshTokenHash;
            this.RefreshTokenExpiresAt = refreshTokenExpiresAt;
            this.RefreshTokenRevokedAt = refreshTokenRevokedAt;
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }

        // Authentication-related fields
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        //
        public string RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public DateTime? RefreshTokenRevokedAt { get; set; }
    }
}
