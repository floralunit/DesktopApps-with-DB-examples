using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace AromatniyMir.Entities
{
    public partial class Product
    {
        public string DiscountText
        {
            get
            {
                if (ProductDiscountAmount == 0 || ProductDiscountAmount == null)
                    return "";
                else
                    return $"* Скидка {ProductDiscountAmount}%";
            }
        }

        public string TotalCost
        {
            get
            {
                if (ProductDiscountAmount == 0 || ProductDiscountAmount == null)
                    return $"{ProductCost:N2} рублей";
                else
                    return $"{CostWithDiscount:N2} рублей";

            }
        }
        public decimal CostWithDiscount
        {
            get
            {
                if (ProductDiscountAmount == 0 || ProductDiscountAmount == null)
                    return (decimal)ProductCost;
                else
                {
                    var costWithDiscount = ProductCost - ((decimal)ProductDiscountAmount / 100) * ProductCost;
                    return costWithDiscount;
                }
            }
        }
        public Visibility DiscountVisibility
        {
            get
            {
                if (ProductDiscountAmount == 0 || ProductDiscountAmount == null)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }

        public string BackColor
        {
            get
            {
                if (ProductDiscountAmount > 15)
                    return "#7fff00";
                else return "White";
            }
        }
        public string AdminControlVisibility
        {
            get
            {
                if (App.CurrentUser?.UserRole == 2 || App.CurrentUser?.UserRole == 3)
                    return "Visible";
                else
                    return "Collapsed";
            }
        }
    }
}