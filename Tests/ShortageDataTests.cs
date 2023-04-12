using Shortages.Data;
using Shortages.Models;

namespace Shortages.Tests
{
    [TestFixture]
    public class Tests
    {
        private ShortageData _shortageData;
        private string _testFilePath;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _testFilePath = "Shortages.json";
            _shortageData = new ShortageData(_testFilePath);
        }

        [TearDown]
        public void TearDown()
        {
            // Remove the test JSON file
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [Test]
        public void RegisterShortage_WhenShortageDoesNotExist_ShouldAddShortageToData()
        {
            // Arrange
            var shortage = new ShortageModel
            {
                Title = "Wireless Speaker is not connectable",
                Name = "Sherlock Holmes",
                Room = "MeetingRoom",
                Category = "Electronics",
                Priority = 5,
                CreatedOn = DateTime.Today,
                UserId = 0,
            };

            // Act
            _shortageData.AddShortage(shortage);
            var actualShortage = _shortageData.GetShortageByTitleAndRoom("Wireless Speaker is not connectable", "MeetingRoom");

            // Assert
            Assert.IsNotNull(actualShortage);
            Assert.AreEqual(shortage.Title, actualShortage.Title);
            Assert.AreEqual(shortage.Name, actualShortage.Name);
            Assert.AreEqual(shortage.Room, actualShortage.Room);
            Assert.AreEqual(shortage.Category, actualShortage.Category);
            Assert.AreEqual(shortage.Priority, actualShortage.Priority);
            Assert.AreEqual(shortage.CreatedOn, actualShortage.CreatedOn);
        }

        [Test]
        public void RegisterShortage_WhenShortageExistsWithLowerPriority_ShouldNotAddShortageToData()
        {
            // Arrange
            var shortage1 = new ShortageModel
            {
                Title = "Wireless Speaker is not connectable",
                Name = "Sherlock Holmes",
                Room = "MeetingRoom",
                Category = "Electronics",
                Priority = 5,
                CreatedOn = DateTime.Today,
                UserId = 0,
            };
            var shortage2 = new ShortageModel
            {
                Title = "Wireless Speaker is not connectable",
                Name = "Sherlock Holmes",
                Room = "MeetingRoom",
                Category = "Electronics",
                Priority = 3,
                CreatedOn = DateTime.Today,
                UserId = 0,
            };
            _shortageData.AddShortage(shortage1);

            // Act
            _shortageData.AddShortage(shortage2);
            var actualShortage = _shortageData.GetShortageByTitleAndRoom("Wireless Speaker is not connectable", "MeetingRoom");

            // Assert
            Assert.IsNotNull(actualShortage);
            Assert.AreEqual(shortage1.Title, actualShortage.Title);
            Assert.AreEqual(shortage1.Name, actualShortage.Name);
            Assert.AreEqual(shortage1.Room, actualShortage.Room);
            Assert.AreEqual(shortage1.Category, actualShortage.Category);
            Assert.AreEqual(shortage1.Priority, actualShortage.Priority);
            Assert.AreEqual(shortage1.CreatedOn, actualShortage.CreatedOn);
        }

        [Test]
        public void RegisterShortage_WhenShortageExistsWithHigherPriority_ShouldUpdateShortageInData()
        {
            // Arrange
            var shortage1 = new ShortageModel
            {
                Title = "Wireless Speaker is not connectable",
                Name = "Sherlock Holmes",
                Room = "MeetingRoom",
                Category = "Electronics",
                Priority = 3,
                CreatedOn = DateTime.Today,
                UserId = 0,
            };
            var shortage2 = new ShortageModel
            {
                Title = "Wireless Speaker is not connectable",
                Name = "Sherlock Holmes",
                Room = "MeetingRoom",
                Category = "Electronics",
                Priority = 5,
                CreatedOn = DateTime.Today,
                UserId = 0,
            };
            _shortageData.AddShortage(shortage1);

            // Act
            _shortageData.AddShortage(shortage2);
            var actualShortage = _shortageData.GetShortageByTitleAndRoom("Wireless Speaker is not connectable", "MeetingRoom");

            // Assert
            Assert.IsNotNull(actualShortage);
            Assert.AreEqual(shortage2.Title, actualShortage.Title);
            Assert.AreEqual(shortage2.Name, actualShortage.Name);
            Assert.AreEqual(shortage2.Room, actualShortage.Room);
            Assert.AreEqual(shortage2.Category, actualShortage.Category);
            Assert.AreEqual(shortage2.Priority, actualShortage.Priority);
            Assert.AreEqual(shortage2.CreatedOn, actualShortage.CreatedOn);
        }

        [Test]
        public void Test_GetShortagesByRoom()
        {
            // Arrange
            var shortage1 = new ShortageModel
            {
                Title = "Wireless Speaker is not connectable",
                Name = "Sherlock Holmes",
                Room = "MeetingRoom",
                Category = "Electronics",
                Priority = 3,
                CreatedOn = DateTime.Today,
                UserId = 0,
            };
            var shortage2 = new ShortageModel
            {
                Title = "Tea ended",
                Name = "Sherlock Holmes",
                Room = "Kitchen",
                Category = "Food",
                Priority = 9,
                CreatedOn = DateTime.Today,
                UserId = 0,
            };
            var shortage3 = new ShortageModel
            {
                Title = "Light bulb",
                Name = "Christen",
                Room = "MeetingRoom",
                Category = "Other",
                Priority = 2,
                CreatedOn = new DateTime(2023, 4, 8)
            };
            _shortageData.AddShortage(shortage1);
            _shortageData.AddShortage(shortage2);
            _shortageData.AddShortage(shortage3);

            FilterModel filter = new();
            filter.Room = "MeetingRoom";
            // Act
            var result = _shortageData.GetShortages(filter, 0, "admin");

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(shortage1, result.First());
            Assert.AreEqual(shortage3, result.Last());
        }

    }

}