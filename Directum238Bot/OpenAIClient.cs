using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Directum238Bot;

public class OpenAIClient
{
  private const string apiKey = "";
  private const string openAIUrl = "https://api.openai.com/v1/completions";

  public async Task<string> GetAnswer(string question)
  {
    var answer = string.Empty;
    var client = new HttpClient();
    client.DefaultRequestHeaders.Add("authorization", $"Bearer {apiKey}");

    var content = new StringContent(
      $"{{\"model\": \"text-davinci-003\",\"prompt\": \"{question}\",\"max_tokens\": 400,\"temperature\": 1}}",
      Encoding.UTF8,
      "application/json");

    var response = await client.PostAsync(openAIUrl, content);
    var responseString = await response.Content.ReadAsStringAsync();
    return responseString;
  }
}