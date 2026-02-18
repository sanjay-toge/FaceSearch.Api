using FaceSearch.Api.Data;
using FaceSearch.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FaceSearch.Api.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly AiService _aiService;
    private readonly AppDbContext _db;

    public SearchController(AiService aiService, AppDbContext db)
    {
        _aiService = aiService;
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Search(IFormFile file, [FromForm] string email)
    {
        var user = _db.Users.FirstOrDefault(x => x.Email == email);

        if (user == null)
        {
            user = new User { Email = email };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        // // 🔒 PAYWALL LOGIC
        // if (!user.IsPaid && user.SearchesUsed >= 1)
        //     return BadRequest("Free search limit reached. Please upgrade.");

        var result = await _aiService.SearchFaceAsync(file);

        // user.SearchesUsed++;
        await _db.SaveChangesAsync();

        return Content(result, "application/json");
    }
}
