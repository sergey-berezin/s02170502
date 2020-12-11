namespace LibraryContracts
{
    public class RecognizedImage
    {
        public string Path { get; set; }
        public string Image { get; set; }
        public string ClassLabel { get; set; }
        public float Probability { get; set; }
    }
}
