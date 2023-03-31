namespace AdvanceHelperWPF
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public string Subdivision { get; set; }
        public string ImagePath { get; set; }

        public Teacher(int id, string name, string birthDate, string subdivision, string imagePath)
        {
            Id = id;
            Name = name;
            BirthDate = birthDate;
            Subdivision = subdivision;
            ImagePath = imagePath;
        }
    }
}