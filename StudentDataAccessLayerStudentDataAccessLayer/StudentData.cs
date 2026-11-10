using Microsoft.Data.SqlClient;
using Student.Contracts;
using StudentDataAccessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace StudentDataAccessLayer
{
    public class StudentData : IStudentData
    {
        static string _connectionString = "Server=localhost\\SQLEXPRESS;Database=StudentsDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public async Task<List<StudentDTO>> GetAllStudentsAsync()
        {
            var StudentsList = new List<StudentDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                   await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            StudentsList.Add(new StudentDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade")),
                              reader.IsDBNull(reader.GetOrdinal("Email"))
    ?                       string.Empty
                            : reader.GetString(reader.GetOrdinal("Email")),

                        reader.IsDBNull(reader.GetOrdinal("PasswordHash"))
                            ? string.Empty
                            : reader.GetString(reader.GetOrdinal("PasswordHash")),

                        reader.IsDBNull(reader.GetOrdinal("Role"))
                            ? string.Empty
                            : reader.GetString(reader.GetOrdinal("Role"))
                                                    ));
                        }
                    }
                }


                return StudentsList;
            }

        }

        public async Task<List<StudentDTO>> GetPassedStudentsAsync()
        {
            var StudentsList = new List<StudentDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetPassedStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            StudentsList.Add(new StudentDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade")),
                                reader.GetString(reader.GetOrdinal("Email")),
                                reader.GetString(reader.GetOrdinal("PasswordHash")),
                                reader.GetString(reader.GetOrdinal("Role"))
                            ));
                        }
                    }
                }


                return StudentsList;
            }

        }

        public async Task<double> GetAverageGradeAsync()
        {
            double averageGrade = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAverageGrade", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();

                    object ? result = await cmd.ExecuteScalarAsync();
                    if (result != DBNull.Value)
                    {
                        averageGrade = Convert.ToDouble(result);
                    }
                    else
                        averageGrade = 0;

                }
            }

            return averageGrade;
        }

        public async Task<StudentDTO?> GetStudentByIdAsync(int studentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_GetStudentById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StudentId", studentId);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new StudentDTO
                       (
                           reader.GetInt32(reader.GetOrdinal("Id")),
                           reader.GetString(reader.GetOrdinal("Name")),
                           reader.GetInt32(reader.GetOrdinal("Age")),
                           reader.GetInt32(reader.GetOrdinal("Grade")),
                          reader.GetString(reader.GetOrdinal("Email")),
                            reader.GetString(reader.GetOrdinal("PasswordHash")),
                             reader.GetString(reader.GetOrdinal("Role"))
                       );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

      
        public async Task<int> AddStudentAsync(StudentDTO StudentDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
               
            using (var command = new SqlCommand("SP_AddStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Name", StudentDTO.Name);
                command.Parameters.AddWithValue("@Age", StudentDTO.Age);
                command.Parameters.AddWithValue("@Grade", StudentDTO.Grade);
                command.Parameters.AddWithValue("@Email", StudentDTO.Email);
                command.Parameters.AddWithValue("@PasswordHash", StudentDTO.PasswordHash);
                command.Parameters.AddWithValue("@Role", StudentDTO.Role);
                var outputIdParam = new SqlParameter("@NewStudentId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();

                return (int)outputIdParam.Value;
            }
        }

        public async Task<bool> UpdateStudentAsync(StudentDTO StudentDTO)
        {
            try
            {

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("SP_UpdateStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@StudentId", StudentDTO.Id);
                    command.Parameters.AddWithValue("@Name", StudentDTO.Name);
                    command.Parameters.AddWithValue("@Age", StudentDTO.Age);
                    command.Parameters.AddWithValue("@Grade", StudentDTO.Grade);
                    command.Parameters.AddWithValue("@Email", StudentDTO.Email);
                    command.Parameters.AddWithValue("@PasswordHash", StudentDTO.PasswordHash);
                    command.Parameters.AddWithValue("@Role", StudentDTO.Role);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                    return true;

                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            try
            {

                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("SP_DeleteStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentId", studentId);

                   await connection.OpenAsync();

                    return Convert.ToInt32(await command.ExecuteScalarAsync()) == 1;


                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<StudentLoginDTO?> LoginAsync(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_Login", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", email);
              
              await  connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new StudentLoginDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("Name")),
                            reader.GetInt32(reader.GetOrdinal("Age")),
                            reader.GetInt32(reader.GetOrdinal("Grade")),
                          reader.GetString(reader.GetOrdinal("Email")),
                            reader.GetString(reader.GetOrdinal("PasswordHash")),
                             reader.GetString(reader.GetOrdinal("Role")),
                             reader.IsDBNull(reader.GetOrdinal("RefreshTokenHash")) ? null : reader.GetString(reader.GetOrdinal("RefreshTokenHash")),
                                reader.IsDBNull(reader.GetOrdinal("RefreshTokenExpiresAt")) ? null : reader.GetDateTime(reader.GetOrdinal("RefreshTokenExpiresAt")),
                                reader.IsDBNull(reader.GetOrdinal("RefreshTokenRevokedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("RefreshTokenRevokedAt"))
                        );
                    }
                    else
                    {
                        return null;
                    }   
                }
            }
        }

        public async Task<bool> UpdateRefreshTokenAsync(
            int studentId,
            string? refreshTokenHash,
            DateTime? expiresAt,
            DateTime? revokedAt)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_UpdateRefreshToken", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@StudentId", SqlDbType.Int).Value = studentId;

                command.Parameters.Add("@RefreshTokenHash", SqlDbType.NVarChar, 255)
                    .Value = (object?)refreshTokenHash ?? DBNull.Value;

                command.Parameters.Add("@ExpiresAt", SqlDbType.DateTime2)
                    .Value = (object?)expiresAt ?? DBNull.Value;

                command.Parameters.Add("@RevokedAt", SqlDbType.DateTime2)
                    .Value = (object?)revokedAt ?? DBNull.Value;

                await connection.OpenAsync();

                object? result = await command.ExecuteScalarAsync();

                return result != null &&
                       result != DBNull.Value &&
                       Convert.ToInt32(result) > 0;
            }
        }
    }
}
