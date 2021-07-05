using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003B1 RID: 945
	public class Hediff_Hangover : HediffWithComps
	{
		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001778 RID: 6008 RVA: 0x0001683A File Offset: 0x00014A3A
		public override bool Visible
		{
			get
			{
				return !this.pawn.health.hediffSet.HasHediff(HediffDefOf.AlcoholHigh, false) && base.Visible;
			}
		}
	}
}
