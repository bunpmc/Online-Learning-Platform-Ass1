using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;
public class TranscriptService(HttpClient httpClient) : ITranscriptService
{
    private readonly HttpClient _http = httpClient;

    public Task<string> GenerateTranscriptFromVideoAsync(string videoUrl) => throw new NotImplementedException();
}
