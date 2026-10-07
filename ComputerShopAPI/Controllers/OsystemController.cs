using ComputerShopAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComputerShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OsystemController : ControllerBase
    {
        public CmpShopDbContext context = new CmpShopDbContext();
        [HttpGet("getAll")]
        public object GetAllOsystem()
        {
            var osystems = context.Osystems.ToList();
            return new { message = "Sikeres lekérdezés", result = osystems };
        }

        [HttpPost]
        public object AddNewOsystem(AddNewOsystemDTO addNewOsystemDTO)
        {
            var osystems = new Osystem
            {
                Id = Guid.NewGuid(),
                Name = addNewOsystemDTO.Name,
                Version = addNewOsystemDTO.Version,
                RegisterTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            context.Osystems.Add(osystems);
            context.SaveChanges();
            return new { message = "Sikeres lekérdezés", result = osystems };
        }
    }
}
