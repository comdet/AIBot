using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AIBot.Control
{
    public enum JoyKey
    {
        LUP,
        LDOWN,
        LLEFT,
        LRIGHT,
        RLEFT = JoystickOffset.Buttons3,
        RUP = JoystickOffset.Buttons0,
        RRIGHT = JoystickOffset.Buttons1,
        RDOWN = JoystickOffset.Buttons2,
        Select = JoystickOffset.Buttons8,
        Start = JoystickOffset.Buttons9,
        L1 = JoystickOffset.Buttons4,
        L2 = JoystickOffset.Buttons6,
        R1 = JoystickOffset.Buttons5,
        R2 = JoystickOffset.Buttons7,
        UNKNOW,
    }
    public enum JoyEventFireMode { Toggle, Burst}
    public class JoystickController
    {
        private Joystick joystick;
        private bool isPooling = false;
        private Thread puller;
        private List<JoyKey> holdKey;
        public delegate void OnJoyEventHandler(object sender, JoyStickEvent key);
        public event OnJoyEventHandler OnKey;
        private JoyEventFireMode firemode;
        public JoystickController(JoyEventFireMode fmode)
        {
            var directInput = new DirectInput();
            var joystickGuid = Guid.Empty;

            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
            {
                joystickGuid = deviceInstance.InstanceGuid;
            }            
            if (joystickGuid == Guid.Empty)
            {
                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                {
                    joystickGuid = deviceInstance.InstanceGuid;
                }
            }            
            if (joystickGuid == Guid.Empty)
            {
                Console.WriteLine("No joystick/Gamepad found.");
                //Console.ReadKey();
                //Environment.Exit(1);
                return;
            }            
            joystick = new Joystick(directInput, joystickGuid);            
            Console.WriteLine("Found Joystick/Gamepad with GUID: {0}", joystickGuid);
             var allEffects = joystick.GetEffects();
            foreach (var effectInfo in allEffects)
            {
                Console.WriteLine("Effect available {0}", effectInfo.Name);
            }            
            joystick.Properties.BufferSize = 128;
            firemode = fmode;         
        }
        public void StartPulling()
        {
            if (joystick == null) return;
            joystick.Acquire();
            isPooling = true;
            holdKey = new List<JoyKey>();
            puller = new Thread(Pull);
            puller.Start();
        }
        public void StopPulling()
        {
            joystick.Unacquire();
            isPooling = false;
        }
        private void Pull()
        {
            while (isPooling)
            {
                joystick.Poll();
                var datas = joystick.GetBufferedData();
                foreach (var state in datas)
                {
                    //Console.WriteLine("JoyKey : {0} , Data : {1}",state.Offset,state.Value);
                    //------ convert enum ------//
                    int val = state.Value;
                    JoyKey key = JoyKey.UNKNOW;                  
                    if(state.Offset == JoystickOffset.X)
                    {                        
                        if (val == 0)
                        {
                            key = JoyKey.LLEFT;
                            val = 128;
                        }else if(val == 65535){
                            key = JoyKey.LRIGHT;
                            val = 128;
                        }else if(val == 32767 && holdKey.Contains(JoyKey.LLEFT)){
                            key = JoyKey.LLEFT;
                            val = 0;
                        }else if(val == 32767 && holdKey.Contains(JoyKey.LRIGHT))
                        {
                            key = JoyKey.LRIGHT;
                            val = 0;
                        }
                    }else if (state.Offset == JoystickOffset.Y)
                    {
                        if (val == 0)
                        {
                            key = JoyKey.LUP;
                            val = 128;
                        }
                        else if (val == 65535)
                        {
                            key = JoyKey.LDOWN;
                            val = 128;
                        }
                        else if (val == 32767 && holdKey.Contains(JoyKey.LUP))
                        {
                            key = JoyKey.LUP;
                            val = 0;
                        }
                        else if (val == 32767 && holdKey.Contains(JoyKey.LDOWN))
                        {
                            key = JoyKey.LDOWN;
                            val = 0;
                        }
                    }
                    else
                    {
                        key = (JoyKey)state.Offset;
                    }
                    if (!holdKey.Contains(key) && val == 128)
                    {
                        holdKey.Add(key);                                                                            
                    }else if(holdKey.Contains(key) && val == 0)
                    {
                        holdKey.Remove(key);
                    }  
                    if(firemode == JoyEventFireMode.Toggle)
                    {
                        if (OnKey != null)
                        {
                            //Console.WriteLine("-->{0} : {1}", key.ToString(), val);
                            OnKey(this, new JoyStickEvent() { key = key, Value = val });
                        }
                    }                  
                }
                //------ fire event -------//
                if (firemode == JoyEventFireMode.Burst)
                {
                    foreach (JoyKey key in holdKey)
                    {
                        if (OnKey != null)
                        {                            
                            OnKey(this, new JoyStickEvent() { key = key, Value = 128 });
                        }
                    }
                }
                Thread.Sleep(50);
            }
        }

    }
    public class JoyStickEvent : EventArgs
    {
        public JoyKey key {get; set; }
        public int Value { get; set; }
    }
}
