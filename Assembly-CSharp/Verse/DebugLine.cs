using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000247 RID: 583
	internal struct DebugLine
	{
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000EDA RID: 3802 RVA: 0x00011299 File Offset: 0x0000F499
		public bool Done
		{
			get
			{
				return this.deathTick <= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x000112B0 File Offset: 0x0000F4B0
		public DebugLine(Vector3 a, Vector3 b, int ticksLeft = 100, SimpleColor color = SimpleColor.White)
		{
			this.a = a;
			this.b = b;
			this.deathTick = Find.TickManager.TicksGame + ticksLeft;
			this.color = color;
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x000112DA File Offset: 0x0000F4DA
		public void Draw()
		{
			GenDraw.DrawLineBetween(this.a, this.b, this.color);
		}

		// Token: 0x04000C39 RID: 3129
		public Vector3 a;

		// Token: 0x04000C3A RID: 3130
		public Vector3 b;

		// Token: 0x04000C3B RID: 3131
		private int deathTick;

		// Token: 0x04000C3C RID: 3132
		private SimpleColor color;
	}
}
