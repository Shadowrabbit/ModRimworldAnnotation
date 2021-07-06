using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020020DE RID: 8414
	public static class CaravanBedUtility
	{
		// Token: 0x0600B2CE RID: 45774 RVA: 0x0007431B File Offset: 0x0007251B
		public static bool InCaravanBed(this Pawn p)
		{
			return p.CurrentCaravanBed() != null;
		}

		// Token: 0x0600B2CF RID: 45775 RVA: 0x0033D0A0 File Offset: 0x0033B2A0
		public static Building_Bed CurrentCaravanBed(this Pawn p)
		{
			Caravan caravan = p.GetCaravan();
			if (caravan == null)
			{
				return null;
			}
			return caravan.beds.GetBedUsedBy(p);
		}

		// Token: 0x0600B2D0 RID: 45776 RVA: 0x00074326 File Offset: 0x00072526
		public static bool WouldBenefitFromRestingInBed(Pawn p)
		{
			return !p.Dead && p.health.hediffSet.HasImmunizableNotImmuneHediff();
		}

		// Token: 0x0600B2D1 RID: 45777 RVA: 0x0033D0C8 File Offset: 0x0033B2C8
		public static string AppendUsingBedsLabel(string str, int bedCount)
		{
			string str2 = (bedCount == 1) ? "UsingBedroll".Translate() : "UsingBedrolls".Translate(bedCount);
			return str + " (" + str2 + ")";
		}
	}
}
