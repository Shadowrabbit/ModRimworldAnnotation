using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000755 RID: 1877
	public class ScreenshotModeHandler
	{
		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06002F4F RID: 12111 RVA: 0x00025114 File Offset: 0x00023314
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06002F50 RID: 12112 RVA: 0x0013ADCC File Offset: 0x00138FCC
		public bool FiltersCurrentEvent
		{
			get
			{
				return this.active && (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout || (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDrag));
			}
		}

		// Token: 0x06002F51 RID: 12113 RVA: 0x0002511C File Offset: 0x0002331C
		public void ScreenshotModesOnGUI()
		{
			if (KeyBindingDefOf.ToggleScreenshotMode.KeyDownEvent)
			{
				this.active = !this.active;
				Event.current.Use();
			}
		}

		// Token: 0x04002019 RID: 8217
		private bool active;
	}
}
