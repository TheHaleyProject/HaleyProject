﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Haley.Enums;
using Haley.Models;
using System.Windows.Media.Imaging;
using System.Windows.Media;

//STRIDE: The stride is the width of a single row of pixels (a scan line), rounded up to a four-byte boundary. If the stride is positive, the bitmap is top-down. If the stride is negative, the bitmap is bottom-up.

// To calculate stride, the formula we generally use is, (Bitsperpixel + 7 )/8 . If we see carefully, this gives us a decimal value. But our main idea is to get integer.
// We have 
// 1 byte (Mono chrome, 8 bits)
// 2 byte(Dual Color 16 bits)
// 3 byte(24 bits)
// 4 byte(32 bits)..We can also have 64 bits, 128 bits etc..

// Example:

// Monochrome: 8 bits = (8 + 7) / 8 = 1.875 = 1 byte
//     Dual : 16 bits = (16 + 7) / 8 = 2.875 = 2 byte.


namespace Haley.Utils
{
    public sealed class ImageUtils
    {
        #region Public Methods

    #region RESIZE
        public static ImageSource resizeImage(
            string path,
            int pixel_height,
            int pixel_width,
            ResizeAffectMode resize_mode = ResizeAffectMode.PixelOnly,
            double dpi = 0)
        {
            try
            {
                Uri image_uri = new Uri(path, UriKind.RelativeOrAbsolute);
                return resizeImage(image_uri, pixel_height, pixel_width, resize_mode, dpi);
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static ImageSource resizeImage(
            Uri path_URI,
            int pixel_height,
            int pixel_width,
            ResizeAffectMode resize_mode = ResizeAffectMode.PixelOnly,
            double dpi = 0)
        {
            try
            {
                BitmapImage image = new BitmapImage(path_URI);
                return resizeImage(image, pixel_height, pixel_width, resize_mode, dpi);
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static ImageSource resizeImage(
            Image image,
            int pixel_height,
            int pixel_width,
            ResizeAffectMode resize_mode = ResizeAffectMode.PixelOnly,
            double dpi = 0)
        {
            try
            {
                bool fit_width = _shouldFitWidth(Convert.ToDouble(image.Height), Convert.ToDouble(image.Width));
                return resizeImage(imageToByte(image), pixel_height, pixel_width, resize_mode, dpi);
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static ImageSource resizeImage(
            byte[] image_byte_array,
            int pixel_height,
            int pixel_width,
            ResizeAffectMode resize_mode = ResizeAffectMode.PixelOnly,
            double dpi = 0)
        {
            try
            {
                BitmapImage result_image = new BitmapImage();
                using(MemoryStream mstream = new MemoryStream(image_byte_array))
                {
                    result_image.BeginInit();
                    result_image.StreamSource = mstream;
                    result_image.EndInit();
                }
                return resizeImage((ImageSource)result_image, pixel_height, pixel_width, resize_mode, dpi);
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static ImageSource resizeImage(
            ImageSource input_image,
            int pixel_height,
            int pixel_width,
            ResizeAffectMode resize_mode = ResizeAffectMode.PixelOnly,
            double dpi = 0)
        {
            try
            {
                bool fit_width = _shouldFitWidth(input_image.Height, input_image.Width);

                BitmapSource base_source = input_image as BitmapSource; //Bitmap source is the base of all imagesources. So, we can convert them to bitmapsource without issue.
                if(base_source == null)
                    return null;

                switch(resize_mode)
                {
                    case ResizeAffectMode.PixelOnly:
                        return _resizeImage_pixelOnly(base_source, pixel_height, pixel_width, fit_width);
                    case ResizeAffectMode.PixelAndImage:
                        return _resizeImage_pixelImage(base_source, pixel_height, pixel_width, fit_width, dpi);
                    case ResizeAffectMode.PixelAndDPI:
                        return _resizeImage_pixelDpi(base_source, pixel_height, pixel_width, fit_width, dpi);
                }
                return null;
            } catch(Exception ex)
            {
                throw ex;
            }
        }
    #endregion

    #region CONVERSION
        public static byte[] imageToByte(Image input_image)
        {
            try
            {
                using(MemoryStream mstream = new MemoryStream())
                {
                    input_image.Save(mstream, input_image.RawFormat);
                    return mstream.ToArray();
                }
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] imageSourceToByte(ImageSource input_image)
        {
            try
            {
                BitmapSource base_source = input_image as BitmapSource; //Bitmap source is the base of all imagesources. So, we can convert them to bitmapsource without issue.
                if(base_source != null)
                {
                    using(MemoryStream mstream = new MemoryStream())
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder(); //Whatever the input file format is, we are trying to decode it using png encoder. Check if it will work.
                        encoder.Frames.Add(BitmapFrame.Create(base_source));
                        encoder.Save(mstream);
                        return mstream.ToArray();
                    }
                }
                return null;
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static Image byteToImage(byte[] input_byte_array)
        {
            try
            {
                Image result;

                using(MemoryStream memstream = new MemoryStream(input_byte_array))
                {
                    result = Image.FromStream(memstream);
                }
                return result;
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static ImageSource byteToImageSource(byte[] input_byte_array)
        {
            try
            {
                BitmapImage result = new BitmapImage();
                using(MemoryStream memstream = new MemoryStream(input_byte_array))
                {
                    result.BeginInit();
                    result.StreamSource = memstream;
                    result.EndInit();
                    result.Freeze(); //FREEZING
                }
                return result;
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static string imageToBase64(Image input_image)
        {
            try
            {
                byte[] _byte_array = imageToByte(input_image);
                return Convert.ToBase64String(_byte_array);
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static string imageSourceToBase64(ImageSource input_image_source)
        {
            try
            {
                byte[] _byte_array = imageSourceToByte(input_image_source);
                return Convert.ToBase64String(_byte_array);
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static Image base64ToImage(string input_string)
        {
            try
            {
                if(!isBase64(input_string))
                    return null; //If not a base 64 string, return null.
                byte[] _byte_array = Convert.FromBase64String(input_string);
                return byteToImage(_byte_array);
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static ImageSource base64ToImageSource(string input_string)
        {
            try
            {
                if(!isBase64(input_string))
                    return null; //If not a base 64 string, return null.
                byte[] byte_array = Convert.FromBase64String(input_string);
                return byteToImageSource(byte_array);
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static Image imageSourceToImage(ImageSource input_image_source)
        {
            try
            {
                byte[] byte_array = imageSourceToByte(input_image_source);
                return byteToImage(byte_array);
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static ImageSource imageToImageSource(Image input_image)
        {
            try
            {
                byte[] byte_array = imageToByte(input_image);
                ImageSource result = byteToImageSource(byte_array);
                result.Freeze();
                return result;
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static ImageSource bitmapToImageSource(Bitmap input_bitmap)
        {
            try
            {
                var bitmap_data = input_bitmap.LockBits(
                    new Rectangle(0, 0, input_bitmap.Width, input_bitmap.Height),
                    ImageLockMode.ReadOnly,
                    input_bitmap.PixelFormat);
                BitmapSource _result = BitmapSource.Create(
                    bitmap_data.Width,
                    bitmap_data.Height,
                    input_bitmap.HorizontalResolution,
                    input_bitmap.VerticalResolution,
                    PixelFormats.Bgr32,
                    null,
                    bitmap_data.Scan0,
                    bitmap_data.Stride * bitmap_data.Height,
                    bitmap_data.Stride);
                input_bitmap.UnlockBits(bitmap_data);
                return _result;
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] imageSourceToPixels(ImageSource input_image)
        {
            try
            {
                BitmapSource bmap_source = (BitmapSource)input_image;
                BitmapImage bmap_image = (BitmapImage)bmap_source;
                int bytes_per_pixel = (bmap_image.Format.BitsPerPixel + 7) / 8; //See notes to understand how this is calculated
                int _stride = bmap_image.PixelWidth * bytes_per_pixel; //Gets the stride (width of single row of pixel). Number of bytes that a single row can contain
                byte[] pixel_array = new byte[bmap_image.PixelHeight * _stride]; //array should be sufficient to copy all pixel value from the bitmap image
                bmap_image.CopyPixels(pixel_array, _stride, 0);
                return pixel_array;
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static ImageSource pixelsToImageSource(
            int _pixel_width,
            int _pixel_height,
            byte[] _pixels,
            System.Windows.Media.PixelFormat _format,
            double dpiX = 96,
            double dpiY = 96,
            BitmapPalette referenced_palette = null)
        {
            try
            {
                //Calculate stride
                int bytes_per_pixel = (_format.BitsPerPixel + 7) / 8;
                int _stride = _pixel_width * bytes_per_pixel; //Number of bytes per row of width
                BitmapSource bmap_source = BitmapSource.Create(
                    _pixel_width,
                    _pixel_height,
                    dpiX,
                    dpiY,
                    _format,
                    referenced_palette,
                    _pixels,
                    _stride); //Sometimes the byte index values may refer some palette. Based on the palette, the color will differ. So if you know that pass that as well, else it will be null and default palette is used.
                bmap_source.Freeze();
                return bmap_source;
            } catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ColorChanger
        public static ImageInfo getImageInfo(ImageSource source)
        {
            try
            {
                BitmapSource _input = source as BitmapSource;
                ImageInfo res = new ImageInfo();
                res.source = _input;
                res.stride = _input.PixelWidth * ((_input.Format.BitsPerPixel + 7) / 8);
                res.length = res.stride * _input.PixelHeight; //Width  * height ( in pixel)
                res.pixel_height = _input.PixelHeight;
                res.pixel_width = _input.PixelWidth;
                res.dpiX = _input.DpiX;
                res.dpiY = _input.DpiY;
                res.format = _input.Format;
                //res.metadata = _input?.Metadata as BitmapMetadata;
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ImageSource cloneImage(ImageSource source, byte[] new_array = null)
        {
            return cloneImage(getImageInfo(source), new_array);
        }

        public static ImageSource cloneImage(ImageInfo source, byte[] new_array = null)
        {
            try
            {
                if (new_array == null)
                {
                    new_array = new byte[source.length]; //matching the length of the source. //THIS IS AN EMPTY ARRAY
                }
                //The new array should match the length of the source. if not throw exception.

                BitmapSource res = BitmapSource.Create(source.pixel_width, source.pixel_height, source.dpiX, source.dpiY, source.format, null, new_array, source.stride);
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ImageSource changeImageColor(ImageInfo source, int red,int green, int blue)
        {
            try
            {
                //Ensure that the color values are with in the allowed range.
                resetColorLimits(ref red);
                resetColorLimits(ref green);
                resetColorLimits(ref blue);

                //Now using the imageinfo source, create a new array and fill it with the input color.
                byte[] newimage = new byte[source.length];

                //We are merely going to refill all the RGB values with the input colors. But remember, we are not changing the transparency. So, it is better to retain the transparency from the parent. So, copy the pixels (only for the transparency values).

                source.source.CopyPixels(newimage, source.stride, 0); //This copies all values. but we replace all the colors below.

                for (int i = 0; i < newimage.Length; i+=4) //increment by 4 number
                {
                    newimage[i + (int)RGB.Red] = Convert.ToByte(red);
                    newimage[i + (int)RGB.Green] = Convert.ToByte(green);
                    newimage[i + (int)RGB.Blue] = Convert.ToByte(blue);
                }

                //At this point, we have obtained an array with transparency (if available).

                return cloneImage(source, newimage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ImageSource changeImageColor(ImageSource source, SolidColorBrush brush)
        {
            try
            {
                var newcolor = brush.Color;
                var imageinfo = getImageInfo(source);
                var res = changeImageColor(imageinfo, int.Parse(newcolor.R.ToString()), int.Parse(newcolor.G.ToString()), int.Parse(newcolor.B.ToString()));
                return res;
            }
            catch (Exception)
            {
                return source; //In case of error, just reuse the source image itself.
            }
        }
        private static void resetColorLimits(ref int actual)
        {
            if (actual > 255) actual = 255;
            if (actual < 0) actual = 0;
        }

        #endregion


        public static bool saveImageSource(ImageSource input_image, string file_path, BitmapEncoder encoder = null)
        {
            try
            {
                BitmapSource base_source = input_image as BitmapSource; //Bitmap source is the base of all imagesources. So, we can convert them to bitmapsource without issue.
                base_source.Freeze();
                if(base_source != null)
                {
                    using(FileStream file_stream = new FileStream(file_path, FileMode.Create))
                    {
                        if(encoder == null)
                            encoder = new PngBitmapEncoder(); //Whatever the input file format is, we are trying to decode it using png encoder. Check if it will work.
                        encoder.Frames.Add(BitmapFrame.Create(base_source));
                        encoder.Save(file_stream);
                    }
                }
                return true;
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        public static bool isBase64(string input_string)
        {
            try
            {
                //Step 1: Check basic items.
                if(string.IsNullOrEmpty(input_string) ||
                    input_string.Length % 4 != 0 ||
                    input_string.Contains(" ") ||
                    input_string.Contains("\t") ||
                    input_string.Contains("\r") ||
                    input_string.Contains("\n"))
                    return false;

                //Step 2 : Try to convert it to base 64. It if fails, return false.
                try
                {
                    Convert.FromBase64String(input_string);
                    return true;
                } catch(Exception)
                {
                    return false;
                }
            } catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Private Methods
        private static ImageSource _resizeImage_pixelOnly(
            BitmapSource base_source,
            int pixel_height,
            int pixel_width,
            bool fit_width,
            bool maintain_ratio = true)
        {
            try
            {
                BitmapImage result = new BitmapImage();
                //If you enclose the stream inside mstream, then it is disposed and cannot be accessed
                MemoryStream mstream = new MemoryStream(); //KEEPING THE STREAM ALIVE.
                //Fetch data from base source into the stream.
                BitmapEncoder encoder = new PngBitmapEncoder(); //Whatever the input file format is, we are trying to decode it using png encoder. Check if it will work.
                encoder.Frames.Add(BitmapFrame.Create(base_source));
                encoder.Save(mstream);

                //Save that data into the new image.
                result.BeginInit();
                result.StreamSource = mstream; //empty image is filled with stream from base source
                if(fit_width)
                {
                    result.DecodePixelWidth = pixel_width;
                } else
                {
                    result.DecodePixelHeight = pixel_height;
                }
                result.EndInit();
                return result;
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        private static ImageSource _resizeImage_pixelImage(
            BitmapSource base_source,
            int pixel_height,
            int pixel_width,
            bool fit_width,
            double dpi,
            bool maintain_ratio = true)
        {
            //REMEMBER, IF WE TRY TO SET DPI, HEIGHT/WIDTH, PIXELHEIGHT/PIXELWIDTH, we are no constraints. We are explicitly setting values for all.
            //We need to create the empty frame to suit the pixels
            try
            {
                if(dpi == 0)
                    dpi = base_source.DpiX; //Dpi cannot be zero.

                //SET IMAGESIZE
                double adjusted_height = pixel_height;
                double adjusted_width = pixel_width;
                switch(fit_width)
                {
                    case true:
                        //Meaning that the image width is longer than the image height. So, we need to modify our new width,height, accordingly.
                        adjusted_width = pixel_width;
                        adjusted_height = (base_source.PixelHeight * adjusted_width) / base_source.PixelWidth;
                        break;
                    case false: //Height is longer. So modify width
                        adjusted_height = pixel_height;
                        adjusted_width = (adjusted_height * base_source.PixelWidth) / base_source.PixelHeight;
                        break;
                }

                Rect empty_rectangle_frame = new Rect(0, 0, adjusted_width, adjusted_height);
                DrawingVisual drawing_visual = new DrawingVisual();
                using(DrawingContext drawing_context = drawing_visual.RenderOpen())
                {
                    drawing_context.DrawImage(base_source, empty_rectangle_frame); //Draw the input image inside the empty rectangular frame.
                }

                //SET DPI, PixelOnly
                RenderTargetBitmap render_bitmap = new RenderTargetBitmap(
                    (int)Math.Round(adjusted_width),
                    (int)Math.Round(adjusted_height),
                    0,
                    0,
                    PixelFormats.Default);
                //RenderTargetBitmap render_bitmap = new RenderTargetBitmap(base_source.PixelWidth, base_source.PixelHeight, dpi, dpi, PixelFormats.Default);

                render_bitmap.Render(drawing_visual);
                var dpi_adjusted_image_source = (BitmapSource)render_bitmap;

                return dpi_adjusted_image_source;
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        private static ImageSource _resizeImage_pixelDpi(
            BitmapSource base_source,
            int pixel_height,
            int pixel_width,
            bool fit_width,
            double dpi,
            bool maintain_ratio = true)
        {
            try
            {
                //CHANGE PIXEL SIZE FIRST AND COPY THE DATA EXCEPT DPI
                BitmapSource pixel_modified_source = _resizeImage_pixelOnly(
                    base_source as BitmapSource,
                    pixel_height,
                    pixel_width,
                    fit_width) as BitmapSource;

                //COPYING ALL VALUES, EXCEPT THE DPI.
                if(dpi == 0)
                    dpi = base_source.DpiX;
                int _stride = (pixel_modified_source.PixelWidth * pixel_modified_source.Format.BitsPerPixel + 7) / 8;
                byte[] empty_pixel_array = new byte[pixel_modified_source.PixelHeight * _stride]; //This has same number of bytes as that of the input base image to hold the new pixel with modified dpi
                pixel_modified_source.CopyPixels(empty_pixel_array, _stride, 0);
                //ImageSource dpi_adjusted_image_source = BitmapSource.Create(base_source.PixelWidth, base_source.PixelHeight, dpi, dpi, base_source.Format, null, empty_pixel_array, _stride);

                ImageSource result = BitmapSource.Create(
                    pixel_modified_source.PixelWidth,
                    pixel_modified_source.PixelHeight,
                    dpi,
                    dpi,
                    pixel_modified_source.Format,
                    null,
                    empty_pixel_array,
                    _stride);
                return result;
            } catch(Exception ex)
            {
                throw ex;
            }
        }

        private static bool _shouldFitWidth(double source_image_height, double source_image_width)
        {
            try
            {
                //Check which is long. Longer side should fit.
                if(source_image_height > source_image_width)
                    return false; //Height is greater, height should fit inside
                return true; //Else width is greater or equal. In both case, we can fit the width.
            } catch(Exception ex)
            {
                throw ex;
            }
        }
    #endregion

        #region Asynchronous SubClass
        public sealed class Async
        {
        }
        #endregion
    }
}
