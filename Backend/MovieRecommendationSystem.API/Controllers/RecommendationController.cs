using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRecommendationSystem.API.Services;

namespace MovieRecommendationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly RecommenderService _recommender;

        public RecommendationController(RecommenderService recommender)
        {
            _recommender = recommender;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId, int n = 5)
        {
            var recommendations = await _recommender.GetRecommendations(userId, n);
            return Ok(recommendations);
        }
    }
}
