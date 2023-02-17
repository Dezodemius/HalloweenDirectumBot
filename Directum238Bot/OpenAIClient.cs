using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Directum238Bot;

public class OpenAIClient
{
  private readonly string apiKey;
  private const string openAIUrl = "https://api.openai.com/v1/completions";

  public async Task<string> GetAnswer(string question)
  {
    if (string.IsNullOrEmpty(question))
      return null;
    var client = new HttpClient();
    client.DefaultRequestHeaders.Add("authorization", $"Bearer {apiKey}");

    var randomTemp = new Random().NextDouble().ToString("0.##", CultureInfo.InvariantCulture);
    var content = new StringContent(
      $"{{\"model\": \"text-davinci-003\",\"prompt\": \"{question}\",\"max_tokens\": 400,\"temperature\": {randomTemp}}}",
      Encoding.UTF8,
      "application/json");

    var response = await client.PostAsync(openAIUrl, content);
    var contentAsString = await response.Content.ReadAsStringAsync();
    var answer = JsonConvert.DeserializeObject<dynamic>(contentAsString)!.choices[0].text;
    return answer;
  }

  public OpenAIClient(string apiKey)
  {
    this.apiKey = apiKey;
  }
}