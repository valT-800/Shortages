using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Shortages.Tests
{
    [TestClass]
    public class ShortageDataTests
    {
        private ShortageData _shortageManager;
        private string _testJsonFilePath;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _testJsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testShortages.json");
            _shortageManager = new ShortageManager(_testJsonFilePath);
        }

        [TearDown]
        public void TearDown()
        {
            // Remove the test JSON file
            if (File.Exists(_testJsonFilePath))
            {
                File.Delete(_testJsonFilePath);
            }
        }

        [Test]
        public void RegisterShortage_WhenShortageDoesNotExist_ShouldAddShortageToData()
        {
            // Arrange
            var shortage = new Shortage
            {
                Title = "Wireless Speaker",
                Name = "John Doe",
                Room = Room.MeetingRoom,
                Category = Category.Electronics,
                Priority = 5,
                CreatedOn = DateTime.Today
            };

            // Act
            _shortageManager.RegisterShortage(shortage);
            var actualShortage = _shortageManager.GetShortageByTitleAndRoom("Wireless Speaker", Room.MeetingRoom);

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
            var shortage1 = new Shortage
            {
                Title = "Wireless Speaker",
                Name = "John Doe",
                Room = "MeetingRoom",
                Category = "Electronics",
                Priority = 5,
                CreatedOn = DateTime.Today
            };
            var shortage2 = new ShortageModel
            {
                Title = "Wireless Speaker",
                Name = "Jane Smith",
                Room = "MeetingRoom",
                Category = "Electronics",
                Priority = 3,
                CreatedOn = DateTime.Today
            };
            _shortageManager.RegisterShortage(shortage1);

            // Act
            _shortageManager.RegisterShortage(shortage2);
            var actualShortage = _shortageManager.GetShortageByTitleAndRoom("Wireless Speaker", Room.MeetingRoom);

            // Assert
            Assert.IsNotNull(actualShortage);
            Assert.AreEqual(shortage1.Title, actualShortage.Title);
            Assert.AreEqual(shortage1.Name, actualShortage.Name);
            Assert.AreEqual(shortage1.Room, actualShortage.Room);
            Assert.AreEqual(shortage1.Category, actualShortage.Category);
            Assert.AreEqual(shortage1.Priority, actualShortage.Priority);
            Assert.AreEqual(shortage1.CreatedOn, actualShortage.CreatedOn);
        }
        
    }
}
