using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartReturns.Services;

namespace SmartReturns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly IAIReasonClassifier _classifier;

        public AIController(IAIReasonClassifier classifier)
        {
            _classifier = classifier;
        }

        [HttpPost("classify-reason")]
        public ActionResult<string> Classify([FromBody] string reasonText)
        {
            var result = _classifier.Classify(reasonText);
            return Ok(result);
        }
    }
}
