using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200161C RID: 5660
	public class ScenPart_ConfigPage_ConfigureStartingPawns : ScenPart_ConfigPage
	{
		// Token: 0x06007B0C RID: 31500 RVA: 0x0024FE24 File Offset: 0x0024E024
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			base.DoEditInterface(listing);
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			scenPartRect.height = ScenPart.RowHeight;
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect = new Rect(scenPartRect.x - 200f, scenPartRect.y + ScenPart.RowHeight, 200f, ScenPart.RowHeight);
			rect.xMax -= 4f;
			Widgets.Label(rect, "ScenPart_StartWithPawns_OutOf".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.TextFieldNumeric<int>(scenPartRect, ref this.pawnCount, ref this.pawnCountBuffer, 1f, 10f);
			scenPartRect.y += ScenPart.RowHeight;
			Widgets.TextFieldNumeric<int>(scenPartRect, ref this.pawnChoiceCount, ref this.pawnCountChoiceBuffer, (float)this.pawnCount, 10f);
		}

		// Token: 0x06007B0D RID: 31501 RVA: 0x00052BB1 File Offset: 0x00050DB1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.pawnCount, "pawnCount", 0, false);
			Scribe_Values.Look<int>(ref this.pawnChoiceCount, "pawnChoiceCount", 0, false);
		}

		// Token: 0x06007B0E RID: 31502 RVA: 0x00052BDD File Offset: 0x00050DDD
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartWithPawns".Translate(this.pawnCount, this.pawnChoiceCount);
		}

		// Token: 0x06007B0F RID: 31503 RVA: 0x00052C04 File Offset: 0x00050E04
		public override void Randomize()
		{
			this.pawnCount = Rand.RangeInclusive(1, 6);
			this.pawnChoiceCount = 10;
		}

		// Token: 0x06007B10 RID: 31504 RVA: 0x0024FF00 File Offset: 0x0024E100
		public override void PostWorldGenerate()
		{
			Find.GameInitData.startingPawnCount = this.pawnCount;
			int num = 0;
			do
			{
				StartingPawnUtility.ClearAllStartingPawns();
				for (int i = 0; i < this.pawnCount; i++)
				{
					Find.GameInitData.startingAndOptionalPawns.Add(StartingPawnUtility.NewGeneratedStartingPawn());
				}
				num++;
				if (num > 20)
				{
					break;
				}
			}
			while (!StartingPawnUtility.WorkTypeRequirementsSatisfied());
			IL_62:
			while (Find.GameInitData.startingAndOptionalPawns.Count < this.pawnChoiceCount)
			{
				Find.GameInitData.startingAndOptionalPawns.Add(StartingPawnUtility.NewGeneratedStartingPawn());
			}
			return;
			goto IL_62;
		}

		// Token: 0x04005098 RID: 20632
		public int pawnCount = 3;

		// Token: 0x04005099 RID: 20633
		public int pawnChoiceCount = 10;

		// Token: 0x0400509A RID: 20634
		private string pawnCountBuffer;

		// Token: 0x0400509B RID: 20635
		private string pawnCountChoiceBuffer;

		// Token: 0x0400509C RID: 20636
		private const int MaxPawnCount = 10;

		// Token: 0x0400509D RID: 20637
		private const int MaxPawnChoiceCount = 10;
	}
}
