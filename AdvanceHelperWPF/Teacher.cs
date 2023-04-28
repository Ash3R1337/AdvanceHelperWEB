namespace AdvanceHelperWPF
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public string Subdivision { get; set; }
        public string ImagePath { get; set; }
        public string WorkExp { get; set; }
        public string Specialization { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public Teacher(int id, string name, string birthDate, string subdivision, string imagePath, string workExp, string specialization, string phone, string email)
        {
            Id = id;
            Name = name;
            BirthDate = birthDate;
            Subdivision = subdivision;
            ImagePath = imagePath;
            WorkExp = workExp;
            Specialization = specialization;
            Phone = phone;
            Email = email;
        }
    }
}