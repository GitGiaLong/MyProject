using Core.Libraries.Interfaces.PythonNet.Codecs;
using Core.Libraries.PythonNet.PythonTypes;

namespace Core.Libraries.PythonNet.Codecs
{
    public static class DecoderGroupExtensions
    {
        /// <summary>
        /// Gets a concrete instance of <see cref="IPyObjectDecoder"/>
        /// (potentially selecting one from a collection),
        /// that can decode from <paramref name="objectType"/> to <paramref name="targetType"/>,
        /// or <c>null</c> if a matching decoder can not be found.
        /// </summary>
        public static IPyObjectDecoder? GetDecoder(this IPyObjectDecoder decoder, PyType objectType, Type targetType)
        {
            if (decoder is null) throw new ArgumentNullException(nameof(decoder));

            if (decoder is IEnumerable<IPyObjectDecoder> composite)
            {
                return composite.Select(nestedDecoder 
                    => nestedDecoder.GetDecoder(objectType, targetType)).FirstOrDefault(d => d != null);
            }

            return decoder.CanDecode(objectType, targetType) ? decoder : null;
        }
    }
}
