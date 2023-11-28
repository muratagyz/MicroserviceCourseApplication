using Course.Basket.Dtos;
using Course.Basket.Services;
using Course.Shared.ControllerBases;
using Course.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace Course.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : CustomBaseController
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BasketsController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var userId = _sharedIdentityService.GetUserId;

            var response = await _basketService.GetBasket(userId);

            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdate(BasketDto basketDto)
        {
            var response = await _basketService.SaveOrUpdate(basketDto);

            return CreateActionResultInstance(response);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var userId = _sharedIdentityService.GetUserId;

            var response = await _basketService.Delete(userId);

            return CreateActionResultInstance(response);
        }
    }
}
