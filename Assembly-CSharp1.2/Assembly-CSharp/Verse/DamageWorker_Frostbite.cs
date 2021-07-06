using System;

namespace Verse
{
	// Token: 0x02000398 RID: 920
	public class DamageWorker_Frostbite : DamageWorker_AddInjury
	{
		// Token: 0x060016FB RID: 5883 RVA: 0x000163DD File Offset: 0x000145DD
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			base.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
		}
	}
}
