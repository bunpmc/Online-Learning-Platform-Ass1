using Online_Learning_Platform_Ass1.Service.Services.Interfaces;
using Google.GenAI;

namespace Online_Learning_Platform_Ass1.Service.Services;
public class AiLessonService(HttpClient httpClient) : IAiLessonService
{
    private readonly HttpClient _http = httpClient;
    private readonly string _apiKey = "AIzaSyA10WcW6djVWp3pXuCCcLuEM4fq77PijxY";

    public async Task<string> AskAsync(string transcript, string question)
    {
        var client = new Client(apiKey: _apiKey);

        string prompt = $"""
            You are a teacher teaching about this lesson. Use the lesson transcript below to answer the question as best as you can.
            Do not answer anything outside this transcript.
            Here are the questions.
            {question}
            """;


        var response = await client.Models.GenerateContentAsync(
          model: "gemini-2.0-flash",
          contents: prompt
        );

        if (response?.Candidates == null || response.Candidates.Count == 0)
        {
            return "Cannot find answer from the transcript.";
        }

        var candidate = response.Candidates[0];
        if (candidate?.Content?.Parts == null || candidate.Content.Parts.Count == 0)
        {
            return "Cannot find answer from the transcript.";
        }

        var part = candidate.Content.Parts[0];
        if (string.IsNullOrEmpty(part?.Text))
        {
            return "Cannot find answer from the transcript.";
        }

        return part.Text;
    }

    public async Task<string> GenerateSummaryAsync(string? content, string? videoUrl) {
        var client = new Client(apiKey: _apiKey);

        string prompt = $"""
            You are a summary lesson teacher teaching about this lesson. Use the lesson content OR video below to summary as best as you can.
            Do not answer anything outside this transcript.
            Here are the content OR video.
            {content}, {videoUrl}
            """;

        var response = await client.Models.GenerateContentAsync(
          model: "gemini-2.0-flash",
          contents: prompt
        );

        if (response?.Candidates == null || response.Candidates.Count == 0)
        {
            return "Cannot find answer from the transcript.";
        }

        var candidate = response.Candidates[0];
        if (candidate?.Content?.Parts == null || candidate.Content.Parts.Count == 0)
        {
            return "Cannot find answer from the transcript.";
        }

        var part = candidate.Content.Parts[0];
        if (string.IsNullOrEmpty(part?.Text))
        {
            return "Cannot find answer from the transcript.";
        }

        return part.Text;
    }
}
