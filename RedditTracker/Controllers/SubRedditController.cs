using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedditTracker.ServiceContracts;

namespace RedditTracker.Controllers
{
    [Route("api/subreddit")]
    [ApiController]
    public class SubRedditController : ControllerBase
    {
        private readonly ISubRedditService _subRedditService;

        public SubRedditController(ISubRedditService subRedditService)
        {
            _subRedditService = subRedditService;
        }

        [HttpGet("{subRedditName}/topPost")]
        public async Task<IActionResult> GetTopPost(string subRedditName)
        {
            if (string.IsNullOrWhiteSpace(subRedditName))
                return BadRequest("Invalid subreddit");

            var isSubRedditTracked = await _subRedditService.IsSubRedditTracked(subRedditName);
            if (!isSubRedditTracked)
                return NotFound($"SubReddit {subRedditName} not tracked");

            var topPost = await _subRedditService.GetTopPost(subRedditName);
            var lastUpdateTime = await _subRedditService.GetLastUpdateTime(subRedditName);

            return Ok(new { post = $"https://www.reddit.com/r/technology/comments/{topPost}/", LastUpdated = $"{lastUpdateTime} UTC" });
        }

        [HttpGet("{subRedditName}/topUser")]
        public async Task<IActionResult> GetTopUser(string subRedditName)
        {
            if (string.IsNullOrWhiteSpace(subRedditName))
                return BadRequest("Invalid subreddit");

            var isSubRedditTracked = await _subRedditService.IsSubRedditTracked(subRedditName);
            if (!isSubRedditTracked)
                return NotFound($"SubReddit {subRedditName} not tracked");

            var topUser = await _subRedditService.GetTopUser(subRedditName);
            var lastUpdateTime = await _subRedditService.GetLastUpdateTime(subRedditName);
            return Ok(new { user = $"https://www.reddit.com/user/{topUser}/", LastUpdated = $"{lastUpdateTime} UTC" });
        }
    }
}
