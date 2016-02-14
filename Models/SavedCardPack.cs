using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHaveIdeas.Models
{
   public class SavedCards
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string VerbWord { get; set; }
        public string AdjectiveWord { get; set; }
        public string NounWord { get; set; }
        public int Image { get; set; }
        public int CardNumber { get; set; }
        public int PackNumber { get; set; }
        
    }
}
