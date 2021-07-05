using System;

namespace Verse
{
	// Token: 0x02000274 RID: 628
	public class DamageWorker_AddGlobal : DamageWorker
	{
		// Token: 0x060011A5 RID: 4517 RVA: 0x000665C0 File Offset: 0x000647C0
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
