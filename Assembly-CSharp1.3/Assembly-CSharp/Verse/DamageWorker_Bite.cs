using System;

namespace Verse
{
	// Token: 0x0200026B RID: 619
	public class DamageWorker_Bite : DamageWorker_AddInjury
	{
		// Token: 0x0600118C RID: 4492 RVA: 0x000657EA File Offset: 0x000639EA
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside, null);
		}
	}
}
