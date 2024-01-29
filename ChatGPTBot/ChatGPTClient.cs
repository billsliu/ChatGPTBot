using Newtonsoft.Json;
using RestSharp;

namespace ChatGPTBot
{
    internal class ChatGPTClient
    {
        private readonly string _apiKey;
        private readonly RestClient _client;

        // Constructor that takes the API key as a parameter
        public ChatGPTClient(string apiKey)
        {
            _apiKey = apiKey;
            // Initialize the RestClient with the ChatGPT API endpoint
            _client = new RestClient("https://api.openai.com/v1/chat/completions");
        }

        // Method to send a message to the ChatGPT API and return the response
        public string SendMessage(string message)
        {
            // Check for empty input
            if (string.IsNullOrWhiteSpace(message))
            {
                return "Sorry, I didn't receive any input. Please try again!";
            }

            try
            {
                // Create a new POST request
                var request = new RestRequest("", Method.Post);
                // Set the Content-Type header
                request.AddHeader("Content-Type", "application/json");
                // Set the Authorization header with the API key
                request.AddHeader("Authorization", $"Bearer {_apiKey}");

                List<dynamic> messages = new List<dynamic>
                {
                    //new
                    //{
                    //    role = "system",
                    //    content = "You are ChatGPT, a large language "
                    //            + "model trained by OpenAI. "
                    //            + "Answer as concisely as possible.  "
                    //            + "Make a joke every few lines just to spice things up."
                    //},
                    new
                    {
                        role = "assistant",
                        content = message
                    }
                };

                var requestBody = new
                {
                    messages,
                    model = "gpt-3.5-turbo-1106", // gpt-3.5-turbo-1106 gpt-3.5-turbo gpt-4-1106-preview gpt-4
                    max_tokens = 100,
                    temperature = 0.6,
                };

                // Add the JSON body to the request
                request.AddJsonBody(JsonConvert.SerializeObject(requestBody));

                // Execute the request and receive the response
                var response = _client.Execute(request);

                // Deserialize the response JSON content
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content ?? string.Empty);

                // Extract and return the chatbot's response text
                return jsonResponse?.choices[0]?.message?.ToString()?.Trim() ?? string.Empty;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the API request
                Console.WriteLine($"Error: {ex.Message}");
                return "Sorry, there was an error processing your request. Please try again later.";
            }
        }
    }
}
