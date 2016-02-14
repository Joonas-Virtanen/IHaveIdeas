using SQLite.Net.Attributes;

namespace IHaveIdeas.Models
{
    public class Verbs
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string VerbWord { get; set; }
    }
}
