using AppImage.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppImage.Models
{
    public class ColorImage : BaseViewModel
    {
        private double bpercent;
        public double Bpercent
        {
            get => bpercent;
            set
            {
                if (!value.Equals(bpercent))
                {
                    bpercent = value;
                    OnPropertyChanged(nameof(Bpercent));
                }
            }
        }

        private double gpercent;
        public double Gpercent
        {
            get => gpercent;
            set
            {
                if (!value.Equals(gpercent))
                {
                    gpercent = value;
                    OnPropertyChanged(nameof(Gpercent));
                }
            }
        }

        private double rpercent;
        public double Rpercent
        {
            get => rpercent;
            set
            {
                if (!value.Equals(rpercent))
                {
                    rpercent = value;
                    OnPropertyChanged(nameof(Rpercent));
                }
            }
        }
    }
}
