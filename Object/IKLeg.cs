using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIBot.Object
{
    public class IKLeg : LineGeometryModel3D, INotifyPropertyChanged
    {
        public int Index { get; private set; }
        public double Angle { get; private set; }
        public double Grammar { get; private set; }
        public double Alpha { get; private set; }
        public double Betha { get; private set; }
        public LineGeometry3D Leg { get; private set; }
        public Vector3[] Lines { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private double actual_angle = 0;
        private double[] rotage_base = new double[] { 150, 90, 30, 330, 270, 210 };
        private double[] angle_base = new double[] { 150, 90, 30, 150, 90, 30 };
        //---------------------------------------------------------------//
        public const string LegUpdatePropertyName = "IsUpdated";
        public bool Position
        {
            get
            {
                return (bool)this.GetValue(LegUpdateProperty);
            }
            set
            {
                this.SetValue(LegUpdateProperty, value);
            }
        }

        public static readonly DependencyProperty LegUpdateProperty =
            DependencyProperty.Register(LegUpdatePropertyName, typeof(bool), typeof(IKLeg), new UIPropertyMetadata(false));
        //----------------------------------------------------------------//
        public IKLeg(int index, Vector3 rPoint, float coxa, float femur, float tibia)
        {
            this.Index = index;
            //--- create joint
            LineBuilder lb = new LineBuilder();
            Lines = new Vector3[] {rPoint,
                    ((index>3)? new Vector3(rPoint.X-coxa, rPoint.Y, rPoint.Z):new Vector3(rPoint.X+coxa, rPoint.Y, rPoint.Z)),
                    ((index>3)? new Vector3(rPoint.X-coxa-femur, rPoint.Y, rPoint.Z):new Vector3(rPoint.X+coxa+femur, rPoint.Y, rPoint.Z)),
                    ((index>3)? new Vector3(rPoint.X-coxa-femur, rPoint.Y - tibia, rPoint.Z):new Vector3(rPoint.X+coxa+femur, rPoint.Y - tibia, rPoint.Z)) };
            lb.Add(false, Lines);
            Leg = lb.ToLineGeometry3D();
            Transform = new System.Windows.Media.Media3D.TranslateTransform3D();
            //--- rotage leg ---//
            RotageLeg(rotage_base[index - 1]);
            Angle = 0;
        }
        public void Calculate()
        {
            var st = Lines[0];
            var sp = Lines[3];
            double zOffset = Math.Abs(st.Y - sp.Y);
            double grammar = (sp.Z - st.Z == 0) ? (Math.PI / 2.0) : (Math.Atan((sp.X - st.X) / (sp.Z - st.Z)));
            double L1 = Math.Sqrt((st.X - sp.X) * (st.X - sp.X) + (st.Z - sp.Z) * (st.Z - sp.Z));
            double L = Math.Sqrt(zOffset * zOffset + (L1 - Config.COXA_LENGTH) * (L1 - Config.COXA_LENGTH));
            double alpha1 = Math.Acos(zOffset / L);
            double alpha2 = Math.Acos((Config.TIBIA_LENGTH * Config.TIBIA_LENGTH - Config.FEMUR_LENGTH * Config.FEMUR_LENGTH - L * L) /
                ((-2) * (Config.FEMUR_LENGTH) * L));
            double alpha = (alpha1 + alpha2);
            double betha = Math.Acos((L * L - Config.TIBIA_LENGTH * Config.TIBIA_LENGTH - Config.FEMUR_LENGTH * Config.FEMUR_LENGTH) /
                ((-2) * Config.TIBIA_LENGTH * Config.FEMUR_LENGTH));
            //------- positioning --------//               
            //if (Index == 1)
            //{
              //  Console.WriteLine("Grammar : {0}, Alpha : {1}, Bheta : {2}", grammar.RadianToDegree(), alpha.RadianToDegree(), betha.RadianToDegree());
            //}
            //Console.WriteLine("L1:{0},L2:{1},L3:{0}", (Lines[1] - Lines[0]).Length(), (Lines[2] - Lines[1]).Length(), (Lines[3] - Lines[2]).Length());

            if (Index > 3)
            {
                alpha = (Math.PI / 2) - (alpha);
                grammar = (grammar < 0 ? (Math.PI) + grammar : grammar);

                Lines[1].X = Lines[0].X - (float)(Math.Sin(grammar) * Config.COXA_LENGTH);
                Lines[1].Z = Lines[0].Z - (float)(Math.Cos(grammar) * Config.COXA_LENGTH);

                Lines[2].Y = Lines[1].Y - (float)(Math.Sin(alpha) * Config.FEMUR_LENGTH);
                var Lx = (float)(Math.Cos(alpha) * Config.FEMUR_LENGTH);
                Lines[2].X = Lines[1].X - (float)(Math.Sin(grammar) * Lx);
                Lines[2].Z = Lines[1].Z - (float)(Math.Cos(grammar) * Lx);
            }
            else
            {
                alpha = alpha - (Math.PI / 2.0);
                grammar = grammar < 0 ? (Math.PI) + grammar : grammar;

                Lines[1].X = Lines[0].X + (float)(Math.Sin(grammar) * Config.COXA_LENGTH);
                Lines[1].Z = Lines[0].Z + (float)(Math.Cos(grammar) * Config.COXA_LENGTH);

                Lines[2].Y = Lines[1].Y + (float)(Math.Sin(alpha) * Config.FEMUR_LENGTH);
                var Lx = (float)(Math.Cos(alpha) * Config.FEMUR_LENGTH);
                Lines[2].X = Lines[1].X + (float)(Math.Sin(grammar) * Lx);
                Lines[2].Z = Lines[1].Z + (float)(Math.Cos(grammar) * Lx);
            }
            Alpha = alpha.RadianToDegree();
            Grammar = grammar.RadianToDegree();
            Betha = betha.RadianToDegree();
            Update();
        }
        public void RotageLeg(double degree)
        {
            actual_angle += degree;
            Angle += degree;

            double tx = Lines[3].X - Lines[0].X;
            double tz = Lines[3].Z - Lines[0].Z;
            double dist = Math.Sqrt(tx * tx + tz * tz);
            Lines[3].X = Lines[0].X + (float)(Math.Sin(actual_angle.DegreeToRadian()) * dist);
            Lines[3].Z = Lines[0].Z + (float)(Math.Cos(actual_angle.DegreeToRadian()) * dist);
            Calculate();
        }
        public void MoveLeg(Vector3 target)
        {
            Lines[3] = target;
            Calculate();
        }
        public void MoveLegRelative(Vector3 distance)
        {
            Lines[3] = Lines[3] + distance;
            Calculate();
        }
        public void Update()
        {
            LineBuilder lb = new LineBuilder();
            lb.Add(false, Lines);
            Leg = lb.ToLineGeometry3D();
            OnPropertyChanged("Leg");
        }
        public string generate()
        {
            int ang, femur, tibia;
            if(Index < 4)
            {
                ang = (int)map(Grammar - angle_base[Index-1], -90, 90, 500, 2500) + Config.LEG_TUNE[Index - 1, 0];
                femur = (int)map(180 - (Alpha + 90), 0, 180, 500, 2500) + Config.LEG_TUNE[Index - 1, 1];
                tibia = (int)map(Betha, 0, 180, 500, 2500) + Config.LEG_TUNE[Index - 1, 2];
            }
            else
            {
                ang = (int)map(Grammar - angle_base[Index - 1], -90, 90, 500, 2500) + Config.LEG_TUNE[Index - 1, 0];
                femur = (int)map(180 - (Math.Abs(Alpha) + 90), 0, 180, 500, 2500) + Config.LEG_TUNE[Index - 1, 1];
                tibia = (int)map(Betha, 0, 180, 500, 2500) + Config.LEG_TUNE[Index - 1, 2];

                femur = 3000 - femur;
                tibia = 3000 - tibia;
            }
            ang = 3000 - ang;
            return "#" + Config.LEG_PINS[Index - 1, 0] + "P" + ang +
                    "#" + Config.LEG_PINS[Index - 1, 1] + "P" + femur +
                    "#" + Config.LEG_PINS[Index - 1, 2] + "P" + tibia;            
        }
        public int getAngleServo(int pos)
        {
            return 0;
        }
        private double map(double x, double in_min, double in_max, double out_min, double out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
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
