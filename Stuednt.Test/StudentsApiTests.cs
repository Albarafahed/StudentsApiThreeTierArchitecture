
using Microsoft.IdentityModel.Tokens;
using Moq;
using Student.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Stuednt.Test
{
    public class StudentsApiTests : IClassFixture<StudentApiFactory>
    {
        private readonly HttpClient _client;
        private readonly StudentApiFactory _factory;

        public StudentsApiTests(StudentApiFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();

            _factory.StudentServiceMock.Reset();
        }

        private string GenerateToken(string role)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, "10"),
        new Claim(ClaimTypes.Role, role)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(StudentApiFactory.TestJwtSecret)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: "StudentApi",
                audience: "StudentApiUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private HttpRequestMessage CreateAuthenticatedRequest(
            string url,
            string role)
        {
            var token = GenerateToken(role);

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                url
            );

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer",
                    token
                );

            return request;
        }

        // GET Passed - Students Found
        [Fact]
        public async Task GetPassedStudents_StudentsExist_ReturnsOk()
        {
            // Arrange
            var students = new List<StudentDTO>
            {
                new StudentDTO(
                    1,
                    "Ahmed",
                    20,
                    90,
                    "ahmed@example.com",
                    "hashedPassword",
                    "Student"
                )
            };

            _factory.StudentServiceMock
                .Setup(x => x.GetPassedStudentsAsync())
                .ReturnsAsync(students);

            // Act
            var response = await _client.GetAsync(
                "/api/Students/Passed"
            );

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content
                .ReadFromJsonAsync<List<StudentDTO>>();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Ahmed", result[0].Name);

            _factory.StudentServiceMock.Verify(
                x => x.GetPassedStudentsAsync(),
                Times.Once
            );
        }
       
        // GET Passed - No Students Found
        [Fact]
        public async Task GetPassedStudents_NoStudents_ReturnsNotFound()
        {
            // Arrange
            _factory.StudentServiceMock
                .Setup(x => x.GetPassedStudentsAsync())
                .ReturnsAsync(new List<StudentDTO>());

            // Act
            var response = await _client.GetAsync(
                "/api/Students/Passed"
            );

            // Assert
            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode
            );

            _factory.StudentServiceMock.Verify(
                x => x.GetPassedStudentsAsync(),
                Times.Once
            );
        }

        // GET Average Grade
        [Fact]
        public async Task GetAverageGrade_ReturnsOk()
        {
            // Arrange
            _factory.StudentServiceMock
                .Setup(x => x.GetAverageGradeAsync())
                .ReturnsAsync(85.5);

            // Act
            var response = await _client.GetAsync(
                "/api/Students/AverageGrade"
            );

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            double result = await response.Content
                .ReadFromJsonAsync<double>();

            Assert.Equal(85.5, result);

            _factory.StudentServiceMock.Verify(
                x => x.GetAverageGradeAsync(),
                Times.Once
            );
        }


        [Fact]
        public async Task GetAllStudents_Admin_ReturnsOk()
        {
            // Arrange
            var students = new List<StudentDTO>
    {
        new StudentDTO(
            1, "Ahmed", 20, 90,
            "ahmed@example.com",
            "hashedPassword",
            "Student"
        )
    };

            _factory.StudentServiceMock
                .Setup(x => x.GetAllStudentsAsync())
                .ReturnsAsync(students);

            using var request = CreateAuthenticatedRequest(
                "/api/Students/All",
                "Admin"
            );

            // Act
            var response = await _client.SendAsync(request);

            Console.WriteLine(
                $"Request Authorization: {request.Headers.Authorization}"
            );

            Console.WriteLine(
                $"Response Status: {response.StatusCode}"
            );
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content
                .ReadFromJsonAsync<List<StudentDTO>>();

            Assert.NotNull(result);
            Assert.Single(result);

            _factory.StudentServiceMock.Verify(
                x => x.GetAllStudentsAsync(),
                Times.Once
            );
        }

        [Fact]
        public async Task GetAllStudents_NoStudents_ReturnsNotFound()
        {
            // Arrange
            _factory.StudentServiceMock
                .Setup(x => x.GetAllStudentsAsync())
                .ReturnsAsync(new List<StudentDTO>());

            using var request = CreateAuthenticatedRequest(
                "/api/Students/All",
                "Admin"
            );

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode
            );

            _factory.StudentServiceMock.Verify(
                x => x.GetAllStudentsAsync(),
                Times.Once
            );
        }

        [Fact]
        public async Task GetAllStudents_WithoutToken_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.GetAsync(
                "/api/Students/All"
            );

            // Assert
            Assert.Equal(
                HttpStatusCode.Unauthorized,
                response.StatusCode
            );

            _factory.StudentServiceMock.Verify(
                x => x.GetAllStudentsAsync(),
                Times.Never
            );
        }

        [Fact]
        public async Task GetAllStudents_StudentRole_ReturnsForbidden()
        {
            // Arrange
            using var request = CreateAuthenticatedRequest(
                "/api/Students/All",
                "Student"
            );

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode
            );

            _factory.StudentServiceMock.Verify(
                x => x.GetAllStudentsAsync(),
                Times.Never
            );
        }

    }
}
