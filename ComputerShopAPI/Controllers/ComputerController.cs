using ComputerShopAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public object AddNewComputer(AddNewComputerDTO addNewComputerDTO)
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
            context.Computers.Add(computer);
            context.SaveChanges();
            return StatusCode(201, new { message = "Sikeres felvétel", result = computer });
        }

        [HttpPut]
        public object UpdateBlogger([FromQuery]Guid Id, [FromBody]AddNewComputerDTO addNewComputerDTO)
        {
            var computer = context.Computers.FirstOrDefault(computer => computer.Id == Id);
            if (computer != null)
            {
                computer.Brand = addNewComputerDTO.Brand;
                computer.Type = addNewComputerDTO.Type;
                computer.Display = addNewComputerDTO.Display;
                computer.Memory = addNewComputerDTO.Memory;
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
    }
}
