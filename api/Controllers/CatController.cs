using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Cat
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatController : ControllerBase
    {
        private readonly ICatService _catService;

        public CatController(ICatService catService)
        {
            _catService = catService;
        }

        [HttpGet]
        public async Task<CatFact> Fact()
        {
            return await _catService.GetCatFactAsync();
        }
    }
}
