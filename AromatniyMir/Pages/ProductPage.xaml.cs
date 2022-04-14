using AromatniyMir.Entities;
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

namespace AromatniyMir.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private List<Product> _products = new();
        public ProductPage()
        {
            InitializeComponent();

            if (App.CurrentUser?.UserRole == 2 || App.CurrentUser?.UserRole == 3)
                btnOrders.Visibility = Visibility.Visible;
            else
                btnOrders.Visibility = Visibility.Collapsed;

            comboDiscount.SelectedIndex = 0;
            comboSortBy.SelectedIndex = 0;
            tBoxSearch.Text = "";
            UpdateProducts();
        }

        private void comboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void comboDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void tBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentProduct = (sender as Button).DataContext as Product;

            var inOrder = App.db.OrderProducts.FirstOrDefault(p => p.ProductArticle == currentProduct.ProductArticleNumber);

            if (inOrder == null)
            {
                if (MessageBox.Show($"Вы уверены, что хотите удалить услугу: {currentProduct.ProductName}?",
                    "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    App.db.Products.Remove(currentProduct);
                    App.db.SaveChanges();
                    UpdateProducts();
                }
            }
            else
                MessageBox.Show("Вы можете удалить данный товар, так как он присутствует в " + "одном из заказов",
                    "Ошибка при удалении товаров", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            comboSortBy.SelectedIndex = 0;
            comboDiscount.SelectedIndex = 0;
            tBoxSearch.Text = "";
            UpdateProducts();
        }

        private void btnOrders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersPage());
        }

        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CartPage(_products));
        }

        private void contextMenuAddToOrder_Click(object sender, RoutedEventArgs e)
        {
            decimal costInCart = 0;

            _products.Add(lViewProducts.SelectedItem as Product);

            if (_products.Count > 0)
                btnCart.Visibility = Visibility.Visible;

            foreach (var p in _products)
                costInCart += p.CostWithDiscount;
            tBlockCostInCart.Text = $"{costInCart:C2}";

            if (costInCart != 0)
                tBlockCostInCart.Visibility = Visibility.Visible;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }
        private void UpdateProducts()
        {
            int count, totalProducts = 0;
            var product = App.db.Products.ToList();
            totalProducts = product.Count;

            foreach (var p in product)
                if (p.ProductPhoto == null)
                    p.ProductPhoto = Properties.Resources.picture;

            if (comboSortBy.SelectedIndex == 0)
                product = product.OrderBy(p => p.CostWithDiscount).ToList();
            else
                product = product.OrderByDescending(p => p.CostWithDiscount).ToList();

            if (comboDiscount.SelectedIndex == 1)
                product = product.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10).ToList();
            if (comboDiscount.SelectedIndex == 2)
                product = product.Where(p => p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15).ToList();
            if (comboDiscount.SelectedIndex == 3)
                product = product.Where(p => p.ProductDiscountAmount > 15).ToList();

            product = product.Where(p => p.ProductName.ToLower().Contains(tBoxSearch.Text.ToLower())).ToList();

            lViewProducts.ItemsSource = product;

            count = product.Count;

            if (count == 0)
                blockRecords.Text = "Результаты не найдены";
            else
                blockRecords.Text = $"Найдено {count} из {totalProducts}";
        }
    }
}
