namespace ImageRater.ViewModels;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using GSL.Util.WpfMvvmBase;

public class ImageRaterViewModel : NotifiableBaseObject
{
    public List<string> ImagePaths { get; set; }
    public int CurrentImageIndex { get; set; }
    public string ImageSource { get; set; } = string.Empty;
    public string CsvFileName { get; set; }
    public string ScoreText => $"Score: {this.Score}";
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnPropertyChanged(nameof(ScoreText));
        }
    }

    public DelegateCommand SaveDefectCommand { get; set; }
    public DelegateCommand NextImageCommand { get; set; }
    public DelegateCommand PreviousImageCommand { get; set; }

    private StreamWriter _csvWriter;
    private int _score;

    public ImageRaterViewModel(string imageDirectory, string fileExtension = ".png")
    {
        this.ImagePaths = new List<string>(Directory.GetFiles(imageDirectory, $"*{fileExtension}"));

        this.SaveDefectCommand = new DelegateCommand((o) =>
        {
            this.SaveRating((string)o);
            this.NextImage();
        });

        this.NextImageCommand = new DelegateCommand((a) => NextImage());
        this.PreviousImageCommand = new DelegateCommand((a) => PreviousImage());


        this.CsvFileName = $".\\ratings-{Path.GetRandomFileName()}.csv";
        _csvWriter = new StreamWriter(this.CsvFileName, true);
        _csvWriter.WriteLine("Defect, Image Filename");
        _csvWriter.Flush();

        LoadImage();
    }

    ~ImageRaterViewModel()
    {
        _csvWriter.Close();
    }

    public void LoadImage()
    {
        if (this.ImagePaths.Count == 0)
            MessageBox.Show("No images found inside the specified directory!", "No images found");

        if (this.ImagePaths.Count - 1 < this.CurrentImageIndex)
            MessageBox.Show("No more images to show!");

        this.ImageSource = this.ImagePaths[this.CurrentImageIndex];
        OnPropertyChanged(nameof(ImageSource));
    }

    public void NextImage()
    {
        this.CurrentImageIndex++;
        this.Score++;

        if (this.CurrentImageIndex >= this.ImagePaths.Count)
        {
            MessageBox.Show("No more images to show!");
            return;
        }
        LoadImage();
    }

    public void PreviousImage()
    {
        this.CurrentImageIndex--;
        this.Score--;

        if (this.CurrentImageIndex < 0)
            this.CurrentImageIndex = 0;

        // todo slow
        this._csvWriter.Close();
        RemoveLastRating();
        this._csvWriter = new StreamWriter(this.CsvFileName, true);

        LoadImage();
    }

    public void SaveRating(string rating)
    {
        string imageName = Path.GetFileName(this.ImagePaths[this.CurrentImageIndex]);
        _csvWriter.WriteLine($"{imageName}, {rating}");
        _csvWriter.Flush();
    }

    public void RemoveLastRating()
    {
        string[] lines = File.ReadAllLines(this.CsvFileName);

        // Remove the last line from the array
        if (lines.Length > 0)
            Array.Resize(ref lines, lines.Length - 1);

        // Write the remaining lines back to the CSV file
        File.WriteAllLines(this.CsvFileName, lines);
    }

}
