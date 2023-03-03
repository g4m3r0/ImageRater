namespace ImageRater;

using System.IO;
using System.Windows;
using System.Windows.Forms;
using ImageRater.ViewModels;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public ImageRaterViewModel? ImageRater { get; set; }
    public MainWindow()
    {
        using (var dialog = new FolderBrowserDialog())

            while (!Directory.Exists(dialog.SelectedPath))
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.ImageRater = new(dialog.SelectedPath);
                    this.DataContext = this.ImageRater;
                }
            }

        InitializeComponent();
    }
}
