namespace AdvanceHelperWPF
{
    public class Teacher
    {
        public string ImagePath { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public Teacher(string name, int id, string imagePath)
        {
            Name = name;
            Id = id;
            ImagePath = imagePath;
        }
    }
}