using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AIBot.Control
{
    public class UARTController
    {
        private TimeSpan TIMEOUT = TimeSpan.FromSeconds(10);
        private TimeSpan USRTIMEOUT = TimeSpan.FromSeconds(60);
        public SerialPort port;
        private bool isExecuted { get; set; }
        private bool isUserStop { get; set; }
        private bool isStarted { get; set; }
        private bool isSimulation { get;set;}
        public List<Command> PreExecuteCommand { get; set; }
        public List<Command> PostExecuteCommand { get; set; }

        public UARTController(SerialPort p,bool isSim=false)
        {
            port = p;
            port.DataReceived += port_DataReceived;
            isSimulation = isSim;
        }

        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string l = port.ReadLine();
            if (l == "#AGF")
            {
                isExecuted = true;
            }
            else if (l == "#CC")
            {
                isExecuted = true;
            }
        }
        public void ClearCommand()
        {
            PreExecuteCommand = null;
            PostExecuteCommand = null;
        }
        private bool ExecuteFunc(Command comm)
        {
            isExecuted = false;
            DateTime sttime = DateTime.Now;
            port.WriteLine(comm.command);
            while (!isExecuted)
            {
                if (DateTime.Now - sttime > TIMEOUT)
                {
                    return false;
                    //break;
                }
                System.Threading.Thread.Sleep(5);
            }
            return true;
        }
        private void ExecuteNoResp(Command comm)
        {
            port.WriteLine(comm.command);
        }
        private bool ExecuteLoopFunc(List<Command> comm)
        {
            isUserStop = false;
            DateTime sttime = DateTime.Now;
            if (PreExecuteCommand != null)
            {
                foreach(Command c in PreExecuteCommand)
                    ExecuteFunc(c);
            }
            while (!isUserStop)
            {
                if (DateTime.Now - sttime > USRTIMEOUT)
                {
                    return false;
                }
                foreach(Command c in comm)
                    ExecuteFunc(c);
            }
            if (PostExecuteCommand != null)
            {
                foreach(Command c in PostExecuteCommand)
                    ExecuteFunc(c);
            }
            return true;
        }
        public void ExecuteOnce(Command comm)
        {
            //if (isStarted) return;
            //isStarted = true;
            //Thread t = new Thread(() => { ExecuteFunc(comm); });
            //t.Start();            
            if (isSimulation)
            {               
                Thread.Sleep(comm.sleep);
            }
            else
            {
                ExecuteFunc(comm);
            }
            //ExecuteNoResp(comm);
            //Thread.Sleep(500);
        }
        public void Test()
        {
            //------- pre -------//
            ExecuteOnce(new Command("#4P1500#5P950#6P871#7P1500#8P969#9P897#10P1500#11P872#12P921#21P2041#22P1967#23P1500#24P1948#25P2037#26P1500#27P2114#28P2056#29P1500T200"));
            ExecuteOnce(new Command("#4P1600#5P750#6P871#7P1500#8P969#9P897#10P1600#11P672#12P921#21P2041#22P1967#23P1500#24P1948#25P2237#26P1600#27P2114#28P2056#29P1500T200"));
            //------- loop ------//
            for (int i = 1; i <= 6; i++)
            {
                ExecuteOnce(new Command("#4P1700#5P950#6P871#7P1500#8P969#9P897#10P1700#11P872#12P921#21P2041#22P1967#23P1500#24P1948#25P2037#26P1700#27P2114#28P2056#29P1500T200"));
                ExecuteOnce(new Command("#4P1600#5P950#6P871#7P1600#8P769#9P897#10P1600#11P872#12P921#21P2041#22P2167#23P1600#24P1948#25P2037#26P1600#27P2114#28P2256#29P1600T200"));
                ExecuteOnce(new Command("#4P1500#5P950#6P871#7P1700#8P969#9P897#10P1500#11P872#12P921#21P2041#22P1967#23P1700#24P1948#25P2037#26P1500#27P2114#28P2056#29P1700T200"));
                ExecuteOnce(new Command("#4P1600#5P750#6P871#7P1600#8P969#9P897#10P1600#11P672#12P921#21P2041#22P1967#23P1600#24P1948#25P2237#26P1600#27P2114#28P2056#29P1600T200"));
            }
            //-------- post ------//
            ExecuteOnce(new Command("#4P1700#5P950#6P871#7P1500#8P969#9P897#10P1700#11P872#12P921#21P2041#22P1967#23P1500#24P1948#25P2037#26P1700#27P2114#28P2056#29P1500T200"));
            ExecuteOnce(new Command("#4P1600#5P950#6P871#7P1500#8P769#9P897#10P1600#11P872#12P921#21P2041#22P2167#23P1500#24P1948#25P2037#26P1600#27P2114#28P2256#29P1500T200"));
            ExecuteOnce(new Command("#4P1500#5P950#6P871#7P1500#8P969#9P897#10P1500#11P872#12P921#21P2041#22P1967#23P1500#24P1948#25P2037#26P1500#27P2114#28P2056#29P1500T200"));
        }
        public void ExecuteLoop(List<Command> comm)
        {
            if (isStarted) return;
            isStarted = true;
            Thread t = new Thread(() => { ExecuteLoopFunc(comm); });
            t.Start();
        }
        public void StopLoop()
        {
            isUserStop = true;
            isStarted = false;
        }
    }
}
