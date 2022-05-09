using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class HttpController : ControllerBase
{
	private readonly HttpService httpService;
	public HttpController(HttpService httpService)
	{
		this.httpService = httpService;
	}

	[HttpGet("send/{payload}")]
	public async Task<ActionResult<string>> Send(string payload)
	{
		var response = await httpService.SendAsync(payload);
		Console.WriteLine(payload);

		return Ok(response);
	}
}