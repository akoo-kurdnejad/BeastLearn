using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastLearn.Infra.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeastLearn.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseApi : ControllerBase
    {
        private BestLearnContext _context;
        public CourseApi(BestLearnContext context)
        {
            _context = context;
        }

        [Produces("application/json")]
        [Route("search")]
        public async Task<IActionResult> Search()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();

                var courseTitle = _context.Courses
                    .Where(c => c.CourseTitle.Contains(term))
                    .Select(c => c.CourseTitle)
                    .ToList();

                return Ok(courseTitle);
            }
            catch 
            {
                return BadRequest();
            }
        }

    }
}
