using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OpenAIClient
{
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.openai.com/v1";

    public OpenAIClient(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<string> GetEmbedding(string text, string model = "text-embedding-ada-002")
    {
        var client = new RestClient($"{BaseUrl}/embeddings");
        //var request = new RestRequest(Method.Post); // Updated Syntax
        var request = new RestRequest();
        request.Method = Method.Post;
        request.AddHeader("Authorization", $"Bearer {_apiKey}");

       var request = new RestRequest();  //calling post method different way Hit API
       request.Method = Method.Post;

               request.AddHeader("Authorization", $"Bearer {_apiKey}");

       request.AddHeader("Content-Type", "application/json");

       var body = new
       {
           input = text,
           model = model
       };

       request.AddJsonBody(body);

       var response = await client.ExecuteAsync(request);
       if (response.IsSuccessful)
       {
           return response.Content;
       }

       throw new Exception($"Error: {response.StatusDescription}\n{response.Content}");
   }
