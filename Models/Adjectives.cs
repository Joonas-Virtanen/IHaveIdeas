using SQLite.Net.Attributes;

namespace IHaveIdeas.Models
{
    public class Adjectives
    {
       [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string AdjectiveWord { get; set; }
    }
}
