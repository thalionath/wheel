using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CrazyWheel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        private WheelMotion motion_;
        private WheelMotionViewModel model_;
        private DispatcherTimer update_timer_ = new DispatcherTimer();
        private Stopwatch stop_watch_ = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();

            // start example //////////////////////////////////////////////////////////////////////

            motion_ = new WheelMotion // the mathematical model for the motion
            {
                MaxRuntime   =    5, // seconds
                MaxVelocity  =  500, // degrees / s
                Acceleration =  300, // degrees / s^2
                Deceleration = -250, // degrees / s^2, note: negative!
                StartAngle   =   90, // degrees
                StopAngle    =  180  // degrees
            };
           
            motion_.update(); // to be called after changing the settings above

            // call motion_.getAngle(seconds_elapsed); in your render function

            // end example ////////////////////////////////////////////////////////////////////////


            // init UI

            model_ = new WheelMotionViewModel(motion_); // this is just a helper class to visualize things

            DataContext = model_;

            update_timer_.Tick += new EventHandler(Timer_Tick);
            update_timer_.Interval = new TimeSpan(0, 0, 0, 0, 30);    
        }

        private void Simulate_Click(object sender, RoutedEventArgs e)
        {
            if( update_timer_.IsEnabled )
            {
                update_timer_.Stop();
                stop_watch_.Reset();
                Simulate.Content = "Start Simulation";
            }
            else
            {
                update_timer_.Start();
                stop_watch_.Start();
                Simulate.Content = "Stop Simulation";
            }
        }

        // 'Render' Method
        private void Timer_Tick(object sender, EventArgs e)
        {
            // calculate the number of seconds elapsed
            double seconds_elapsed = stop_watch_.Elapsed.TotalMilliseconds / 1000.0;

            // get the angle in function of the elapsed time
            // this is all you need to do
            double angle = motion_.getAngle(seconds_elapsed);

            // update UI
            Position.X2 = model_.Center.X + Math.Cos(angle * Math.PI / 180) * model_.Radius;
            Position.Y2 = model_.Center.Y + Math.Sin(angle * Math.PI / 180) * model_.Radius;

            SimulationTime.Text = seconds_elapsed.ToString("F2");
            Angle.Text = angle.ToString("F2");
        }
    }
}
