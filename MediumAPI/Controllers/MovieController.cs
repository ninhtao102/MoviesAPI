using MediumAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : Controller
    {
        private readonly MovieContext _movieContext;

        public MovieController(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        // GET: api/Movie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            if (_movieContext.Movies == null)
            {
                return NotFound();
            }
            return await _movieContext.Movies.ToListAsync();
        }

        // GET: api/Movie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            if (_movieContext.Movies == null)
            {
                return NotFound();
            }
            var movie = await _movieContext.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }
            return movie;
        }

        // POST: api/Movie
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _movieContext.Movies.Add(movie);
            await _movieContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // PUT: api/Movie/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _movieContext.Entry(movie).State = EntityState.Modified;

            try
            {
                await _movieContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Movie/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (_movieContext.Movies == null)
            {
                return NotFound();
            }

            var movie = await _movieContext.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            _movieContext.Movies.Remove(movie);
            await _movieContext.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(long id)
        {
            return (_movieContext.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
