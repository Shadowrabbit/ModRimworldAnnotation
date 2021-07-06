using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000246 RID: 582
	internal sealed class DebugCell
	{
		// Token: 0x06000ED7 RID: 3799 RVA: 0x00011266 File Offset: 0x0000F466
		public void Draw()
		{
			if (this.customMat != null)
			{
				CellRenderer.RenderCell(this.c, this.customMat);
				return;
			}
			CellRenderer.RenderCell(this.c, this.colorPct);
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x000B45B8 File Offset: 0x000B27B8
		public void OnGUI()
		{
			if (this.displayString != null)
			{
				Vector2 vector = this.c.ToUIPosition();
				Rect rect = new Rect(vector.x - 20f, vector.y - 20f, 40f, 40f);
				if (new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Overlaps(rect))
				{
					Widgets.Label(rect, this.displayString);
				}
			}
		}

		// Token: 0x04000C34 RID: 3124
		public IntVec3 c;

		// Token: 0x04000C35 RID: 3125
		public string displayString;

		// Token: 0x04000C36 RID: 3126
		public float colorPct;

		// Token: 0x04000C37 RID: 3127
		public int ticksLeft;

		// Token: 0x04000C38 RID: 3128
		public Material customMat;
	}
}
