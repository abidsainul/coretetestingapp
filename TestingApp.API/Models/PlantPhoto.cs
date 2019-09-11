using System;

namespace TestingApp.API.Models
{
    public class PlantPhoto
    {
        public int Id { get; set; }
        public string url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

    }
}