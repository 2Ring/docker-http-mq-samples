public class HttpService
{
	private Uri backendUri;
	public HttpService()
	{
		var dotnetBackend = Environment.GetEnvironmentVariable("DOTNET_BACKEND") ?? throw new Exception("Empty DOTNET_BACKEND env variable.");
		backendUri = new Uri(dotnetBackend);
	}

	public async Task<string> Send(string content)
	{
		using (var client = new HttpClient())
		{
			client.BaseAddress = backendUri;

			var response = await client.GetAsync(content);

			Console.WriteLine(response.StatusCode);

			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStringAsync();
		}
	}
}