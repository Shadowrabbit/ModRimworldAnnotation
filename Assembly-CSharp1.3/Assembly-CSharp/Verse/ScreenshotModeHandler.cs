using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200041E RID: 1054
	public class ScreenshotModeHandler
	{
		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001FAF RID: 8111 RVA: 0x000C4EBF File Offset: 0x000C30BF
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001FB0 RID: 8112 RVA: 0x000C4EC8 File Offset: 0x000C30C8
		public bool FiltersCurrentEvent
		{
			get
			{
				return this.active && (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout || (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDrag));
			}
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x000C4F24 File Offset: 0x000C3124
		public void ScreenshotModesOnGUI()
		{
			if (KeyBindingDefOf.ToggleScreenshotMode.KeyDownEvent)
			{
				this.active = !this.active;
				Event.current.Use();
			}
		}

		// Token: 0x0400133D RID: 4925
		private bool active;
	}
}
