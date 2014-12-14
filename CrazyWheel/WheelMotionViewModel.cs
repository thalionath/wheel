﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CrazyWheel
{
    class WheelMotionViewModel : INotifyPropertyChanged
    {
        private WheelMotion motion_ = new WheelMotion();

        private void updateModel()
        {
            motion_.updateModel();
            RaisePropertyChanged("Runtime");
            RaisePropertyChanged("PeakVelocity");
            RaisePropertyChanged("AccelerationTime");
            RaisePropertyChanged("DecelerationTime");
        }

        public double Runtime
        {
            get { return motion_.Runtime; }
        }

        public double PeakVelocity
        {
            get { return motion_.PeakVelocity; }
        }

        public double AccelerationTime
        {
            get { return motion_.AccelerationTime; }
        }

        public double DecelerationTime
        {
            get { return motion_.DecelerationTime; }
        }

        public double MaxRuntime
        {
            get { return motion_.MaxRuntime; }
            set
            {
                motion_.MaxRuntime = value;
                updateModel();
                RaisePropertyChanged();
            }
        }

        public double MaxVelocity
        {
            get { return motion_.MaxVelocity; }
            set
            { 
                motion_.MaxVelocity = value;
                updateModel();
                RaisePropertyChanged();
            }
        }

        public double Acceleration
        {
            get { return motion_.Acceleration; }
            set
            {
                motion_.Acceleration = value;
                updateModel();
                RaisePropertyChanged();
            }
        }

        public double Deceleration
        {
            get { return motion_.Deceleration; }
            set
            {
                motion_.Deceleration = value;
                updateModel();
                RaisePropertyChanged();
            }
        }

        public double StartAngle
        {
            get { return motion_.StartAngle; }
            set
            {
                motion_.StartAngle = value;
                updateModel();
                RaisePropertyChanged();
            }
        }

        public double StopAngle
        {
            get { return motion_.StopAngle; }
            set
            {
                motion_.StopAngle = value;
                updateModel();
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if( PropertyChanged != null )
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }

    }
}