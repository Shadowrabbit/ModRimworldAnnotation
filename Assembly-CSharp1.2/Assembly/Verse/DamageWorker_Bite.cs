using System;

namespace Verse
{
	// Token: 0x02000390 RID: 912
	public class DamageWorker_Bite : DamageWorker_AddInjury
	{
		// Token: 0x060016D6 RID: 5846 RVA: 0x000162AA File Offset: 0x000144AA
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside, null);
		}
	}
}
