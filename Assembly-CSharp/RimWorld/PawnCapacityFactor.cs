using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE8 RID: 4072
	public class PawnCapacityFactor
	{
		// Token: 0x060058D2 RID: 22738 RVA: 0x001D0DA0 File Offset: 0x001CEFA0
		public float GetFactor(float capacityEfficiency)
		{
			float num = capacityEfficiency;
			if (this.allowedDefect != 0f && num < 1f)
			{
				num = Mathf.InverseLerp(0f, 1f - this.allowedDefect, num);
			}
			if (num > this.max)
			{
				num = this.max;
			}
			if (this.useReciprocal)
			{
				if (Mathf.Abs(num) < 0.001f)
				{
					num = 5f;
				}
				else
				{
					num = Mathf.Min(1f / num, 5f);
				}
			}
			return num;
		}

		// Token: 0x04003B36 RID: 15158
		public PawnCapacityDef capacity;

		// Token: 0x04003B37 RID: 15159
		public float weight = 1f;

		// Token: 0x04003B38 RID: 15160
		public float max = 9999f;

		// Token: 0x04003B39 RID: 15161
		public bool useReciprocal;

		// Token: 0x04003B3A RID: 15162
		public float allowedDefect;

		// Token: 0x04003B3B RID: 15163
		private const float MaxReciprocalFactor = 5f;
	}
}
