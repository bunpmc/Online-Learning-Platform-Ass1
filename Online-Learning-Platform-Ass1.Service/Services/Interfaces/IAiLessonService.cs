using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Learning_Platform_Ass1.Service.Services.Interfaces;
public interface IAiLessonService
{
    Task<string> AskAsync(string transcript, string question);

    Task<string> GenerateSummaryAsync(string? content, string? videoUrl);
}
