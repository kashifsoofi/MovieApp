using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieApp.Controllers.Api
{
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly MovieAppContext _dbContext;

        public MoviesController(MovieAppContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<Movie> Get()
        {
            return _dbContext.Movies;
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return new ObjectResult(movie);
            }
        }

        [Authorize(Policy = "CanEdit")]
        [HttpPost]
        public IActionResult Post([FromBody]Movie movie)
        {
            if (ModelState.IsValid)
            {
                if (movie.Id == 0)
                {
                    _dbContext.Movies.Add(movie);
                    _dbContext.SaveChanges();
                    return new ObjectResult(movie);
                }
                else
                {
                    var original = _dbContext.Movies.FirstOrDefault(m => m.Id == movie.Id);
                    original.Title = movie.Title;
                    original.Director = movie.Director;
                    _dbContext.SaveChanges();
                    return new ObjectResult(original);
                }
            }
            return new BadRequestObjectResult(ModelState);
        }

        [Authorize(Policy = "CanEdit")]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var movie = _dbContext.Movies.FirstOrDefault(m => m.Id == id);
            _dbContext.Movies.Remove(movie);
            _dbContext.SaveChanges();
            return new StatusCodeResult(200);
        }
    }
}
