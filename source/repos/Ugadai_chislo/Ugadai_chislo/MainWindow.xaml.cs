using System.Windows;

namespace Ugadai_chislo
{
    public partial class MainWindow : Window
    {
        private const int MinNumber = 1;
        private const int MaxNumber = 100;
        private const int MaxAttempts = 10;

        private readonly Random random = new Random();

        private int secretNumber;
        private int attempts;
        private bool gameOver;

        public MainWindow()
        {
            InitializeComponent();
            StartNewGame();
        }

        private void StartNewGame()
        {
            secretNumber = random.Next(MinNumber, MaxNumber + 1);
            attempts = 0;
            gameOver = false;

            txtNumber.Text = "";
            txtNumber.IsEnabled = true;
            btnCheck.IsEnabled = true;
            txtHint.Text = "Я загадал число от 1 до 100. Попыток: " + MaxAttempts;

            UpdateAttemptsUi();
        }

        private void BtnNewGame_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            int number = int.Parse(txtNumber.Text);
            try
            {


                if (number < MinNumber || number > MaxNumber)
                    throw new ArgumentOutOfRangeException(nameof(number), number, "Число должно быть от 1 до 100");

                attempts++;

                if (number == secretNumber)
                {
                    txtHint.Text = "Угадал!";
                    gameOver = true;
                }
                else if (number < secretNumber)
                {
                    txtHint.Text = "Больше";
                }
                else
                {
                    txtHint.Text = "Меньше";
                }

                if (!gameOver && attempts >= MaxAttempts)
                    throw new GameRuleException(secretNumber);
            }

            catch (FormatException)
            {
                txtHint.Text = "Нужно ввести целое число.";
            }
            catch (OverflowException)
            {
                txtHint.Text = "Слишком большое число.";
            }
            catch (ArgumentOutOfRangeException)
            {
                txtHint.Text = "Число должно быть от 1 до 100.";
            }
            catch (GameRuleException ex)
            {
                txtHint.Text = ex.Message;
                gameOver = true;
            }
            finally
            {
                
                UpdateAttemptsUi();

                if (gameOver)
                {
                    btnCheck.IsEnabled = false;
                    txtNumber.IsEnabled = false;
                }
            }
        }

        private void UpdateAttemptsUi()
        {
            progressAttempts.Maximum = MaxAttempts;
            progressAttempts.Value = attempts;
            txtAttempts.Text = "Попыток: " + attempts + " из " + MaxAttempts;
        }
    }
}
