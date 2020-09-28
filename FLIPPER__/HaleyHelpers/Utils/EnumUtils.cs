using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Helpers.Utils
{
    public enum RestMethod
    {
        Post,
        Get,
        Put,
        Delete
    }

    public enum RestParamType
    {
        QueryString,
        RequestBody,
        Header
    }

    public enum ReturnFormat
    {
        Json,
        XML,
        None
    }
}
