using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Chat;
using System.IO;

namespace CApp.Services
{
    public class OpenAIService
    {
        private readonly ChatClient _chatClient;

        public OpenAIService(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            throw new ArgumentNullException("API key not configured.");

            // Create ChatClient with model and API key
            _chatClient = new ChatClient(
                model: "gpt-4o-mini",
                apiKey: apiKey
            );
        }

        public async Task<string> GetDoctorRecommendation(string searchInput)
        {
            var messages = new List<ChatMessage>
            {
                 new UserChatMessage($" {searchInput}  ")
            };

            // Here CompleteChatAsync returns ChatCompletion (not ClientResult)
            ChatCompletion completion = await _chatClient.CompleteChatAsync(messages);

            // Now read the response
            return completion.Content[0].Text;
        }

          // -----------------------
        // 2️⃣ Code Review by File
        // -----------------------
        public async Task<string> ReviewCodeFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Code file not found", filePath);

            string codeContent = await File.ReadAllTextAsync(filePath);

            var messages = new List<ChatMessage>
            {
                new UserChatMessage(
                    $"Please review this code file '{Path.GetFileName(filePath)}' for correctness, best practices, potential bugs, and improvements:\n\n{codeContent}"
                )
            };

            ChatCompletion completion = await _chatClient.CompleteChatAsync(messages);
            return completion.Content[0].Text;
        }
    }
}
