using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200179E RID: 6046
	public static class CaravanBedUtility
	{
		// Token: 0x06008C21 RID: 35873 RVA: 0x00324329 File Offset: 0x00322529
		public static bool InCaravanBed(this Pawn p)
		{
			return p.CurrentCaravanBed() != null;
		}

		// Token: 0x06008C22 RID: 35874 RVA: 0x00324334 File Offset: 0x00322534
		public static Building_Bed CurrentCaravanBed(this Pawn p)
		{
			Caravan caravan = p.GetCaravan();
			if (caravan == null)
			{
				return null;
			}
			return caravan.beds.GetBedUsedBy(p);
		}

		// Token: 0x06008C23 RID: 35875 RVA: 0x00324359 File Offset: 0x00322559
		public static bool WouldBenefitFromRestingInBed(Pawn p)
		{
			return !p.Dead && p.health.hediffSet.HasImmunizableNotImmuneHediff();
		}

		// Token: 0x06008C24 RID: 35876 RVA: 0x00324378 File Offset: 0x00322578
		public static string AppendUsingBedsLabel(string str, int bedCount)
		{
			string str2 = (bedCount == 1) ? "UsingBedroll".Translate() : "UsingBedrolls".Translate(bedCount);
			return str + " (" + str2 + ")";
		}
	}
}
