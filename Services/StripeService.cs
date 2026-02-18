using Stripe;
using Stripe.Checkout;

namespace FaceSearch.Api.Services;

public class StripeService
{
    private readonly IConfiguration _config;

    public StripeService(IConfiguration config)
    {
        _config = config;
    }

    public string CreateCheckoutSession(string email)
    {
        var options = new SessionCreateOptions
        {
            Mode = "subscription",
            CustomerEmail = email,
            SuccessUrl = "http://localhost:4200/success",
            CancelUrl = "http://localhost:4200",
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = _config["Stripe:PriceId"],
                    Quantity = 1,
                }
            }
        };

        var service = new SessionService();
        var session = service.Create(options);

        return session.Url;
    }
}
