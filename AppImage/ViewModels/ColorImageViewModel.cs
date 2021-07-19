using AppImage.Common;
using AppImage.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using System.Windows;

namespace AppImage.ViewModels
{
    public class ColorImageViewModel : BaseViewModel
    {
        private int R;
        private int G;
        private int B;
        private int Rows;
        private int Cols;
        private Bitmap bm;

        private string filePath;
        public string FilePath
        {
            get => filePath;
            set
            {
                if (!value.Equals(filePath))
                {
                    filePath = value;
                    OnPropertyChanged(nameof(FilePath));
                }
            }
        }

        private ColorImage colorImages;
        public ColorImage ColorImages
        {
            get => colorImages;
            set
            {
                if (!value.Equals(ColorImages))
                {
                    ColorImages = value;
                    OnPropertyChanged(nameof(ColorImages));
                }
            }
        }
        public ColorImageViewModel()
        {
            colorImages = new ColorImage();
        }

        private DelegateCommand loadFileButtonCommand;
        public DelegateCommand LoadFileButtonCommand
        {
            get
            {
                return loadFileButtonCommand ?? (loadFileButtonCommand = new DelegateCommand((obj) =>
                {
                    IsLoad = true;
                    OpenFileDialog openFileDialog = new OpenFileDialog();

                    if (openFileDialog.ShowDialog() == true)
                    {
                        openFileDialog.Filter = "(*.png,svg,jpeg,psd)|*.png";

                        FilePath = openFileDialog.FileName;
                    }
                    IsLoad = false;
                }));
            }
        }

        private DelegateCommand countRGBButtonCommand;
        public DelegateCommand CountRGBButtonCommand
        {
            get
            {
                return countRGBButtonCommand ?? (countRGBButtonCommand = new DelegateCommand((obj) =>
                {
                    IsLoad = true;

                    if (filePath == null || filePath == string.Empty)
                    {
                        MessageBox.Show("You need to select file with another extention!");
                    }
                    else
                    {
                        bm = new Bitmap(FilePath);

                        Cols = bm.Width;
                        Rows = bm.Height;

                        R = 0;
                        G = 0;
                        B = 0;

                        for (int y = 0; y < Rows; y++)
                        {
                            for (int x = 0; x < Cols; x++)
                            {
                                R += bm.GetPixel(x, y).R;
                                G += bm.GetPixel(x, y).G;
                                B += bm.GetPixel(x, y).B;
                            }
                        }

                        int sumRGB = R + B + G;

                        var r = (double)R / sumRGB * 100;
                        var b = (double)G / sumRGB * 100;
                        var g = (double)B / sumRGB * 100;

                        colorImages.Rpercent = Math.Round(r, 2);
                        colorImages.Gpercent = Math.Round(g, 2);
                        colorImages.Bpercent = Math.Round(b, 2);
                    }

                    IsLoad = false;
                }));
            }
        }
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            {
                if (propertyName == nameof(ColorImages))
                {
                    ColorImages = ColorImages;
                }
                if (propertyName == nameof(FilePath))
                {
                    ColorImages.Rpercent = 0;
                    ColorImages.Gpercent = 0;
                    ColorImages.Bpercent = 0;
                }
            }
        }
    }
}
