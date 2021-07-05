using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FCA RID: 4042
	public class CompProperties_RitualEffectSpawnOnRole : CompProperties_RitualEffectSpawnOnPawn
	{
		// Token: 0x06005F25 RID: 24357 RVA: 0x00208C14 File Offset: 0x00206E14
		public CompProperties_RitualEffectSpawnOnRole()
		{
			this.compClass = typeof(CompRitualEffect_SpawnOnRole);
		}

		// Token: 0x040036D0 RID: 14032
		[NoTranslate]
		public string roleId;
	}
}
