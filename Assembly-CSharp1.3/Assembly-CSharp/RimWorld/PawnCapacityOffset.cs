using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AC6 RID: 2758
	public class PawnCapacityOffset
	{
		// Token: 0x06004139 RID: 16697 RVA: 0x0015F0A2 File Offset: 0x0015D2A2
		public float GetOffset(float capacityEfficiency)
		{
			return (Mathf.Min(capacityEfficiency, this.max) - 1f) * this.scale;
		}

		// Token: 0x040026ED RID: 9965
		public PawnCapacityDef capacity;

		// Token: 0x040026EE RID: 9966
		public float scale = 1f;

		// Token: 0x040026EF RID: 9967
		public float max = 9999f;
	}
}
