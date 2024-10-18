namespace Core.Libraries.WPF.Interfaces
{
    public interface IValueRange<T>
    {
        T Start { get; set; }

        T End { get; set; }
    }
}
