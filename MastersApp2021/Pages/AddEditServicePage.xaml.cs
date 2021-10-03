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

namespace MastersApp2021.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditServicePage.xaml
    /// </summary>
    public partial class AddEditServicePage : Page
    {
        private Entities.Service _currentService = null;
        private byte[] _mainImageData;
        public AddEditServicePage()
        {
            InitializeComponent();
        }
        public AddEditServicePage(Entities.Service service)
        {
            InitializeComponent();
            _currentService = service;
            Title = "Редактировать услугу";
            TBoxTitle.Text = _currentService.Title;
            TBoxCost.Text = _currentService.Cost.ToString("N2");
            TBoxDuration.Text = (_currentService.DurationInSeconds / 60).ToString();
            TBoxDescription.Text = _currentService.Description;
            if (_currentService.Discount > 0)
                TBoxDiscount.Text = (_currentService.Discount.Value * 100).ToString();
            if (_currentService.MainImage != null)
                ImageService.Source = (ImageSource)new ImageSourceConverter()
                    .ConvertFrom(_currentService.MainImage);
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckErrors();
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (_currentService == null)
                {
                    var service = new Entities.Service
                    {
                        Title = TBoxTitle.Text,
                        Cost = decimal.Parse(TBoxCost.Text),
                        DurationInSeconds = int.Parse(TBoxDuration.Text) * 60,
                        Description = TBoxDescription.Text,
                        Discount = string.IsNullOrWhiteSpace(TBoxDiscount.Text)
                        ? 0 : double.Parse(TBoxDiscount.Text) / 100,
                        MainImage = _mainImageData
                    };
                    App.Context.Services.Add(service);
                    App.Context.SaveChanges();
                    MessageBox.Show("Услуга успешно добавлена!");
                }
                else
                {
                    _currentService.Title = TBoxTitle.Text;
                    _currentService.Cost = decimal.Parse(TBoxCost.Text);
                    _currentService.DurationInSeconds = int.Parse(TBoxDuration.Text) * 60;
                    _currentService.Description = TBoxDescription.Text;
                    _currentService.Discount = string.IsNullOrWhiteSpace(TBoxDiscount.Text)
                        ? 0 : double.Parse(TBoxDiscount.Text) / 100;
                    if (_mainImageData != null)
                        _currentService.MainImage = _mainImageData;
                    App.Context.SaveChanges();
                    MessageBox.Show("Информация об услуге успешно обновлена!");
                }
                NavigationService.GoBack();
            }
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
                errorBuilder.AppendLine("Название услуги обязательно для заполнения;");

            var serviceFromDB = App.Context.Services.ToList().FirstOrDefault(p => p.Title.ToLower() == TBoxTitle.Text.ToLower());
            if (serviceFromDB != null && serviceFromDB != _currentService)
                errorBuilder.AppendLine("Такая услуга уже есть в базе данных;");
            decimal cost =0;
            if (decimal.TryParse(TBoxCost.Text, out cost) == false || cost <= 0)
                errorBuilder.AppendLine("Стоимость услуги должна быть положительным числом;");
            int durationInMinutes = 0;
            if (int.TryParse(TBoxDuration.Text, out durationInMinutes) == false || durationInMinutes > 240 || durationInMinutes <= 0)
            {
                errorBuilder.AppendLine("Длительность оказания услуги должна быть положительным " + "числом (не больше, чем 4 часа);");
            }
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
