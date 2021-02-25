using System.Windows;
using System.Windows.Markup;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

[assembly: XmlnsPrefix("http://schemas.hpod9.com/haley/wpf", "hlyWPF")]

//FOR XAML NAMESPACES - WPF
//[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/wpf", "Haley.WPF.ViewModels")]
//[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/wpf", "Haley.WPF.Views")]
[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/wpf", "Haley.WPF.BaseControls")]
[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/wpf", "Haley.WPF")]