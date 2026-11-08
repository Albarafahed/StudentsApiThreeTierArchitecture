using Moq;
using Student.Contracts;

using StudentAPIBusinessLayer;
using StudentDataAccessLayer.Interface;

namespace Stuednt.Test
{
    public class StudentServiceTests
    {
        private readonly Mock<IStudentData> _studentDataMock;
        private readonly StudentService _studentService;

        public StudentServiceTests()
        {
            _studentDataMock = new Mock<IStudentData>();

            _studentService = new StudentService(
                _studentDataMock.Object
            );
        }

        // Helper
        private StudentDTO CreateStudent()
        {
            return new StudentDTO(
                10,
                "Ahmed",
                20,
                90,
                "ahmed@example.com",
                "hashedPassword",
                "Student"
            );
        }

        // 1. Save - Add New Student
        [Fact]
        public async Task SaveAsync_AddNewStudent_ReturnsStudentDTO()
        {
            // Arrange
            var studentDTO = CreateStudent();
            studentDTO.Id = 0;

            _studentDataMock
                .Setup(x => x.AddStudentAsync(It.IsAny<StudentDTO>()))
                .ReturnsAsync(10);

            // Act
            var result = await _studentService.SaveAsync(
                studentDTO,
                StudentMode.AddNew
            );

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Id);

            _studentDataMock.Verify(
                x => x.AddStudentAsync(It.IsAny<StudentDTO>()),
                Times.Once
            );
        }

        // 2. Save - Update Student Successfully
        [Fact]
        public async Task SaveAsync_UpdateStudent_ReturnsStudentDTO()
        {
            // Arrange
            var studentDTO = CreateStudent();

            _studentDataMock
                .Setup(x => x.UpdateStudentAsync(It.IsAny<StudentDTO>()))
                .ReturnsAsync(true);

            // Act
            var result = await _studentService.SaveAsync(
                studentDTO,
                StudentMode.Update
            );

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
            Assert.Equal("Ahmed", result.Name);

            _studentDataMock.Verify(
                x => x.UpdateStudentAsync(It.IsAny<StudentDTO>()),
                Times.Once
            );
        }

        // 3. Save - Update Student Failed
        [Fact]
        public async Task SaveAsync_UpdateStudent_ReturnsNull()
        {
            // Arrange
            var studentDTO = CreateStudent();

            _studentDataMock
                .Setup(x => x.UpdateStudentAsync(It.IsAny<StudentDTO>()))
                .ReturnsAsync(false);

            // Act
            var result = await _studentService.SaveAsync(
                studentDTO,
                StudentMode.Update
            );

            // Assert
            Assert.Null(result);

            _studentDataMock.Verify(
                x => x.UpdateStudentAsync(It.IsAny<StudentDTO>()),
                Times.Once
            );
        }

        // 4. Find - Student Found
        [Fact]
        public async Task FindAsync_StudentExists_ReturnsStudentDTO()
        {
            // Arrange
            var studentDTO = CreateStudent();

            _studentDataMock
                .Setup(x => x.GetStudentByIdAsync(10))
                .ReturnsAsync(studentDTO);

            // Act
            var result = await _studentService.FindAsync(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Id);

            _studentDataMock.Verify(
                x => x.GetStudentByIdAsync(10),
                Times.Once
            );
        }

        // 5. Find - Student Not Found
        [Fact]
        public async Task FindAsync_StudentDoesNotExist_ReturnsNull()
        {
            // Arrange
            _studentDataMock
                .Setup(x => x.GetStudentByIdAsync(100))
                .ReturnsAsync((StudentDTO?)null);

            // Act
            var result = await _studentService.FindAsync(100);

            // Assert
            Assert.Null(result);

            _studentDataMock.Verify(
                x => x.GetStudentByIdAsync(100),
                Times.Once
            );
        }

        // 6. Login - Student Found
        [Fact]
        public async Task LoginAsync_StudentExists_ReturnsStudentLoginDTO()
        {
            // Arrange
            var loginDTO = new StudentLoginDTO(
                10,
                "Ahmed",
                20,
                90,
                "ahmed@example.com",
                "hashedPassword",
                "Student",
                "refreshHash",
                DateTime.UtcNow.AddDays(7),
                null
            );

            _studentDataMock
                .Setup(x => x.LoginAsync("ahmed@example.com"))
                .ReturnsAsync(loginDTO);

            // Act
            var result = await _studentService.LoginAsync(
                "ahmed@example.com"
            );

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
            Assert.Equal("ahmed@example.com", result.Email);

            _studentDataMock.Verify(
                x => x.LoginAsync("ahmed@example.com"),
                Times.Once
            );
        }

