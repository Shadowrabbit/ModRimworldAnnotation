using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200036B RID: 875
	public class MoteText : MoteThrown
	{
		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x060018C6 RID: 6342 RVA: 0x00092295 File Offset: 0x00090495
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

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x060018C7 RID: 6343 RVA: 0x000922B1 File Offset: 0x000904B1
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.TimeBeforeStartFadeout + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x0000313F File Offset: 0x0000133F
		public override void Draw()
		{
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x000922D8 File Offset: 0x000904D8
		public override void DrawGUIOverlay()
		{
			float a = 1f - (base.AgeSecs - this.TimeBeforeStartFadeout) / this.def.mote.fadeOutTime;
			Color color = new Color(this.textColor.r, this.textColor.g, this.textColor.b, a);
			GenMapUI.DrawText(new Vector2(this.exactPosition.x, this.exactPosition.z), this.text, color);
		}

		// Token: 0x040010CB RID: 4299
		public string text;

		// Token: 0x040010CC RID: 4300
		public Color textColor = Color.white;

		// Token: 0x040010CD RID: 4301
		public float overrideTimeBeforeStartFadeout = -1f;
	}
}
