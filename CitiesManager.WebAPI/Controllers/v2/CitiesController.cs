using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitiesManager.WebAPI.DatabaseContext;
using CitiesManager.WebAPI.Models;

namespace CitiesManager.WebAPI.Controllers.v2;

[ApiVersion("2.0")]
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
    // [Produces("application/xml")]
    public async Task<ActionResult<IEnumerable<City>>> GetCities()
    {
        List<City> cities = await _context.Cities
            .OrderBy(temp => temp.Name).ToListAsync();

        return cities;
    }
}
