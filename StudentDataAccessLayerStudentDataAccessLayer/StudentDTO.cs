using System;
using System.Data;


namespace StudentDataAccessLayer
{
    public class StudentDTO
    {
        public StudentDTO(int id, string name, int age, int grade, string email, string passwordHash, string role)
        {
            this.Id = id;
            this.Name = name;
            this.Age = age;
            this.Grade = grade;
            this.Email = email;
            this.PasswordHash = passwordHash;
            this.Role = role;
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }

        // Authentication-related fields
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

      
    }

   
}
