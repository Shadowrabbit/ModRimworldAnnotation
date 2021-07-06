using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE9 RID: 4073
	public class PawnCapacityOffset
	{
		// Token: 0x060058D4 RID: 22740 RVA: 0x0003DB6F File Offset: 0x0003BD6F
		public float GetOffset(float capacityEfficiency)
		{
			return (Mathf.Min(capacityEfficiency, this.max) - 1f) * this.scale;
		}

		// Token: 0x04003B3C RID: 15164
		public PawnCapacityDef capacity;

		// Token: 0x04003B3D RID: 15165
		public float scale = 1f;

		// Token: 0x04003B3E RID: 15166
		public float max = 9999f;
	}
}
