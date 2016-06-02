using System.Windows;
using System.Windows.Controls;
using EasyLabWPF.Common;
using EasyLabWPF.ViewModel;

namespace EasyLabWPF
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static void NavigateTo(Page nextPage)
        {
            if (nextPage == null) return;
            var main = ViewModelLocator.Get<MainWindowVM>();
            main.ContentApp.Navigate(nextPage);
        }

        public static MessageBoxResult ShowMessage(string message, MessageBoxButton typeMessageBox = MessageBoxButton.OK)
        {
            return MessageBox.Show(message, "Hello!", typeMessageBox);
        }
    }
}