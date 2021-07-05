using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200100C RID: 4108
	public class ScenPart_PawnFilter_Age : ScenPart
	{
		// Token: 0x060060DA RID: 24794 RVA: 0x0020ED39 File Offset: 0x0020CF39
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Widgets.IntRange(listing.GetScenPartRect(this, 31f), (int)listing.CurHeight, ref this.allowedAgeRange, 15, 120, null, 4);
		}

		// Token: 0x060060DB RID: 24795 RVA: 0x0020ED60 File Offset: 0x0020CF60
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.allowedAgeRange, "allowedAgeRange", default(IntRange), false);
		}

		// Token: 0x060060DC RID: 24796 RVA: 0x0020ED90 File Offset: 0x0020CF90
		public override string Summary(Scenario scen)
		{
			if (this.allowedAgeRange.min > 15)
			{
				if (this.allowedAgeRange.max < 10000)
				{
					return "ScenPart_StartingPawnAgeRange".Translate(this.allowedAgeRange.min, this.allowedAgeRange.max);
				}
				return "ScenPart_StartingPawnAgeMin".Translate(this.allowedAgeRange.min);
			}
			else
			{
				if (this.allowedAgeRange.max < 10000)
				{
					return "ScenPart_StartingPawnAgeMax".Translate(this.allowedAgeRange.max);
				}
				throw new Exception();
			}
		}

		// Token: 0x060060DD RID: 24797 RVA: 0x0020EE45 File Offset: 0x0020D045
		public override bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return this.allowedAgeRange.Includes(pawn.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x060060DE RID: 24798 RVA: 0x0020EE60 File Offset: 0x0020D060
		public override void Randomize()
		{
			this.allowedAgeRange = new IntRange(15, 120);
			switch (Rand.RangeInclusive(0, 2))
			{
			case 0:
				this.allowedAgeRange.min = Rand.Range(20, 60);
				break;
			case 1:
				this.allowedAgeRange.max = Rand.Range(20, 60);
				break;
			case 2:
				this.allowedAgeRange.min = Rand.Range(20, 60);
				this.allowedAgeRange.max = Rand.Range(20, 60);
				break;
			}
			this.MakeAllowedAgeRangeValid();
		}

		// Token: 0x060060DF RID: 24799 RVA: 0x0020EEF4 File Offset: 0x0020D0F4
		private void MakeAllowedAgeRangeValid()
		{
			if (this.allowedAgeRange.max < 19)
			{
				this.allowedAgeRange.max = 19;
			}
			if (this.allowedAgeRange.max - this.allowedAgeRange.min < 4)
			{
				this.allowedAgeRange.min = this.allowedAgeRange.max - 4;
			}
		}

		// Token: 0x0400374D RID: 14157
		public IntRange allowedAgeRange = new IntRange(0, 999999);

		// Token: 0x0400374E RID: 14158
		private const int RangeMin = 15;

		// Token: 0x0400374F RID: 14159
		private const int RangeMax = 120;

		// Token: 0x04003750 RID: 14160
		private const int RangeMinMax = 19;

		// Token: 0x04003751 RID: 14161
		private const int RangeMinWidth = 4;
	}
}
