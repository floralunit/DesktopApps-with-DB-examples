using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AddEditProductPage.xaml
    /// </summary>
    public partial class AddEditProductPage : Page
    {
        private byte[] _mainImageData;
        private Product _currentProduct = null;
        public AddEditProductPage(Product p)
        {
            InitializeComponent();
            if (p != null)
            {
                this.DataContext = p;
                _currentProduct = p;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckErrors();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (_currentProduct == null)
            {
                var product = new Product
                {

                    ProductName = TBoxTitle.Text,
                    ProductCost = decimal.Parse(TBoxCost.Text),
                    ProductDescription = TBoxDescription.Text,
                    ProductDiscountAmount = (byte)(string.IsNullOrWhiteSpace(TBoxDiscount.Text) ? 0 : double.Parse(TBoxDiscount.Text) / 100),
                    ProductPhoto = _mainImageData
                };
                App.Context.Product.Add(product);
                App.Context.SaveChanges();
                MessageBox.Show("Товар успешно добавлен!");
            }
            else
            {
                Product product = this.DataContext as Product;
                _currentProduct = product;
                App.Context.SaveChanges();
                MessageBox.Show("Информация о товаре успешно обновлена!");
            }
            NavigationService.GoBack();
        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image |*.png; *.jpg; *.jpeg";
            if (ofd.ShowDialog() == true)
            {
                _mainImageData = File.ReadAllBytes(ofd.FileName);
                ImageService.Source = (ImageSource)new ImageSourceConverter()
                    .ConvertFrom(_mainImageData);
            }
        }
        private string CheckErrors()
        {
            var errorBuilder = new StringBuilder();
            if (string.IsNullOrWhiteSpace(TBoxTitle.Text))
                errorBuilder.AppendLine("Название товара обязательно для заполнения;");

            var productFromDB = App.Context.Product.ToList().FirstOrDefault(p => p.ProductName.ToLower() == TBoxTitle.Text.ToLower());
            if (productFromDB != null && productFromDB != _currentProduct)
                errorBuilder.AppendLine("Такая услуга уже есть в базе данных;");
            decimal cost = 0;
            if (decimal.TryParse(TBoxCost.Text, out cost) == false || cost <= 0)
                errorBuilder.AppendLine("Цена товара должна быть положительным числом;");
            if (!string.IsNullOrEmpty(TBoxDiscount.Text))
            {
                int discount = 0;
                if (int.TryParse(TBoxDiscount.Text, out discount) == false || discount < 0 || discount > 100)
                {
                    errorBuilder.AppendLine("Размер скидки - целое число в диапозоне от 0 до 100%;"); ;
                }
            }
            if (errorBuilder.Length > 0)
                errorBuilder.Insert(0, "Устраните следующие ошибки:\n");
            return errorBuilder.ToString();
        }
    }
}
