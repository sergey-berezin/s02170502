namespace LibraryContracts
{
    public class DatabaseGet
    {
        public DatabaseGet(string label, int number)
        {
            ClassLabel = label;
            Number = number;
        }
        public string ClassLabel { get; set; }
        public int Number { get; set; }
    }
}
