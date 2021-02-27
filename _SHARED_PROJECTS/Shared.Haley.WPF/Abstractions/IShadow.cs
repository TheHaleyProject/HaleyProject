using System.Windows.Media;

namespace Haley.WPF.Abstractions
{
    public interface IShadow
    {
        bool ShadowOnlyOnMouseOver { get; set; }
        bool ShowShadow { get; set; }
        Brush ShadowColor { get; set; }
    }
}
