using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200018A RID: 394
	internal struct DebugLine
	{
		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x0003CAF0 File Offset: 0x0003ACF0
		public bool Done
		{
			get
			{
				return this.deathTick <= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0003CB07 File Offset: 0x0003AD07
		public DebugLine(Vector3 a, Vector3 b, int ticksLeft = 100, SimpleColor color = SimpleColor.White)
		{
			this.a = a;
			this.b = b;
			this.deathTick = Find.TickManager.TicksGame + ticksLeft;
			this.color = color;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0003CB31 File Offset: 0x0003AD31
		public void Draw()
		{
			GenDraw.DrawLineBetween(this.a, this.b, this.color, 0.2f);
		}

		// Token: 0x04000948 RID: 2376
		public Vector3 a;

		// Token: 0x04000949 RID: 2377
		public Vector3 b;

		// Token: 0x0400094A RID: 2378
		private int deathTick;

		// Token: 0x0400094B RID: 2379
		private SimpleColor color;
	}
}
