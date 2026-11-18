using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using WPFekz.Models;

namespace WPFekz
{
    public partial class ProductCard : UserControl
    {
        public ProductCard()
        {
            InitializeComponent();
            DataContextChanged += (s, e) => UpdateCard();
        }

        private void UpdateCard()
        {
            if (DataContext is not Tovary t) return;

            TbTitle.Text = $"{t.IdCategoryNavigation?.NameCategory} | {t.NameTovary}";
            TbDesc.Text = $"Описание: {t.OpisTovar}";
            TbManufac.Text = $"Производитель: {t.IdManufacNavigation?.NameManufac} | Поставщик: {t.IdPostNavigation?.NamePost}";
            TbCount.Text = $"Остаток: {t.Kolvo} {t.IdEdNavigation?.NameEd}";
            TbDiscount.Text = $"{t.Scidka ?? 0}%";

            
            decimal price = t.Sell ?? 0;
            int discount = t.Scidka ?? 0;

            if (discount > 0)
            {
                decimal newPrice = Math.Round(price * (100 - discount) / 100m, 2);
                TbOldPrice.Text = $"{price:N2}";
                TbOldPrice.Visibility = Visibility.Visible;
                TbPrice.Text = $"{newPrice:N2}";
            }
            else
            {
                TbOldPrice.Visibility = Visibility.Collapsed;
                TbPrice.Text = $"{price:N2}";
            }

           
            if ((t.Kolvo ?? 0) == 0)
            {
                CardBorder.Background = Brushes.LightGray; 
                SetTextColor(Brushes.Black);
            }
            else if (discount > 15)
            {
                CardBorder.Background = Brushes.HotPink;
                SetTextColor(Brushes.Black);
            }
            else
            {
                CardBorder.Background = Brushes.White;
                SetTextColor(Brushes.Black);
            }
        }

        private void SetTextColor(Brush b)
        {
            TbTitle.Foreground = b;
            TbDesc.Foreground = b;
            TbManufac.Foreground = b;
            TbCount.Foreground = b;
        }
    }
}

