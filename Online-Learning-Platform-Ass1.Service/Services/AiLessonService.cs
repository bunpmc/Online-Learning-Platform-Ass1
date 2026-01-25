using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Online_Learning_Platform_Ass1.Service.DTOs.Lesson;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

public class AiLessonService( HttpClient httpClient, ITranscriptService transcriptService,
    IProgressRepository progressRepository,
    ILessonRepository lessonRepository ) : IAiLessonService
{
    private readonly HttpClient _http = httpClient;
    private readonly ITranscriptService _transcriptService = transcriptService;
    private readonly IProgressRepository _progressRepository = progressRepository;
    private readonly ILessonRepository _lessonRepository = lessonRepository;

    private const string _aiEndpoint = "https://api.groq.com/openai/v1/chat/completions";
    private readonly string _groqApiKey = Environment.GetEnvironmentVariable("GroqAPIKey__Key") ?? "";

    public async Task<string> GenerateSummaryAsync(ProgressDTO dto)
    {
        var progress =
            await _progressRepository.GetByIdAsync(dto.Id)
            ?? throw new Exception("Progress not found");

        if (progress.AiSummaryStatus == AiSummaryStatus.Completed)
            return progress.AiSummary!;

        if (progress.AiSummaryStatus == AiSummaryStatus.Pending)
            return "Đợi xíu, AI đang tóm tắt bài này...";

        progress.AiSummaryStatus = AiSummaryStatus.Pending;
        await _progressRepository.UpdateAsync(progress);

        try
        {
            var transcript = await EnsureTranscriptAsync(progress);
            var summary = await CallAiSummary(transcript);

            progress.AiSummary = summary;
            progress.AiSummaryStatus = AiSummaryStatus.Completed;
            progress.UpdatedAt = DateTime.UtcNow;

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

    public async Task<string> AskAsync(ProgressDTO dto, string question)
    {
        var progress =
            await _progressRepository.GetByIdAsync(dto.Id)
            ?? throw new Exception("Progress not found");

        var context =
            !string.IsNullOrWhiteSpace(progress.AiSummary)
                ? progress.AiSummary!
                : await EnsureTranscriptAsync(progress);

        return await CallAiAsk(context, question);
    }

    private async Task<string> EnsureTranscriptAsync(LessonProgress progress)
    {
        if (!string.IsNullOrWhiteSpace(progress.Transcript))
            return progress.Transcript!;

        var lesson =
            await _lessonRepository.GetByIdAsync(progress.LessonId)
            ?? throw new Exception("Lesson not found");

        if (string.IsNullOrWhiteSpace(lesson.ContentUrl))
            throw new Exception("Lesson has no video");

        var transcript =
            await _transcriptService.GenerateTranscriptFromVideoAsync(lesson.ContentUrl);

        progress.Transcript = transcript;
        progress.UpdatedAt = DateTime.UtcNow;

        await _progressRepository.UpdateAsync(progress);
        return transcript;
    }

    private Task<string> CallAiAsk(string context, string question)
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

        return SendToAi(payload);
    }

    private Task<string> CallAiSummary(string transcript)
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
                        "If the transcript is unclear or empty, respond exactly: " +
                        "'Không thể tóm tắt vì nội dung không rõ ràng.'"
                },
                new
                {
                    role = "user",
                    content = transcript
                }
            },
            temperature = 0.3
        };

        return SendToAi(payload);
    }

    private async Task<string> SendToAi(object payload)
    {
        var json = JsonSerializer.Serialize(payload);

        using var request = new HttpRequestMessage(HttpMethod.Post, AiEndpoint)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", _groqApiKey);

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        return doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString()!;
    }
}
