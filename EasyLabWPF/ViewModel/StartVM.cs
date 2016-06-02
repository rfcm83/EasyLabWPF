using System.Windows.Input;
using EasyLabWPF.Common;
using EasyLabWPF.Views;

namespace EasyLabWPF.ViewModel
{
    public class StartVM : ObservableObject
    {
        private ICommand _products;
        public ICommand Products => _products ?? (_products = new RelayCommand(GetProducts));

        private void GetProducts(object sender)
        {
            App.NavigateTo(new Products());
        }
    }
}