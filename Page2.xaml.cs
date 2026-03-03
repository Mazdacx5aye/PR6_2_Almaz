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

namespace PR4_Test
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Считываем x и m
                double x = double.Parse(txtX.Text);
                double m = double.Parse(txtM.Text);

                // Определяем выбранную функцию f(x)
                double fx;
                if (rbtnSh.IsChecked == true)
                    fx = Math.Sinh(x);               // sh(x)
                else if (rbtnX2.IsChecked == true)
                    fx = x * x;                        // x^2
                else if (rbtnExp.IsChecked == true)
                    fx = Math.Exp(x);                  // e^x
                else
                {
                    MessageBox.Show("Выберите функцию f(x).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                double j; // результат

                // Проверяем условия согласно кусочной функции
                // Первое условие: -1 < m < x (т.е. m > -1 И m < x)
                if (m > -1 && m < x)
                {
                    // sin(5f(x) + 3m|f(x)|)
                    j = Math.Sin(5 * fx + 3 * m * Math.Abs(fx));
                }
                // Второе условие: x > m (при этом не выполнилось первое)
                else if (x > m)
                {
                    // cos(3f(x) + 5m|f(x)|)
                    j = Math.Cos(3 * fx + 5 * m * Math.Abs(fx));
                }
                // Третье условие: x == m (с учётом возможной погрешности)
                else if (Math.Abs(x - m) < 1e-10) // сравниваем с допуском
                {
                    // (f(x) + m)^2
                    j = Math.Pow(fx + m, 2);
                }
                else
                {
                    // Если ни одно условие не подошло (например, m <= -1 и x <= m, но не равно)
                    // По логике задачи это не должно произойти, но на всякий случай обработаем
                    MessageBox.Show("Не удалось определить ветвь функции. Проверьте значения x и m.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                txtResult.Text = j.ToString("F6"); // вывод с 6 знаками
            }
            catch (FormatException)
            {
                MessageBox.Show("Введите числа в поля x и m.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            // Очищаем поля ввода и сбрасываем результат
            txtX.Clear();
            txtM.Clear();
            txtResult.Clear();
            // Можно сбросить выбор на sh(x) (по умолчанию)
            rbtnSh.IsChecked = true;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page3());
        }
    }
}
