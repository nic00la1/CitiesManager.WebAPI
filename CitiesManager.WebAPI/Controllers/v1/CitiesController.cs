using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitiesManager.WebAPI.DatabaseContext;
using CitiesManager.WebAPI.Models;

namespace CitiesManager.WebAPI.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class CitiesController : CustomControllerBase
{
    private readonly ApplicationDbContext _context;

    public CitiesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Cities
    /// <summary>
    /// To get list of cities from 'cities' table
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Produces("application/json")]
    public async Task<ActionResult<IEnumerable<City>>> GetCities()
    {
        if (_context.Cities == null) return NotFound();

        List<City> cities =
            await _context.Cities.OrderBy(temp => temp.Name).ToListAsync();

        return Ok(cities);
    }

    // GET: api/Cities/5
    [HttpGet("{id}")]
    [Produces("application/json")]
    public async Task<ActionResult<City>> GetCity(Guid id)
    {
        City? city =
            await _context.Cities.FirstOrDefaultAsync(temp => temp.Id == id);

        if (city == null) return Problem("Invalid CityID", "City Search", 400);

        return Ok(city);
    }

    // PUT: api/Cities/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> PutCity(Guid id,
                                             [Bind(nameof(city.Id),
                                                 nameof(city.Name))]
                                             City city
    )
    {
        if (id != city.Id) return BadRequest(); // HTTP 400

        City? existingCity = await _context.Cities.FindAsync(id);
        if (existingCity == null) return NotFound(); // HTTP 404

        existingCity.Name = city.Name;

        await _context.SaveChangesAsync();

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CityExists(id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // POST: api/Cities
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Produces("application/json")]
    public async Task<ActionResult<City>> PostCity(
        [Bind(nameof(city.Id), nameof(city.Name))]
        City city
    )
    {
        // if (ModelState.IsValid == false) return BadRequest(ModelState);

        _context.Cities.Add(city);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCity", new { id = city.Id }, city);
    }

    // DELETE: api/Cities/5
    [HttpDelete("{id}")]
    [Produces("application/json")]
    public async Task<IActionResult> DeleteCity(Guid id)
    {
        City? city = await _context.Cities.FindAsync(id);
        if (city == null) return NotFound();

        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CityExists(Guid id)
    {
        return _context.Cities.Any(e => e.Id == id);
    }
}
