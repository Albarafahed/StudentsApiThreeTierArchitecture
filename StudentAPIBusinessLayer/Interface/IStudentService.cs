using Student.Contracts;
using StudentDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAPIBusinessLayer.Interface
{
    public interface IStudentService
    {
       Task<StudentDTO?> SaveAsync(StudentDTO studentDTO, StudentMode mode = StudentMode.AddNew);
       Task< StudentDTO?> FindAsync(int studentId);
       Task< StudentLoginDTO?> LoginAsync(string email);
       Task< bool> UpdateRefreshTokenAsync(
           int studentId,
           string refreshTokenHash,
           DateTime? expiresAt);
       Task<List<StudentDTO>> GetAllStudentsAsync();
        Task<List<StudentDTO>> GetPassedStudentsAsync();
       Task< bool> DeleteStudentAsync(int studentId);
       Task< double> GetAverageGradeAsync();
    }
}
