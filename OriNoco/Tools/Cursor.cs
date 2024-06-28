using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OriNoco.Tools
{
	public static class CursorToolkit
	{
		public static List<Cursor> cursorHistory = new List<Cursor>();
		
		public static bool cursorVisibility
		{
			set
			{
				if(value)
					Cursor.Show();
				else
					Cursor.Hide();
			}
		}
		
		public static void ChangeCursor(Cursor to)
		{
			Cursor.Current = to;
			cursorHistory.Add(to);
		}
		
		public static void UndoCursor()
		{
			cursorHistory.RemoveAt(cursorHistory.Count - 1);
			Cursor.Current = cursorHistory.Count > 0 ? cursorHistory[cursorHistory.Count - 1] : Cursors.Default;
		}
	}
}
