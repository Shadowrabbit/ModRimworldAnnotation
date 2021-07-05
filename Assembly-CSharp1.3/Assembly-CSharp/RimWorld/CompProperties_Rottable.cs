using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A0A RID: 2570
	public class CompProperties_Rottable : CompProperties
	{
		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06003EFC RID: 16124 RVA: 0x00157DCA File Offset: 0x00155FCA
		public int TicksToRotStart
		{
			get
			{
				return Mathf.RoundToInt(this.daysToRotStart * 60000f);
			}
		}

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06003EFD RID: 16125 RVA: 0x00157DDD File Offset: 0x00155FDD
		public int TicksToDessicated
		{
			get
			{
				return Mathf.RoundToInt(this.daysToDessicated * 60000f);
			}
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x00157DF0 File Offset: 0x00155FF0
		public CompProperties_Rottable()
		{
			this.compClass = typeof(CompRottable);
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x00157E29 File Offset: 0x00156029
		public CompProperties_Rottable(float daysToRotStart)
		{
			this.daysToRotStart = daysToRotStart;
		}

		// Token: 0x06003F00 RID: 16128 RVA: 0x00157E59 File Offset: 0x00156059
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

		// Token: 0x040021F4 RID: 8692
		public float daysToRotStart = 2f;

		// Token: 0x040021F5 RID: 8693
		public bool rotDestroys;

		// Token: 0x040021F6 RID: 8694
		public float rotDamagePerDay = 40f;

		// Token: 0x040021F7 RID: 8695
		public float daysToDessicated = 999f;

		// Token: 0x040021F8 RID: 8696
		public float dessicatedDamagePerDay;

		// Token: 0x040021F9 RID: 8697
		public bool disableIfHatcher;
	}
}
