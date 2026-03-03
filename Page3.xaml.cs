using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting; // Для Chart
using System.Windows.Forms.Integration; // Для WindowsFormsHost

namespace PR4_Test
{
    /// <summary>
    /// Логика взаимодействия для Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();

            // Инициализация диаграммы
            InitializeChart();

            txtX0.TextChanged += TxtX0_TextChanged;
            UpdateXkValue();
        }

        // Инициализация диаграммы
        private void InitializeChart()
        {
            // Создаем область построения диаграммы
            ChartPayments.ChartAreas.Add(new ChartArea("MainArea"));

            // Настройка внешнего вида области построения
            var chartArea = ChartPayments.ChartAreas["MainArea"];
            chartArea.AxisX.Title = "x";
            chartArea.AxisX.TitleFont = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            chartArea.AxisY.Title = "y";
            chartArea.AxisY.TitleFont = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            chartArea.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;

            // Создаем серию данных для графика функции
            var series = new Series("Функция y = 9x⁴ + sin(57.2 + x)")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = System.Drawing.Color.Blue,
                IsValueShownAsLabel = false,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8,
                MarkerColor = System.Drawing.Color.Red
            };

            ChartPayments.Series.Add(series);

            // Настройка легенды
            ChartPayments.Legends[0].Docking = Docking.Top;
            ChartPayments.Legends[0].Alignment = StringAlignment.Center;
            ChartPayments.Legends[0].Font = new System.Drawing.Font("Segoe UI", 10);
        }

        // Обновление Xk при изменении X0
        private void UpdateXkValue()
        {
            if (double.TryParse(txtX0.Text, out double x0))
            {
                txtXk.Text = (x0 + 1).ToString("F2");
            }
            else
            {
                txtXk.Text = "";
            }
        }

        private void TxtX0_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateXkValue();
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка ввода X (значение x для вычисления одной точки)
                if (!double.TryParse(txtX.Text, out double x))
                {
                    MessageBox.Show("Введите корректное значение для x", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверка ввода B (не используется в формуле, но оставлен для совместимости)
                if (!double.TryParse(txtB.Text, out double b))
                {
                    MessageBox.Show("Введите корректное значение для B", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверка ввода X0
                if (!double.TryParse(txtX0.Text, out double x0))
                {
                    MessageBox.Show("Введите корректное значение для X0", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверка ввода dx
                if (!double.TryParse(txtDX.Text, out double dx) || dx <= 0)
                {
                    MessageBox.Show("Введите положительное значение для шага dx", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                double xk = x0 + 1; // Xk = X0 + 1

                // Очищаем предыдущие результаты
                txtResult.Clear();

                // Очищаем диаграмму
                UpdateChart(b, x0, xk, dx);

                StringBuilder results = new StringBuilder();
                int count = 0;

                // Цикл табуляции функции от X0 до Xk с шагом dx
                for (double currentX = x0; currentX <= xk + 1e-10; currentX += dx)
                {
                    try
                    {
                        // Вычисление y = 9*x^4 + sin(57.2 + x)
                        double y = 9 * Math.Pow(currentX, 4) + Math.Sin(57.2 + currentX);

                        results.AppendLine($"x = {currentX:F4} \t y = {y:F6}");
                        count++;
                    }
                    catch (Exception ex)
                    {
                        results.AppendLine($"x = {currentX:F4} → ошибка: {ex.Message}");
                    }
                }

                // Добавляем отдельно вычисление для введенного x (из поля txtX)
                if (double.TryParse(txtX.Text, out double singleX))
                {
                    double ySingle = 9 * Math.Pow(singleX, 4) + Math.Sin(57.2 + singleX);
                    results.AppendLine($"\nДля x = {singleX:F4}: y = {ySingle:F6}");
                }

                // Выводим все результаты в txtResult
                txtResult.Text = results.ToString();

                // Обновляем Xk в интерфейсе
                txtXk.Text = xk.ToString("F2");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вычислениях: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для обновления диаграммы
        private void UpdateChart(double b, double x0, double xk, double dx)
        {
            try
            {
                Series currentSeries = ChartPayments.Series.FirstOrDefault();

                if (currentSeries != null)
                {
                    currentSeries.Points.Clear();
                    currentSeries.ChartType = SeriesChartType.Line;

                    for (double xVal = x0; xVal <= xk + 1e-10; xVal += dx)
                    {
                        // Функция определена для всех x
                        double y = 9 * Math.Pow(xVal, 4) + Math.Sin(57.2 + xVal);

                        int pointIndex = currentSeries.Points.AddXY(xVal, y);

                        // Подписи для начальной и конечной точек
                        if (Math.Abs(xVal - x0) < 1e-10 || Math.Abs(xVal - xk) < 1e-10)
                        {
                            currentSeries.Points[pointIndex].Label = $"({xVal:F2}; {y:F2})";
                            currentSeries.Points[pointIndex].Font = new System.Drawing.Font("Segoe UI", 8);
                            currentSeries.Points[pointIndex].LabelForeColor = System.Drawing.Color.DarkGreen;
                        }
                    }

                    ChartPayments.ChartAreas["MainArea"].RecalculateAxesScale();
                    ChartPayments.Update();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при построении диаграммы: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtX.Text = "";
            txtB.Text = "";
            txtX0.Text = "";
            txtDX.Text = "";
            txtXk.Text = "";
            txtResult.Clear();

            Series currentSeries = ChartPayments.Series.FirstOrDefault();
            if (currentSeries != null)
            {
                currentSeries.Points.Clear();
                ChartPayments.Update();
            }
        }
    }
}
