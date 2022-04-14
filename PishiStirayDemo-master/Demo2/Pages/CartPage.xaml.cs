using Demo2.Models;
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

namespace Demo2.Pages
{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        public int quantityInCart { get; set; }
        public decimal costWithQuantity { get; set; }
        public OrderProduct orderProduct { get; set; }

        public CartPage()
        {
            InitializeComponent();
        }

        public CartPage(List<Product> products)
        {
            InitializeComponent();
            dataGridProducts.ItemsSource = products.OrderBy(p => p.ProductName).Distinct();
            foreach (var p in products.OrderBy(p => p.ProductName).Distinct())
            {
                orderProduct = new OrderProduct
                {
                    OrderProductArticleNumber = p.ProductArticleNumber,
                    OrderProductQuantity = products.Where(e => e == p).Count()
                };
            }
        }

        private void buttonQuantityPlus_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonQuantityMinus_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
