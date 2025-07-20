using Core.Libraries.Interfaces.PythonNet.Codecs;

namespace Core.Libraries.PythonNet.Codecs
{
    public static class EncoderGroupExtensions
    {
        /// <summary>
        /// Gets specific instances of <see cref="IPyObjectEncoder"/>
        /// (potentially selecting one from a collection),
        /// that can encode the specified <paramref name="type"/>.
        /// </summary>
        public static IEnumerable<IPyObjectEncoder> GetEncoders(this IPyObjectEncoder decoder, Type type)
        {
            if (decoder is null) throw new ArgumentNullException(nameof(decoder));

            if (decoder is IEnumerable<IPyObjectEncoder> composite)
            {
                foreach (var nestedEncoder in composite)
                {
                    foreach (var match in nestedEncoder.GetEncoders(type))
                    {
                        yield return match;
                    }
                }
            }
            else if (decoder.CanEncode(type))
            {
                yield return decoder;
            }
        }
    }
}
