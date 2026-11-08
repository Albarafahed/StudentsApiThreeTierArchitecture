using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student.Contracts;
using StudentAPIBusinessLayer;
using StudentAPIBusinessLayer.Interface;
using StudentDataAccessLayer;

//using StudentApi.Models;
//using StudentApi.DataSimulation;
using System.Collections.Generic;
using System.Security.Claims;


namespace StudentApi.Controllers
{
    [Authorize]
    [ApiController] // Marks the class as a Web API controller with enhanced features.
                    //  [Route("[controller]")] // Sets the route for this controller to "students", based on the controller name.
    [Route("api/Students")]

    public class StudentsController(IStudentService studentService) : ControllerBase // Declare the controller class inheriting from ControllerBase.
    {
        [Authorize(Roles = "Admin")] // Restrict access to this endpoint to users with the "Admin" role.
        [HttpGet("All", Name = "GetAllStudents")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudentsAsync() // Define a method to get all students.
        {
           

            List<StudentDTO> StudentsList = await studentService.GetAllStudentsAsync();
            if (StudentsList.Count == 0)
            {
                return NotFound("No Students Found!");
            }
            return Ok(StudentsList); // Returns the list of students.

        }

        [AllowAnonymous] // Allow access to this endpoint without authentication.
        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetPassedStudentsAsync()
        {
         
            List<StudentDTO> PassedStudentsList = await studentService.GetPassedStudentsAsync();
            if (PassedStudentsList.Count == 0)
            {
                return NotFound("No Students Found!");
            }

            return Ok(PassedStudentsList); // Return the list of students who passed.
        }

        [AllowAnonymous] // Allow access to this endpoint without authentication.
        [HttpGet("AverageGrade", Name = "GetAverageGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<double>> GetAverageGrade()
        {
            double averageGrade = await studentService.GetAverageGradeAsync();
            return Ok(averageGrade);
        }


        [HttpGet("{id}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<StudentDTO>> GetStudentByIdAsync(int id, [FromServices] IAuthorizationService authorizationService)
        {

            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }
          
          StudentDTO? studentDto = await studentService.FindAsync(id);

            if (studentDto == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }


            var authResult = await authorizationService.AuthorizeAsync(
                  User,
                  id,
                  "StudentOwnerOrAdmin");

            // If the policy evaluation failed, the user is authenticated
            // but not authorized to access this resource.
            // This correctly returns HTTP 403 Forbidden.
            if (!authResult.Succeeded)
                return Forbid(); // 403 Forbidden

            //here we get only the DTO object to send it back.
           

            //we return the DTO not the student object.
            return Ok(studentDto);

        }

        //for add new we use Http Post
        [Authorize(Roles = "Admin")] // Restrict access to this endpoint to users with the "Admin" role.
        [HttpPost(Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<StudentDTO>> AddStudentAsync(StudentDTO newStudentDTO)
        {
            //we validate the data here
            if (newStudentDTO == null || string.IsNullOrEmpty(newStudentDTO.Name) || newStudentDTO.Age < 0 || newStudentDTO.Grade < 0)
            {
                return BadRequest("Invalid student data.");
            }

            var createdStudent = await studentService.SaveAsync(
                newStudentDTO,
                StudentMode.AddNew);

            if (createdStudent == null)
                return StatusCode(500, "Failed to create student.");

            return CreatedAtRoute(
                "GetStudentById",
                new { id = createdStudent.Id },
                createdStudent);

        }


        [Authorize(Roles = "Admin")] // Restrict access to this endpoint to users with the "Admin" role.
        //here we use http put method for update
        [HttpPut("{id}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDTO>> UpdateStudentAsync(int id, StudentDTO updatedStudent)
        {
            if (id < 1 || updatedStudent == null || string.IsNullOrEmpty(updatedStudent.Name) || updatedStudent.Age < 0 || updatedStudent.Grade < 0)
            {
                return BadRequest("Invalid student data.");
            }


           var student = await studentService.FindAsync(id);

            if (student == null)
                return NotFound($"Student with ID {id} not found.");

            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.Grade = updatedStudent.Grade;
            student.Email = updatedStudent.Email;
            student.PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                updatedStudent.PasswordHash);
            student.Role = updatedStudent.Role;

            var result = await studentService.SaveAsync(
                student,
                StudentMode.Update);

            if (result == null)
                return StatusCode(500, "Failed to update student.");

            return Ok(result);
        }

        [Authorize(Roles = "Admin")] // Restrict access to this endpoint to users with the "Admin" role.
        //here we use HttpDelete method
        [HttpDelete("{id}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteStudentAsync(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            
            if (await studentService.DeleteStudentAsync(id))

                return Ok($"Student with ID {id} has been deleted.");
            else
                return NotFound($"Student with ID {id} not found. no rows deleted!");
        }


    }
}
