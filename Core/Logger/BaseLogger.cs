using CoreUtilities.DI;
using CoreUtilities.Logger.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text;

namespace CoreUtilities.Logger
{

    public interface IBaseLogger
    {
        void LogDebug(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0);
        void LogDebug(string message, object entity, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0);
        void LogInformation(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0);
        void LogWarning(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0);
        void LogWarning(string message, Exception exception, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0);
        void LogError(string message, Exception exception, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0);
        void LogCritical(string message, Exception exception, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0);
    }

    [InjectDependency(typeof(IBaseLogger))]
    public class BaseLogger(ILogger<BaseLogger> logger) : IBaseLogger
    {
        private readonly ILogger _logger = logger;
        private static readonly JsonSerializerSettings ignoreConverter = new()
        {
            ContractResolver = new IgnorableSerializerContractResolver(new List<Type>() { typeof(DoNotLogAttribute), typeof(System.Text.Json.Serialization.JsonIgnoreAttribute) })
        };

        public void LogDebug(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                Require.NotNullOrEmpty(message, nameof(message));
                StringBuilder builder = new();
                BuildBase(builder, "Debug", caller, lineNumber);
                BuildCustomMessage(builder, message);
                _logger.LogDebug(builder.ToString());
            }
        }

        public void LogDebug(string message, object entity, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                Require.NotNullOrEmpty(message, nameof(message));
                StringBuilder builder = new();
                BuildBase(builder, "Debug", caller, lineNumber);
                BuildCustomMessage(builder, message);
                BuildObjectMessage(builder, entity);
                _logger.LogDebug(builder.ToString(), entity);
            }
        }

        public void LogInformation(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                Require.NotNullOrEmpty(message, nameof(message));
                StringBuilder builder = new();
                BuildBase(builder, "Information", caller, lineNumber);
                BuildCustomMessage(builder, message);
                _logger.LogInformation(builder.ToString());
            }
        }

        public void LogWarning(string message, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                Require.NotNullOrEmpty(message, nameof(message));
                StringBuilder builder = new();
                BuildBase(builder, "Warning", caller, lineNumber);
                BuildCustomMessage(builder, message);
                _logger.LogWarning(builder.ToString());
            }
        }

        public void LogWarning(string message, Exception exception, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                Require.NotNullOrEmpty(message, nameof(message));
                StringBuilder builder = new();
                BuildBase(builder, "Warning", caller, lineNumber);
                BuildCustomMessage(builder, message);
                BuildException(builder, exception);
                _logger.LogWarning(builder.ToString(), exception);
            }
        }

        public void LogError(string message, Exception exception, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                Require.NotNullOrEmpty(message, nameof(message));
                StringBuilder builder = new();
                BuildBase(builder, "Error", caller, lineNumber);
                BuildCustomMessage(builder, message);
                BuildException(builder, exception);
                _logger.LogError(builder.ToString(), exception);
            }
        }

        public void LogCritical(string message, Exception exception, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (_logger.IsEnabled(LogLevel.Critical))
            {
                Require.NotNullOrEmpty(message, nameof(message));
                StringBuilder builder = new();
                BuildBase(builder, "Critical", caller, lineNumber);
                BuildCustomMessage(builder, message);
                BuildException(builder, exception);
                _logger.LogCritical(builder.ToString(), exception);
            }
        }

        private static void BuildBase(StringBuilder builder, string logLevel, string caller, int lineNumber)
        {
            builder.AppendLine($"Log Level: {logLevel}");
            builder.AppendLine($"Log Time: {DateTime.UtcNow}");
            builder.AppendLine($"Caller: {caller} Line Number: {lineNumber}");
        }

        private static void BuildCustomMessage(StringBuilder builder, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            builder.AppendLine("Message:");
            builder.AppendLine(message);
        }

        private static void BuildObjectMessage(StringBuilder builder, object entity)
        {
            if (entity == null)
            {
                return;
            }
            builder.AppendLine("Object:");
            builder.AppendLine(JsonConvert.SerializeObject(entity, ignoreConverter));
        }

        private static void BuildException(StringBuilder builder, Exception exception)
        {
            builder.AppendLine("Exception:");
            builder.AppendLine(exception.ToString());
        }
    }
}
