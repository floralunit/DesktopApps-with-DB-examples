using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TradeDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            //if (App.CurrentUser.UserRole == 2)
            //{
            //    BtnAddProduct.Visibility = Visibility.Visible;
 
            //}
            //else if (App.CurrentUser.UserRole == 3)
            //{
            //    BtnAddProduct.Visibility = Visibility.Collapsed;
            //}
            ComboDiscount.SelectedIndex = 0;
            ComboSortBy.SelectedIndex = 0;
            UpdateProducts();
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage(null)); ;
        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentProduct = (sender as Button).DataContext as Product;
            if (MessageBox.Show($"Вы уверены, что хотите удалить товар: " + $"{currentProduct.ProductName}?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                App.Context.Product.Remove(currentProduct);
                App.Context.SaveChanges();
                UpdateProducts();
                MessageBox.Show("Товар удален");
            }
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var currentProduct = (sender as Button).DataContext as Product;
            NavigationService.Navigate(new AddEditProductPage(currentProduct));
        }

        private void ComboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }
        private void ComboDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void UpdateProducts()
        {
            var products = App.Context.Product.ToList();
            //sort by price
            if (ComboSortBy.SelectedIndex == 0)
                products = products.OrderBy(p => p.ProductCost).ToList();
            else
                products = products.OrderByDescending(p => p.ProductCost).ToList();
            //skidka
            if (ComboDiscount.SelectedIndex == 1)
                products = products.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 9.99).ToList();
            if (ComboDiscount.SelectedIndex == 2)
                products = products.Where(p => p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 14.99).ToList();
            if (ComboDiscount.SelectedIndex == 3)
                products = products.Where(p => p.ProductDiscountAmount >= 15 && p.ProductDiscountAmount < 100).ToList();
            //поиск по названию
            products = products.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            foreach (Product product in products)
            {
                product.ProductManufacturerName = App.Context.Manufacturer.FirstOrDefault(p => p.Id_Manufacturer == product.ProductManufacturer).ProductManufacturer;
            }
            LViewProducts.ItemsSource = products;
        }

    }
}
