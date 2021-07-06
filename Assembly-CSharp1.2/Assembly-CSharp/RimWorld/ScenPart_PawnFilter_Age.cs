using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001604 RID: 5636
	public class ScenPart_PawnFilter_Age : ScenPart
	{
		// Token: 0x06007A94 RID: 31380 RVA: 0x000525EF File Offset: 0x000507EF
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Widgets.IntRange(listing.GetScenPartRect(this, 31f), (int)listing.CurHeight, ref this.allowedAgeRange, 15, 120, null, 4);
		}

		// Token: 0x06007A95 RID: 31381 RVA: 0x0024EE60 File Offset: 0x0024D060
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.allowedAgeRange, "allowedAgeRange", default(IntRange), false);
		}

		// Token: 0x06007A96 RID: 31382 RVA: 0x0024EE90 File Offset: 0x0024D090
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

		// Token: 0x06007A97 RID: 31383 RVA: 0x00052615 File Offset: 0x00050815
		public override bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return this.allowedAgeRange.Includes(pawn.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x06007A98 RID: 31384 RVA: 0x0024EF48 File Offset: 0x0024D148
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

		// Token: 0x06007A99 RID: 31385 RVA: 0x0024EFDC File Offset: 0x0024D1DC
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

		// Token: 0x04005058 RID: 20568
		public IntRange allowedAgeRange = new IntRange(0, 999999);

		// Token: 0x04005059 RID: 20569
		private const int RangeMin = 15;

		// Token: 0x0400505A RID: 20570
		private const int RangeMax = 120;

		// Token: 0x0400505B RID: 20571
		private const int RangeMinMax = 19;

		// Token: 0x0400505C RID: 20572
		private const int RangeMinWidth = 4;
	}
}
