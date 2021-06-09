using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000774 RID: 1908
	public static class Mouse
	{
		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x0600300E RID: 12302 RVA: 0x0013D728 File Offset: 0x0013B928
		public static bool IsInputBlockedNow
		{
			get
			{
				WindowStack windowStack = Find.WindowStack;
				return (Widgets.mouseOverScrollViewStack.Count > 0 && !Widgets.mouseOverScrollViewStack.Peek()) || windowStack.MouseObscuredNow || !windowStack.CurrentWindowGetsInput;
			}
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x00025DDA File Offset: 0x00023FDA
		public static bool IsOver(Rect rect)
		{
			return rect.Contains(Event.current.mousePosition) && !Mouse.IsInputBlockedNow;
		}
	}
}
