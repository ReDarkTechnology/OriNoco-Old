using System;

namespace OriNoco
{
	public class Handle
	{
		public IntPtr handle;
		
		public Handle()
		{
			
		}
		public Handle(IntPtr ptr)
		{
			handle = ptr;
		}
	
		public static implicit operator IntPtr(Handle v)
		{
			return v.handle;
		}
		
		public static implicit operator Handle(IntPtr intPtr)
		{
			return new Handle(intPtr);
		}
	}
}
