using Microsoft.AspNetCore.Mvc;

[Route("/")]
[ApiController]
public class DefaultController : ControllerBase
{
	private readonly HttpService httpService;
	private readonly MqService mqService;
	public DefaultController(HttpService httpService, MqService mqService)
	{
		this.httpService = httpService;
		this.mqService = mqService;
	}

	[HttpGet("http-send/{payload}")]
	public async Task<ActionResult<string>> SendHttp(string payload)
	{
		var response = await httpService.Send(payload);

		return Ok(response);
	}

	[HttpGet("mq-send/{payload}")]
	public ActionResult SendMq(string payload)
	{
		mqService.Send(payload);

		return Ok();
	}

	[HttpGet("mq-send-and-reply/{payload}")]
	public async Task<ActionResult<string>> SendMqAndGetResponse(string payload)
	{
		var response = await mqService.SendAndGetResponse(payload);

		return Ok(response);
	}
}