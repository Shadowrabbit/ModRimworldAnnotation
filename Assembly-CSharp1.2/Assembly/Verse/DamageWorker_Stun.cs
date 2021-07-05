using System;

namespace Verse
{
	// Token: 0x0200039C RID: 924
	public class DamageWorker_Stun : DamageWorker
	{
		// Token: 0x06001705 RID: 5893 RVA: 0x000163EB File Offset: 0x000145EB
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult damageResult = base.Apply(dinfo, victim);
			damageResult.stunned = true;
			return damageResult;
		}
	}
}
