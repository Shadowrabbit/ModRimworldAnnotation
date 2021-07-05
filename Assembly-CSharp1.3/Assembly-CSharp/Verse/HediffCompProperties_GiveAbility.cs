using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000298 RID: 664
	public class HediffCompProperties_GiveAbility : HediffCompProperties
	{
		// Token: 0x06001277 RID: 4727 RVA: 0x0006A679 File Offset: 0x00068879
		public HediffCompProperties_GiveAbility()
		{
			this.compClass = typeof(HediffComp_GiveAbility);
		}

		// Token: 0x04000DF9 RID: 3577
		public AbilityDef abilityDef;
	}
}
