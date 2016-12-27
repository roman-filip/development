using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using PhotoGadget.Properties;

namespace PhotoGadget.Views
{
    public class PhotoGadgetViewModel : ViewModelBase
    {
        private Random _random = new Random();

        private DispatcherTimer _timer;

        private List<string> _images;

        private Stack<int> _displayedIndexes = new Stack<int>();

        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                Set(ref _imagePath, value);
                ShowImageInViewerCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand ShowNextImageCommand { get; private set; }

        public RelayCommand ShowPreviousImageCommand { get; private set; }

        public RelayCommand ShowImageInViewerCommand { get; private set; }

        public PhotoGadgetViewModel()
        {
            InitializeCommandes();
            LoadImages();
            OnShowNextImage();
            InitTimer();
        }

        private void InitializeCommandes()
        {
            ShowNextImageCommand = new RelayCommand(OnShowNextImage);
            ShowPreviousImageCommand = new RelayCommand(OnShowPreviousImage, () => _displayedIndexes.Count > 0);
            ShowImageInViewerCommand = new RelayCommand(OnShowImageInViewer, () => !string.IsNullOrEmpty(ImagePath));
        }

        private void LoadImages()
        {
            var imagesPath = Settings.Default.ImagesPath;
            _images =
                Directory.EnumerateFiles(imagesPath, "*.*", SearchOption.AllDirectories)
                .Where(s => s.EndsWith(".jpg") || s.EndsWith(".jpeg") || s.EndsWith(".gif") || s.EndsWith(".bmp") || s.EndsWith(".png"))
                .ToList();
        }

        private void InitTimer()
        {
            _timer = new DispatcherTimer { Interval = Settings.Default.RefreshInterval };
            _timer.Tick += (sender, e) => OnShowNextImage();

            _timer.Start();
        }

        private void OnShowNextImage()
        {
            var imageIndex = _random.Next(0, _images.Count);

            _displayedIndexes.Push(imageIndex);
            ImagePath = _images[imageIndex];

            ShowPreviousImageCommand.RaiseCanExecuteChanged();
        }

        private void OnShowPreviousImage()
        {
            var imageIndex = _displayedIndexes.Pop();
            ImagePath = _images[imageIndex];

            ShowPreviousImageCommand.RaiseCanExecuteChanged();
        }

        private void OnShowImageInViewer()
        {
            Process.Start(ImagePath);
        }
    }
}