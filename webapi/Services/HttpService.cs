public class HttpService
{
	private Uri backendUri;
	public HttpService()
	{
		var dotnetBackend = Environment.GetEnvironmentVariable("HTTP_BACKEND") ?? throw new Exception("Empty HTTP_BACKEND env variable.");
		backendUri = new Uri(dotnetBackend);
	}

	public async Task<string> Send(string content)
	{
		using (var client = new HttpClient())
		{
			client.BaseAddress = backendUri;

			var response = await client.GetAsync(content);

			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStringAsync();
		}
	}
}