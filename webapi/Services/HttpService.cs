public class HttpService
{
	private readonly HttpClient httpClient;
	public HttpService(HttpClient httpClient)
	{
		var dotnetBackend = Environment.GetEnvironmentVariable("DOTNET_BACKEND") ?? throw new Exception("Empty DOTNET_BACKEND env variable.");
		this.httpClient = httpClient;
		this.httpClient.BaseAddress = new Uri(dotnetBackend);
	}

	public async Task<string> SendAsync(string content)
	{
		var request = new HttpRequestMessage
		{
			Method = HttpMethod.Post,
			Content = new StringContent(content)
		};

		var response = await httpClient.SendAsync(request);
		Console.WriteLine(response.StatusCode);
		return await response.Content.ReadAsStringAsync();
	}
}