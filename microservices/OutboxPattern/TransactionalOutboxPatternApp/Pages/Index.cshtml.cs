namespace TransactionalOutboxPatternApp.Pages;

using Microsoft.AspNetCore.Mvc.RazorPages;

using TransactionalOutboxPatternApp.Infrastructure.Service;

public class IndexModel : PageModel
{
    private readonly IOrderService _orderService;

    public int? OrderId { get; private set; }

    public IndexModel(IOrderService orderService)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    public void OnGet()
    {
    }

    public async Task OnPostCreateOrder()
    {
        OrderId = await _orderService.CreateOrderAsync();
    }
}