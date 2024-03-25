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
                else if (win.WindowState == WindowState.Maximized)
                {
                    win.WindowState = WindowState.Normal;
                }
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
        // Close App
        //public void CloseApp(object obj)
        //{
        //    MainWindow win = obj as MainWindow;
        //    win.Close();
        //}

        public ICommand CloseAppCommand { get; set; }

        // Maximize App
        //public void MaxApp(object obj)
        //{
        //    MainWindow win = obj as MainWindow;

        //    if (win.WindowState == WindowState.Normal)
        //    {
        //        win.WindowState = WindowState.Maximized;
        //    }
        //    else if (win.WindowState == WindowState.Maximized)
        //    {
        //        win.WindowState = WindowState.Normal;
        //    }
        //}

        // Maximize App Command
        public ICommand MaxAppCommand { get; set; }

        public void CloseMenu()
        {
            IsPanelVisible = false;
        }

        // Show Menu
        //public void ShowMenu()
        //{
        //    IsPanelVisible = true;
        //}

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
