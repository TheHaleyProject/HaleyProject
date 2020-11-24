using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using Haley.Abstractions;
using Haley.Enums;
using Haley.Models;
using Haley.Utils;

namespace Haley.Log
{
    public sealed class LogStore
    {
        private ConcurrentDictionary<string, ILogger> _loggers = new ConcurrentDictionary<string, ILogger>();

        public ILogger BaseLog { get; set; } //This is the singleton logger to be used.
        private static LogStore _singleton;
        public static LogStore Singleton { get { return _singleton; } }

        public static void CreateSingleton(ILogger sourceLog)
        {
            _singleton = new LogStore();
            _singleton.BaseLog = sourceLog;
        }
        public ILogger logger(Enum @enum)
        {
            string _key = @enum.getKey();
            return logger(_key);
        }

        public ILogger logger(string key)
        {
                if(_loggers.ContainsKey(key))
                {
                    ILogger _result = null;
                    _loggers.TryGetValue(key, out _result);
                    return _result;
                }
            return null;
        }

        public bool AddLog(ILogger source,Enum @enum)
        {
            return AddLog(source, @enum.getKey());
        }

        public bool AddLog(ILogger source,  string key)
        {
            if (_loggers.ContainsKey(key)) return false;
            return _loggers.TryAdd(key, source);
        }

        public LogStore() { }
        }
    }
