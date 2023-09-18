using MedBrainyAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedBrainyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageProcessorController : ControllerBase
    {
        private readonly ILogger<ImageProcessorController> _logger;
        private BoundingBoxService _boundingBoxService { get; set; }
        private OCRService _ocrService { get; set; }
        private OpenAIService _openAIService { get; set; }
        private AudioService _audioService { get; set; }

        public ImageProcessorController(ILogger<ImageProcessorController> logger, BoundingBoxService boundingBoxService, OCRService ocrService, OpenAIService openAIService, AudioService audioService)
        {
            _logger = logger;
            _boundingBoxService = boundingBoxService;
            _ocrService = ocrService;
            _openAIService = openAIService;
            _audioService = audioService;
        }

        [HttpPost(Name = "ImageProcessor")]
        public async Task<AudioServiceResult> Post([FromBody]ImageProcessor img)
        {
            var croppedImg = _boundingBoxService.GetBoundingBox(img.Base64Img);
            var ocrResult = await _ocrService.OCRText(croppedImg);
            var expiryDate = _openAIService.GetExpiryDate(ocrResult);
            var translation = await _audioService.Translate("The expiry Date is " + expiryDate, img.Language);
            _logger.LogInformation(ocrResult, expiryDate);

            return translation;
        }
    }

    public class ImageProcessor
    {
        public string Base64Img { get; set; }
        public string Language { get; set; }
    }
}