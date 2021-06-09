using System;

namespace Verse
{
	// Token: 0x0200039E RID: 926
	public class DamageWorker_AddGlobal : DamageWorker
	{
		// Token: 0x06001709 RID: 5897 RVA: 0x000DA8BC File Offset: 0x000D8ABC
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				Hediff hediff = HediffMaker.MakeHediff(dinfo.Def.hediff, pawn, null);
				hediff.Severity = dinfo.Amount;
				pawn.health.AddHediff(hediff, null, new DamageInfo?(dinfo), null);
			}
			return new DamageWorker.DamageResult();
		}
	}
}
