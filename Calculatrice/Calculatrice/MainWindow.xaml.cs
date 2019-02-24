﻿using System;
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

        private bool darkColorTheme;

        public MainWindow()
        {
            ListeOperations = new ObservableCollection<Operation>();
            this.DataContext = this;
            setColorsNightMode();
            InitializeComponent();
        }

        private void setColorsDayMode()
        {

            Application.Current.Resources["windowBackgroundColor"] = Color.FromRgb(200, 200, 200);
            Application.Current.Resources["globalForegroundColor"] = Color.FromRgb(0, 0, 0);
            Application.Current.Resources["normalButtonBackgroundColor"] = Color.FromRgb(220, 220, 220);
            Application.Current.Resources["numericButtonBackgroundColor"] = Color.FromRgb(240, 240, 240);
            darkColorTheme = false;
        }

        private void setColorsNightMode()
        {
            Application.Current.Resources["windowBackgroundColor"] = Color.FromRgb(52, 52, 52);
            Application.Current.Resources["globalForegroundColor"] = Color.FromRgb(255, 255, 255);
            Application.Current.Resources["normalButtonBackgroundColor"] = Color.FromRgb(21, 21, 21);
            Application.Current.Resources["numericButtonBackgroundColor"] = Color.FromRgb(29, 29, 29);
            darkColorTheme = true;

        }

        private void changeColorTheme()
        {
            if (darkColorTheme)
            {
                setColorsDayMode();
            }
            else
            {
                setColorsNightMode();
            }
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

        private Window GetParentWindow( DependencyObject child)
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if(parentObject == null)
            {
                return null;
            }

            Window parent = parentObject as Window;
            if(parent == null)
            {
                return parent;
            }
            else
            {
                return GetParentWindow(parentObject);
            }
        }

        private void resetFocus(FrameworkElement element)
        {
            var scope = FocusManager.GetFocusScope(element);
            FocusManager.SetFocusedElement(scope, null);
            Keyboard.Focus(GetParentWindow(element));
        }

        private String[] simpleEntries = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ",", "-", "+", "/", "(", ")" };

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button sourceButton = (Button)sender;
            resetFocus(sourceButton);
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
                else if (buttonContent.Equals("x"))
                {
                    Saisie += "*";
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
                    if (!string.IsNullOrEmpty(Saisie))
                    {
                        ListeOperations.Add(new Operation(Saisie));
                        Saisie = "";
                    }
                }
                else if (buttonContent.Equals("<"))
                {
                    erase();
                }
                else if (buttonContent.Equals("sin (s)"))
                {
                    Saisie += "sin(";
                }
                else if (buttonContent.Equals("cos (c)"))
                {
                    Saisie += "cos(";
                }
                else if (buttonContent.Equals("√ (r)"))
                {
                    Saisie += "√(";
                }
                else if (buttonContent.Equals("exp (e)"))
                {
                    Saisie += "exp(";
                }
                else if (buttonContent.Equals("ln (l)"))
                {
                    Saisie += "ln(";
                }
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            Operation selectedOperation = (Operation)listBox.SelectedItem;
            if (selectedOperation != null)
            {
                Popup popup = new Popup(this, selectedOperation);
                popup.Show();
                Point mouseLocation = PointToScreen(Mouse.GetPosition(this));
                popup.Left = mouseLocation.X;
                popup.Top = mouseLocation.Y - popup.Height;
            }
            listBox.UnselectAll();

        }

        private void ListBox_FocusLost(object sender, RoutedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            listBox.UnselectAll();
        }

        private void erase()
        {
            int length = Saisie.Length;

            if (length > 3)
            {
                //"sin(" "cos(" "exp("
                char avantDernierChar = Saisie.ElementAt(Saisie.Length - 2);
                if(avantDernierChar == 'n' || avantDernierChar == 's' || avantDernierChar == 'p')
                {
                    Saisie = Saisie.Substring(0, Saisie.Length - 4);
                }
                else
                {
                    Saisie = Saisie.Substring(0, Saisie.Length - 1);
                }

            }
            else if (length > 2)
            {
                //"ln("
                char avantDernierChar = Saisie.ElementAt(Saisie.Length - 2);
                if (avantDernierChar == 'n')
                {
                    Saisie = Saisie.Substring(0, Saisie.Length - 3);
                }
                else
                {
                    Saisie = Saisie.Substring(0, Saisie.Length - 1);
                }
            }
            else if (length > 1)
            {
                //"√("
                char avantDernierChar = Saisie.ElementAt(Saisie.Length - 2);
                if (avantDernierChar == '√')
                {
                    Saisie = Saisie.Substring(0, Saisie.Length - 2);
                }
                else
                {
                    Saisie = Saisie.Substring(0, Saisie.Length - 1);
                }

            }
            else if (length > 0)
            {
                //"^" "[0-9]"
                Saisie = Saisie.Substring(0, Saisie.Length - 1);
            }
        }

        private Key[] validKeyslist = { Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4, Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9, Key.Divide, Key.Multiply, Key.Subtract, Key.Add, Key.Return, Key.Decimal, Key.D5, Key.OemOpenBrackets, Key.Back, Key.S, Key.C, Key.R, Key.E, Key.L};

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
                    if (!string.IsNullOrEmpty(Saisie))
                    {
                        ListeOperations.Add(new Operation(Saisie));
                        Saisie = "";
                    }
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
                    erase();
                    break;
                case 19:
                    Saisie += "sin(";
                    break;
                case 20:
                    Saisie += "cos(";
                    break;
                case 21:
                    Saisie += "√(";
                    break;
                case 22:
                    Saisie += "exp(";
                    break;
                case 23:
                    Saisie += "ln(";
                    break;
                default:
                    Saisie += "";
                    break;
            }
        }
        private void Event_KeyUp(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(Saisie))
            {
                Saisie = "";
            }
            TranslateKeyPress(e.Key);
            //Console.WriteLine(e.Key.ToString());
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

        private void modebutton_Click(object sender, RoutedEventArgs e)
        {
            changeColorTheme();
            resetFocus((Button)sender);
        }
    }
}