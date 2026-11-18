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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFekz.Models;

namespace WPFekz
{
    /// <summary>
    /// Логика взаимодействия для OrderCard.xaml
    /// </summary>
    public partial class OrderCard : UserControl
    {
        public OrderCard()
        {
            InitializeComponent();
            DataContextChanged += (s, e) => UpdateCard();
        }
        private void UpdateCard()
        {
            if (DataContext is not Zakaz t) return;

            TbTitle.Text = $"{t.IdZakaz}";
            TbStatus.Text = $"{t.IdStatusNavigation.NameStatus}";
            TbAdress.Text = $"{t.IdPunktNavigation.Adress}";
            TbDateOrder.Text = $"{t.DateZakaz}";
            TbDateDost.Text = $"{t.DataDost}";
        }
    }
}
