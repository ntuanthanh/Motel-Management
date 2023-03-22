using System.IO;
using System.Text;

namespace MotelManagement.Common
{
    public class UploadFileUnit
    {
        private readonly IWebHostEnvironment _env;

        public UploadFileUnit(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<string[]> UploadFile(IFormFile[] Files, string PrefixImage, int? MaxNumber)
        {
            if (Files.Length == 0) return null;
            string RootPath = _env.WebRootPath + "\\upload\\";
            string[] result = new string[Files.Length];
            int CountFiles = 0;
            if(MaxNumber==null) MaxNumber = Files.Length;
            string imageName = "";
            foreach (IFormFile File in Files)
            {
                CountFiles += 1;
                try
                {
                    string FileExtension = System.IO.Path.GetExtension(File.FileName);
                    imageName = PrefixImage + Guid.NewGuid().ToString() + FileExtension;
                    string FileName = RootPath + imageName;
                    using (var FileStream = new FileStream(FileName, FileMode.Create))
                    {
                        await File.CopyToAsync(FileStream);
                    }
                    result[CountFiles - 1] = imageName;
                    if (CountFiles == MaxNumber) break;
                }
                catch(Exception ex) {
                    result[CountFiles - 1] = null;
                }
            }
            return result;
        }
    }
}
