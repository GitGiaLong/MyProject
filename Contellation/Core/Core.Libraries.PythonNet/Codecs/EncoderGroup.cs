using Core.Libraries.Interfaces.PythonNet.Codecs;
using Core.Libraries.PythonNet.PythonTypes;
using System.Collections;

namespace Core.Libraries.PythonNet.Codecs
{
    /// <summary>
    /// Represents a group of <see cref="IPyObjectDecoder"/>s. Useful to group them by priority.
    /// </summary>
    public sealed class EncoderGroup : IPyObjectEncoder, IEnumerable<IPyObjectEncoder>, IDisposable
    {
        readonly List<IPyObjectEncoder> encoders = new();

        /// <summary>
        /// Add specified encoder to the group
        /// </summary>
        public void Add(IPyObjectEncoder item)
        {
            if (item is null) { throw new ArgumentNullException(nameof(item)); }
            this.encoders.Add(item);
        }

        /// <summary>
        /// Remove all encoders from the group
        /// </summary>
        public void Clear() => this.encoders.Clear();

        /// <inheritdoc />
        public bool CanEncode(Type type) => this.encoders.Any(encoder => encoder.CanEncode(type));
        /// <inheritdoc />
        public PyObject? TryEncode(object value)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));

            foreach (var encoder in this.GetEncoders(value.GetType()))
            {
                var result = encoder.TryEncode(value);
                if (result != null) { return result; }
            }

            return null;
        }

        /// <inheritdoc />
        public IEnumerator<IPyObjectEncoder> GetEnumerator() => this.encoders.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.encoders.GetEnumerator();

        public void Dispose()
        {
            foreach (var encoder in this.encoders.OfType<IDisposable>())
            {
                encoder.Dispose();
            }
            this.encoders.Clear();
        }
    }
}
