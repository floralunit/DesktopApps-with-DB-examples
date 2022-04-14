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
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private List<Product> _products = new();

        public ProductPage()
        {
            InitializeComponent();

            // Определяем какие кнопки будут видны а какие скрыты в зависимости от роли
            if (App.currentUser?.UserRole == 2 || App.currentUser?.UserRole == 3)
                buttonOrders.Visibility = Visibility.Visible;
            else
                buttonOrders.Visibility = Visibility.Collapsed;

            // Изначально сбрасываем все фильтры
            comboBoxSortByDiscount.SelectedIndex = 0;
            comboBoxSortBy.SelectedIndex = 0;
            textBoxSearch.Text = "";
            UpdateProducts();
        }

        // Кнопка "Сбросить фильтры"
        private void resetToDefault_Click(object sender, RoutedEventArgs e)
        {
            comboBoxSortBy.SelectedIndex = 0;
            comboBoxSortByDiscount.SelectedIndex = 0;
            textBoxSearch.Text = "";
            UpdateProducts();
        }

        // Кнопка "Корзина"
        private void buttonCart_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CartPage(_products));
        }

        // Кнопка "Просмотр закзаов"
        private void buttonOrders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersPage());
        }

        // Обработка нажатия "Добавить в корзину" по нажатию на ПКМ
        private void contextMenuAddToOrder_Click(object sender, RoutedEventArgs e)
        {
            // Переменная для расчёта стоимости
            decimal costInCart = 0;
            // Добавление выбранного продукта в список
            _products.Add(listViewProducts.SelectedItem as Product);
            // Отображение кнопки "Коризна"
            if (_products.Count > 0)
                buttonCart.Visibility = Visibility.Visible;
            // Расчёт стоимости и вывод на экран
            foreach (var p in _products)
                costInCart += p.CostWithDiscount;
            textBlockCostInCart.Text = $"{costInCart:C2}";
            // Если стоимость не 0, показывать textBlock 
            if (costInCart != 0)
                textBlockCostInCart.Visibility = Visibility.Visible;
        }

        // Кнопка "Удалить"
        private void buttonDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            // Получаем продукт на котором была нажата кнопка
            var currentProduct = (sender as Button).DataContext as Product;

            // Пытаемся найти его артикул в заказах
            var inOrder = App.db.OrderProducts.FirstOrDefault(p => p.OrderProductArticleNumber ==
            currentProduct.ProductArticleNumber);

            // Если товар отсутсвует в заказе
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
                MessageBox.Show("Вы не можете удалить данный товар, так как он присутствует в " +
                    "одном из заказов", "Ошибка при удаленни товара", MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }

        private void comboBoxSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void comboBoxSortByDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateProducts();
        }

        // Метод получения товаров и обновления в зависимости от выбранных фильтров
        private void UpdateProducts()
        {
            // Переменные для отображение кол-ва товаров выведенных на экран
            int count, totalProducts = 0;
            var product = App.db.Products.ToList();
            totalProducts = product.Count;

            // Для заполнения фотографий товаров где они отсутствуют
            foreach (var p in product)
                if(p.ProductPhoto == null)
                p.ProductPhoto = Properties.Resources.picture;

            // Обработка сортировки по возрастанию (убыванию). SelectedIndex представляет собой
            // Условный массив, где 0 - это по возрастанию, 1 - по убыванию. Аналогично работают
            // И остальные comboBox
            if (comboBoxSortBy.SelectedIndex == 0)
                product = product.OrderBy(p => p.CostWithDiscount).ToList();
            else
                product = product.OrderByDescending(p => p.CostWithDiscount).ToList();

            // Обработка выбранного диапозона по скидке (Диапозоны указаны в задании)
            if (comboBoxSortByDiscount.SelectedIndex == 1)
                product = product.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10).ToList();
            if (comboBoxSortByDiscount.SelectedIndex == 2)
                product = product.Where(p => p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15).ToList();
            if (comboBoxSortByDiscount.SelectedIndex == 3)
                product = product.Where(p => p.ProductDiscountAmount > 15).ToList();

            // Работа поиска. Для корректной работы приводим всё к нижнему регистру ToLower()
            product = product.Where(p => p.ProductName.ToLower().Contains(textBoxSearch.Text.ToLower())).ToList();

            // Выводим всё в наш контейнер listView
            listViewProducts.ItemsSource = product;
            // Считаем количество товаров по списку сформированному из требований выше
            count = product.Count;
            // Вывод результатов
            if (count == 0)
                blockRecords.Text = "Результаты не найдены";
            else
                blockRecords.Text = $"Найдено {count} из {totalProducts}";
        }
    }
}
