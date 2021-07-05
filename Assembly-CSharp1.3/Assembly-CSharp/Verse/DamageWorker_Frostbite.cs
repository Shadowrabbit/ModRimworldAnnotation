using System;

namespace Verse
{
	// Token: 0x0200026F RID: 623
	public class DamageWorker_Frostbite : DamageWorker_AddInjury
	{
		// Token: 0x06001199 RID: 4505 RVA: 0x000660F2 File Offset: 0x000642F2
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			base.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
		}
	}
}
