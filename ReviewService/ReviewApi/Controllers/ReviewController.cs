using Microsoft.AspNetCore.Mvc;
using ReviewApplication.Interface;
using ReviewApplication.Models;
using ReviewDomain.Entities;
using Common.Paging;

namespace ReviewApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;
        private readonly IImageService _imageService;

        public ReviewController(IReviewService service, IImageService imageService)
        {
            _service = service;
            _imageService = imageService;
        }

        [HttpGet("/api/healthz")] // <-- Change: Explicitly set the absolute path
        public IActionResult HealthCheck() => Ok("Healthy");

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromForm] ReviewCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? imageUrl = null;
            if (request.Image != null)
                imageUrl = await _imageService.UploadImageAsync(request.Image);

            var dto = new ReviewCreateDto
            {
                UserId = request.UserId,
                BookId = request.BookId,
                Rating = request.Rating,
                Comment = request.Comment,
                ImageUrl = imageUrl

            };

            var created = await _service.CreateReviewAsync(dto);
            return CreatedAtAction(nameof(GetReviewById), new { id = created.ReviewId }, created);
        }

        [HttpGet]
        public async Task<IActionResult> GetReviewsByBookId([FromQuery] int bookId, [FromQuery] int pageNo = 1, [FromQuery] int pageSize = 10)
        {
            PagedResult<Review> result = await _service.GetReviewByBookId(bookId, pageNo, pageSize);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await _service.GetReviewByIdAsync(id);
            if (review == null)
                return NotFound();

            return Ok(review);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            bool deleted = await _service.DeleteReviewAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
