using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Drawing.Imaging;

namespace MedBrainyAPI.Services
{
    public class BoundingBoxService
    {
        private readonly string trainingEndpoint = "https://cvmedbrainy.cognitiveservices.azure.com/";
        private readonly string trainingKey = "Your-Bounding-Box-Training-Key";
        private readonly string predictionEndpoint = "https://cvmedbrainy-prediction.cognitiveservices.azure.com/";
        private readonly string predictionKey = "Your-Bounding-Box-Prediction-Key";
        private readonly string predictionResourceId = "your-prediction-resource-id";
        private CustomVisionPredictionClient predictionApi { get; set; }
        ILogger<BoundingBoxService> _logger { get; set; }

        public BoundingBoxService(ILogger<BoundingBoxService> logger)
        {
            _logger = logger;
        }

        CustomVisionPredictionClient AuthenticatePrediction(string endpoint, string predictionKey)
        {
            // Create a prediction endpoint, passing in the obtained prediction key
            CustomVisionPredictionClient predictionApi = new CustomVisionPredictionClient(new ApiKeyServiceClientCredentials(predictionKey))
            {
                Endpoint = endpoint
            };
            return predictionApi;
        }
        public byte[] GetBoundingBox(string base64Img)
        {
            byte[] imageBytes = Convert.FromBase64String(base64Img);
            predictionApi = AuthenticatePrediction(predictionEndpoint, predictionKey);
            return TestIteration(imageBytes);
        }
        byte[] TestIteration(byte[] imageBytes)
        {
            Console.WriteLine("Making a prediction:");
            ImagePrediction result;
            PredictionModel bestPrediction;
            System.Drawing.Image originalImage;
            using (Stream testImage = new MemoryStream(imageBytes))
            {
                originalImage = System.Drawing.Image.FromStream(testImage);
            }
            using (Stream testImage = new MemoryStream(imageBytes))
            {
                result = predictionApi.DetectImage(Guid.Parse("your-project-id"), "MedBrainy-1", testImage);
                bestPrediction = result.Predictions.MaxBy(bb => bb.Probability);
                byte[] croppedImg = CropImage(imageBytes, new((int)(bestPrediction.BoundingBox.Left*originalImage.Width), (int)(bestPrediction.BoundingBox.Top*originalImage.Height), (int)Math.Ceiling(bestPrediction.BoundingBox.Width*originalImage.Width), (int)Math.Ceiling(bestPrediction.BoundingBox.Height*originalImage.Height)));
                return croppedImg;
            }
        }
        public static byte[] CropImage(byte[] sourceImageByteArr, Rectangle cropRectangle)
        {
            using (MemoryStream sourceStream = new MemoryStream(sourceImageByteArr))
            {
                Bitmap sourceImage = System.Drawing.Image.FromStream(sourceStream) as Bitmap;
                using (MemoryStream targetStream = new MemoryStream())
                {
                    using (var targetImage = new Bitmap(cropRectangle.Width, cropRectangle.Height))
                    {
                        using (Graphics g = Graphics.FromImage(targetImage))
                        {
                            g.DrawImage(sourceImage, new Rectangle(0, 0, targetImage.Width, targetImage.Height), cropRectangle, GraphicsUnit.Pixel);
                        }

                        targetImage.Save(targetStream, ImageFormat.Png);
                        return targetStream.ToArray();
                    }
                }
            }
        }
    }
}
