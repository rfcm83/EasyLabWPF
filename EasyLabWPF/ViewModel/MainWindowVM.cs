using System.Windows.Controls;
using EasyLabWPF.Common;
using EasyLabWPF.Views;

namespace EasyLabWPF.ViewModel
{
    public class MainWindowVM : ObservableObject
    {
        private Frame _contentApp;

        public MainWindowVM()
        {
            ContentApp.Navigate(new Start());
        }

        public Frame ContentApp => _contentApp ?? (_contentApp = new Frame());
    }
}