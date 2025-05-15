using MnemosyneDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MnemosyneDomain.Commands.Images
{
    public class ImageCommandHandler
    {
        private readonly MnemosyneContext _context;

        public ImageCommandHandler(MnemosyneContext context) 
        { 
            _context = context;
        }

        public async Task<Guid> HandleAsync(SaveImage request, CancellationToken cancellationToken = default)
        {
            // TODO: generate GUID, write file to disk at configured location (create a configured location), figure out a short ID from the GUID
            // return short ID or some kind of image DTO
            Guid imageId = Guid.NewGuid();

            string fileLocation = @$"C:\mnemosyne\images\{imageId}{Path.GetExtension(request.FileName)}";

            using var writer = new FileStream(fileLocation, new FileStreamOptions { Mode = FileMode.OpenOrCreate, Access = FileAccess.Write });

            await request.Stream.CopyToAsync(writer, cancellationToken);

            writer.Flush();
            writer.Close();

            Models.Image image = new()
            {
                ImageId = Guid.NewGuid(),
                UserId = request.User.UserId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                FileLocation = fileLocation      
            };


            await _context.Images.AddAsync(image, cancellationToken);
            await _context.SaveChangesAsync();

            return image.ImageId;
        }
    }
}
