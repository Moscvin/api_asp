namespace Common.Models
{
    public class Book : MongoDocument
    {
        public string Name { get; set; }
        public List<string> Author { get; set; } = new();
        public int Year { get; set; }
        public string Genre { get; set; }
    }
}
