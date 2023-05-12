namespace AdvanceHelperWPF
{
    public class Certificate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }

        public Certificate(int id, string name, string filePath)
        {
            Id = id;
            Name = name;
            FilePath = filePath;
        }
    }
}
