using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public class StudentsController : ControllerBase // Declare the controller class inheriting from ControllerBase.
    {
        [Authorize(Roles = "Admin")] // Restrict access to this endpoint to users with the "Admin" role.
        [HttpGet("All", Name = "GetAllStudents")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents() // Define a method to get all students.
        {
           

            List<StudentDTO> StudentsList = StudentAPIBusinessLayer.Student.GetAllStudents();
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
        public ActionResult<IEnumerable<StudentDTO>> GetPassedStudents()
        {
         
            List<StudentDTO> PassedStudentsList = StudentAPIBusinessLayer.Student.GetPassedStudents();
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
        public ActionResult<double> GetAverageGrade()
        {
            double averageGrade = StudentAPIBusinessLayer.Student.GetAverageGrade();
            return Ok(averageGrade);
        }


        [HttpGet("{id}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<StudentDTO>> GetStudentById(int id, [FromServices] IAuthorizationService authorizationService)
        {

            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }
          
            StudentAPIBusinessLayer.Student ? student = StudentAPIBusinessLayer.Student.Find(id);

            if (student == null)
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
            StudentDTO SDTO = student.SDTO;

            //we return the DTO not the student object.
            return Ok(SDTO);

        }

        //for add new we use Http Post
        [Authorize(Roles = "Admin")] // Restrict access to this endpoint to users with the "Admin" role.
        [HttpPost(Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<StudentDTO> AddStudent(StudentDTO newStudentDTO)
        {
            //we validate the data here
            if (newStudentDTO == null || string.IsNullOrEmpty(newStudentDTO.Name) || newStudentDTO.Age < 0 || newStudentDTO.Grade < 0)
            {
                return BadRequest("Invalid student data.");
            }


            StudentAPIBusinessLayer.Student student = new StudentAPIBusinessLayer.Student(new StudentDTO(newStudentDTO.Id, newStudentDTO.Name, newStudentDTO.Age, newStudentDTO.Grade, newStudentDTO.Email, BCrypt.Net.BCrypt.HashPassword(newStudentDTO.PasswordHash)  , newStudentDTO.Role));
            student.Save();

            newStudentDTO.Id = student.ID;

            //we return the DTO only not the full student object
            //we dont return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetStudentById", new { id = newStudentDTO.Id }, newStudentDTO);

        }


        [Authorize(Roles = "Admin")] // Restrict access to this endpoint to users with the "Admin" role.
        //here we use http put method for update
        [HttpPut("{id}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDTO> UpdateStudent(int id, StudentDTO updatedStudent)
        {
            if (id < 1 || updatedStudent == null || string.IsNullOrEmpty(updatedStudent.Name) || updatedStudent.Age < 0 || updatedStudent.Grade < 0)
            {
                return BadRequest("Invalid student data.");
            }


            StudentAPIBusinessLayer.Student student = StudentAPIBusinessLayer.Student.Find(id);


            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }


            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.Grade = updatedStudent.Grade;
            student.Email = updatedStudent.Email;
            student.PasswordHash = BCrypt.Net.BCrypt.HashPassword( updatedStudent.PasswordHash);
            student.Role = updatedStudent.Role; 
            student.Save();

            //we return the DTO not the full student object.
            return Ok(student.SDTO);

        }

        [Authorize(Roles = "Admin")] // Restrict access to this endpoint to users with the "Admin" role.
        //here we use HttpDelete method
        [HttpDelete("{id}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteStudent(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            
            if (StudentAPIBusinessLayer.Student.DeleteStudent(id))

                return Ok($"Student with ID {id} has been deleted.");
            else
                return NotFound($"Student with ID {id} not found. no rows deleted!");
        }


    }
}
