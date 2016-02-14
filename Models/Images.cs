using SQLite.Net.Attributes;

namespace IHaveIdeas.Models
{
    public class Images
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ImageNumber { get; set; }
    }
}
