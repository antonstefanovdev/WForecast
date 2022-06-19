using System.Text;

public class Parser
{

    private readonly HttpClient _client;
    private readonly Uri _baseAddress;
    public Parser(string baseAddress)
    {
        _baseAddress = new Uri(baseAddress);
        _client = new HttpClient();
        _client.BaseAddress = _baseAddress;
    }

    public async Task<string> GetContent(string relativeAddress = "")
    {
        try
        {
            HttpResponseMessage response = await _client
                        .GetAsync($"{_client.BaseAddress}{relativeAddress}");

            var contenttype = response.Content.Headers
                .First(h => h.Key.Equals("Content-Type"));

            var rawencoding = contenttype.Value
                .First();

            if (rawencoding.ToLower().Contains("utf8")
                || rawencoding.ToLower().Contains("utf-8"))
            {
                var bytes = await response.Content
                    .ReadAsByteArrayAsync();

                return Encoding.UTF8.GetString(bytes);
            }
            else
                return await response.Content
                    .ReadAsStringAsync();
        }
        catch

        {
            return String.Empty;
        }
    }
}
