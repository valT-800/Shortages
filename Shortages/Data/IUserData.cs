using Shortages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shortages.Data
{
    public interface IUserData
    {
        public UserModel Login(string username, string password);
    }
}
