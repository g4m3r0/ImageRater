namespace ImageRater;

using ImageRater.ViewModels;
using System.Windows;
using System.Windows.Forms;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public ImageRaterViewModel? ImageRater { get; set; }
    public MainWindow()
    {
        using (var dialog = new FolderBrowserDialog())
        {
            dialog.Description = "Please select the folder that contains your input images.";
            dialog.UseDescriptionForTitle = true;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ImageRater = new(dialog.SelectedPath);
                this.DataContext = this.ImageRater;
                this.ContentRendered += MainWindow_ContentRendered;
            }
            else
            {
                this.Close();
            }
        }

        InitializeComponent();
    }

    // This event handler is called after the MainWindow's content has been rendered. 
    // It sets the Topmost property of the MainWindow to true and then false, 
    // so that the window is brought to the front and stays on top after everything is rendered.
    private void MainWindow_ContentRendered(object? sender, System.EventArgs e)
    {
        this.Topmost = true;
        this.Topmost = false;
    }
}
