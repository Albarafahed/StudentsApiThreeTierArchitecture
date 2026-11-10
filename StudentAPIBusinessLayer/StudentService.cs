
using Student.Contracts;
using StudentAPIBusinessLayer.Interface;
using StudentDataAccessLayer;
using StudentDataAccessLayer.Interface;


namespace StudentAPIBusinessLayer
{
    public class StudentService(IStudentData studentData)
        : IStudentService
    {
        // Add New Student
        private async Task< StudentDTO?> AddNewStudentAsync(StudentDTO studentDTO)
        {
            int newId = await studentData.AddStudentAsync(studentDTO);

            if (newId == -1)
                return null;

            studentDTO.Id = newId;

            return studentDTO;
        }

        // Update Existing Student
        private async Task< StudentDTO?> UpdateStudentAsync(StudentDTO studentDTO)
        {
            bool updated = await studentData.UpdateStudentAsync(studentDTO);

            return updated ? studentDTO : null;
        }

        // Save Student
        public async Task<StudentDTO?> SaveAsync(
            StudentDTO  studentDTO,
            StudentMode mode = StudentMode.AddNew)
        {
            return mode switch
            {
                StudentMode.AddNew => await AddNewStudentAsync(studentDTO),

                StudentMode.Update => await UpdateStudentAsync(studentDTO),

                _ => null
            };
        }

        // Find Student By ID
        public async Task< StudentDTO?> FindAsync(int studentId)
        {
            return await studentData.GetStudentByIdAsync(studentId);
        }

        // Login
        public async Task<StudentLoginDTO?> LoginAsync(string email)
        {
            return await studentData.LoginAsync(email);
        }

        // Update Refresh Token
        public async Task<bool> UpdateRefreshTokenAsync(
    int studentId,
    string? refreshTokenHash,
    DateTime? expiresAt,
    DateTime? revokedAt)
        {
            return await studentData.UpdateRefreshTokenAsync(
                studentId,
                refreshTokenHash,
                expiresAt,
                revokedAt);
        }

        // Get All Students
        public async Task<List<StudentDTO>> GetAllStudentsAsync()
        {
            return await studentData.GetAllStudentsAsync();
        }

        // Get Passed Students
        public async Task<List<StudentDTO>> GetPassedStudentsAsync()
        {
            return await studentData.GetPassedStudentsAsync();
        }

        // Get Average Grade
        public async Task<double> GetAverageGradeAsync()
        {
            return await studentData.GetAverageGradeAsync();
        }

        // Delete Student
        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            return await studentData.DeleteStudentAsync(studentId);
        }
    }
}
