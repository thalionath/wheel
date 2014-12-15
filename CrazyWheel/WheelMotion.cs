using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyWheel
{
    /**
     * @brief A mathematical model for a wheel motion with uniform acceleration and deceleration.
     * 
     * Runs no longer than t_max, not faster than v_max and stops exactly at StopAngle. 
     * 
     * @author Mario Gruber
     */
    class WheelMotion
    {
        private uint CIRCLE_DEGREES = 360;

        private double v_max_ = 0.0;
        private double t_max_ = 0.0;

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
        }

        public void update()
        {
            // calculate v_peak of uniform acceleration followed by uniform deceleration (ignoring v_max)
            // to get the distance we are going to cover.

            // => the sum of accerleation and deceleration must equal zero, solving the equation system leads to:

            t_acceleration_ = -1.0 * (t_max_ * Deceleration) / (Acceleration - Deceleration);
            t_deceleration_ = t_max_ - t_acceleration_;

            v_peak_ = t_acceleration_ * Acceleration;

            double s = 0.0;
            
            if( v_peak_ >= v_max_ )
            {
                t_acceleration_ =  v_max_ / Acceleration;
                t_deceleration_ = -v_max_ / Deceleration;

                double t_flat = t_max_ - t_acceleration_ - t_deceleration_;

                s = (t_acceleration_ + t_deceleration_) * v_max_ / 2.0 + t_flat * v_max_;
            }
            else
            {
                s = (t_max_ * v_peak_) / 2.0; // we do not reach v_max if v_peak < v_max
            }

            // adjust the actual distance to the target...

            double angle = (StartAngle + s) % CIRCLE_DEGREES;

            if( StopAngle > angle )
            {
                s -= angle;
                s -= (CIRCLE_DEGREES - StopAngle);
            }
            else
            {
                s -= (angle - StopAngle);
            }

            // ...and calculate back from the total distance to the times

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

        /**
         * @brief Returns the total distance covered so far as a function of elapsed motion time.
         */
        public double getDistance(double t)
        {
            if( t < t_acceleration_ )
            {
                return t * t * Acceleration / 2.0;
            }
            else if( t < (t_total_ - t_deceleration_) )
            {
                return t_acceleration_ * v_peak_ / 2.0 + (t - t_acceleration_) * v_peak_;
            }
            else if( t < t_total_ )
            {
                double t_const = t_total_ - t_acceleration_ - t_deceleration_;

                t = t_total_ - t;

                return t_acceleration_ * v_peak_ / 2.0
                     + t_const * v_peak_
                     + t_deceleration_ * v_peak_ / 2.0
                     + t * t * Deceleration / 2.0;
            }
            else
            {
                double t_const = t_total_ - t_acceleration_ - t_deceleration_;

                return t_acceleration_ * v_peak_ / 2.0 + t_const * v_peak_ + t_deceleration_ * v_peak_ / 2.0;
            }
        }

        /**
         * @brief Returns the current rotation angle as a function of elapsed motion time.
         */
        public double getAngle(double t)
        {
            return (StartAngle + getDistance(t)) % CIRCLE_DEGREES;
        }

    }
}
