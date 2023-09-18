using Microsoft.CognitiveServices.Speech;
using System.Text;
using System.Text.Json;

namespace MedBrainyAPI.Services
{
    public class AudioService
    {
        private static readonly string translatorKey = "Your-Translator-key";
        private static readonly string speechKey = "your-speech-key";
        private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com";

        // location, also known as region.
        // required if you're using a multi-service or regional (not global) resource. It can be found in the Azure portal on the Keys and Endpoint page.
        private static readonly string location = "eastus";

        public async Task<AudioServiceResult> Translate(string textToTranslate, string language)
        {
            string route = "/translate?api-version=3.0&from=en&to=" + TranslatorConfig(language);
            object[] body = new object[] { new { Text = textToTranslate } };
            var requestBody = JsonSerializer.Serialize(body);

            string translationResult = "";
            List<TranslatedText> translatedText;

            AudioServiceResult result = new();
            result.originalText = textToTranslate;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", translatorKey);
                // location required if you're using a multi-service or regional (not global) resource.
                request.Headers.Add("Ocp-Apim-Subscription-Region", location);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                translationResult = await response.Content.ReadAsStringAsync();

                translatedText = JsonSerializer.Deserialize<List<TranslatedText>>(translationResult);
                result.translatedText = translatedText[0].translations[0].text;

            }

            var speechConfig = SpeechConfig.FromSubscription(speechKey, location);

            // The language of the voice that speaks.
            speechConfig.SpeechSynthesisVoiceName = SpeechConfiguration(language);

            using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
            {
                string text = translatedText[0].translations[0].text;

                var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
                var x = Convert.ToBase64String(speechSynthesisResult.AudioData, 0, speechSynthesisResult.AudioData.Length);
                result.audioBase64 = x;
            }
            return result;

        }
        static string SpeechConfiguration(string language)
        {
            switch (language.ToLower())
            {
                case "hindi":
                    return "hi-IN-SwaraNeural";
                case "bengali":
                    return "bn-IN-TanishaaNeural";
                case "gujarati":
                    return "gu-IN-DhwaniNeural";
                case "kannada":
                    return "kn-IN-SapnaNeural";
                case "malayalam":
                    return "ml-IN-SobhanaNeural";
                case "marathi":
                    return "mr-IN-AarohiNeural";
                case "tamil":
                    return "ta-IN-PallaviNeural";
                case "telugu":
                    return "te-IN-ShrutiNeural";
                case "urdu":
                    return "ur-IN-GulNeural";
                default:
                    return "en-IN-NeerjaNeural";
            }
        }

        static string TranslatorConfig(string language)
        {
            switch (language.ToLower())
            {
                case "hindi":
                    return "hi";
                case "bengali":
                    return "bn";
                case "gujarati":
                    return "gu";
                case "kannada":
                    return "kn";
                case "malayalam":
                    return "ml";
                case "marathi":
                    return "mr";
                case "tamil":
                    return "ta";
                case "telugu":
                    return "te";
                case "urdu":
                    return "ur";
                default:
                    return "en";
            }
        }
    }
    public class TranslatedText
    {
        public List<Translation> translations { get; set; }
    }

    public class Translation
    {
        public string text { get; set; }

        public string to { get; set; }
    }
    public class AudioServiceResult
    {
        public string translatedText { get; set; }
        public string audioBase64 { get; set; }
        public string originalText { get; set; }
    }
}
