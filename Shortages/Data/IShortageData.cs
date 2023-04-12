using Shortages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortages.Data
{
    public interface IShortageData
    {
        public void AddShortage(ShortageModel shortage);
        public void DeleteShortage(int id, int userId, string userType);
        public IEnumerable<ShortageModel> GetShortages(FilterModel filter, int userId, string userType);
        public ShortageModel GetShortageByTitleAndRoom(string title, string room);



    }
}
