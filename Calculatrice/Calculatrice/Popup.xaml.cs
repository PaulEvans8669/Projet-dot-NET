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
using System.Windows.Shapes;

namespace Calculatrice
{
    /// <summary>
    /// Logique d'interaction pour popup.xaml
    /// </summary>
    public partial class Popup : Window
    {
        
        public MainWindow CallerWindow { get; set; }
        
        public Operation ClickedOperation { get; set; }
        
        public bool IsClosing { get; set; }



        public Popup(MainWindow mainWindow, Operation operation)
        {
            this.CallerWindow = mainWindow;
            this.ClickedOperation = operation;
            this.IsClosing = false;
            InitializeComponent();
        }

        private void buttonFormule_Click(object sender, RoutedEventArgs e)
        {
            CallerWindow.Saisie += ClickedOperation.Entree;
            IsClosing = true;
            this.Close();
        }

        private void buttonResultat_Click(object sender, RoutedEventArgs e)
        {
            CallerWindow.Saisie += ClickedOperation.Resultat;
            IsClosing = true;
            this.Close();
        }

        private void popup_deactivated(object sender, EventArgs e)
        {
            if (!IsClosing)
            {
                this.Close();
            }
        }
    }
}
