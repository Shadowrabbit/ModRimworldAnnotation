using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000275 RID: 629
	public class DamageWorker_Extinguish : DamageWorker
	{
		// Token: 0x060011A7 RID: 4519 RVA: 0x00066614 File Offset: 0x00064814
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult result = new DamageWorker.DamageResult();
			Fire fire = victim as Fire;
			if (fire == null || fire.Destroyed)
			{
				return result;
			}
			base.Apply(dinfo, victim);
			fire.fireSize -= dinfo.Amount * 0.01f;
			if (fire.fireSize <= 0.1f)
			{
				fire.Destroy(DestroyMode.Vanish);
			}
			return result;
		}

		// Token: 0x04000D77 RID: 3447
		private const float DamageAmountToFireSizeRatio = 0.01f;
	}
}
