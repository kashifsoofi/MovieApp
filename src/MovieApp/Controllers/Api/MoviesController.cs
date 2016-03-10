using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MovieApp.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieApp.Controllers.Api
{
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        [HttpGet]
        public IEnumerable<Movie> Get()
        {
            return new List<Movie>
            {
                new Movie {Id=1, Title="Star Wars", Director="Lucas"},
                new Movie {Id=2, Title="King Kong", Director="Jackson"},
                new Movie {Id=3, Title="Memento", Director="Nolan"}
            };
        }
    }
}
