using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace Surging.Core.KestrelHttpServer
{
    public class ImageResult : FileResult
    {
        private byte[] _fileContents;

        public ImageResult(byte[] fileContents, string contentType)
            : this(fileContents, MediaTypeHeaderValue.Parse(contentType))
        {
            if (fileContents == null)
            {
                throw new ArgumentNullException(nameof(fileContents));
            }
        }

        public ImageResult(byte[] fileContents, MediaTypeHeaderValue contentType)
            : base(contentType?.ToString())
        {
            if (fileContents == null)
            {
                throw new ArgumentNullException(nameof(fileContents));
            }

            FileContents = fileContents;
        }

        public byte[] FileContents
        {
            get => _fileContents;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _fileContents = value;
            }
        }
        public async override Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            try
            {
                //从图片中读取流
                using (var imgStream = new MemoryStream(FileContents)) 
                {
                    var httpResponse = context.HttpContext.Response;
                    httpResponse.Headers.Add("Content-Type", this.ContentType);
                    httpResponse.Headers.Add("Content-Length", FileContents.Length.ToString());
                    await httpResponse.Body.WriteAsync(FileContents, 0, FileContents.Length);
                    httpResponse.Body.Close();
                }
                
            }
            catch (OperationCanceledException)
            {
                context.HttpContext.Abort();
            }
        }
    }
}
