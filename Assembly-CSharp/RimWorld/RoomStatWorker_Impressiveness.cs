using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001313 RID: 4883
	public class RoomStatWorker_Impressiveness : RoomStatWorker
	{
		// Token: 0x060069BD RID: 27069 RVA: 0x00208D64 File Offset: 0x00206F64
		public override float GetScore(Room room)
		{
			float factor = this.GetFactor(room.GetStat(RoomStatDefOf.Wealth) / 1500f);
			float factor2 = this.GetFactor(room.GetStat(RoomStatDefOf.Beauty) / 3f);
			float factor3 = this.GetFactor(room.GetStat(RoomStatDefOf.Space) / 125f);
			float factor4 = this.GetFactor(1f + Mathf.Min(room.GetStat(RoomStatDefOf.Cleanliness), 0f) / 2.5f);
			float a = (factor + factor2 + factor3 + factor4) / 4f;
			float b = Mathf.Min(factor, Mathf.Min(factor2, Mathf.Min(factor3, factor4)));
			float num = Mathf.Lerp(a, b, 0.35f);
			float num2 = factor3 * 5f;
			if (num > num2)
			{
				num = Mathf.Lerp(num, num2, 0.75f);
			}
			return num * 100f;
		}

		// Token: 0x060069BE RID: 27070 RVA: 0x000480BA File Offset: 0x000462BA
		private float GetFactor(float baseFactor)
		{
			if (Mathf.Abs(baseFactor) < 1f)
			{
				return baseFactor;
			}
			if (baseFactor > 0f)
			{
				return 1f + Mathf.Log(baseFactor);
			}
			return -1f - Mathf.Log(-baseFactor);
		}
	}
}
