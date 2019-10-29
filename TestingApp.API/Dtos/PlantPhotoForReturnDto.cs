using System;

namespace TestingApp.API.Dtos
{
    public class PlantPhotoForReturnDto
    {
        public int Id { get; set; }
        public string url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
    }
}