using MovieRecommendationSystem.API.DTO;

namespace MovieRecommendationSystem.API.Services
{
    public class RecommenderService
    {
        private readonly HttpClient _http;

        public RecommenderService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<MovieDto>> GetRecommendations(int userId, int n = 5)
        {
            var url = $"http://localhost:8000/recommend/{userId}?n={n}";
            var result = await _http.GetFromJsonAsync<List<MovieDto>>(url);
            return result ?? new List<MovieDto>();
        }
    }
}
