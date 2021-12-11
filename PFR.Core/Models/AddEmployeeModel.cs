namespace PFR.Core.Models
{
    public class AddEmployeeModel
    {
        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string Department { get; set; }

        public string Division { get; set; }

        public string Position { get; set; }

        public int Code { get; set; }
    }
}