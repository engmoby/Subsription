using System;
using System.Drawing;
using System.IO;

namespace Subscription.API.Infrastructure
{
    public class ImageConvert
    {
        public string GetBase64FromImage(string path)
        {
            using (Image image = Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return "data:image/jpeg;base64,"+base64String;
                }
            }
        }
    }
}