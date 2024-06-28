using System;

namespace OriNoco
{
	public static class Debug
	{
		public static Action<DebugInfo> OnWarning;
		public static Action<DebugInfo> OnInfo;
		public static Action<DebugInfo> OnError;

		public static void Log(object sender, object argument)
		{
			var inf = new DebugInfo() {
					type = DebugType.Info,
					sender = sender,
					arguments = argument
			};
			if (OnInfo != null)
				OnInfo.Invoke(inf);
			else
				Console.WriteLine(Log(inf));
		}
		
		public static void Log(object argument)
		{
			//if(!Core.logMode) return;
			var inf = new DebugInfo() {
					type = DebugType.Info,
					arguments = argument
			};
			if (OnInfo != null)
				OnInfo.Invoke(inf);
			else
				Console.WriteLine(Log(inf));
		}
		
		public static void LogError(object sender, object argument)
		{
			var inf = new DebugInfo() {
					type = DebugType.Error,
					sender = sender,
					arguments = argument
			};
			if (OnError != null)
				OnError.Invoke(inf);
			else
				Console.WriteLine(Log(inf));
		}
		
		public static void LogError(object argument)
		{
			var inf = new DebugInfo() {
					type = DebugType.Error,
					arguments = argument
			};
			if (OnError != null)
				OnError.Invoke(inf);
			else
				Console.WriteLine(Log(inf));
		}
		
		public static void LogWarning(object sender, object argument)
		{
			var inf = new DebugInfo() {
					type = DebugType.Warning,
					sender = sender,
					arguments = argument
			};
			if (OnWarning != null)
				OnWarning.Invoke(inf);
			else
				Console.WriteLine(Log(inf));
		}
		
		public static void LogWarning(object argument)
		{
			var inf = new DebugInfo() {
					type = DebugType.Warning,
					arguments = argument
			};
			if (OnWarning != null)
				OnWarning.Invoke(inf);
			else
				Console.WriteLine(Log(inf));
		}
		
		static string Log(DebugInfo info)
		{
			string say = info.type.ToString("G");
			if(info.sender != null)
			{
				say = info.sender.ToString();
				if(info.sender.GetType().FullName != "System.String") say = info.sender.GetType().FullName;
			}
			return info.type.ToString("G") + "[" + say + "]"+ ": " + info.arguments.ToString();
		}
	}
	[Serializable]
	public class DebugInfo
	{
		public DebugType type;
		public object sender;
		public object arguments;
	}
	public enum DebugType
	{
		Error,
		Warning,
		Info
	}
}