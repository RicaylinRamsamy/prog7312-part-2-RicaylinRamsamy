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

namespace PROGPOEPART2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> callNumbers;
        List<string> sortedCallNumbers;
        int userPoints = 0;

        public List<string> createRandomCallNumbers(int count)
        {
            Random random = new Random();
            List<string> numbers = new List<string>();

            for (int i = 0; i < count; i++)
            { 
                string number = $"{random.Next(1000):000}.{random.Next(100):00} {createAuthor()}";
                numbers.Add(number);
            }
            return numbers;
        }

        private string createAuthor()
        {
            Random random = new Random();
            string initials = "";
            for (int i = 0; i < 3; i++)
            {
                initials += (char)('A' + random.Next(26));
            }
            return initials;
        }

        public string createCallNumbers()
        {
            callNumbers = createRandomCallNumbers(10);
            sortedCallNumbers = callNumbers.OrderBy(cn => cn).ToList();

            return string.Join(Environment.NewLine, callNumbers);
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BackRB_Click(object sender, RoutedEventArgs e)
        {
            HomeStackPanel.Visibility = Visibility.Visible;
            ReplacingBooks.Visibility = Visibility.Hidden;
        }

        private void SubmitRB_Click(object sender, RoutedEventArgs e)
        {
            List<string> userOrderedNumbers = UserOrderTextBox.Text.Split(new[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries).ToList();

            if (userOrderedNumbers.SequenceEqual(sortedCallNumbers))
            {
                userPoints += 10;
                PointsLabelRB.Content = $"Points: {userPoints}";
                MessageBox.Show("You have successfully replaced the books!");
            }
            else
            {
                MessageBox.Show("Sorry, the order is incorrect. Try Again!");
            }
        }

        private void RestartRB_Click(object sender, RoutedEventArgs e)
        {
            UserOrderTextBox.Clear();
            CallNumberTextBox.Text = createCallNumbers();
        }

        private void StartReplacingBooksTask_Click(object sender, RoutedEventArgs e)
        {
            userPoints = 0;
            UserOrderTextBox.Clear();
            CallNumberTextBox.Text = createCallNumbers();

            HomeStackPanel.Visibility = Visibility.Hidden;
            ReplacingBooks.Visibility = Visibility.Visible;
        }

        private void StartIdentifyingTask_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
