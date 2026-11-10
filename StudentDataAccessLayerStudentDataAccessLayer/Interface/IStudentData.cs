
using Student.Contracts;
using System;
using System.Collections.Generic;

namespace StudentDataAccessLayer.Interface
{
    public interface IStudentData
    {
       Task<int> AddStudentAsync(StudentDTO studentDTO);

        Task<bool> UpdateStudentAsync(StudentDTO studentDTO);

       Task<StudentDTO?> GetStudentByIdAsync(int studentId);

        Task<StudentLoginDTO?> LoginAsync(string email);

        Task<bool> UpdateRefreshTokenAsync(
           int studentId,
           string? refreshTokenHash,
           DateTime? expiresAt,
           DateTime? revokedAt);
        Task< List<StudentDTO> > GetAllStudentsAsync();

       Task< List<StudentDTO>> GetPassedStudentsAsync();

        Task<double> GetAverageGradeAsync();

       Task<bool> DeleteStudentAsync(int studentId);
    }
}
