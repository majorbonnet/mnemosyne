using JasperFx.Core;
using Microsoft.AspNetCore.Mvc;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Commands.Images;
using Wolverine;
using Wolverine.Http;

namespace MnemosyneApi.Endpoints
{
    public static class ImageEndpoints
    {
        private static string[] ValidFileExtensions =
        {
            ".png",
            ".jpg",
            ".jpeg",
            ".gif"
        };

        public class Image
        {
            public Guid ImageId { get; set; }
        }

        [WolverinePost("/api/images")]
        public static async Task<Image> UploadImage(IFormFile file, IMessageBus bus, [NotBody] User user, CancellationToken cancellationToken)
        {
            if (!ValidFileExtensions.Contains(Path.GetExtension(file.FileName))) throw new ArgumentException("Only .png, .jpg, .jpeg, .gif extensions allowed");

            Guid imageId = await bus.InvokeAsync<Guid>(new SaveImage(
                user,
                file.OpenReadStream(),
                file.FileName,
                file.ContentType));

            return new Image { ImageId = imageId };
        }

        [WolverineGet("/api/images/{imageId}")]
        public static async Task<IActionResult> GetImage(Guid imageId, [NotBody] User user)
        {
            FileStream fileStream = new FileStream(@$"C:\testfile\text.txt", new FileStreamOptions { Mode = FileMode.OpenOrCreate });

            return new FileContentResult(await fileStream.ReadAllBytesAsync(), "image");
        }
    }
}
