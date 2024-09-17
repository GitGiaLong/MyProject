using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Core.Libraries.WPF.Extensions.Intputs
{
    /// <summary>
    /// A generic command whose sole purpose is to relay its functionality to other
    /// objects by invoking delegates. The default return value for the CanExecute
    /// method is <see langword="true"/>. This class allows you to accept command parameters
    /// in the <see cref="Execute(T)"/> and <see cref="CanExecute(T)"/> callback methods.
    /// </summary>
    /// <typeparam Name="T">The Type of parameter being passed as input to the callbacks.</typeparam>
    public class RelayCommand<T> : IRelayCommand<T>
    {

        /// <summary>
        /// The <see cref="Action"/> to invoke when <see cref="Execute(T)"/> is used.
        /// </summary>
        private readonly Action<T?> _execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class that can always execute.
        /// </summary>
        /// <param Name="execute">The execution logic.</param>
        /// <remarks>
        /// Due to the fact that the <see cref="System.Windows.Input.ICommand"/> interface exposes methods that accept a
        /// nullable <see cref="object"/> parameter, it is recommended that if <typeparamref Name="T"/> is a reference Type,
        /// you should always declare it as nullable, and to always perform checks within <paramref Name="execute"/>.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref Name="execute"/> is <see langword="null"/>.</exception>
        public RelayCommand(Action<T?> execute)
        {
            if (execute is null) { throw new ArgumentNullException(nameof(execute)); }

            _execute = execute;
        }

        /// <summary>
        /// The optional action to invoke when <see cref="CanExecute(T)"/> is used.
        /// </summary>
        private readonly Predicate<T?>? _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param Name="execute">The execution logic.</param>
        /// <param Name="canExecute">The execution status logic.</param>
        /// <remarks>
        /// Due to the fact that the <see cref="System.Windows.Input.ICommand"/> interface exposes methods that accept a
        /// nullable <see cref="object"/> parameter, it is recommended that if <typeparamref Name="T"/> is a reference Type,
        /// you should always declare it as nullable, and to always perform checks within <paramref Name="execute"/>.
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref Name="execute"/> or <paramref Name="canExecute"/> are <see langword="null"/>.</exception>
        public RelayCommand(Predicate<T> canExecute, Action<T> execute)
        {
            //if (execute == null) { throw new ArgumentNullException("execute"); }
            if (execute is null) { throw new ArgumentNullException(nameof(execute)); }

            //if (canExecute == null) { throw new ArgumentNullException("canExecute"); }
            if (canExecute is null) { throw new ArgumentNullException(nameof(canExecute)); }

            _execute = execute;
            _canExecute = canExecute;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanExecute(T? parameter) { return _canExecute?.Invoke(parameter) != false; }

        public bool CanExecute(object? parameter)
        {
            /// Special case a null value for a value Type argument Type.
            /// This ensures that no exceptions are thrown during initialization.
            if (parameter is null && default(T) is not null) { return false; }

            if (!TryGetCommandArgument(parameter, out T? result))
            {
                ThrowArgumentExceptionForInvalidCommandArgument(parameter);
            }

            return CanExecute(result);
            //try { return _canExecute == null ? true : _canExecute((T)parameter); }
            //catch { return true; }
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Execute(T? parameter)
        {
            _execute(parameter);
        }
        /// <inheritdoc/>
        public void Execute(object? parameter)
        {
            if (!TryGetCommandArgument(parameter, out T? result))
            {
                ThrowArgumentExceptionForInvalidCommandArgument(parameter);
            }

            Execute(result);
        }

        /// <summary>
        /// Tries to get a command argument of compatible Type <typeparamref Name="T"/> from an input <see cref="object"/>.
        /// </summary>
        /// <param Name="parameter">The input parameter.</param>
        /// <param Name="result">The resulting <typeparamref Name="T"/> value, if any.</param>
        /// <returns>Whether or not a compatible command argument could be retrieved.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryGetCommandArgument(object? parameter, out T? result)
        {
            /// If the argument is null and the default value of T is also null, then the
            /// argument is valid. T might be a reference Type or a nullable value Type.
            if (parameter is null && default(T) is null)
            {
                result = default;

                return true;
            }

            /// Check if the argument is a T value, so either an instance of a Type or a derived
            /// Type of T is a reference Type, an interface implementation if T is an interface,
            /// or a boxed value Type in case T was a value Type.
            if (parameter is T argument)
            {
                result = argument;

                return true;
            }

            result = default;

            return false;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if an invalid command argument is used.
        /// </summary>
        /// <param Name="parameter">The input parameter.</param>
        /// <exception cref="ArgumentException">Thrown with an error message to give info on the invalid parameter.</exception>
        internal static void ThrowArgumentExceptionForInvalidCommandArgument(object? parameter)
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            static Exception GetException(object? parameter)
            {
                if (parameter is null)
                {
                    return new ArgumentException($"Parameter \"{nameof(parameter)}\" (object) must not be null, as the command type requires an argument of type {typeof(T)}.",
                        nameof(parameter));
                }

                return new ArgumentException($"Parameter \"{nameof(parameter)}\" (object) cannot be of type {parameter.GetType()}, as the command type requires an argument of type {typeof(T)}.",
                    nameof(parameter));
            }

            throw GetException(parameter);
        }

        /// <inheritdoc/>
        public event EventHandler? _CanExecuteChanged;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <inheritdoc/>
        public void NotifyCanExecuteChanged() { _CanExecuteChanged?.Invoke(this, EventArgs.Empty); }
    }
}
