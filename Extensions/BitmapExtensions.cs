using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Extensions
{
    public static class BitmapExtensions
    {
        public static byte[] ToByteArray(this Bitmap img)
        {
            if (img == null) return null;
            byte[] byteArray;
            using (var stream = new MemoryStream())
            {
                var codecInfo = ImageCodecInfo.GetImageDecoders().FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
                var parameters = new EncoderParameters(1);
                parameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                img.Save(stream, codecInfo, parameters);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        public static string ToBase64(this Bitmap img)
        {
            return Convert.ToBase64String(img.ToByteArray());
        }
    }
}
