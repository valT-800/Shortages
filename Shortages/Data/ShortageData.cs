using Shortages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shortages.Data
{
    public class ShortageData : IShortageData
    {
        private string _filePath = "Shortages.json";
        private List<ShortageModel> _shortages;

        public ShortageData(string filePath)
        {
            if (File.Exists(filePath))
            {
                _filePath = filePath;
                var jsonString = File.ReadAllText(filePath);
                _shortages = JsonSerializer.Deserialize<List<ShortageModel>>(jsonString);
            }
            else
            {
                _shortages = new List<ShortageModel>();
            }
        }
        public void AddShortage(ShortageModel shortage)
        {
            if (_shortages.Any(s => s.Title == shortage.Title && s.Room == shortage.Room))
            {
                var existingShortage = _shortages.First(s => s.Title == shortage.Title && s.Room == shortage.Room);
                if (shortage.Priority < existingShortage.Priority)
                {
                    existingShortage.Priority = shortage.Priority;
                    existingShortage.CreatedOn = shortage.CreatedOn;
                    SaveDataToJsonFile();
                }
                else
                {
                    throw new InvalidOperationException("A shortage with the same title and room already exists.");
                }
            }
            else
            {
                shortage.Id = GetNextId();
                _shortages.Add(shortage);
                SaveDataToJsonFile();
            }
        }

        public void DeleteShortage(int id, int userId, string userType)
        {
            var query = _shortages.Where(s => s.Id == id);
            if(userType != "admin")
            {
                query = query.Where(s => s.UserId.Equals(userId));
            }
            if (query != null)
            {
                var shortage = query.FirstOrDefault();
                _shortages.Remove(shortage);
                SaveDataToJsonFile();
            }
            else
            {
                throw new InvalidOperationException("Shortage not found.");
            }
        }

        public IEnumerable<ShortageModel> GetShortages(FilterModel filter, int userId, string userType)
        {
            var query = _shortages.AsEnumerable();
            if(userType != "admin")
            {
                query = query.Where(s => s.UserId.Equals(userId));
            }
            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(s => s.Title.Contains(filter.Title, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(filter.Room))
            {
                query = query.Where(s => s.Room.Equals(filter.Room, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(filter.Category))
            {
                query = query.Where(s => s.Category.Equals(filter.Category, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.StartDate != null)
            {
                query = query.Where(s => s.CreatedOn >= filter.StartDate.Value);
            }

            if (filter.EndDate != null)
            {
                query = query.Where(s => s.CreatedOn <= filter.EndDate.Value);
            }

            if (query != null)
            {
                return query.OrderByDescending(s => s.Priority);
            }
            else
            {
                throw new InvalidOperationException("Not found");
            }
            
        }
        public ShortageModel GetShortageByTitleAndRoom(string title, string room)
        {
            var query = _shortages.Where(s => s.Title == title);
            query = query.Where(s => s.Room.Equals(room));
            if(query != null)
            {
                return query.FirstOrDefault();
            }
            else
            {
                throw new InvalidOperationException("Shortage not found");
            }
        }
        private int GetNextId()
        {
            return _shortages.Count > 0 ? _shortages.Max(s => s.Id) + 1 : 1;
        }

        private void SaveDataToJsonFile()
        {
            string jsonString = JsonSerializer.Serialize(_shortages);
            File.WriteAllText(_filePath, jsonString);
        }
    }
}