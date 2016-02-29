using AIBot.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AIBot
{
    public enum WalkDirection { Up,Down,Left,Right}
    public class Motions
    {
        private MainViewModel model = null;
        private UARTController control = null;

        private TimeSpan TIMEOUT = TimeSpan.FromSeconds(10);
        private TimeSpan USRTIMEOUT = TimeSpan.FromSeconds(60);        
        private bool isExecuted { get; set; }
        private bool isUserStop { get; set; }
        private bool isStarted { get; set; }
        public Motions(UARTController c)
        {
            model = MainViewModel.GetInstance();
            control = c;
        }
        //---------------------//
        public void StartWalk1(WalkDirection dir = WalkDirection.Up, int duration = Config.STD_MOVETIME)
        {
            Task.Factory.StartNew(() =>
            {
                isUserStop = false;
                DateTime sttime = DateTime.Now;
                int ud = (dir == WalkDirection.Up || dir == WalkDirection.Left) ? -1 :  1;
                string lr = (dir == WalkDirection.Left || dir == WalkDirection.Right) ? "X" : "Z";
                //int lr = (dir == WalkDirection.Left) ? "X" : ((dir == WalkDirection.Right) ? "Z" : 1);
                //---- pre ----//
                model.HexModel.UpdateLeg(Config.LEGUP, 3, "Y", 1, 3, 5);
                model.HexModel.UpdateLeg(ud*Config.GAIT_DISTANCE/2, 3, lr, 1, 3, 5);
                model.HexModel.UpdateLeg(-(ud*Config.GAIT_DISTANCE / 2), 3, lr, 2, 4, 6);
                control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                //Thread.Sleep(1000);
                model.HexModel.UpdateLeg(-Config.LEGUP, 3, "Y", 1, 3, 5);
                model.HexModel.UpdateLeg(ud*Config.GAIT_DISTANCE / 2, 3, lr, 1, 3, 5);
                model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE / 2), 3, lr, 2, 4, 6);
                control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                //Thread.Sleep(1000);
                while (!isUserStop)
                {
                    if (DateTime.Now - sttime > USRTIMEOUT)
                    {
                        return;
                    }
                    model.HexModel.UpdateLeg(Config.LEGUP, 3, "Y", 2, 4, 6);
                    model.HexModel.UpdateLeg(ud * Config.GAIT_DISTANCE, 3, lr, 2, 4, 6);
                    model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE), 3, lr, 1, 3, 5);
                    control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                    //Thread.Sleep(1000);
                    model.HexModel.UpdateLeg(-Config.LEGUP, 3, "Y", 2, 4, 6);
                    model.HexModel.UpdateLeg(ud * Config.GAIT_DISTANCE, 3, lr, 2, 4, 6);
                    model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE), 3, lr, 1, 3, 5);
                    control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                    //Thread.Sleep(1000);
                    model.HexModel.UpdateLeg(Config.LEGUP, 3, "Y", 1, 3, 5);
                    model.HexModel.UpdateLeg(ud * Config.GAIT_DISTANCE, 3, lr, 1, 3, 5);
                    model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE), 3, lr, 2, 4, 6);
                    control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                    //Thread.Sleep(1000);
                    model.HexModel.UpdateLeg(-Config.LEGUP, 3, "Y", 1, 3, 5);
                    model.HexModel.UpdateLeg(ud * Config.GAIT_DISTANCE, 3, lr, 1, 3, 5);
                    model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE), 3, lr, 2, 4, 6);
                    control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                    //Thread.Sleep(1000);
                }
                model.HexModel.UpdateLeg(Config.LEGUP, 3, "Y", 2, 4, 6);
                model.HexModel.UpdateLeg(ud * Config.GAIT_DISTANCE/2, 3, lr, 2, 4, 6);
                model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE/2), 3, lr, 1, 3, 5);
                control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                //Thread.Sleep(1000);
                model.HexModel.UpdateLeg(-Config.LEGUP, 3, "Y", 2, 4, 6);
                model.HexModel.UpdateLeg(ud * Config.GAIT_DISTANCE/2, 3, lr, 2, 4, 6);
                model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE/2), 3, lr, 1, 3, 5);
                control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                //Thread.Sleep(1000);
            });
        }
        public void EndWalk1()
        {
            isUserStop = true;
        }
        //---------------------//
        public void StartWalk2(WalkDirection dir = WalkDirection.Up, int duration = Config.STD_MOVETIME)
        {
            int[] step = { 1, 3, 5, 2, 4, 6 };
            Task.Factory.StartNew(() =>
            {
                isUserStop = false;
                DateTime sttime = DateTime.Now;
                int ud = (dir == WalkDirection.Up || dir == WalkDirection.Left) ? -1 : 1;
                string lr = (dir == WalkDirection.Left || dir == WalkDirection.Right) ? "X" : "Z";
                //int lr = (dir == WalkDirection.Left) ? "X" : ((dir == WalkDirection.Right) ? "Z" : 1);
                //---- pre ----//
                model.HexModel.UpdateLeg(Config.LEGUP, 3, "Y", 1);
                model.HexModel.UpdateLeg(ud * Config.GAIT_DISTANCE/2, 3, lr, 1);
                model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE / 6), 3, lr, 2,3,4,5,6);
                //control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                Thread.Sleep(1000);
                model.HexModel.UpdateLeg(-Config.LEGUP, 3, "Y", 1);
                model.HexModel.UpdateLeg(ud * Config.GAIT_DISTANCE / 2, 3, lr, 1);
                model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE / 6), 3, lr, 2, 3, 4, 5, 6);
                //control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                Thread.Sleep(1000);

                model.HexModel.UpdateLeg(Config.LEGUP, 3, "Y", 3);
                model.HexModel.UpdateLeg(ud * (Config.GAIT_DISTANCE / 2 - Config.GAIT_DISTANCE/6)/2, 3, lr, 3);
                model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE / 6), 3, lr, 1, 2, 4, 5, 6);
                //control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                Thread.Sleep(1000);
                model.HexModel.UpdateLeg(-Config.LEGUP, 3, "Y", 3);
                model.HexModel.UpdateLeg(ud * (Config.GAIT_DISTANCE / 2 - Config.GAIT_DISTANCE / 6) / 2, 3, lr, 3);
                model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE / 6), 3, lr, 1, 2, 4, 5, 6);
                //control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                Thread.Sleep(1000);

                model.HexModel.UpdateLeg(Config.LEGUP, 3, "Y", 5);
                model.HexModel.UpdateLeg(ud * (Config.GAIT_DISTANCE / 2 - Config.GAIT_DISTANCE / 6*2) / 2, 3, lr, 5);
                model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE / 6), 3, lr, 1, 2, 3, 4, 6);
                //control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                Thread.Sleep(1000);
                model.HexModel.UpdateLeg(-Config.LEGUP, 3, "Y", 5);
                model.HexModel.UpdateLeg(ud * (Config.GAIT_DISTANCE / 2 - Config.GAIT_DISTANCE / 6 * 2) / 2, 3, lr, 5);
                model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE / 6), 3, lr, 1, 2, 3, 4, 6);
                //control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                Thread.Sleep(1000);

                model.HexModel.UpdateLeg(Config.LEGUP, 3, "Y", 2);
                model.HexModel.UpdateLeg(ud * (Config.GAIT_DISTANCE / 2 - Config.GAIT_DISTANCE / 6 * 3) / 2, 3, lr, 2);
                model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE / 6), 3, lr, 1, 3, 4, 5, 6);
                //control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                Thread.Sleep(1000);
                model.HexModel.UpdateLeg(-Config.LEGUP, 3, "Y", 2);
                model.HexModel.UpdateLeg(ud * (Config.GAIT_DISTANCE / 2 - Config.GAIT_DISTANCE / 6 * 3) / 2, 3, lr, 2);
                model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE / 6), 3, lr, 1, 3, 4, 5, 6);
                //control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                Thread.Sleep(1000);

                model.HexModel.UpdateLeg(Config.LEGUP, 3, "Y", 4);
                model.HexModel.UpdateLeg(ud * (Config.GAIT_DISTANCE / 2 - Config.GAIT_DISTANCE / 6 * 4) / 2, 3, lr, 4);
                model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE / 6), 3, lr, 1, 2, 3, 5, 6);
                //control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                Thread.Sleep(1000);
                model.HexModel.UpdateLeg(-Config.LEGUP, 3, "Y", 4);
                model.HexModel.UpdateLeg(ud * (Config.GAIT_DISTANCE / 2 - Config.GAIT_DISTANCE / 6 * 4) / 2, 3, lr, 4);
                model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE / 6), 3, lr, 1, 2, 3, 5, 6);
                //control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                Thread.Sleep(1000);

                model.HexModel.UpdateLeg(Config.LEGUP, 3, "Y", 6);
                model.HexModel.UpdateLeg(ud * (Config.GAIT_DISTANCE / 2 - Config.GAIT_DISTANCE / 6 * 5) / 2, 3, lr, 6);
                model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE / 6), 3, lr, 1, 2, 3, 4, 5);
                //control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                Thread.Sleep(1000);
                model.HexModel.UpdateLeg(-Config.LEGUP, 3, "Y", 6);
                model.HexModel.UpdateLeg(ud * (Config.GAIT_DISTANCE / 2 - Config.GAIT_DISTANCE / 6 * 5) / 2, 3, lr, 6);
                model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE / 6), 3, lr, 1, 2, 3, 4, 5);
                //control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                Thread.Sleep(1000);
                for (int i = 0; i < 6; i++)
                {
                    int leg = step[i];
                    //--- full step
                    model.HexModel.UpdateLeg(Config.LEGUP, 3, "Y", leg);
                    model.HexModel.UpdateLeg(ud * Config.GAIT_DISTANCE, 3, lr, leg);
                    int[] s = step.Where(val => val != leg).ToArray();
                    model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE / 6), 3, lr, s);
                    //control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                    Thread.Sleep(1000);
                    model.HexModel.UpdateLeg(-Config.LEGUP, 3, "Y", 1);
                    model.HexModel.UpdateLeg(ud * Config.GAIT_DISTANCE, 3, lr, leg);
                    model.HexModel.UpdateLeg(-(ud * Config.GAIT_DISTANCE / 6), 3, lr, s);
                    //control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                    Thread.Sleep(1000);
                    
                }
            });
        }
        //---------------------//
        public void StartRotage(bool clockdir,int duration = Config.STD_MOVETIME)
        {
            Task.Factory.StartNew(() =>
            {
                isUserStop = false;
                DateTime sttime = DateTime.Now;
                // --- pre excution ---//
                model.HexModel.UpdateLeg(Config.LEGUP, 3, "Y", 1, 3, 5);
                model.HexModel.UpdateLeg(((clockdir) ? 1 : -1) * Config.ROTATION_STEP_DEG / 2, 1, 3, 5);
                model.HexModel.UpdateLeg(((clockdir) ? -1 : 1) * Config.ROTATION_STEP_DEG / 2, 2, 4, 6);
                control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                //Thread.Sleep(duration);
                model.HexModel.UpdateLeg(-Config.LEGUP, 3, "Y", 1, 3, 5);
                model.HexModel.UpdateLeg(((clockdir) ? 1 : -1) * Config.ROTATION_STEP_DEG / 2, 1, 3, 5);
                model.HexModel.UpdateLeg(((clockdir) ? -1 : 1) * Config.ROTATION_STEP_DEG / 2, 2, 4, 6);
                control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                //Thread.Sleep(duration);
                //---------------------//                
                while (!isUserStop)
                {
                    if (DateTime.Now - sttime > USRTIMEOUT)
                    {
                        return;
                    }
                    // verse 1 ( 2 4 6 --> half rotage )
                    model.HexModel.UpdateLeg(Config.LEGUP, 3, "Y", 2, 4, 6);
                    model.HexModel.UpdateLeg(((clockdir) ? 1 : -1) * Config.ROTATION_STEP_DEG, 2, 4, 6);
                    model.HexModel.UpdateLeg(((clockdir) ? -1 : 1) * Config.ROTATION_STEP_DEG, 1, 3, 5);
                    control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                    //Thread.Sleep(duration);
                    // verse 2 ( 2 4 6 --> another half rotage )
                    model.HexModel.UpdateLeg(-Config.LEGUP, 3, "Y", 2, 4, 6);
                    model.HexModel.UpdateLeg(((clockdir) ? 1 : -1) * Config.ROTATION_STEP_DEG, 2, 4, 6);
                    model.HexModel.UpdateLeg(((clockdir) ? -1 : 1) * Config.ROTATION_STEP_DEG, 1, 3, 5);
                    control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                    //Thread.Sleep(duration);
                    // verse 3 ( 1 3 5 --> half rotage)
                    model.HexModel.UpdateLeg(Config.LEGUP, 3, "Y", 1, 3, 5);
                    model.HexModel.UpdateLeg(((clockdir) ? 1 : -1) * Config.ROTATION_STEP_DEG, 1, 3, 5);
                    model.HexModel.UpdateLeg(((clockdir) ? -1 : 1) * Config.ROTATION_STEP_DEG, 2, 4, 6);
                    control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                    //Thread.Sleep(duration);
                    // verse 4 ( 1 3 5 --> another half rotage)
                    model.HexModel.UpdateLeg(-Config.LEGUP, 3, "Y", 1, 3, 5);
                    model.HexModel.UpdateLeg(((clockdir) ? 1 : -1) * Config.ROTATION_STEP_DEG, 1, 3, 5);
                    model.HexModel.UpdateLeg(((clockdir) ? -1 : 1) * Config.ROTATION_STEP_DEG, 2, 4, 6);
                    control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                    //Thread.Sleep(duration);
                }
                // verse 5 ( 2 4 6 --> half rotage)
                model.HexModel.UpdateLeg(Config.LEGUP, 3, "Y", 2, 4, 6);
                model.HexModel.UpdateLeg(((clockdir) ? 1 : -1) * Config.ROTATION_STEP_DEG/2, 2, 4, 6);
                model.HexModel.UpdateLeg(((clockdir) ? -1 : 1) * Config.ROTATION_STEP_DEG/2, 1, 3, 5);
                control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                //Thread.Sleep(duration);
                // verse 6 ( 2 4 6 --> another half rotage)
                model.HexModel.UpdateLeg(-Config.LEGUP, 3, "Y", 2, 4, 6);
                model.HexModel.UpdateLeg(((clockdir) ? 1 : -1) * Config.ROTATION_STEP_DEG/2, 2, 4, 6);
                model.HexModel.UpdateLeg(((clockdir) ? -1 : 1) * Config.ROTATION_STEP_DEG/2, 1, 3, 5);
                control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                //Thread.Sleep(duration);
            });
        }
        public void EndRotage()
        {
            isUserStop = true;
        }
        public void BodyUp(double distance = Config.STD_BODYMOVE_LEGNTH, int duration = Config.STD_MOVETIME)
        {
            model.HexModel.RaiseBody(distance);
            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
        }
        public void BodyDown(double distance = Config.STD_BODYMOVE_LEGNTH,int duration = Config.STD_MOVETIME)
        {
            model.HexModel.LowerBody(1);
            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
        }
        //----------------------//
        
        public void Cal90Up()
        {
            Task.Factory.StartNew(() =>
            {                
                model.HexModel.Leg2.MoveLeg(new SharpDX.Vector3(Config.COXA_LENGTH + Config.TIBIA_LENGTH, Config.FEMUR_LENGTH, 0));//
                model.HexModel.Leg5.MoveLegRelative(new SharpDX.Vector3(-Config.TIBIA_LENGTH, Config.FEMUR_LENGTH, 0));//
            });
        }
        public void BaseBody()
        {
            //Task.Factory.StartNew(() =>
            //{
                model.HexModel.LowerBody(Config.TIBIA_LENGTH - Config.Z_BASEOFFSET);
                control.ExecuteOnce(new Command(model.HexModel.generate(500)));
            //});
        }
        public void TestLeg()
        {
            Task.Factory.StartNew(() =>
            {
                int thsleep = 3000;
                int duration = 300;
                for (int i = 1; i <= 6; i++)
                {
                    switch (i)
                    {
                        case 1:
                            model.HexModel.Leg1.MoveLegRelative(new SharpDX.Vector3(5, 0, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg1.MoveLegRelative(new SharpDX.Vector3(-5, 0, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg1.MoveLegRelative(new SharpDX.Vector3(0, 5, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg1.MoveLegRelative(new SharpDX.Vector3(0, -5, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg1.MoveLegRelative(new SharpDX.Vector3(0, 0, 5));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg1.MoveLegRelative(new SharpDX.Vector3(0, 0, -5));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            break;
                        case 2:
                            model.HexModel.Leg2.MoveLegRelative(new SharpDX.Vector3(5, 0, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg2.MoveLegRelative(new SharpDX.Vector3(-5, 0, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg2.MoveLegRelative(new SharpDX.Vector3(0, 5, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg2.MoveLegRelative(new SharpDX.Vector3(0, -5, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg2.MoveLegRelative(new SharpDX.Vector3(0, 0, 5));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg2.MoveLegRelative(new SharpDX.Vector3(0, 0, -5));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            break;
                        case 3:
                            model.HexModel.Leg3.MoveLegRelative(new SharpDX.Vector3(5, 0, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg3.MoveLegRelative(new SharpDX.Vector3(-5, 0, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg3.MoveLegRelative(new SharpDX.Vector3(0, 5, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg3.MoveLegRelative(new SharpDX.Vector3(0, -5, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg3.MoveLegRelative(new SharpDX.Vector3(0, 0, 5));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg3.MoveLegRelative(new SharpDX.Vector3(0, 0, -5));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            break;
                        case 4:
                            model.HexModel.Leg4.MoveLegRelative(new SharpDX.Vector3(5, 0, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg4.MoveLegRelative(new SharpDX.Vector3(-5, 0, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg4.MoveLegRelative(new SharpDX.Vector3(0, 5, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg4.MoveLegRelative(new SharpDX.Vector3(0, -5, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg4.MoveLegRelative(new SharpDX.Vector3(0, 0, 5));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg4.MoveLegRelative(new SharpDX.Vector3(0, 0, -5));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            break;
                        case 5:
                            model.HexModel.Leg5.MoveLegRelative(new SharpDX.Vector3(5, 0, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg5.MoveLegRelative(new SharpDX.Vector3(-5, 0, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg5.MoveLegRelative(new SharpDX.Vector3(0, 5, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg5.MoveLegRelative(new SharpDX.Vector3(0, -5, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg5.MoveLegRelative(new SharpDX.Vector3(0, 0, 5));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg5.MoveLegRelative(new SharpDX.Vector3(0, 0, -5));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            break;
                        case 6:
                            model.HexModel.Leg6.MoveLegRelative(new SharpDX.Vector3(5, 0, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg6.MoveLegRelative(new SharpDX.Vector3(-5, 0, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg6.MoveLegRelative(new SharpDX.Vector3(0, 5, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg6.MoveLegRelative(new SharpDX.Vector3(0, -5, 0));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg6.MoveLegRelative(new SharpDX.Vector3(0, 0, 5));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            model.HexModel.Leg6.MoveLegRelative(new SharpDX.Vector3(0, 0, -5));
                            control.ExecuteOnce(new Command(model.HexModel.generate(duration)));
                            //Thread.Sleep(thsleep);
                            break;
                    }
                }
            });
        }
    }
}
