﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;
using System;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Haley MVVM")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Haley Project")]
[assembly: AssemblyProduct("Haley MVVM")]
[assembly: AssemblyCopyright("Copyright ©  2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("148e0a6a-d4b3-4526-a152-cce7f280db35")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("3.0.8.0")]
[assembly: AssemblyFileVersion("3.0.8.0")]

[assembly: XmlnsPrefix("http://schemas.hpod9.com/haley/mvvm", "hly")]
//FOR XAML NAMESPACES - MVVM
[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/mvvm", "Haley.Enums")]
[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/mvvm", "Haley.MVVM.Converters")]
[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/mvvm", "Haley.Abstractions")]
[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/mvvm", "Haley.Models")]
[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/mvvm", "Haley.MVVM")]

//FOR XAML NAMESPACES - WPF
[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/wpf", "Haley.WPF.ViewModels")]
[assembly: XmlnsDefinition("http://schemas.hpod9.com/haley/wpf", "Haley.WPF.Views")]
