using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFekz.Models;

namespace WPFekz
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private List<Zakaz> all = new List<Zakaz>();
        public OrderWindow()
        {
            InitializeComponent();
            loadData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow card = new ProductWindow();
            card.Show();
            Close();
        }
        private void loadData()
        {
            all = Appconnect.db.Zakazs.Include(t => t.IdPunktNavigation).Include(t => t.IdStatusNavigation).ToList();

            LbProducts.ItemsSource = all.ToList();
            Filter();
        }

        private void Cbstatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();    
        }
        private void Filter()
        {
            if (LbProducts == null || all == null) return;
            var res = all.AsEnumerable();
            if (Cbstatus != null)
            {

                switch (Cbstatus.SelectedIndex)
                {
                    case 1: res = res.Where(x => x.IdStatus == 1); break;
                    case 2: res = res.Where(x => x.IdStatus == 2); break;
                }
            }
            LbProducts.ItemsSource = res.ToList();
        }
    }
}
