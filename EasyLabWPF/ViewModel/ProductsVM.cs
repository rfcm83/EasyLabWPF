using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EasyLabWPF.Common;
using EasyLabWPF.Proxy.Data;

namespace EasyLabWPF.ViewModel
{
    public class ProductsVM : ObservableObject
    {
        private ICommand _adding;
        private ICommand _autoGenerating;
        private ICommand _change;
        private ICommand _keyPressed;
        private ObservableCollection<Product> _products;
        private Product _selected;

        public ProductsVM()
        {
            Init();
        }

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public Product Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged();
            }
        }

        public ICommand Adding => _adding ?? (_adding = new RelayCommand(AddNew));
        public ICommand Change => _change ?? (_change = new RelayCommand(ChangeProduct));
        public ICommand AutoGenerating => _autoGenerating ?? (_autoGenerating = new RelayCommand(AutoGenerate));
        public ICommand KeyPressed => _keyPressed ?? (_keyPressed = new RelayCommand(KeyPress));

        private void Init()
        {
            Products = new ObservableCollection<Product>();
            Task.Run(() => GetProductsAsync());
        }

        private void AddNew(object sender)
        {
            Selected = new Product();
        }

        private async void ChangeProduct(object sender)
        {
            var e = sender as SelectionChangedEventArgs;

            try
            {
                var currentItem = e?.RemovedItems.Cast<Product>().FirstOrDefault();
                if (currentItem == null) return;
                using (var client = new DataServiceClient())
                {
                    if (currentItem.ProductID == 0)
                        Selected = await client.AddProductAsync(currentItem);
                    else
                        await client.SaveProductAsync(currentItem);
                }
            }
            catch (InvalidCastException) { /* Go on */}
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                App.ShowMessage(ex.Message);
            }
        }

        private async Task GetProductsAsync()
        {
            try
            {
                using (var client = new DataServiceClient())
                {
                    Products = await client.GetProductsAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                App.ShowMessage(ex.Message);
            }
        }

        private void AutoGenerate(object sender)
        {
            var e = sender as DataGridAutoGeneratingColumnEventArgs;
            if (e != null)
                e.Cancel = e.PropertyName == "ExtensionData";
        }

        private void KeyPress(object sender)
        {
            var e = sender as KeyEventArgs;
            if (e?.Key != Key.Delete) return;

            if (App.ShowMessage("Delete item?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var client = new DataServiceClient())
                    {
                        client.DeleteProduct(Selected);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    App.ShowMessage(ex.Message);
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}