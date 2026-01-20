using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Lesson;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class AiLessonService(HttpClient httpClient, ITranscriptService transcriptService, IProgressRepository progressRepository, ILessonRepository lessonRepository) : IAiLessonService
{
    private readonly HttpClient _http = httpClient;
    private readonly ITranscriptService _transcriptService = transcriptService;
    private readonly IProgressRepository _progressRepository = progressRepository;
    private readonly ILessonRepository _lessonRepository = lessonRepository;

    private const string _aiEndpoint = "https://api.groq.com/openai/v1/chat/completions";
    private readonly string _groqApiKey = Environment.GetEnvironmentVariable("GroqAPIKey__Key") ?? "";

    public async Task<string> GenerateSummaryAsync(ProgressDTO dto)
    {
        var progress = await _progressRepository.GetByIdAsync(dto.Id)
            ?? throw new Exception("Progress not found");

        if (progress.AiSummaryStatus == AiSummaryStatus.Done)
            return progress.AiSummary!;

        if (progress.AiSummaryStatus == AiSummaryStatus.Processing)
            return "Đợi xíu, AI đang tóm tắt bài này...";

        progress.AiSummaryStatus = AiSummaryStatus.Processing;
        await _progressRepository.UpdateAsync(progress);

        try
        {
            var transcript = await EnsureTranscriptAsync(progress);
            var summary = await CallAiSummary(transcript);

            progress.AiSummary = summary;
            progress.AiSummaryStatus = AiSummaryStatus.Done;
            await _progressRepository.UpdateAsync(progress);

            return summary;
        }
        catch
        {
            progress.AiSummaryStatus = AiSummaryStatus.Failed;
            await _progressRepository.UpdateAsync(progress);
            throw;
        }
    }

    public async Task<string> AskAsync(ProgressDTO lessonProgress, string question)
    {
        var progress = await _progressRepository.GetByIdAsync(lessonProgress.Id)
            ?? throw new Exception("Progress not found");
        var context = !string.IsNullOrWhiteSpace(lessonProgress.AiSummary)
            ? lessonProgress.AiSummary!
            : await EnsureTranscriptAsync(progress);

        return await CallAiAsk(context, question);
    }

    private async Task<string> EnsureTranscriptAsync(LessonProgress progress)
    {

        if (!string.IsNullOrWhiteSpace(progress.Transcript))
            return progress.Transcript!;

        var lesson = await _lessonRepository.GetByIdAsync(progress.LessonId);

        var transcript = await _transcriptService
            .GenerateTranscriptFromVideoAsync(lesson!.VideoUrl);

        progress.Transcript = transcript;
        await _progressRepository.UpdateAsync(progress);

        return transcript;
    }


    private async Task<string> CallAiAsk(string context, string question)
    {
        var payload = new
        {
            model = "llama-3.3-70b-versatile",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content =
                        "You are a teacher. Answer ONLY using the provided content. " +
                        "If the content does not contain the answer, say you do not know."
                },
                new
                {
                    role = "user",
                    content = $"Content:\n{context}\n\nQuestion:\n{question}"
                }
            },
            temperature = 0.3
        };

        return await SendToAi(payload);
    }

    private async Task<string> CallAiSummary(string transcript)
    {
        var payload = new
        {
            model = "llama-3.3-70b-versatile",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content =
                        "You are a Vietnamese teacher. " +
                        "ONLY summarize the lesson using the provided transcript. " +
                        "Write in clear Vietnamese. " +
                        "If the transcript is unclear or empty, respond exactly: 'Không thể tóm tắt vì nội dung không rõ ràng.'"
                },
                new
                {
                    role = "user",
                    content = transcript
                }
            },
            temperature = 0.3
        };

        return await SendToAi(payload);
    }

    private async Task<string> SendToAi(object payload)
    {
        var json = JsonSerializer.Serialize(payload);

        using var request = new HttpRequestMessage(HttpMethod.Post, _aiEndpoint)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", _groqApiKey);

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseJson);

        return doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString()!;
    }
}
