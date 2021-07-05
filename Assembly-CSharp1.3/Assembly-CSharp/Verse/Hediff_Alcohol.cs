using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002C7 RID: 711
	public class Hediff_Alcohol : Hediff_High
	{
		// Token: 0x0600133B RID: 4923 RVA: 0x0006D400 File Offset: 0x0006B600
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

		// Token: 0x0600133C RID: 4924 RVA: 0x000126F5 File Offset: 0x000108F5
		private bool HangoverSusceptible(Pawn pawn)
		{
			return true;
		}

		// Token: 0x04000E51 RID: 3665
		private const int HangoverCheckInterval = 300;
	}
}
