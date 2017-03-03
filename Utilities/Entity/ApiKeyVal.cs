using System.Runtime.Serialization;

namespace Utilities.Entity
{
    public class ApiKeyVal
    {
        // ReSharper disable once InconsistentNaming
        public string k { get; set; }

        // ReSharper disable once InconsistentNaming
        public object v { get; set; }
    }

    public class ApiKeyVal<TKey, TValue>
    {
        // ReSharper disable once InconsistentNaming
        public TKey k { get; set; }

        // ReSharper disable once InconsistentNaming
        public TValue v { get; set; }
    }
}