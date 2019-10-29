using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TestingApp.API.Data;
using TestingApp.API.Dtos;
using TestingApp.API.Helpers;
using TestingApp.API.Models;

namespace TestingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/plantphotos")]
    [ApiController]
    public class PlantPhotosController : ControllerBase
    {
        private readonly IPlantingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;

        private Cloudinary _cloudinary;
        public PlantPhotosController(IPlantingRepository repo, IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._mapper = mapper;
            this._repo = repo;

            // Cloudinary account
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);

        }

        [HttpGet("{id}",Name = "GetPlantPhoto")]
        public async Task<IActionResult> GetPlantPhoto(int id){
            var photoFromRepo =await _repo.GetPlantPhoto(id);
            var photo = _mapper.Map<PlantPhotoForReturnDto>(photoFromRepo);
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPlantPhotoForUser(int userId,
           [FromForm] PlantPhotoForCreationDto plantPhotoForCreationDto)
        {

            if (userId != (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);

            var file = plantPhotoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            //upload to cloudinary 
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500)
                            .Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }

            }

            //save upload details in db

            plantPhotoForCreationDto.Url = uploadResult.Uri.ToString();
            plantPhotoForCreationDto.PublicId = uploadResult.PublicId;

            var plantPhoto = _mapper.Map<PlantPhoto>(plantPhotoForCreationDto);

            if (!userFromRepo.PlantPhotos.Any(u => u.IsMain))
                plantPhoto.IsMain = true;

            userFromRepo.PlantPhotos.Add(plantPhoto);

            if (await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PlantPhotoForReturnDto>(plantPhoto);
                return CreatedAtRoute("GetPlantPhoto",new { id = plantPhoto.Id }, photoToReturn );
            }

            return BadRequest("Could not add the photo");

        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPlantPhoto(int userId,int id)
        {
            
            if (userId != (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)))
                return Unauthorized();

            var user = await _repo.GetUser(userId);
            if(!user.PlantPhotos.Any(p=>p.Id == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPlantPhoto(id);
            if(photoFromRepo.IsMain)
              return BadRequest("This is already the main photo");

            var currentMainPhoto = await _repo.GetMainPlantPhoto(userId);
            currentMainPhoto.IsMain = false;

            photoFromRepo.IsMain = true;

            if(await _repo.SaveAll())
                return NoContent();

            return BadRequest("Could not set photo to main!!");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlantPhoto(int userId,int id) {
            
            if (userId != (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)))
                return Unauthorized();

            var user = await _repo.GetUser(userId);
            if(!user.PlantPhotos.Any(p=>p.Id == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetPlantPhoto(id);
            if(photoFromRepo.IsMain)
              return BadRequest("You cannot delete your main photo");

            if (photoFromRepo.PublicId != null)
            {

                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _repo.Delete(photoFromRepo);
                }
            }

            if (photoFromRepo.PublicId == null)
            {
                _repo.Delete(photoFromRepo);
            }

            if (await _repo.SaveAll())
            {
                return Ok();
            }

              return BadRequest("Failed to delete the photo");
        }

    }
}