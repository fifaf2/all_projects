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

namespace WPFekz
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        { 
            var u = Appconnect.db.Users.FirstOrDefault(x => x.Login == TbLogin.Text && x.Password == PbPass.Password);
            if (u == null)
            {
                MessageBox.Show("Неверный пароль илилогин!", "бебеб", MessageBoxButton.OK);
                return;
            }
            Appconnect.user = u;
            OpenCatalog();
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            Appconnect.user = null;
            OpenCatalog();
        }

        private void OpenCatalog()
        {
            new ProductWindow().Show();
            Close();
        }
    }
}