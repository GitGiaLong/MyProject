using Core.Libraries.Interfaces.PythonNet.Codecs;
using Core.Libraries.PythonNet.PythonTypes;
using System.Collections;

namespace Core.Libraries.PythonNet.Codecs
{
    /// <summary>
    /// Represents a group of <see cref="IPyObjectDecoder"/>s. Useful to group them by priority.
    /// </summary>
    public sealed class DecoderGroup : IPyObjectDecoder, IEnumerable<IPyObjectDecoder>, IDisposable
    {
        readonly List<IPyObjectDecoder> decoders = new();

        /// <summary>
        /// Add specified decoder to the group
        /// </summary>
        public void Add(IPyObjectDecoder item)
        {
            if (item is null) { throw new ArgumentNullException(nameof(item)); }

            this.decoders.Add(item);
        }

        /// <summary>
        /// Remove all decoders from the group
        /// </summary>
        public void Clear() => this.decoders.Clear();

        /// <inheritdoc />
        public bool CanDecode(PyType objectType, Type targetType) => this.decoders.Any(decoder => decoder.CanDecode(objectType, targetType));

        /// <inheritdoc />
        public bool TryDecode<T>(PyObject pyObj, out T? value)
        {
            if (pyObj is null) { throw new ArgumentNullException(nameof(pyObj)); }

            var decoder = this.GetDecoder(pyObj.GetPythonType(), typeof(T));
            if (decoder is null)
            {
                value = default;
                return false;
            }
            return decoder.TryDecode(pyObj, out value);
        }

        /// <inheritdoc />
        public IEnumerator<IPyObjectDecoder> GetEnumerator() => this.decoders.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.decoders.GetEnumerator();

        public void Dispose()
        {
            foreach (var decoder in this.decoders.OfType<IDisposable>())
            {
                decoder.Dispose();
            }
            this.decoders.Clear();
        }
    }
}
