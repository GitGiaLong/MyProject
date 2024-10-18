﻿namespace Core.Libraries.WPF.Controls.Snackbars
{

    public class SnackbarPresenter : System.Windows.Controls.ContentPresenter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("WpfAnalyzers.DependencyProperty", "WPF0012:CLR property type should match registered type", 
            Justification = "seems harmless")]
        public new Snackbar? Content
        {
            get { return (Snackbar?)GetValue(ContentProperty); }
            protected set { SetValue(ContentProperty, value); }
        }

        public SnackbarPresenter()
        {
            Unloaded += static (sender, _) =>
            {
                var self = (SnackbarPresenter)sender;
                self.OnUnloaded();
            };
        }

        protected Queue<Snackbar> Queue { get; } = new();

        protected CancellationTokenSource CancellationTokenSource { get; set; } = new();

        protected virtual void OnUnloaded()
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
        }

        protected void ResetCancellationTokenSource()
        {
            CancellationTokenSource.Dispose();
            CancellationTokenSource = new CancellationTokenSource();
        }

        public virtual void AddToQue(Snackbar snackbar)
        {
            Queue.Enqueue(snackbar);

            if (Content is null) { ShowQueuedSnackbars();  }
        }

        public virtual async Task ImmediatelyDisplay(Snackbar snackbar)
        {
            await HideCurrent();
            await ShowSnackbar(snackbar);

            await ShowQueuedSnackbars();
        }

        public virtual async Task HideCurrent()
        {
            if (Content is null) { return; }

            CancellationTokenSource.Cancel();
            await HidSnackbar(Content);
            ResetCancellationTokenSource();
        }

        /// <summary>
        /// TODO: Fix detached process
        /// </summary>
        /// <returns></returns>
        private async Task ShowQueuedSnackbars()
        {
            while (Queue.Count > 0 && !CancellationTokenSource.IsCancellationRequested)
            {
                Snackbar snackbar = Queue.Dequeue();

                await ShowSnackbar(snackbar);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("WpfAnalyzers.DependencyProperty", "WPF0041:Set mutable dependency properties using SetCurrentValue", 
            Justification = "SetCurrentValue(ContentProperty, ...) will not work")]
        private async Task ShowSnackbar(Snackbar snackbar)
        {
            Content = snackbar;

            snackbar.SetCurrentValue(Snackbar.IsShownProperty, true);

            try { await Task.Delay(snackbar.Timeout, CancellationTokenSource.Token); }
            catch { return; }

            await HidSnackbar(snackbar);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("WpfAnalyzers.DependencyProperty", "WPF0041:Set mutable dependency properties using SetCurrentValue", 
            Justification = "SetCurrentValue(ContentProperty, ...) will not work")]
        private async Task HidSnackbar(Snackbar snackbar)
        {
            snackbar.SetCurrentValue(Snackbar.IsShownProperty, false);

            await Task.Delay(300);

            Content = null;
        }
    }
}
