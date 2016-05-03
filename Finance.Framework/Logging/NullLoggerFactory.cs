using System;

namespace Finance.Framework.Logging {
    class NullLoggerFactory : ILoggerFactory {
        public ILogger CreateLogger(Type type) {
            return NullLogger.Instance;
        }
    }
}