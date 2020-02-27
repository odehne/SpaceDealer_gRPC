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
			Console.WriteLine(message);
		}
	}
}
