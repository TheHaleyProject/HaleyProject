using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions
{
    //Both target and parameters are runtime arguments
    public delegate AxiomResponse AxiomAction<T>(T target, params object[] parameters);
}
