using ComputerShopAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public object GetById([FromQuery] Guid Id)
        {
            var computer = context.Computers.SingleOrDefault(x => x.Id == Id);
            if (computer != null)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = computer });
            }
            else return StatusCode(404, new { message = "Nincs ilyen Id", result = computer });
        }

        [HttpPost]
        public object AddNewComputer([FromBody] AddNewComputerDTO addNewComputerDTO)
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
        public object UpdateBlogger([FromQuery] Guid Id, [FromBody] UpdateComputerDTO updateComputerDTO)
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
                return StatusCode(200, new { message = "Sikeres frissítés", result = computer });
            }
            else return StatusCode(404, new { message = "Sikertelen frissítés", result = computer });
        }

        [HttpDelete]
        public object DeleteComputer([FromQuery] Guid Id)
        {
            var computer = context.Computers.Find(Id);
            if (computer != null)
            {
                context.Computers.Remove(computer);
                context.SaveChanges();
                return StatusCode(200, new { message = "Sikeres törlés" });
            }
            else return StatusCode(404, new { message = "Sikertelen törlés" });
        }

        // 1.
        [HttpGet("allOsystemsAndComps")]
        public object AllOsystemsAndComps()
        {
            var computers = context.Osystems
                .Select(o => new { o.Id, o.Name, o.Version, o.Computers })
                .ToList();

            if (computers.Count > 0) return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            else return StatusCode(404, new { message = "Sikertelen lekérdezés" });
        }

        // 2.
        [HttpGet("allCompsAndOsystems")]
        public object AllCompsAndOsystems()
        {
            var computers = context.Osystems.Select(o => new { o.Name, o.Version, o.Computers }).ToList();
            if (computers.Any()) return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            else return StatusCode(404, new { message = "Sikertelen lekérdezés" });
        }

        // 3.
        [HttpGet("oneOsystemAndComputers")]
        public object OneOsystemAndComputers([FromQuery] Guid id)
        {
            var computers = context.Osystems.Include(o => o.Computers).Where(o => o.Id == id).Select(o => new { o.Name, Computers = o.Computers }).FirstOrDefault();
            if (computers != null) return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            else return StatusCode(404, new { message = "Sikertelen lekérdezés" });
        }

        // 4.
        [HttpGet("oneCompOneOsys")]
        public object OneCompOneOsys([FromQuery] Guid id)
        {
            var computers = context.Computers.Select(c => new { c.Brand, c.Type }).ToList();

            if (computers.Any()) return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            else return StatusCode(404, new { message = "Sikertelen lekérdezés" });
        }

        // 5.
        [HttpGet("numberOfComps")]
        public object NumberOfComps()
        {
            var osystems = context.Osystems.Select(o => new { OpNev = o.Name, Darab = o.Computers.Count() }).ToList();
            if (osystems.Any()) return StatusCode(200, new { message = "Sikeres lekérdezés", result = osystems });
            else return StatusCode(404, new { message = "Sikertelen lekérdezés" });
        }

        // 6.
        [HttpGet("~/api/query/brand/{brand}")]
        public object GetComputersByBrand([FromRoute] string brand)
        {
            var computers = context.Computers
                .Where(c => c.Brand.ToLower() == brand.ToLower())
                .Select(c => new { c.Brand, c.Type, c.Display, c.Memory, OsysName = c.OsysId.Name, OsysVersion = c.OsysId.Version })
                .ToList();

            if (computers.Any()) return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            else return StatusCode(404, new { message = "Nincs gép ilyen márkával" });
        }

        // 7.
        [HttpGet("~/api/query/computers-sorted")]
        public object GetComputersSorted()
        {
            var computers = context.Computers
                .OrderBy(c => c.Brand)
                .ThenBy(c => c.Type)
                .Select(c => new { c.Brand, c.Type, c.Display, c.Memory, OsysName = c.OsysId.Name, OsysVersion = c.OsysId.Version })
                .ToList();

            if (computers.Any()) return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            else return StatusCode(404, new { message = "Nincsenek gépek az adatbázisban" });
        }

        // 8.
        [HttpGet("~/api/query/min-display/{size}")]
        public object GetComputersByMinDisplay([FromRoute] double size)
        {
            var computers = context.Computers
                .Where(c => c.Display > size)
                .Select(c => new { c.Brand, c.Type, c.Display, c.Memory, OsysName = c.OsysId.Name, OsysVersion = c.OsysId.Version })
                .ToList();

            if (computers.Any()) return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            else return StatusCode(404, new { message = $"Nincs a megadott méretnél ({size}\") nagyobb gép" });
        }

        // 9.
        [HttpGet("~/api/query/os-stats")]
        public object GetOsStats()
        {
            var stats = context.Osystems
                .Select(o => new
                {
                    OsName = o.Name,
                    OsVersion = o.Version,
                    TotalComputers = o.Computers.Count(),
                    AverageMemory = o.Computers.Any() ? o.Computers.Average(c => c.Memory) : 0,
                    AverageDisplay = o.Computers.Any() ? o.Computers.Average(c => c.Display) : 0
                })
                .ToList();

            return StatusCode(200, new { message = "Sikeres lekérdezés", result = stats });
        }

        // 10.
        [HttpGet("~/api/query/by-os")]
        public object GetComputersByOsName([FromQuery] string osName)
        {
            var computers = context.Computers
                .Where(c => c.OsysId.Name.ToLower() == osName.ToLower())
                .Select(c => new { c.Brand, c.Type, c.Display, c.Memory, OsysName = c.OsysId.Name })
                .ToList();

            if (computers.Any()) return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            else return StatusCode(404, new { message = $"Nem található gép ezzel az op-rendszerrel: {osName}" });
        }

        // 11.
        [HttpGet("~/api/query/unused-osystems")]
        public object GetUnusedOsystems()
        {
            var unusedOs = context.Osystems
                .Where(o => !o.Computers.Any())
                .Select(o => new { o.Id, o.Name, o.Version })
                .ToList();

            if (unusedOs.Any()) return StatusCode(200, new { message = "Sikeres lekérdezés", result = unusedOs });
            else return StatusCode(404, new { message = "Minden operációs rendszerhez tartozik legalább egy gép" });
        }

        // 12.
        [HttpGet("~/api/query/max-memory-per-os")]
        public object GetMaxMemoryPerOs()
        {
            var result = context.Osystems
                .Select(o => new
                {
                    OsName = o.Name,
                    OsVersion = o.Version,
                    MaxMemoryComputer = o.Computers
                        .OrderByDescending(c => c.Memory)
                        .Select(c => new { c.Brand, c.Type, c.Memory })
                        .FirstOrDefault()
                })
            .ToList();
            return StatusCode(200, new { message = "Sikeres lekérdezés", result });
        }

        // 13.
        [HttpGet("~/api/query/filter-by-mem-and-osver")]
        public object FilterByMemoryAndOsVersion([FromQuery] int minMemory, [FromQuery] string osVersion)
        {
            var computers = context.Computers
            .Select(c => new { c.Brand, c.Type, c.Memory, OsysName = c.OsysId.Name, OsysVersion = c.OsysId.Version })
            .ToList();
            if (computers.Any()) return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            else return StatusCode(404, new { message = "Nincs a szűrési feltételeknek megfelelő gép" });
        }

        // 14.
        [HttpGet("~/api/query/latest")]
        public object GetLatestComputers()
        {
            var latestComputers = context.Computers
            .OrderByDescending(c => c.CreatedTime)
            .Take(5)
            .Select(c => new { c.Brand, c.Type, c.CreatedTime, OsysName = c.OsysId.Name })
            .ToList();
            if (latestComputers.Any()) return StatusCode(200, new { message = "Sikeres lekérdezés", result = latestComputers });
            else return StatusCode(404, new { message = "Nincsenek gépek az adatbázisban" });
        }
        // 15.
        [HttpGet("~/api/query/brand-summary")]
        public object GetBrandSummary()
        {
            var summary = context.Computers
            .GroupBy(c => c.Brand)
            .Select(g => new
            {
                Brand = g.Key,
                Count = g.Count(),
                Models = g.Select(c => new { c.Type, c.Memory }).ToList()
            })
            .ToList();
            return StatusCode(200, new { message = "Sikeres lekérdezés", result = summary });
        }
    }
}
