using System;
using System.Collections.Generic;
using TestingApp.API.Models;

namespace TestingApp.API.Dtos
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created  { get; set; }
        public DateTime LastActive  { get; set; }
         public string PhotoUrl { get; set; }

         public ICollection<PlantPhotoForDetailedDto> PlantPhotos {get;set;}
    }
}