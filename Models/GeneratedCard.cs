using SQLite.Net.Attributes;

namespace IHaveIdeas.Models
{
    public class GeneratedCards
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string VerbWord { get; set; }
        public string AdjectiveWord { get; set; }
        public string NounWord { get; set; }
        public int Image { get; set; }
    }
}