        // 7. Login - Student Not Found
        [Fact]
        public async Task LoginAsync_StudentDoesNotExist_ReturnsNull()
        {
            // Arrange
            _studentDataMock
                .Setup(x => x.LoginAsync("unknown@example.com"))
                .ReturnsAsync((StudentLoginDTO?)null);

            // Act
            var result = await _studentService.LoginAsync(
                "unknown@example.com"
            );

            // Assert
            Assert.Null(result);

            _studentDataMock.Verify(
                x => x.LoginAsync("unknown@example.com"),
                Times.Once
            );
        }

        // 8. Update Refresh Token - Success
        [Fact]
        public async Task UpdateRefreshTokenAsync_Success_ReturnsTrue()
        {
            // Arrange
            int studentId = 10;
            string refreshTokenHash = "newRefreshHash";
            DateTime? expiresAt = DateTime.UtcNow.AddDays(7);

            _studentDataMock
                .Setup(x => x.UpdateRefreshTokenAsync(
                    studentId,
                    refreshTokenHash,
                    expiresAt))
                .ReturnsAsync(true);

            // Act
            var result = await _studentService.UpdateRefreshTokenAsync(
                studentId,
                refreshTokenHash,
                expiresAt
            );

            // Assert
            Assert.True(result);

            _studentDataMock.Verify(
                x => x.UpdateRefreshTokenAsync(
                    studentId,
                    refreshTokenHash,
                    expiresAt),
                Times.Once
            );
        }

        // 9. Update Refresh Token - Failed
        [Fact]
        public async Task UpdateRefreshTokenAsync_Failed_ReturnsFalse()
        {
            // Arrange
            int studentId = 10;
            string refreshTokenHash = "newRefreshHash";
            DateTime? expiresAt = DateTime.UtcNow.AddDays(7);

            _studentDataMock
                .Setup(x => x.UpdateRefreshTokenAsync(
                    studentId,
                    refreshTokenHash,
                    expiresAt))
                .ReturnsAsync(false);

            // Act
            var result = await _studentService.UpdateRefreshTokenAsync(
                studentId,
                refreshTokenHash,
                expiresAt
            );

            // Assert
            Assert.False(result);

            _studentDataMock.Verify(
                x => x.UpdateRefreshTokenAsync(
                    studentId,
                    refreshTokenHash,
                    expiresAt),
                Times.Once
            );
        }

        // 10. Get All Students
        [Fact]
        public async Task GetAllStudentsAsync_ReturnsStudentsList()
        {
            // Arrange
            var students = new List<StudentDTO>
            {
                CreateStudent(),
                new StudentDTO(
                    11,
                    "Ali",
                    21,
                    85,
                    "ali@example.com",
                    "hashedPassword",
                    "Student"
                )
            };

            _studentDataMock
                .Setup(x => x.GetAllStudentsAsync())
                .ReturnsAsync(students);

            // Act
            var result = await _studentService.GetAllStudentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            _studentDataMock.Verify(
                x => x.GetAllStudentsAsync(),
                Times.Once
            );
        }

        // 11. Get Passed Students
        [Fact]
        public async Task GetPassedStudentsAsync_ReturnsPassedStudents()
        {
            // Arrange
            var students = new List<StudentDTO>
            {
                CreateStudent()
            };

            _studentDataMock
                .Setup(x => x.GetPassedStudentsAsync())
                .ReturnsAsync(students);

            // Act
            var result = await _studentService.GetPassedStudentsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(90, result[0].Grade);

            _studentDataMock.Verify(
                x => x.GetPassedStudentsAsync(),
                Times.Once
            );
        }

        // 12. Get Average Grade
        [Fact]
        public async Task GetAverageGradeAsync_ReturnsAverageGrade()
        {
            // Arrange
            _studentDataMock
                .Setup(x => x.GetAverageGradeAsync())
                .ReturnsAsync(85.5);

            // Act
            var result = await _studentService.GetAverageGradeAsync();

            // Assert
            Assert.Equal(85.5, result);

            _studentDataMock.Verify(
                x => x.GetAverageGradeAsync(),
                Times.Once
            );
        }

        // 13. Delete Student - Success
        [Fact]
        public async Task DeleteStudentAsync_Success_ReturnsTrue()
        {
            // Arrange
            _studentDataMock
                .Setup(x => x.DeleteStudentAsync(10))
                .ReturnsAsync(true);

            // Act
            var result = await _studentService.DeleteStudentAsync(10);

            // Assert
            Assert.True(result);

            _studentDataMock.Verify(
                x => x.DeleteStudentAsync(10),
                Times.Once
            );
        }

        // 14. Delete Student - Failed
        [Fact]
        public async Task DeleteStudentAsync_Failed_ReturnsFalse()
        {
            // Arrange
            _studentDataMock
                .Setup(x => x.DeleteStudentAsync(10))
                .ReturnsAsync(false);

            // Act
            var result = await _studentService.DeleteStudentAsync(10);

            // Assert
            Assert.False(result);

            _studentDataMock.Verify(
                x => x.DeleteStudentAsync(10),
                Times.Once
            );
        }
    }
}