using SQLite.Net.Attributes;

namespace IHaveIdeas.Models
{
    public  class Nouns
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NounWord { get; set; }
    }
}
