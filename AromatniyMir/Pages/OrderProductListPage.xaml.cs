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
    /// Логика взаимодействия для OrderProductListPage.xaml
    /// </summary>
    public partial class OrderProductListPage : Page
    {
        public OrderProductListPage()
        {
            InitializeComponent();
        }

        public OrderProductListPage(int index)
        {
            InitializeComponent();

            decimal resultCost = 0;
            List<HelpClass> listHelp = new();
            var listProduct = App.db.OrderProducts.Where(p => p.OrderId == index);
            foreach (var p in listProduct)
            {
                HelpClass helpClass = new HelpClass
                {
                    Article = p.ProductArticle,
                    Name = p.ProductArticleNavigation.ProductDescription,
                    Manufacturer = p.ProductArticleNavigation.ProductManufacturerNavigation.ManufacturerName,
                    Count = p.ProductAmount,
                    Cost = p.ProductAmount * p.ProductArticleNavigation.CostWithDiscount
                };
                listHelp.Add(helpClass);
            }
            dataGridOrderListProduct.ItemsSource = listHelp;

            foreach (var res in listHelp)
                resultCost += res.Cost * res.Count;
            textBlockResult.Text = String.Format("{0:C2}", resultCost);
        }
    }
}
