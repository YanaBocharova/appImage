using AppImage.Common;
using AppImage.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows;

namespace AppImage.ViewModels
{
    public class ColorImageViewModel : BaseViewModel
    {
        static int countImage = 0;
        private int R;
        private int G;
        private int B;
        private int Rows;
        private int Cols;
        private Bitmap bm;
        private int tmpX;
        private int leftX;
        private int rightX;
        private byte[] rightGuy;
        private byte[] leftGuy;
        private Color notExist;


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
                if (!value.Equals(colorImages))
                {
                    colorImages = value;
                    OnPropertyChanged(nameof(ColorImages));
                }
            }
        }

        private string interpolatePath;
        public string InterpolatePath
        {
            get => interpolatePath;
            set
            {
                if (!value.Equals(interpolatePath))
                {
                    interpolatePath = value;
                    OnPropertyChanged(nameof(InterpolatePath));
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

        private DelegateCommand interpolateButtonCommand;
        public DelegateCommand InterpolateButtonCommand
        {
            get
            {
                return interpolateButtonCommand ?? (interpolateButtonCommand = new DelegateCommand((obj) =>
                {
                    IsLoad = true;

                    FilePath = @"C:\Users\hp\Desktop\test1.png";

                    if (FilePath == null || FilePath == string.Empty)
                    {
                        MessageBox.Show("You need to load file");
                    }
                    else
                    {
                        bm = new Bitmap(FilePath);
                        Cols = bm.Width;
                        Rows = bm.Height;

                        for (int x = 0; x < Cols; x++)
                        {
                            for (int y = 0; y < Rows; y++)
                            {

                                var value = bm.GetPixel(x, y);

                                notExist = Color.FromArgb(255, 127, 127, 127);

                                if (value.Equals(notExist) && x == 0)
                                {
                                    var valuePref = bm.GetPixel(x, y);
                                    leftX = x;
                                    leftGuy = new byte[] { valuePref.A, valuePref.R, valuePref.G, valuePref.B };
                                    tmpX = x;

                                    while (value.Equals(notExist) && x < Cols - 1)
                                    {
                                        value = bm.GetPixel(x, y);
                                        x++;
                                    }
                                }

                                if (value.Equals(notExist))
                                {
                                    var valuePref = bm.GetPixel(x - 1, y);
                                    leftX = x - 1;
                                    leftGuy = new byte[] { valuePref.A, valuePref.R, valuePref.G, valuePref.B };
                                    tmpX = x;

                                    while (value.Equals(notExist) && x < Cols -1)
                                    {
                                        value = bm.GetPixel(x, y);
                                        x++;
                                    }
                                }

                                if (!(value.Equals(notExist)) && tmpX != 0 && x < Cols - 1)
                                {
                                    rightX = x;
                                    rightGuy = new byte[] { value.A, value.R, value.G, value.B };
                                    byte[] byteColor = InterpolateMe(leftGuy, rightGuy, leftX, rightX, tmpX);
                                    bm.SetPixel(tmpX, y, Color.FromArgb(byteColor[0], byteColor[1], byteColor[2], byteColor[3]));
                                    x = tmpX;
                                    tmpX = 0;
                                }
                            }
                        }

                        var interpolarImgPath = @"C:\Users\hp\Desktop\img" + countImage.ToString() + ".png";
                        countImage++;
                        bm.Save(interpolarImgPath);
                        InterpolatePath = interpolarImgPath;
                    }

                    IsLoad = false;
                }));
            }
        }
        private byte[] InterpolateMe(byte[] leftGuy, byte[] rightGuy, int x1, int x2, int x)
        {
            byte[] interArr = new byte[4];
            interArr[0] = InterpolateVal(leftGuy[0], rightGuy[0], x1, x2, x);
            interArr[1] = InterpolateVal(leftGuy[1], rightGuy[1], x1, x2, x);
            interArr[2] = InterpolateVal(leftGuy[2], rightGuy[2], x1, x2, x);
            interArr[3] = InterpolateVal(leftGuy[3], rightGuy[3], x1, x2, x);
            return interArr;
        }
        private byte InterpolateVal(byte val1, byte val2, int x1, int x2, int x)
        {
            return Convert.ToByte(val1 + ((val2 - val1) / (x2 - x1)) * (x - x1));
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            {
                if (propertyName == nameof(ColorImages))
                {
                    ColorImages = ColorImages;
                }
                if(propertyName == nameof(InterpolatePath))
                {
                    InterpolatePath = InterpolatePath;
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
