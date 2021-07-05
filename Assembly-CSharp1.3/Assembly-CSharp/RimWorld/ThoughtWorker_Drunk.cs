using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000992 RID: 2450
	public class ThoughtWorker_Drunk : ThoughtWorker
	{
		// Token: 0x06003DAC RID: 15788 RVA: 0x00152E90 File Offset: 0x00151090
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (!p.IsTeetotaler())
			{
				return false;
			}
			if (!other.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				return false;
			}
			Hediff firstHediffOfDef = other.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.AlcoholHigh, false);
			if (firstHediffOfDef == null || !firstHediffOfDef.Visible)
			{
				return false;
			}
			return true;
		}
	}
}
