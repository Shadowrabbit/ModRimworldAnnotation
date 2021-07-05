using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200138E RID: 5006
	public class PawnColumnWorker_Age : PawnColumnWorker_Text
	{
		// Token: 0x17001564 RID: 5476
		// (get) Token: 0x060079BE RID: 31166 RVA: 0x0001276E File Offset: 0x0001096E
		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		// Token: 0x060079BF RID: 31167 RVA: 0x002B029C File Offset: 0x002AE49C
		public override int Compare(Pawn a, Pawn b)
		{
			return a.ageTracker.AgeBiologicalYears.CompareTo(b.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x060079C0 RID: 31168 RVA: 0x002B02C8 File Offset: 0x002AE4C8
		protected override string GetTextFor(Pawn pawn)
		{
			return pawn.ageTracker.AgeBiologicalYears.ToString();
		}
	}
}
