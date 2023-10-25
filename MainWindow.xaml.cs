using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static int myScore;
        public static int maxScore;
            private bool check = true; 
        Random random = new Random();
        IDictionary<string, string> baseQuestions = new Dictionary<string, string>();
        IDictionary<string, string> usedQuestions = new Dictionary<string, string>();


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

        private void LoadQuestions()
        {
            baseQuestions.Clear();
            baseQuestions.Add("000-099", "General Works");
            baseQuestions.Add("100-199", "Philosophy and Psychology");
            baseQuestions.Add("200-299", "Religion");
            baseQuestions.Add("300-399", "Social Sciences");
            baseQuestions.Add("400-499", "Language");
            baseQuestions.Add("500-599", "Natural Sciences and Mathematics");
            baseQuestions.Add("600-699", "Technology");
            baseQuestions.Add("700-799", "The Arts");
            baseQuestions.Add("800-899", "Literature and Rhetoric");
            baseQuestions.Add("900-999", "History, Biography and Geography");
        }
        private void StartIdentifyingTask_Click(object sender, RoutedEventArgs e)
        {
            LoadQuestions();
            RepopulateLists();
            HomeStackPanel.Visibility = Visibility.Hidden;
            IdentifyBooks.Visibility = Visibility.Visible;
        }
        private void UpdateScore(int score)
        {
            myScore += score;
            maxScore += 4;
            PointsLabelIA.Content = $"Score: {myScore}/{maxScore}";
        }

        private void EnableMovementButtons()
        {
            btnUp.IsEnabled = true;
            btnDown.IsEnabled = true;
        }

        private void DisableMovementButtons()
        {
            btnUp.IsEnabled = false;
            btnDown.IsEnabled = false;
        }

        private void RepopulateLists()
        {
            lstQuestions.Items.Clear();
            lstAnswers.Items.Clear();

            var keys = baseQuestions.Keys.ToList();
            for (int i = 0; i < 4; i++)
            {
                string key = keys[random.Next(keys.Count)];
                string value = baseQuestions[key];

                lstQuestions.Items.Add(check ? key : value);
                lstAnswers.Items.Add(check ? value : key);

                keys.Remove(key);
            }

            for (int i = 0; i < 3; i++)
            {
                string key = keys[random.Next(keys.Count)];
                lstAnswers.Items.Add(check ? baseQuestions[key] : key);
                keys.Remove(key);
            }

            ListControls.RandomizeList(lstQuestions);
            ListControls.RandomizeList(lstAnswers);

            check = !check;
        }

        private int CalcScore()
        {
            int score = 0;

            List<ListBoxItem> list = new List<ListBoxItem>();
            ListControls.FindChildren(list, lstAnswers);

            for (int i = 0; i < 4; i++)
            {
                string callNumber = check ? lstQuestions.Items[i].ToString() : lstAnswers.Items[i].ToString();
                string description = check ? lstAnswers.Items[i].ToString() : lstQuestions.Items[i].ToString();

                if (usedQuestions[callNumber] == description)
                {
                    list[i].Background = new SolidColorBrush(Colors.Green);
                    score++;
                }
                else
                {
                    list[i].Background = new SolidColorBrush(Colors.Red);
                }
            }

            for (int i = 4; i < lstAnswers.Items.Count; i++)
            {
                list[i].Background = new SolidColorBrush(Colors.PaleVioletRed);
            }

            return score;
        }

        private void UpdateScoreIA(int score)
        {
            myScore += score;
            maxScore += 4;
            PointsLabelIA.Content = $"Score: {myScore}/{maxScore}";
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            ListControls.SwapIndexes(1, lstAnswers);
        }

        private void RestartIA_Click(object sender, RoutedEventArgs e)
        {
            if (btnSubmitIA.IsEnabled == true)
            {
                MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to start a new game?", "This will reset your score", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    myScore = 0;
                    maxScore = 0;
                    PointsLabelIA.Content = "Begin New Game - Match columns";
                }
                else
                {
                    return;
                }
            }

            LoadQuestions();
            RepopulateLists();
            EnableMovementButtons();
        }

        private void btnSubmttIA_Click(object sender, RoutedEventArgs e)
        {
            int score = CalcScore();
            UpdateScore(score);
            ResetGame();
        }

        private void ResetGame()
        {
            LoadQuestions();
            RepopulateLists();
            DisableMovementButtons();
        }

        private void BackIA_Click(object sender, RoutedEventArgs e)
        {
            HomeStackPanel.Visibility = Visibility.Visible;
            IdentifyBooks.Visibility = Visibility.Hidden;
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            ListControls.SwapIndexes(-1, lstAnswers);
        }
    }

    internal class ListControls
    {
        public static void SwapIndexes(int change, ListBox listbox)
        {
            if (listbox.SelectedItem == null || listbox.SelectedIndex < 0)
            {
                return;
            }

            int newIndex = listbox.SelectedIndex + change;

            if (newIndex < 0 || newIndex >= listbox.Items.Count)
            {
                return;
            }

            listbox.Items.Insert(newIndex, listbox.SelectedItem);
            listbox.Items.RemoveAt(newIndex + (change > 0 ? 0 : 1));
            listbox.SelectedIndex = newIndex;
        }

        public static void RandomizeList(ListBox listBox)
        {
            Random random = new Random();
            List<string> items = new List<string>();

            foreach (string item in listBox.Items)
            {
                items.Add(item);
            }

            int n = items.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                string value = items[k];
                items[k] = items[n];
                items[n] = value;
            }

            listBox.Items.Clear();

            foreach (string item in items)
            {
                listBox.Items.Add(item);
            }
        }

        internal static void FindChildren<T>(List<T> results, DependencyObject startNode)
    where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(startNode);
            for (int i = 0; i < count; i++)
            {
                DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
                if ((current.GetType()).Equals(typeof(T)) || (current.GetType()).GetTypeInfo().IsSubclassOf(typeof(T)))
                {
                    T asType = (T)current;
                    results.Add(asType);
                }
                FindChildren<T>(results, current);
            }
        }

    }
}
