using System;

namespace Finance.Framework.Logging {
    public interface ILoggerFactory {
        ILogger CreateLogger(Type type);
    }
}