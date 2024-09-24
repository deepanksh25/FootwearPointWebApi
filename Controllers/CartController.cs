using FootwearPointWebApi.DataAccess;
using FootwearPointWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootwearPointWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        [HttpGet("GetCartDetails")]
        [Authorize(Roles ="Customer,Admin")]
        public IResult GetCartDetails([FromBody] string Email)
        {
            IList<CartListViewModel> cartDetails = null;
            CartRepository repository = new CartRepository();
            cartDetails = repository.GetCartDetails(Email);
                        return Results.Ok(cartDetails);
        }

        [HttpPost("AddToCart")]
        [Authorize(Roles ="Customer,Admin")]
        public IResult AddToCart([FromBody]AddToCartViewModel model)
        {
            CartRepository repo = new CartRepository();
            int eff = repo.AddToCart(model);
            return Results.Ok(eff);
        }

        [HttpPost("Delete")]
        [Authorize(Roles = "Customer,Admin")]
        public IResult Delete([FromBody] AddToCartViewModel model)
        {
            CartRepository repo = new CartRepository();
            int eff = repo.Delete(model);
            return Results.Ok(eff);
        }

    }
}
