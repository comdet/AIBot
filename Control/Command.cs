using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIBot.Control
{
    public class Command
    {
        public string command { get; set; }
        public int loop { get; set; }
        private int group { get; set; }
        public int sleep { get; set; }
        public Command(string comm)
        {
            command = comm;
            if (comm.IndexOf('T') != -1)
            {
                string t = comm.Substring(comm.IndexOf('T')+1);
                sleep = int.Parse(t);
            }
        }
        public Command(int group, int loop)
        {
            this.loop = loop;
            this.group = group;
        }
        public Command(params Servo[] servo)
        {

        }
        public void CreateByServo(int server_number, int position)
        {

        }
        public void CreateByServo(Servo s)
        {

        }
        private void Generate()
        {

        }
    }
    public class Servo
    {
        public int Number { get; set; }
        public int Position { get; set; }
        public int Angle { get; set; }
    }
}
