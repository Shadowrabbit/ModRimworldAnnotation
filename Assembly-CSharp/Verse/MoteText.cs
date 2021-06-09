using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004FC RID: 1276
	public class MoteText : MoteThrown
	{
		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001FC4 RID: 8132 RVA: 0x0001C01F File Offset: 0x0001A21F
		protected float TimeBeforeStartFadeout
		{
			get
			{
				if (this.overrideTimeBeforeStartFadeout < 0f)
				{
					return base.SolidTime;
				}
				return this.overrideTimeBeforeStartFadeout;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001FC5 RID: 8133 RVA: 0x0001C03B File Offset: 0x0001A23B
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.TimeBeforeStartFadeout + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void Draw()
		{
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x00100F70 File Offset: 0x000FF170
		public override void DrawGUIOverlay()
		{
			float a = 1f - (base.AgeSecs - this.TimeBeforeStartFadeout) / this.def.mote.fadeOutTime;
			Color color = new Color(this.textColor.r, this.textColor.g, this.textColor.b, a);
			GenMapUI.DrawText(new Vector2(this.exactPosition.x, this.exactPosition.z), this.text, color);
		}

		// Token: 0x0400164C RID: 5708
		public string text;

		// Token: 0x0400164D RID: 5709
		public Color textColor = Color.white;

		// Token: 0x0400164E RID: 5710
		public float overrideTimeBeforeStartFadeout = -1f;
	}
}
