using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Haley.WPF.Abstractions
{
    public interface IImageHolder
    {
        ImageSource DefaultImage { get; set; }
        ImageSource HoverImage { get; set; }
        ImageSource PressedImage { get; set; }
        Color DefaultImageColor { get; set; }
        Color HoverImageColor { get; set; }
        Color PressedImageColor { get; set; }
    }
}
