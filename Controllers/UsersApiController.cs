using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvidenciaStudentov.Infrastructure.Persistence;
using EvidenciaStudentov.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EvidenciaStudentov.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UsersApi
        // Vráti zoznam všetkých používateľov vo formáte JSON
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pouzivatel>>> GetUsers()
        {
            var users = await _context.Pouzivatelia.ToListAsync();
            return Ok(users);
        }

        // GET: api/UsersApi/5
        // Vráti konkrétneho používateľa podľa ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Pouzivatel>> GetUser(int id)
        {
            var user = await _context.Pouzivatelia.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}


