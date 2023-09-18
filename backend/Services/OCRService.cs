using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace MedBrainyAPI.Services
{
    public class OCRService
    {
        static string key = "your-ocr-key";
        static string endpoint = "https://acs-hackathon.cognitiveservices.azure.com/";

        public async Task<string> OCRText(byte[] img)
        {
            ComputerVisionClient client = Authenticate(endpoint, key);
            string base64Image = Convert.ToBase64String(img);

            return await ReadFileBase64(client, base64Image);

        }
        public static async Task<string> ReadFileBase64(ComputerVisionClient client, string b64string)
        {
            byte[] imageBytes = Convert.FromBase64String(b64string);
            string ans = "";
            using (Stream imageStream = new MemoryStream(imageBytes))
            {
                try
                {
                    // Recognize text in the image
                    //OcrResult result = await client.RecognizePrintedTextInStreamAsync(false, imageStream);
                    OcrResult result = await client.RecognizePrintedTextInStreamAsync(false, imageStream);
                    // Extract and print the recognized text
                    Console.WriteLine("Recognized Text:");
                    foreach (OcrRegion region in result.Regions)
                    {
                        foreach (OcrLine line in region.Lines)
                        {
                            foreach (OcrWord word in line.Words)
                            {
                                Console.Write(word.Text + " ");
                                ans += word.Text + " ";
                            }
                            ans += "\n";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                return ans;
            }
        }

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new Microsoft.Azure.CognitiveServices.Vision.ComputerVision.ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }
    }
}
