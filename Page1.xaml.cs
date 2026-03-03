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
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Считываем значения из полей
                double x = double.Parse(txtX.Text);
                double y = double.Parse(txtY.Text);
                double z = double.Parse(txtZ.Text);

                // Проверка: знаменатель (arctg(x) + arctg(z)) не должен быть равен 0
                double denominator = Math.Atan(x) + Math.Atan(z);
                if (denominator == 0)
                {
                    MessageBox.Show("Ошибка: знаменатель (arctg(x) + arctg(z)) не может быть равен нулю.",
                        "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверка: аргумент логарифма должен быть положительным (для ln^2 y)
                if (y <= 0)
                {
                    MessageBox.Show("Ошибка: y должно быть положительным для вычисления ln(y).",
                        "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Вычисляем первую часть: e^{|x-y|} * |x-y|^(x+y) / (arctg(x) + arctg(z))
                double absDiff = Math.Abs(x - y);

                // Проверка: |x-y|^(x+y) - основание должно быть неотрицательным, а для отрицательного основания показатель должен быть целым
                if (absDiff < 0 && (x + y) % 1 != 0)
                {
                    MessageBox.Show("Ошибка: при отрицательном |x-y| показатель степени должен быть целым.",
                        "Некорректный ввод", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                double expPart = Math.Exp(absDiff);                          // e^{|x-y|}
                double powerPart = Math.Pow(absDiff, x + y);                 // |x-y|^(x+y)
                double firstPart = (expPart * powerPart) / denominator;      // (e^{|x-y|} * |x-y|^(x+y)) / (arctg(x) + arctg(z))

                // Вычисляем вторую часть: кубический корень из (x^2 + ln^2 y)
                double lnY = Math.Log(y);                                    // ln(y)
                double lnYSquare = Math.Pow(lnY, 2);                         // ln^2 y
                double xSquare = Math.Pow(x, 2);                             // x^2
                double underCubeRoot = xSquare + lnYSquare;                  // x^2 + ln^2 y

                // Кубический корень можно вычислить через Math.Pow с показателем 1/3
                // Для отрицательных чисел кубический корень существует
                double secondPart;
                if (underCubeRoot >= 0)
                {
                    secondPart = Math.Pow(underCubeRoot, 1.0 / 3.0);
                }
                else
                {
                    // Для отрицательных чисел используем знак
                    secondPart = -Math.Pow(-underCubeRoot, 1.0 / 3.0);
                }

                // Общий результат: сумма двух частей
                double result = firstPart + secondPart;

                // Выводим результат
                txtResult.Text = result.ToString("F6"); // можно изменить формат
            }
            catch (FormatException)
            {
                MessageBox.Show("Пожалуйста, введите числа во все поля.",
                    "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            // Очищаем все поля ввода и сбрасываем результат на 0
            txtX.Clear();
            txtY.Clear();
            txtZ.Clear();
            txtResult.Text = "0";
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
        }
    }
}
