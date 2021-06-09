using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200039F RID: 927
	public class DamageWorker_Extinguish : DamageWorker
	{
		// Token: 0x0600170B RID: 5899 RVA: 0x000DA910 File Offset: 0x000D8B10
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

		// Token: 0x040011B0 RID: 4528
		private const float DamageAmountToFireSizeRatio = 0.01f;
	}
}
