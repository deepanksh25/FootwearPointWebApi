using FootwearPointWebApi.DataAccess;
using FootwearPointWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootwearPointWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
      
        
           
            public IResult Get()
            {
                ProductRepository repo = new ProductRepository();
                IList<CategoryCount> categoryCounts = repo.getbyCategory();

                if (categoryCounts == null || categoryCounts.Count == 0)
                {
                    return Results.NotFound("No categories found.");
                }

                return Results.Ok(categoryCounts);
            }


        }
    }
