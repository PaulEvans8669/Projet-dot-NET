using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Calculatrice
{
    
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public MainWindow()
        {
            ListeOperations = new ObservableCollection<Operation>();
            this.DataContext = this;
            InitializeComponent();
            Console.WriteLine(operationIsValid("465465+9849846+946544+4"));
            Console.WriteLine(operationIsValid("/465465+9849846+946544+4"));
            Console.WriteLine(operationIsValid("+465465+9849846+946544+4"));
            Console.WriteLine(operationIsValid("-46/(54)*65+9+(849)-8+4/6+9(4*6*5)544+(4)"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Operation> _listeOperations;

        public ObservableCollection<Operation> ListeOperations
        {
            get
            {
                return _listeOperations;
            }
            set
            {
                _listeOperations = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ListeOperations)));
            }
        }

        private string _saisie;

        public string Saisie
        {
            get { return _saisie; }
            set {
                _saisie = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Saisie)));
            }
        }

        private String[] simpleEntries = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ",", "-", "+", "/", "x", "(", ")" };

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button sourceButton = (Button)sender;
            string buttonContent = sourceButton.Content.ToString();
            if (string.IsNullOrEmpty(Saisie))
            {
                Saisie = "";
            }
            if (simpleEntries.Contains(buttonContent))
            {
                
                Saisie += buttonContent;
            }
            else
            {
                if (buttonContent.Equals("Clear"))
                {
                    if (string.IsNullOrEmpty(Saisie))
                    {
                        ListeOperations.Clear();
                    }
                    else
                    {
                        Saisie = "";
                    }
                }
                else if (buttonContent.Equals("Rep"))
                {
                    if(ListeOperations.Count != 0)
                    {
                        Saisie += ListeOperations[ListeOperations.Count - 1].Resultat;
                    }

                }
                else if (buttonContent.Equals("="))
                {
                    ListeOperations.Add(new Operation(Saisie));
                    Saisie = "";
                }
                else if (buttonContent.Equals("<"))
                {
                    if (Saisie.Length > 0)
                    {
                        Saisie = Saisie.Substring(0, Saisie.Length - 1);
                    }
                }
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            Operation selectedOperation = (Operation)listBox.SelectedItem;
            if (selectedOperation != null)
            {
                Saisie += selectedOperation.Entree;
            }

        }

        private void ListBox_FocusLost(object sender, RoutedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            listBox.UnselectAll();
        }

        private Key[] validKeyslist = { Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4, Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9, Key.Divide, Key.Multiply, Key.Subtract, Key.Add, Key.Return, Key.Decimal, Key.D5, Key.OemOpenBrackets };

        private void TranslateKeyPress(Key key)
        {
            int index;
            for (index = 0; index < validKeyslist.Count(); index++)
            {
                if (key == validKeyslist[index])
                {
                    break;
                }
            }
            if (index <= 9)
            {
                Saisie += index.ToString();
            }
            switch (index)
            {
                case 10:
                    Saisie += "/";
                    break;
                case 11:
                    Saisie += "*";
                    break;
                case 12:
                    Saisie += "-";
                    break;
                case 13:
                    Saisie += "+";
                    break;
                case 14:
                    ListeOperations.Add(new Operation(Saisie));
                    Saisie = "";
                    break;
                case 15:
                    Saisie += ",";
                    break;
                case 16:
                    Saisie += "(";
                    break;
                case 17:
                    Saisie += ")";
                    break;
                case 18:
                    Saisie = Saisie.Substring(0, Saisie.Length - 1);
                    break;
                default:
                    Saisie += "";
                    break;
            }
        }
        private void Event_KeyUp(object sender, KeyEventArgs e)
        {
            TranslateKeyPress(e.Key);
            Console.WriteLine(e.Key.ToString());
        }

        private bool isANumber(char c)
        {
            Regex regex = new Regex("[0-9]");
            return regex.IsMatch(c.ToString());
        }

        public bool operationIsValid(string operation)
        {
            char firstChar = operation.ElementAt(0);
            if (firstChar == '/' || firstChar == '*')
            {
                return false;
            }
            else
            {
                if (isANumber(firstChar))
                {
                    operation = "+" + operation;
                }
            }

            char secondChar = operation.ElementAt(1);
            if (!isANumber(secondChar)){
                return false;
            }

            int cpt = 0;
            foreach(char c in operation)
            {
                if(c == '(')
                {
                    cpt++;
                }
                if(c == ')')
                {
                    cpt--;
                }
            }
            if(cpt != 0)
            {
                return false;
            }

            for (int i = 2; i <operation.Length-1; i++)
            {
                char previousChar = operation.ElementAt(i - 1);
                char c = operation.ElementAt(i);
                char nextChar = operation.ElementAt(i + 1);
                if (c == '+')
                {
                    if(!(previousChar == ')' || isANumber(previousChar) || nextChar == '(' || isANumber(previousChar)))
                    {
                        return false;
                    }
                }
                else if(c == '-')
                {
                    if (!(previousChar == ')' || isANumber(previousChar) || nextChar == '(' || isANumber(previousChar)))
                    {
                        return false;
                    }
                }
                else if(c == '/')
                {
                    if (!(previousChar == ')' || isANumber(previousChar) || nextChar == '(' || isANumber(previousChar)))
                    {
                        return false;
                    }
                }
                else if (c == '*')
                {
                    if (!(previousChar == ')' || isANumber(previousChar) || nextChar == '(' || isANumber(previousChar)))
                    {
                        return false;
                    }
                }
                else if(c == '(')
                {
                    if (!(previousChar == '+' || previousChar == '-' || previousChar == '/' || previousChar == '*' || isANumber(previousChar) || isANumber(nextChar) || nextChar == '-'))
                    {
                        return false;
                    }
                }
                else if(c == ')')
                {
                    if (!(nextChar == '+' || nextChar == '-' || nextChar == '/' || nextChar == '*' || isANumber(nextChar) || isANumber(previousChar) || previousChar == '-'))
                    {
                        return false;
                    }
                }
            }
            char lastChar = operation.ElementAt(operation.Length-1);
            if (!(isANumber(lastChar) || lastChar == ')'))
            {
                return false;
            }
            return true;
        }
    }
}
