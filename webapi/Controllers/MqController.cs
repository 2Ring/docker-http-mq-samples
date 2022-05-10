using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class MqController : ControllerBase
{
	private readonly MqService mqService;
	public MqController(MqService mqService)
	{
		this.mqService = mqService;
	}

	[HttpGet("send/{payload}")]
	public async Task<ActionResult<string>> Send(string payload)
	{
		var response = await mqService.SendAsync(payload);
		Console.WriteLine(payload);

		return Ok(response);
	}
}