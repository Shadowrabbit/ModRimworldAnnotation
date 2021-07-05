using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000189 RID: 393
	internal sealed class DebugCell
	{
		// Token: 0x06000B27 RID: 2855 RVA: 0x0003CA40 File Offset: 0x0003AC40
		public void Draw()
		{
			if (this.customMat != null)
			{
				CellRenderer.RenderCell(this.c, this.customMat);
				return;
			}
			CellRenderer.RenderCell(this.c, this.colorPct);
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0003CA74 File Offset: 0x0003AC74
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

		// Token: 0x04000943 RID: 2371
		public IntVec3 c;

		// Token: 0x04000944 RID: 2372
		public string displayString;

		// Token: 0x04000945 RID: 2373
		public float colorPct;

		// Token: 0x04000946 RID: 2374
		public int ticksLeft;

		// Token: 0x04000947 RID: 2375
		public Material customMat;
	}
}
