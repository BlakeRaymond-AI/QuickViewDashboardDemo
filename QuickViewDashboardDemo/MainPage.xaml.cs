using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using ComponentArt.Silverlight.DataVisualization.Utils;

namespace QuickViewDashboardDemo
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            DashboardControlViewModel dcViewModel = new DashboardControlViewModel();
            dcViewModel.LoadData(1000000);
            DataContext = dcViewModel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }

    public class DashboardControlViewModel : INotifyPropertyChanged
    {
        public void LoadData(int numValues)
        {
            DataPoints = new DemoDataValues();
            Random rndm = new Random();
            DateTime time = DateTime.Now.AddYears(-10);
            double value = 100;
            int i = 0;
            while (i < numValues)
            {
                time = time.AddMinutes(1);
                value += rndm.Next(-5, 6);
                DataPoints.Add(new DemoDataValue
                {
                    Date = time, 
                    Y = value
                });
                ++i;
            }
            StartTime = DataPoints.First().Date;
            EndTime = DataPoints.Last().Date;
            SelectedStartTime = DataPoints[3*numValues / 8].Date;
            SelectedEndTime = DataPoints[5*numValues / 8].Date;
        }

        #region AverageValue (INotifyPropertyChanged Property)
        private double _averageValue;

        public double AverageValue
        {
            get
            {
                return 0.0;
                double total = 0;
                int count = 0;

                foreach (DemoDataValue t in DataPoints.Where(t => t.Date >= SelectedStartTime && t.Date <= SelectedEndTime))
                {
                    total += t.Y;
                    count++;
                }
                AverageValue = (count > 0) ? total / count : _averageValue;
                return _averageValue;
            }
            set
            {
                if (_averageValue != value)
                {
                    _averageValue = value;
                    RaisePropertyChanged("AverageValue");
                }
            }
        }
        #endregion

        #region SelectedStartTime (INotifyPropertyChanged Property)
        private DateTime _selectedStartTime;

        public DateTime SelectedStartTime
        {
            get { return _selectedStartTime; }
            set
            {
                if (_selectedStartTime != value)
                {
                    _selectedStartTime = value;
                    RaisePropertyChanged("SelectedStartTime");
                    double a = AverageValue; // cause AverageValue update.
                }
            }
        }
        #endregion

        #region SelectedEndTime (INotifyPropertyChanged Property)
        private DateTime _selectedEndTime;

        public DateTime SelectedEndTime
        {
            get { return _selectedEndTime; }
            set
            {
                if (_selectedEndTime != value)
                {
                    _selectedEndTime = value;
                    RaisePropertyChanged("SelectedEndTime");
                    double a = AverageValue; // cause AverageValue update.
                }
            }
        }
        #endregion

        #region StartTime (INotifyPropertyChanged Property)
        private DateTime _startTime;

        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    RaisePropertyChanged("StartTime");
                }
            }
        }
        #endregion

        #region EndTime (INotifyPropertyChanged Property)
        private DateTime _endTime;

        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    RaisePropertyChanged("EndTime");
                }
            }
        }
        #endregion

        #region DataPoints (INotifyPropertyChanged Property)
        private DemoDataValues _dataPoints;

        public DemoDataValues DataPoints
        {
            get { return _dataPoints; }
            set
            {
                if (_dataPoints != value)
                {
                    _dataPoints = value;
                    RaisePropertyChanged("DataPoints");
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged values

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
