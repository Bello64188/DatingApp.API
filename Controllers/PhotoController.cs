using System.Linq;
namespace DatingApp.API.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using DatingApp.API.Data;
    using DatingApp.API.IGenericRepository;
    using DatingApp.API.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

     
    [Route("api/user/{userid}/[controller]")]  
    [ApiController]
    [Authorize]
    public class PhotoController : ControllerBase
    {
        private readonly IRepository<Photo> _repoPhoto;
        private readonly IRepository<UserData> _repository;
        private readonly IMapper _map;
        private readonly IOptions<CloudinarySettings> _cloudinarysetting;
        private Cloudinary _cloudinary;

        public PhotoController(IRepository<UserData> repo, IMapper mapper, 
                          IOptions<CloudinarySettings> cloudinarySetting,IRepository<Photo> repophoto)
        {
            _repoPhoto=repophoto;
            _repository= repo;
            _map=mapper;
            _cloudinarysetting=cloudinarySetting;
            Account acc = new Account(
                _cloudinarysetting.Value.CloudName,
                _cloudinarysetting.Value.ApiKey,
                _cloudinarysetting.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }
           [HttpGet("{id}", Name ="GetUserPhoto")]
        public async Task<IActionResult> GetUserPhoto(int id)
        {   
            var photoFromRepo = await _repository.GetPhoto(id);
            var photo = _map.Map<PhotoForReturnDTO>(photoFromRepo);
            return Ok(photo);
        }
        [HttpPost]
      
        public async Task<IActionResult> AddPhotoForUser(string userid, [FromForm]PhotoForCreationDTO photoDTO)
        {
         var user = await _repository.GetUser(userid); 
         if(user==null)
         return BadRequest("User cannot be found!!");
         var file = photoDTO.file;
         var uploadResult= new ImageUploadResult();
         if (file.Length > 0)
         {
             using( var stream = file.OpenReadStream()){
                 var uploadParams= new ImageUploadParams(){
                     File= new FileDescription(file.Name,stream),
                    Transformation= new  Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                 };
                 uploadResult= _cloudinary.Upload(uploadParams);
             }
         }
            photoDTO.url=uploadResult.Url.ToString();
            photoDTO.publicId=uploadResult.PublicId;
            var photo = _map.Map<Photo>(photoDTO);
            photo.userData= user;
            if(!user.photos.Any(m=>m.isMain)){
                  photo.isMain= true;
            }
            
              user.photos.Add(photo);
              if (!await _repository.SaveAll())
              {
                return BadRequest("Could not add the Photo");
                  
              }
              
              var photoToReturn= _map.Map<PhotoForReturnDTO>(photo);
              return CreatedAtRoute(nameof(GetUserPhoto), new {userid, id= photo.id }, photoToReturn);
              
        }
         [HttpPost("{id}/setMain")]
    public async Task<ActionResult> SetMainPhoto(string userid,int id)
    {
        var user = await _repository.GetUser(userid);
        if (!user.photos.Any(p=>p.id==id))
        {
            return Unauthorized();
        }
        var photofromRepo= await _repository.GetPhoto(id);
        if (photofromRepo.isMain)
        {
            return BadRequest("This is already the main photo.");

        }
        var currentPhoto = await _repository.GetMainPhotoForUser(userid);
        currentPhoto.isMain= false;
        photofromRepo.isMain=true;
        if(await _repository.SaveAll()){
            return NoContent();
        }
        return BadRequest("Could not set photo to main");
    }
     [HttpDelete("{id}")]  
     public async Task<IActionResult> DeletePhoto(string userid, int id)
    {
    var user = await _repository.GetUser(userid);
        if (!user.photos.Any(p=>p.id==id))
        {
            return Unauthorized();
        }
        var photofromRepo= await _repository.GetPhoto(id);
        if (photofromRepo.isMain)
        {
            return BadRequest("You cannot delete the main photo.");

        }
        if (photofromRepo.publicId!=null)
        {
            var deleteParams= new DeletionParams(photofromRepo.publicId);
       var result = _cloudinary.Destroy(deleteParams);
       if (result.Result=="ok")       
         _repoPhoto.Delete(photofromRepo);
        }
       if (photofromRepo.publicId==null)
       {
         _repoPhoto.Delete(photofromRepo);
        
       }
       if(await _repository.SaveAll())
       return Ok();
       return BadRequest("Unable to delete the photo");
   }
    }
   
}