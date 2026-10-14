using ComputerShopAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;

namespace ComputerShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        public CmpShopDbContext context = new CmpShopDbContext();
        [HttpGet("getAll")]
        public object getAllComputers()
        {
            var computer = context.Computers.ToList();
            return new { message = "Sikeres lekérdezés", result = computer };
        }

        [HttpGet("byId")]
        public object GetById([FromQuery]Guid Id)
        {
            var computer = context.Computers.SingleOrDefault(x => x.Id == Id);
            if (computer != null)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = computer });
            }
            else return StatusCode(404, new { message = "Nincs ilyen Id", result = computer });
        }

        [HttpPost]
        public object AddNewComputer([FromBody]AddNewComputerDTO addNewComputerDTO)
        {
            var computer = new Computer
            {
                Id = Guid.NewGuid(),
                Brand = addNewComputerDTO.Brand,
                Type = addNewComputerDTO.Type,
                Display = addNewComputerDTO.Display,
                Memory = addNewComputerDTO.Memory,
                CreatedTime = DateTime.Now
            };
            if (computer != null)
            {
                context.Computers.Add(computer);
                context.SaveChanges();
                return StatusCode(201, new { message = "Sikeres felvétel", result = computer });
            }
            else return StatusCode(404, new { message = "Sikertelen felvétel", result = computer });
        }

        [HttpPut]
        public object UpdateBlogger([FromQuery]Guid Id, [FromBody]UpdateComputerDTO updateComputerDTO)
        {
            var computer = context.Computers.FirstOrDefault(computer => computer.Id == Id);
            if (computer != null)
            {
                computer.Brand = updateComputerDTO.Brand;
                computer.Type = updateComputerDTO.Type;
                computer.Display = updateComputerDTO.Display;
                computer.Memory = updateComputerDTO.Memory;
                context.Computers.Update(computer);
                context.SaveChanges();
                return StatusCode(200, new { message = "Sikeres frisssítés", result = computer });
            }
            else return StatusCode(404, new { message = "Sikertelen frissítés", result = computer });
        }

        [HttpDelete]
        public object DeleteComputer([FromQuery]Guid Id)
        {
            var computer = context.Osystems.Find(Id);
            if (computer != null)
            {
                context.Osystems.Remove(computer);
                context.SaveChanges();
                return StatusCode(204, new { message = "Sikeres törlés" });
            }
            else return StatusCode(404, new { message = "Sikertelen törlés" });
        }

        // 1.
        [HttpGet("allOsystemsAndComps")]
        public object AllOsystemsAndComps()
        {
            var computers = context.Osystems.Include(o => o.Computers).ToList();
            if (computers.Count > 0) return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            else return StatusCode(404, new { message = "Sikertelen lekérdezés" });
        }

        // 2.
        [HttpGet("allCompsAndOsystems")]
        public object AllCompsAndOsystems()
        {
            var computers = context.Osystems.Include(o => o.Computers).Select(o => new { o.Name, o.Version, o.Computers });
            var cmp = context.Osystems.Join(context.Computers, o => o.Id, c => c.Osys, (o, c) => new { o.Name, o.Version, c.Brand, c.Type });
            if (computers != null) return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            else return StatusCode(404, new { message = "Sikertelen lekérdezés" });
        }

        // 3.
        [HttpGet("oneOsystemAndComputers")]
        public object OneOsystemAndComputers([FromQuery]Guid id)
        {
            var computers = context.Osystems.Where(o => o.Id == id).Join(context.Computers, o => o.Id, c => c.Osys, (o, c) => new { o.Name, c });
            if (computers != null) return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            else return StatusCode(404, new { message = "Sikertelen lekérdezés" });
        }
    }
}
