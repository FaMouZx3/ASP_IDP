using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceServer.Data;
using ResourceServer.Models;

namespace ResourceServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("ClientIdPolicy")]
    public class AnimalController : ControllerBase
    {
        private readonly AnimalDbContext dbContext;

        public AnimalController(AnimalDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        // GET: api/animal
        [HttpGet]
        public ActionResult GetResponse()
        {
            var list = dbContext.TbAnimal.ToList();
            return new JsonResult(list);
        }

        // POST: api/animal
        [HttpPost]
        public ActionResult InsertNewAnimal([FromBody] TbAnimal animal)
        {
            if (animal != null)
            {
                dbContext.TbAnimal.Add(animal);
                dbContext.SaveChanges();
                return new JsonResult(animal);
            }
            else
            {
                return new JsonResult(new TbAnimal());
            }
        }

        // PUT: api/animal/edit/{id}
        [HttpPut("edit/{id}")]
        public ActionResult EditAnimal(int id, [FromBody] TbAnimal sendAnimal)
        {
            if (sendAnimal != null)
            {
                var animal = dbContext.TbAnimal.Where(x => x.id == id).Single<TbAnimal>();
                if (animal != null)
                {
                    animal.name = sendAnimal.name;
                    animal.type = sendAnimal.type;
                    dbContext.SaveChanges();
                    return new JsonResult(animal);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE: api/animal/delete/{id}
        [HttpDelete("delete/{id}")]
        public ActionResult DeleteAnimal(int id)
        {
            TbAnimal animal = dbContext.TbAnimal.Where(x => x.id == id).Single<TbAnimal>();
            var result = dbContext.TbAnimal.Remove(animal);
            if (result != null)
            {
                dbContext.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
