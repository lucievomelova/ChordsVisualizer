using System;
using System.ComponentModel;
using System.Windows;

namespace Akordy
{
    /// <summary> Settings window </summary>
    public partial class Settings : Window
    {
        private MainWindow mainWindow;
        public Settings(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            TimerSlider.Value = mainWindow.TimerInterval;
            TimerSlider.Minimum = 1;
            
            //check correct radio button depending on whether timer is used or not
            if (mainWindow.UseTimer)
                UseTimer.IsChecked = true;
            else
                UseNext.IsChecked = true;
        }

        /// <summary> When Settings window is closed, enable main window again </summary>
        private void EnableMainWindow(object sender, CancelEventArgs cancelEventArgs)
        {
            if (UseNext.IsChecked == true)
                mainWindow.UseTimer = false;
            else
                mainWindow.UseTimer = true;

            if (mainWindow.IsTimerRunning)
                mainWindow.SetControlsBeforeRun();
            
            //if timer was paused
            else if ((string) mainWindow.PauseBt.Content == "Continue")
            {
                mainWindow.SetControlsBeforeRun();
                mainWindow.StopTimer();
            }
            else
                mainWindow.SetControlsAfterRun();
            
            mainWindow.IsEnabled = true;
        }

        /// <summary> set interval of the timer when slider value is changed </summary>
        private void SetInterval(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SliderValue.Content = TimerSlider.Value + "s";
            mainWindow.TimerInterval = (int) TimerSlider.Value;
            mainWindow.Timer.Interval = new TimeSpan(0, 0, mainWindow.TimerInterval);
        }
    }
}