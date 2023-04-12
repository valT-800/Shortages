using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shortages.Models
{
    public class ShortageModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Room { get; set; }
        public string Category { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UserId { get; set; }



    }
}
