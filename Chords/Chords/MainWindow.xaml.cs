using System;
using System.Windows;

namespace Akordy
{
    public partial class MainWindow
    {
        /// <summary>
        /// variable that sets which chord is shown
        /// it's value is from 0 to (number_of_chords - 1), when the highest value is reached, chordNumber is set to 0
        /// </summary>
        private int chordNumber = 0;

        /// <summary> array of all chords, that were given as the input </summary>
        private Chords.Chord[] allChords;

        /// <summary> determines whether timer is used (true) or  not (false)</summary>
        public bool UseTimer = true;

        public bool IsTimerRunning = false;

        public int TimerInterval = 1;

        public Draw draw;

        private readonly ReadInput readInput;

        private readonly Chords chords;

        public readonly System.Windows.Threading.DispatcherTimer Timer;
        
        public MainWindow()
        {
            InitializeComponent();
            this.SizeChanged += MainWindow_SizeChanged;
            draw = new Draw(this);
            readInput = new ReadInput();
            chords = new Chords(this);
            draw.PlainPiano();

            Timer = new System.Windows.Threading.DispatcherTimer();
            Timer.Tick += new EventHandler(dispatcherTimer_Tick);
            Timer.Interval = new TimeSpan(0,0,TimerInterval);
            SetControlsAfterRun();
        }

        private void StartTimer()
        {
            IsTimerRunning = true;
            Timer.Start();
        }

        public void StopTimer()
        {
            if (Timer.IsEnabled)
            {
                Timer.Stop();
                IsTimerRunning = false;
            }
        }
        
        /// <summary> timer tick event - draw nex chord </summary>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            chords.MakeChord(allChords[chordNumber]);
            IncrementChordNumber();
        }

        /// <summary> set controls (buttons) depending on whether timer is used or not </summary>
        public void SetControlsBeforeRun()
        {
            if(Timer.IsEnabled)
                StopTimer();
            if(UseTimer)
                StartTimer();
            
            //PauseBt and StopBt are enabled when timer is used, NextBt is enabled otherwise
            PauseBt.IsEnabled = UseTimer;
            NextBt.IsEnabled = !UseTimer;
            StopBt.IsEnabled = UseTimer;
            
            if(!UseTimer)
                PauseBt.Content = "Pause";
        }
        
        /// <summary> after run - disable buttons, stop timer if necessary and draw plain piano </summary>
        public void SetControlsAfterRun()
        {
            chordNumber = 0;
            StopTimer();
            PauseBt.IsEnabled = false;
            NextBt.IsEnabled = false;
            StopBt.IsEnabled = false;
            draw.PlainPiano();
            ChordLabel.Content = "Chord: ---";
            PauseBt.Content = "Pause";
        }
        
        //open settings window
        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings(this);
            //disable main window while settings window is opened
            this.IsEnabled = false;
            settings.Show();
        }

        /// <summary> draw first chord and start timer (if needed) </summary>
        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            chordNumber = 0;
            //read input
            allChords = chords.AllChords(readInput.SplitChords(chordTB.Text));
            
            //invalid input -> allChords is empty -> show error message
            if (allChords.Length == 0)
            {
                SetControlsAfterRun();
                MessageBox.Show("Wrong input.");
                draw.PlainPiano();
            }
            else
            {
                //set MiddlePos to NO_MIDDLE, because new MiddlePos has to be set
                chords.SetMiddlePos();
                
                //draw first chord and set MiddlePos
                chords.MakeChord(allChords[chordNumber]);
                IncrementChordNumber();
                
                SetControlsBeforeRun();
                if(UseTimer)
                    StartTimer();
            }
        }

        /// <summary> draw next chord after NextBt click </summary>
        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            chords.MakeChord(allChords[chordNumber]);
            IncrementChordNumber();
        }

        private void IncrementChordNumber()
        {
            chordNumber++;
            if (chordNumber >= allChords.Length)
                chordNumber = 0;
        }
        
        /// <summary> stop timer and draw plain piano </summary>
        private void StopButtonClick(object sender, RoutedEventArgs e)
        {
            allChords = new Chords.Chord[] { };
            SetControlsAfterRun();
        }
        
        //pause or unpause timer
        private void PauseButtonClick(object sender, RoutedEventArgs e)
        {
            if ((string)PauseBt.Content == "Pause")
            {
                StopTimer();
                PauseBt.Content = "Continue";
            }
            else
            {
                StartTimer();
                PauseBt.Content = "Pause";
            }
        }
        
        //redraw piano when the window is resized
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //stop timer if it's running
            if (Timer.IsEnabled)
            {
                Timer.Stop();
                IsTimerRunning = true;
            }

            //redraw piano
            piano.Children.Clear();
            draw = new Draw(this);
            draw.PlainPiano();

            //redraw last chord
            if (allChords != null && allChords.Length > 0)
            {
                chords.MakeChord(allChords[chordNumber]);

                if (chordNumber >= allChords.Length)
                    chordNumber = 0;
                
                //start timer again
                if (IsTimerRunning)
                    StartTimer();
            }
        }
    }
}