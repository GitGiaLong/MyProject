using GSMF.Extensions.Onchanged;
using GUIVWPF.Common;
using System.Windows;
using System.Windows.Input;

namespace GUIVWPF.ViewModels
{
    public class MainVM : OnChanged
    {
        public MainVM()
        {

            // Set SideMenu Visibility 
            IsPanelVisible = false;
            CloseAppCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //CloseApp(p); 
                MainWindow win = p as MainWindow;
                win.Close();
            });
            MaxAppCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                //CloseApp(p); 
                MainWindow win = p as MainWindow;

                if (win.WindowState == WindowState.Normal)
                {
                    win.WindowState = WindowState.Maximized;
                }
                else
                {
                    win.WindowState = WindowState.Normal;
                }
            });
            MinAppCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindow win = p as MainWindow;

                if (win.WindowState == WindowState.Normal)
                {
                    win.WindowState = WindowState.Minimized;
                }
                else
                {
                    win.WindowState = WindowState.Normal;
                }
            });
            MouseMoveCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MainWindow win = p as MainWindow;
                win.DragMove();
                //DragMove();
            });

            ShowMenuCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsPanelVisible = true;
            });
            CloseMenuCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsPanelVisible = false;
            });

        }

        public ICommand CloseAppCommand { get; set; }

        // Maximize App Command
        public ICommand MaxAppCommand { get; set; }
        public ICommand MinAppCommand { get; set; }
        public void CloseMenu()
        {
            IsPanelVisible = false;
        }

        public ICommand MouseMoveCommand { get; set; }
        // Show Menu Command
        public ICommand ShowMenuCommand { get; set; }

        // Close Menu Command
        public ICommand CloseMenuCommand { get; set; }

        private bool _isPanelVisible;
        public bool IsPanelVisible
        {
            get
            {
                return _isPanelVisible;
            }
            set
            {
                _isPanelVisible = value;
                OnPropertyChanged("IsPanelVisible");
            }
        }
    }
}
