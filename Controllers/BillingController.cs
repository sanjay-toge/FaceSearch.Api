using Microsoft.AspNetCore.Mvc;
using FaceSearch.Api.Services;
using FaceSearch.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace FaceSearch.Api.Controllers;

[ApiController]
[Route("api/billing")]
public class BillingController : ControllerBase
{
    private readonly StripeService _stripe;

    public BillingController(StripeService stripe)
    {
        _stripe = stripe;
    }

    [HttpPost("checkout")]
    public IActionResult Checkout([FromBody] CheckoutRequest request)
    {
        // Simple string body binding can be tricky with JSON, utilizing a DTO is safer or dynamic
        // But following user instruction roughly, if sending raw string in body from angular:
        // Angular: http.post(url, "email@example.com", { headers: { 'Content-Type': 'application/json' } }) 
        // typically sends "email@example.com" as a JSON string.
        // Let's assume a simple object wrapper for safety: { "email": "..." }
        
        var url = _stripe.CreateCheckoutSession(request.Email);
        return Ok(new { url });
    }

    [HttpPost("activate")]
    public async Task<IActionResult> Activate([FromBody] CheckoutRequest request, [FromServices] AppDbContext db)
    {
        var user = await db.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user != null)
        {
            user.IsPaid = true;
            await db.SaveChangesAsync();
        }
        return Ok();
    }
}

public class CheckoutRequest
{
    public string Email { get; set; } = "";
}
