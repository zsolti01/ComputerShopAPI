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
            var osystem = context.Osystems.ToList();
            return new { message = "Sikeres lekérdezés", result = osystem };
        }

        [HttpPost]
        public object AddNewOsystem(AddNewOsystemDTO addNewOsystemDTO)
        {
            var osystem = new Osystem
            {
                Id = Guid.NewGuid(),
                Name = addNewOsystemDTO.Name,
                Version = addNewOsystemDTO.Version,
                RegisterTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            context.Osystems.Add(osystem);
            context.SaveChanges();
            return StatusCode(201, new { message = "Sikeres felvétel", result = osystem });
        }

        [HttpPut]
        public object UpdateOsystem([FromQuery]Guid id, [FromBody]UpdateOsystemDTO updateOsystemDTO)
        {
            var osystem = context.Osystems.FirstOrDefault(osystem => osystem.Id == id);
            if (osystem != null)
            {
                osystem.Name = updateOsystemDTO.Name;
                osystem.Version = updateOsystemDTO.Version;
                osystem.UpdateTime = DateTime.Now;
                context.Osystems.Update(osystem);
                context.SaveChanges();
                return StatusCode(200, new { message = "Sikeres frissítés", result = osystem });
            }
            return StatusCode(404, new { message = "Sikertelen frissítés", result = osystem });
        }

        [HttpDelete]
        public object DeleteOsystem([FromQuery]Guid id)
        {
            var osystem = context.Osystems.Find(id);
            if (osystem != null)
            {
                context.Osystems.Remove(osystem);
                context.SaveChanges();
                return StatusCode(204, new { message = "Sikeres törlés"});
            }
            return StatusCode(404, new { message = "Sikertelen törlés"});
        }
    }
}
