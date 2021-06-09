using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003B0 RID: 944
	public class Hediff_Alcohol : Hediff_High
	{
		// Token: 0x06001775 RID: 6005 RVA: 0x000DC624 File Offset: 0x000DA824
		public override void Tick()
		{
			base.Tick();
			if (this.CurStageIndex >= 3 && this.pawn.IsHashIntervalTick(300) && this.HangoverSusceptible(this.pawn))
			{
				Hediff hediff = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hangover, false);
				if (hediff != null)
				{
					hediff.Severity = 1f;
					return;
				}
				hediff = HediffMaker.MakeHediff(HediffDefOf.Hangover, this.pawn, null);
				hediff.Severity = 1f;
				this.pawn.health.AddHediff(hediff, null, null, null);
			}
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x0000A2A7 File Offset: 0x000084A7
		private bool HangoverSusceptible(Pawn pawn)
		{
			return true;
		}

		// Token: 0x04001213 RID: 4627
		private const int HangoverCheckInterval = 300;
	}
}
