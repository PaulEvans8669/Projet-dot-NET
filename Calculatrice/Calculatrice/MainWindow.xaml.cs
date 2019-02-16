using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace Calculatrice
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            ListeOperations = new ObservableCollection<Operation>();
            this.DataContext = this;
            InitializeComponent();
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
            set { _saisie = value; }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ListeOperations.Add(new Operation(Saisie));
        }
    }
}
