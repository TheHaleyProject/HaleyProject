using System;

namespace Haley.Enums
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
