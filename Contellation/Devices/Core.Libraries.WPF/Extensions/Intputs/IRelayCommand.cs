using System.Windows.Input;

namespace Core.Libraries.WPF.Extensions.Intputs
{
    /// <summary>
    /// An interface expanding <see cref="ICommand"/> with the ability to raise
    /// the <see cref="ICommand.CanExecuteChanged"/> event externally.
    /// </summary>
    public interface IRelayCommand : ICommand
    {
        /// <summary>
        /// Notifies that the <see cref="ICommand.CanExecute"/> property has changed.
        /// </summary>
        void NotifyCanExecuteChanged();
    }

    /// <summary>
    /// A generic interface representing a more specific version of <see cref="IRelayCommand"/>.
    /// </summary>
    /// <typeparam Name="T">The Type used as argument for the interface methods.</typeparam>
    public interface IRelayCommand<in T> : IRelayCommand
    {
        /// <summary>
        /// Provides a strongly-typed variant of <see cref="ICommand.CanExecute(object)"/>.
        /// </summary>
        /// <param Name="parameter">The input parameter.</param>
        /// <returns>Whether or not the current command can be executed.</returns>
        /// <remarks>Use this overload to avoid boxing, if <typeparamref Name="T"/> is a value Type.</remarks>
        bool CanExecute(T? parameter);

        /// <summary>
        /// Provides a strongly-typed variant of <see cref="ICommand.Execute(object)"/>.
        /// </summary>
        /// <param Name="parameter">The input parameter.</param>
        /// <remarks>Use this overload to avoid boxing, if <typeparamref Name="T"/> is a value Type.</remarks>
        void Execute(T? parameter);
    }
}
