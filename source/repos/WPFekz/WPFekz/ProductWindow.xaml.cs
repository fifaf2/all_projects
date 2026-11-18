using Microsoft.EntityFrameworkCore;
using System.Windows;
using WPFekz.Models;

namespace WPFekz
{
    public partial class ProductWindow : Window
    { 
            private List<Tovary> all = new();

            public ProductWindow()
            {
                InitializeComponent();

                
                TbFio.Text = Appconnect.user != null ? Appconnect.user.Fio : "Гость";

                bool isStaff = Appconnect.user?.IdRole == 1 || Appconnect.user?.IdRole == 2;
                FilterPanel.Visibility = isStaff ? Visibility.Visible : Visibility.Collapsed;
                BtnOrders.Visibility = isStaff ? Visibility.Visible : Visibility.Collapsed;
                AdminPanel.Visibility = Appconnect.user?.IdRole == 1 ? Visibility.Visible : Visibility.Collapsed;

                LoadData();
            }

            private void LoadData()
            {
                all = Appconnect.db.Tovaries.Include(t => t.IdCategoryNavigation)
                    .Include(t => t.IdEdNavigation)
                    .Include(t => t.IdManufacNavigation)
                    .Include(t => t.IdPostNavigation)
                    .ToList();

                ApplyFilter();
            }

            private void Filter_Changed(object sender, RoutedEventArgs e) => ApplyFilter();

            private void ApplyFilter()
            {
                
                if (LbProducts == null || all == null) return;

                var res = all.AsEnumerable();

                if (FilterPanel.Visibility == Visibility.Visible)
                {
                    
                    string q = TbSearch.Text?.Trim().ToLower() ?? "";
                    if (!string.IsNullOrEmpty(q))
                    {
                        res = res.Where(x =>
                            (x.NameTovary != null && x.NameTovary.ToLower().Contains(q)) ||
                            (x.OpisTovar != null && x.OpisTovar.ToLower().Contains(q)) ||
                            (x.Articule != null && x.Articule.ToLower().Contains(q)));
                    }

                    
                    if (CbDiscount != null)
                    {
                        switch (CbDiscount.SelectedIndex)
                        {
                            case 1: res = res.Where(x => (x.Scidka ?? 0) <= 10); break;          
                            case 2: res = res.Where(x => (x.Scidka ?? 0) >= 11 && (x.Scidka ?? 0) <= 14); break; 
                            case 3: res = res.Where(x => (x.Scidka ?? 0) >= 15); break;  
                        }
                    }
                }

                LbProducts.ItemsSource = res.ToList();
            }
            private void BtnDelete_Click(object sender, RoutedEventArgs e)
            {
                if (LbProducts.SelectedItem is not Tovary item)
                {
                    MessageBox.Show("Выберите товар!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                
                if (Appconnect.db.Tovzakazs.Any(tz => tz.Id == item.Id))
                {
                    MessageBox.Show("Нельзя удалить товар, так как он есть в оформленных заказах!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }

                if (MessageBox.Show($"Удалить {item.NameTovary}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Appconnect.db.Tovaries.Remove(item);
                    Appconnect.db.SaveChanges();
                    LoadData();
                }
            }

            private void BtnOrders_Click(object sender, RoutedEventArgs e)
            {   
                new OrderWindow().Show();
                Close();
            }

            private void BtnLogout_Click(object sender, RoutedEventArgs e)
            {
                Appconnect.user = null;
                new MainWindow().Show();
                Close();
            }
        }
    }

