using GreenhouseApi.Data;
using GreenhouseApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenhouseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantsController : ControllerBase
    {
        private readonly GreenhouseDbContext _context;

        public PlantsController(GreenhouseDbContext context)
        {
            _context = context;
        }


        // 1.1 READ: Получить одно конкретное растение по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetPlant(int id)
        {
            var plant = await _context.Plants.FindAsync(id);
            if (plant == null) return NotFound();
            return plant;
        }

        // 2. CREATE: Добавить новое растение
        [HttpPost]
        public async Task<ActionResult<Plant>> PostPlant(Plant plant)
        {
            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPlant), new { id = plant.Id }, plant);
        }

        // 3. UPDATE: Изменить данные растения
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlant(int id, Plant plant)
        {
            if (id != plant.Id) return BadRequest();

            _context.Entry(plant).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 4. DELETE: Удалить растение из базы
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            var plant = await _context.Plants.FindAsync(id);
            if (plant == null) return NotFound();

            _context.Plants.Remove(plant);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}