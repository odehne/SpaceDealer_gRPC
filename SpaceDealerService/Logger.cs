using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SpaceDealer
{
	public interface ILogger
	{
		TraceEventType LogLevel { get; set; }
		 void Log(string message, TraceEventType severity);
	}
	public class Logger : ILogger
	{
		public TraceEventType LogLevel { get; set; }

		public Logger(TraceEventType logLevel)
		{
			LogLevel = logLevel;
		}

		public void Log(string message, TraceEventType severity)
		{
			if (severity == TraceEventType.Verbose)
			{
				if (LogLevel == TraceEventType.Information)
					return;
			}
            OutputToConsole(severity, message);

        }

        private void OutputToConsole(TraceEventType eventType, string message)
        {
          
            switch (eventType)
            {
                case TraceEventType.Start:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("[STR]\t");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(message);
                    break;
                case TraceEventType.Stop:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("[STP]\t");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(message);
                    break;
                case TraceEventType.Verbose:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("[VER]\t");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(message);
                    break;
                case TraceEventType.Information:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("[INF]\t");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(message);
                    break;
                case TraceEventType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[ERR]\t");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(message);
                    break;
                case TraceEventType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("[WRN]\t");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(message);
                    break;
            }
        }
    }
}
