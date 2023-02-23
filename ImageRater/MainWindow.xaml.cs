namespace ImageRater;

using System.Windows;
using ImageRater.ViewModels;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public ImageRaterViewModel ImageRater { get; set; }
    public MainWindow()
    {
        this.ImageRater = new("C:\\workingDirectory\\ForschungsPraktikum\\D600\\screenshots\\screenshots\\screen_all_in_one_folder");
        this.DataContext = this.ImageRater;

        InitializeComponent();
    }
}
