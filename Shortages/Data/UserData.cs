using Shortages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shortages.Data
{
    public class UserData : IUserData
    {
        private const string filePath = "Users.json";
        //string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);

        private List<UserModel> _users;

        public UserData()
        {
            if (File.Exists(filePath))
            {
                var jsonString = File.ReadAllText(filePath);
                _users = JsonSerializer.Deserialize<List<UserModel>>(jsonString);
            }
            else
            {
                _users = new List<UserModel>();
            }
        }
        public UserModel Login(string username, string password)
        {
            var query = _users.AsEnumerable();
            query = query.Where(s => s.Username.Equals(username));
            query = query.Where(s => s.Password.Equals(password)); 

            if(query != null)
            {
                return query.FirstOrDefault();
            }
            else
            {
                throw new InvalidOperationException("User not found.");
            }
        }
    }
}
