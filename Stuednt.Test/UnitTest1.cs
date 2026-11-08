using StudentApi.Service;

namespace Stuednt.Test
{
    public class UnitTest1
    {
        private readonly Studentrvice studentService;
        public UnitTest1()
        {
            studentService = new Studentrvice();
        }
        [Fact]
        public void Add_TowNoumbers_ReturnsCorrectResult()
        {
            //Arrange
            int x = 20;
            int y = 30;
            //Act
            int result = x + y;
            //Assert
            Assert.Equal(50, result);


        }

        [Theory]
        [InlineData(20, 30, 50)]
        [InlineData(20, 33, 53)]
        [InlineData(100, 200, 300)]
        public void Add_ReutrnssCorrectResult(int number1, int number2, int expected)
        {
            //Arrange

            //Act
            int actual = number1 + number2;
            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RegisterStudent_ValidInput_ReturnsTrue()
        {
            // Arrange
            
            string name = "John Doe";
            int age = 20;
            // Act
            bool result = studentService.RegisterStudent(name, age);
            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("", 20)]
        [InlineData("Ahmed", 15)]
        [InlineData(null, 20)]
        public void RegisterStudent_WithInvalidData_ReturnsFalse(
            string name, int age)
        {
            // Act
            bool result = studentService.RegisterStudent(name, age);

            // Assert
            Assert.True(result);
        }
    }
}