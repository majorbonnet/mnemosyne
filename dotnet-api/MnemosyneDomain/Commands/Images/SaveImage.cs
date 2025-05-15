using MnemosyneDomain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Commands.Images
{
    public class SaveImage(User user, Stream stream, string fileName, string contentType) : BaseRequest
    {
        public User User => user;
        public Stream Stream => stream;
        public string FileName => fileName;
        public string ContentType => contentType;
    }
}
