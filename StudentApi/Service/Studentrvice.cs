namespace StudentApi.Service
{
    public class Studentrvice
    {
        public bool RegisterStudent(string name, int age)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            if (age < 18)
                return false;

            return true;
        }
    }
}