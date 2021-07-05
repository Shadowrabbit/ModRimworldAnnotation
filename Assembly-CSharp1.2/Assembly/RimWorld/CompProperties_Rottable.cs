using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F18 RID: 3864
	public class CompProperties_Rottable : CompProperties
	{
		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x0600556A RID: 21866 RVA: 0x0003B3EE File Offset: 0x000395EE
		public int TicksToRotStart
		{
			get
			{
				return Mathf.RoundToInt(this.daysToRotStart * 60000f);
			}
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x0600556B RID: 21867 RVA: 0x0003B401 File Offset: 0x00039601
		public int TicksToDessicated
		{
			get
			{
				return Mathf.RoundToInt(this.daysToDessicated * 60000f);
			}
		}

		// Token: 0x0600556C RID: 21868 RVA: 0x0003B414 File Offset: 0x00039614
		public CompProperties_Rottable()
		{
			this.compClass = typeof(CompRottable);
		}

		// Token: 0x0600556D RID: 21869 RVA: 0x0003B44D File Offset: 0x0003964D
		public CompProperties_Rottable(float daysToRotStart)
		{
			this.daysToRotStart = daysToRotStart;
		}

		// Token: 0x0600556E RID: 21870 RVA: 0x0003B47D File Offset: 0x0003967D
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal && parentDef.tickerType != TickerType.Rare)
			{
				yield return string.Concat(new object[]
				{
					"CompRottable needs tickerType ",
					TickerType.Rare,
					" or ",
					TickerType.Normal,
					", has ",
					parentDef.tickerType
				});
			}
			yield break;
			yield break;
		}

		// Token: 0x04003689 RID: 13961
		public float daysToRotStart = 2f;

		// Token: 0x0400368A RID: 13962
		public bool rotDestroys;

		// Token: 0x0400368B RID: 13963
		public float rotDamagePerDay = 40f;

		// Token: 0x0400368C RID: 13964
		public float daysToDessicated = 999f;

		// Token: 0x0400368D RID: 13965
		public float dessicatedDamagePerDay;

		// Token: 0x0400368E RID: 13966
		public bool disableIfHatcher;
	}
}
