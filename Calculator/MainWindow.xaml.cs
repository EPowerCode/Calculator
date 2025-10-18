using Calculator.Mortgage;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MortgageBtn_Click(object sender, RoutedEventArgs e)
    {
        MortgageView view = new();
        view.Show();
        this.Close();
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}