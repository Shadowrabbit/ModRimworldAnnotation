using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000432 RID: 1074
	public static class Mouse
	{
		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06002063 RID: 8291 RVA: 0x000C8C84 File Offset: 0x000C6E84
		public static bool IsInputBlockedNow
		{
			get
			{
				WindowStack windowStack = Find.WindowStack;
				return (Widgets.mouseOverScrollViewStack.Count > 0 && !Widgets.mouseOverScrollViewStack.Peek()) || windowStack.MouseObscuredNow || !windowStack.CurrentWindowGetsInput;
			}
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x000C8CC7 File Offset: 0x000C6EC7
		public static bool IsOver(Rect rect)
		{
			return rect.Contains(Event.current.mousePosition) && !Mouse.IsInputBlockedNow;
		}
	}
}
