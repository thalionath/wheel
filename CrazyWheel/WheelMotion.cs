using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyWheel
{
    class WheelMotion
    {
        private double v_max_ = 100.0;
        private double t_max_ = 30.0;

        private double t_acceleration_ = 0.0;
        private double t_deceleration_ = 0.0;

        private double t_total_ = 0.0;
        private double v_peak_ = 0.0;

        public double MaxVelocity
        {
            get { return v_max_; }
            set { v_max_ = value; }
        }

        public double MaxRuntime
        { 
            get { return t_max_; }
            set { t_max_ = value; }
        }

        public double Runtime
        {
            get { return t_total_; }
        }

        public double PeakVelocity
        {
            get { return v_peak_; }
        }

        public double AccelerationTime
        {
            get { return t_acceleration_; }
        }

        public double DecelerationTime
        {
            get { return t_deceleration_; }
        }

        public double Acceleration { get; set; }
        public double Deceleration { get; set; }

        public double StartAngle { get; set; }
        public double StopAngle { get; set; }

        public double AccerlerationTime
        {
            get { return t_acceleration_; }
        }

        public WheelMotion()
        {
            Acceleration = 4;
            Deceleration = -6;
            StartAngle = 90;
            StopAngle = 85;
        }

        public void updateModel()
        {
            // calculate v_peak of uniform acceleration followed by uniform deceleration (ignoring v_max)
            // => the sum of accerleation and deceleration must equal zero

            t_acceleration_ = -1.0 * (t_max_ * Deceleration) / (Acceleration - Deceleration);
            t_deceleration_ = t_max_ - t_acceleration_;

            v_peak_ = t_acceleration_ * Acceleration;

            double s = 0.0;

            // we do not reach v_max if v_peak < v_max
            if( v_peak_ > v_max_ )
            {
                t_acceleration_ =  v_max_ / Acceleration;
                t_deceleration_ = -v_max_ / Deceleration;

                double t_flat = t_max_ - t_acceleration_ - t_deceleration_;

                s = (t_acceleration_ + t_deceleration_) * v_max_ / 2.0
                  + t_flat + v_max_;
            }
            else
            {
                s = (t_max_ * v_peak_) / 2;
            }

            // adjust the actual distance to the target...

            double angle = (StartAngle + s) % 360;

            if( StopAngle > angle )
            {
                s -= angle;
                s -= (360 - StopAngle);
            }
            else
            {
                s -= (angle - StopAngle);
            }

            // ...and back-calculate the runtimes

            t_acceleration_ = Math.Sqrt((-2.0 * Deceleration * s) / (Acceleration * (Acceleration - Deceleration)));
            t_deceleration_ = (Acceleration * t_acceleration_) / (-Deceleration);

            t_total_ = t_acceleration_ + t_deceleration_;

            v_peak_ = t_acceleration_ * Acceleration; // inflexion point

            // do we reach max. velocity?
            if( v_peak_ > v_max_ )
            {
                t_acceleration_ =  v_max_ / Acceleration;
                t_deceleration_ = -v_max_ / Deceleration;

                double s1 = +0.5 * Acceleration * t_acceleration_ * t_acceleration_;
                double s2 = -0.5 * Deceleration * t_deceleration_ * t_deceleration_;

                double sm = s - s1 - s2;
                double tm = sm / v_max_;

                s = s1 + sm + s2;

                t_total_ = t_acceleration_ + tm + t_deceleration_;

                v_peak_ = v_max_;
            }
        }

    }
}
