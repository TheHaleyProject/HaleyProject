using System;
using System.Collections.Generic;
using System.Text;

namespace Haley.Log.Interfaces
{
    public enum MessageType
    {
        information = 0,
        warning,
        error,
        property,
        exception,
        debug
    }

    public enum OutputType
    {
        txt_simple,
        txt_detailed,
        xml,
        json
    }

    public enum LogType
    {
        app,
        user,
        config,
        history,
    }
}
