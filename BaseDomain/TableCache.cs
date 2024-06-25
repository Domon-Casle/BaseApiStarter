using BaseDomain.Attributes;
using CoreUtilities;
using System.Reflection;

namespace BaseDomain
{
    public class TableCache
    {
        internal FieldInfo[] Fields { get; private set; } = [];
        internal string Name { get; private set; } = string.Empty;
        internal bool LogRead { get; private set; } = false;
        internal bool LogCreate { get; private set; } = false;
        internal bool LogUpdate { get; private set; } = false;
        internal bool LogDelete { get; private set; } = false;

        internal TableCache(Type entityType)
        {
            Require.NotNull(entityType, nameof(entityType));
            Name = entityType.Name;
            PopulateFields(entityType);
            SetLogEventsFlags(entityType);
        }

        private void PopulateFields(Type entityType)
        {
            Fields = entityType.GetFields();
        }

        private void SetLogEventsFlags(Type entityType)
        {
            LogRead = entityType.GetCustomAttribute<LogReadAttribute>() != null;
            LogCreate = entityType.GetCustomAttribute<LogCreateAttribute>() != null;
            LogUpdate = entityType.GetCustomAttribute<LogUpdateAttribute>() != null;
            LogDelete = entityType.GetCustomAttribute<LogDeleteAttribute>() != null;
        }
    }
}
