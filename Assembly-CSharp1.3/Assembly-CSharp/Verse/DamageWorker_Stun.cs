using System;

namespace Verse
{
	// Token: 0x02000272 RID: 626
	public class DamageWorker_Stun : DamageWorker
	{
		// Token: 0x060011A1 RID: 4513 RVA: 0x000663E9 File Offset: 0x000645E9
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult damageResult = base.Apply(dinfo, victim);
			damageResult.stunned = true;
			return damageResult;
		}
	}
}
