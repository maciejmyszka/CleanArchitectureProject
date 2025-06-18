using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GrpcService;

public class GrpcClientModel : PageModel
{
    private readonly Greeter.GreeterClient _grpcClient;

    [BindProperty]
    public string Input { get; set; } = string.Empty;

    public string? Output { get; set; }

    public GrpcClientModel(Greeter.GreeterClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Twój request wg greet.proto
        var request = new HelloRequest { Name = Input };

        var reply = await _grpcClient.SayHelloAsync(request);

        Output = reply.Message;

        return Page();
    }
}
