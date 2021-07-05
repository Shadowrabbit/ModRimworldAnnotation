using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AC5 RID: 2757
	public class PawnCapacityFactor
	{
		// Token: 0x06004137 RID: 16695 RVA: 0x0015F008 File Offset: 0x0015D208
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

		// Token: 0x040026E7 RID: 9959
		public PawnCapacityDef capacity;

		// Token: 0x040026E8 RID: 9960
		public float weight = 1f;

		// Token: 0x040026E9 RID: 9961
		public float max = 9999f;

		// Token: 0x040026EA RID: 9962
		public bool useReciprocal;

		// Token: 0x040026EB RID: 9963
		public float allowedDefect;

		// Token: 0x040026EC RID: 9964
		private const float MaxReciprocalFactor = 5f;
	}
}
