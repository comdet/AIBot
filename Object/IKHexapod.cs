using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBot.Object
{
    public class IKHexapod : INotifyPropertyChanged
    {
        public IKLeg Leg1 { get; private set; }
        public IKLeg Leg2 { get; private set; }
        public IKLeg Leg3 { get; private set; }
        public IKLeg Leg4 { get; private set; }
        public IKLeg Leg5 { get; private set; }
        public IKLeg Leg6 { get; private set; }
        public LineGeometry3D ModelSkeleton { get; private set; }
        public System.Windows.Media.Media3D.Transform3D SkeletonTransform { get; private set; }

        private double lowest_body = 0;
        public IKHexapod(Vector3 center_point)
        {
            var e1 = new LineBuilder();
            e1.Add(true,
                new Vector3(-Config.HALF_X2LENGTH, 0, 0),
                new Vector3(-Config.HALF_X1LENGTH, 0, Config.HALF_ZLENGTH),
                new Vector3(Config.HALF_X1LENGTH, 0, Config.HALF_ZLENGTH),
                new Vector3(Config.HALF_X2LENGTH, 0, 0),
                new Vector3(Config.HALF_X1LENGTH, 0, -Config.HALF_ZLENGTH),
                new Vector3(-Config.HALF_X1LENGTH, 0, -Config.HALF_ZLENGTH));
            e1.Add(true, new Vector3(-Config.HALF_X2LENGTH, Config.BODY_HIGH, 0),
                new Vector3(-Config.HALF_X1LENGTH, Config.BODY_HIGH, Config.HALF_ZLENGTH),
                new Vector3(Config.HALF_X1LENGTH, Config.BODY_HIGH, Config.HALF_ZLENGTH),
                new Vector3(Config.HALF_X2LENGTH, Config.BODY_HIGH, 0),
                new Vector3(Config.HALF_X1LENGTH, Config.BODY_HIGH, -Config.HALF_ZLENGTH),
                new Vector3(-Config.HALF_X1LENGTH, Config.BODY_HIGH, -Config.HALF_ZLENGTH));

            e1.AddLine(
                new Vector3(Config.HALF_X1LENGTH, Config.BODY_HIGH, -Config.HALF_ZLENGTH),
                new Vector3(Config.HALF_X1LENGTH, 0, -Config.HALF_ZLENGTH));
            e1.AddLine(
                new Vector3(Config.HALF_X2LENGTH, Config.BODY_HIGH, 0),
                new Vector3(Config.HALF_X2LENGTH, 0, 0));
            e1.AddLine(
                new Vector3(Config.HALF_X1LENGTH, Config.BODY_HIGH, Config.HALF_ZLENGTH),
                new Vector3(Config.HALF_X1LENGTH, 0, Config.HALF_ZLENGTH));
            e1.AddLine(
                new Vector3(-Config.HALF_X1LENGTH, Config.BODY_HIGH, Config.HALF_ZLENGTH),
                new Vector3(-Config.HALF_X1LENGTH, 0, Config.HALF_ZLENGTH));
            e1.AddLine(
                new Vector3(-Config.HALF_X2LENGTH, Config.BODY_HIGH, 0),
                new Vector3(-Config.HALF_X2LENGTH, 0, 0));
            e1.AddLine(
                new Vector3(-Config.HALF_X1LENGTH, Config.BODY_HIGH, -Config.HALF_ZLENGTH),
                new Vector3(-Config.HALF_X1LENGTH, 0, -Config.HALF_ZLENGTH));
            //var leg = new IKLeg(0, new Vector3(), 1, 1, 1);
            Leg1 = new IKLeg(1,
                new Vector3(Config.HALF_X1LENGTH, Config.TIBIA_LENGTH, -Config.HALF_ZLENGTH),
                Config.COXA_LENGTH, Config.FEMUR_LENGTH, Config.TIBIA_LENGTH);
            Leg2 = new IKLeg(2,
                new Vector3(Config.HALF_X2LENGTH, Config.TIBIA_LENGTH, 0),
                Config.COXA_LENGTH, Config.FEMUR_LENGTH, Config.TIBIA_LENGTH);
            Leg3 = new IKLeg(3,
                new Vector3(Config.HALF_X1LENGTH, Config.TIBIA_LENGTH, Config.HALF_ZLENGTH),
                Config.COXA_LENGTH, Config.FEMUR_LENGTH, Config.TIBIA_LENGTH);
            Leg4 = new IKLeg(4,
                new Vector3(-Config.HALF_X1LENGTH, Config.TIBIA_LENGTH, Config.HALF_ZLENGTH),
                Config.COXA_LENGTH, Config.FEMUR_LENGTH, Config.TIBIA_LENGTH);
            Leg5 = new IKLeg(5,
                new Vector3(-Config.HALF_X2LENGTH, Config.TIBIA_LENGTH, 0),
                Config.COXA_LENGTH, Config.FEMUR_LENGTH, Config.TIBIA_LENGTH);
            Leg6 = new IKLeg(6,
                new Vector3(-Config.HALF_X1LENGTH, Config.TIBIA_LENGTH, -Config.HALF_ZLENGTH),
                Config.COXA_LENGTH, Config.FEMUR_LENGTH, Config.TIBIA_LENGTH);

            ModelSkeleton = e1.ToLineGeometry3D();
            SkeletonTransform = new System.Windows.Media.Media3D.TranslateTransform3D(center_point.ToVector3D());
            Calculate();
        }
        public void LowerBody(double distance)
        {
            SkeletonTransform = new System.Windows.Media.Media3D.TranslateTransform3D(
                SkeletonTransform.Value.OffsetX,
                SkeletonTransform.Value.OffsetY - distance,
                SkeletonTransform.Value.OffsetZ);
            UpdateAllLeg(-distance, 0, "Y");
            UpdateAllLeg(-distance, 1, "Y");
            Calculate();
        }
        public void LowerAllLeg(double distance)
        {
            UpdateAllLeg(-distance, 3, "Y");
            Calculate();
        }
        public void RaiseBody(double distance)
        {
            SkeletonTransform = new System.Windows.Media.Media3D.TranslateTransform3D(
                SkeletonTransform.Value.OffsetX,
                SkeletonTransform.Value.OffsetY + distance,
                SkeletonTransform.Value.OffsetZ);
            UpdateAllLeg(distance, 0, "Y");
            UpdateAllLeg(distance, 1, "Y");
            Calculate();
        }
        public void RaiseAllLeg(double distance)
        {
            UpdateAllLeg(distance, 3, "Y");
            Calculate();
        }

        private void Calculate()
        {
            //-------- Leg Update --------//
            Leg1.Calculate();
            Leg2.Calculate();
            Leg3.Calculate();
            Leg4.Calculate();
            Leg5.Calculate();
            Leg6.Calculate();
            //CalculateFallBack();
            Update();
        }
        private void Update()
        {
            OnPropertyChanged("SkeletonTransform");
        }
        private void ResolveIK()
        {
            double zOffset = SkeletonTransform.Value.OffsetY;
        }
        public void UpdateLegGroup1(double value, int line, string target)
        {
            UpdateLeg(value, line, target, 1, 3, 5);
        }
        public void UpdateLegGroup2(double value, int line, string target)
        {
            UpdateLeg(value, line, target, 2, 4, 6);
        }
        public void UpdateLegExtend(double value, int line, string target, params int[] channel)
        {
            //UpdateLeg(value, line, target,channel);
        }
        public void UpdateAllLeg(double value, int line, string target)
        {
            UpdateLeg(value, line, target, 1, 2, 3, 4, 5, 6);
        }

        public void UpdateLeg(double value, int line, string target, params int[] channel)
        {
            if (channel.Contains(1))
            {
                if (target == "X")
                    Leg1.Lines[line].X = Leg1.Lines[line].X + (float)value;
                if (target == "Y")
                    Leg1.Lines[line].Y = Leg1.Lines[line].Y + (float)value;
                if (target == "Z")
                    Leg1.Lines[line].Z = Leg1.Lines[line].Z + (float)value;
            }
            if (channel.Contains(2))
            {
                if (target == "X")
                    Leg2.Lines[line].X = Leg2.Lines[line].X + (float)value;
                if (target == "Y")
                    Leg2.Lines[line].Y = Leg2.Lines[line].Y + (float)value;
                if (target == "Z")
                    Leg2.Lines[line].Z = Leg2.Lines[line].Z + (float)value;
            }
            if (channel.Contains(3))
            {
                if (target == "X")
                    Leg3.Lines[line].X = Leg3.Lines[line].X + (float)value;
                if (target == "Y")
                    Leg3.Lines[line].Y = Leg3.Lines[line].Y + (float)value;
                if (target == "Z")
                    Leg3.Lines[line].Z = Leg3.Lines[line].Z + (float)value;
            }
            if (channel.Contains(4))
            {
                if (target == "X")
                    Leg4.Lines[line].X = Leg4.Lines[line].X + (float)value;
                if (target == "Y")
                    Leg4.Lines[line].Y = Leg4.Lines[line].Y + (float)value;
                if (target == "Z")
                    Leg4.Lines[line].Z = Leg4.Lines[line].Z + (float)value;
            }
            if (channel.Contains(5))
            {
                if (target == "X")
                    Leg5.Lines[line].X = Leg5.Lines[line].X + (float)value;
                if (target == "Y")
                    Leg5.Lines[line].Y = Leg5.Lines[line].Y + (float)value;
                if (target == "Z")
                    Leg5.Lines[line].Z = Leg5.Lines[line].Z + (float)value;
            }
            if (channel.Contains(6))
            {
                if (target == "X")
                    Leg6.Lines[line].X = Leg6.Lines[line].X + (float)value;
                if (target == "Y")
                    Leg6.Lines[line].Y = Leg6.Lines[line].Y + (float)value;
                if (target == "Z")
                    Leg6.Lines[line].Z = Leg6.Lines[line].Z + (float)value;
            }
            Calculate();
        }
        public void UpdateLeg(double rotation, params int[] channel)
        {
            if (channel.Contains(1)) Leg1.RotageLeg(rotation);
            if (channel.Contains(2)) Leg2.RotageLeg(rotation);
            if (channel.Contains(3)) Leg3.RotageLeg(rotation);
            if (channel.Contains(4)) Leg4.RotageLeg(rotation);
            if (channel.Contains(5)) Leg5.RotageLeg(rotation);
            if (channel.Contains(6)) Leg6.RotageLeg(rotation);
        }
        public string generate(int duration)
        {
            return Leg1.generate() + Leg2.generate() + Leg3.generate() + 
                Leg4.generate() + Leg5.generate() + Leg6.generate()+"T"+duration;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
