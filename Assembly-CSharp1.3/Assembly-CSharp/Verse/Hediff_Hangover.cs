using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002C8 RID: 712
	public class Hediff_Hangover : HediffWithComps
	{
		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x0600133E RID: 4926 RVA: 0x0006D4AB File Offset: 0x0006B6AB
		public override bool Visible
		{
			get
			{
				return !this.pawn.health.hediffSet.HasHediff(HediffDefOf.AlcoholHigh, false) && base.Visible;
			}
		}
	}
}
