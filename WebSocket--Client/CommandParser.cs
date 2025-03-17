using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket_Client
{
    public class CommandParser
    {
        public Action<CommArgs> OnCommandTask = (e) => { };
        public void Start()
        {
            while (true)
            {
                var cmd = Console.ReadLine();
                OnCommandTask.Invoke(new CommArgs(cmd));
            }
        }

        public class CommArgs
        {
            public string title;
            public string command;
            public string[] CommandSeries;

            public CommArgs(string comstr)
            {
                CommandSeries = comstr.Split(' ');
                title = CommandSeries[0];
                command = CommandSeries.Length > 1 ? CommandSeries[1] : "";
            }

        }
    }
}
