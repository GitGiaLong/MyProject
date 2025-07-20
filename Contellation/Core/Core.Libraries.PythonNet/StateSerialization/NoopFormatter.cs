using System.Runtime.Serialization;

namespace Core.Libraries.PythonNet.StateSerialization
{
    public class NoopFormatter : IFormatter
    {
        public object Deserialize(Stream s) => throw new NotImplementedException();
        public void Serialize(Stream s, object o) { }

        public SerializationBinder? Binder { get; set; }
        public StreamingContext Context { get; set; }
        public ISurrogateSelector? SurrogateSelector { get; set; }
    }
}
