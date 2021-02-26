using System.Windows.Media;

namespace Haley.WPF.Abstractions
{
    public interface IShadow
    {
        bool ShadowOnlyOnMouseOver { get; set; }
        bool ShowShadow { get; set; }
        Color ShadowColor { get; set; }
    }
}
