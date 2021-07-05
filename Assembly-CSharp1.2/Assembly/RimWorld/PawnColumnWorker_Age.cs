using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B78 RID: 7032
	public class PawnColumnWorker_Age : PawnColumnWorker_Text
	{
		// Token: 0x17001869 RID: 6249
		// (get) Token: 0x06009AF4 RID: 39668 RVA: 0x0000A2E4 File Offset: 0x000084E4
		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		// Token: 0x06009AF5 RID: 39669 RVA: 0x002D809C File Offset: 0x002D629C
		public override int Compare(Pawn a, Pawn b)
		{
			return a.ageTracker.AgeBiologicalYears.CompareTo(b.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x06009AF6 RID: 39670 RVA: 0x002D80C8 File Offset: 0x002D62C8
		protected override string GetTextFor(Pawn pawn)
		{
			return pawn.ageTracker.AgeBiologicalYears.ToString();
		}
	}
}
