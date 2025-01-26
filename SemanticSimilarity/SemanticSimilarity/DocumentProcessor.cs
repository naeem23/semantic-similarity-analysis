using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;
public class DocumentProcessor
{
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.openai.com/v1/embeddings";

    public DocumentProcessor(string apiKey)
    {
        _apiKey = apiKey;
    }
