using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001015 RID: 4117
	public class ScenPart_ConfigPage_ConfigureStartingPawns : ScenPart_ConfigPage
	{
		// Token: 0x06006114 RID: 24852 RVA: 0x0020FD38 File Offset: 0x0020DF38
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

		// Token: 0x06006115 RID: 24853 RVA: 0x0020FE12 File Offset: 0x0020E012
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.pawnCount, "pawnCount", 0, false);
			Scribe_Values.Look<int>(ref this.pawnChoiceCount, "pawnChoiceCount", 0, false);
		}

		// Token: 0x06006116 RID: 24854 RVA: 0x0020FE3E File Offset: 0x0020E03E
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartWithPawns".Translate(this.pawnCount, this.pawnChoiceCount);
		}

		// Token: 0x06006117 RID: 24855 RVA: 0x0020FE65 File Offset: 0x0020E065
		public override void Randomize()
		{
			this.pawnCount = Rand.RangeInclusive(1, 6);
			this.pawnChoiceCount = 10;
		}

		// Token: 0x06006118 RID: 24856 RVA: 0x0020FE7C File Offset: 0x0020E07C
		public override void PostIdeoChosen()
		{
			Find.GameInitData.startingPawnCount = this.pawnCount;
			if (ModsConfig.IdeologyActive)
			{
				Faction ofPlayerSilentFail = Faction.OfPlayerSilentFail;
				bool flag;
				if (ofPlayerSilentFail == null)
				{
					flag = (null != null);
				}
				else
				{
					FactionIdeosTracker ideos = ofPlayerSilentFail.ideos;
					flag = (((ideos != null) ? ideos.PrimaryIdeo : null) != null);
				}
				if (flag)
				{
					foreach (Precept precept in Faction.OfPlayerSilentFail.ideos.PrimaryIdeo.PreceptsListForReading)
					{
						if (precept.def.defaultDrugPolicyOverride != null)
						{
							Current.Game.drugPolicyDatabase.MakePolicyDefault(precept.def.defaultDrugPolicyOverride);
						}
					}
				}
			}
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
			IL_EF:
			while (Find.GameInitData.startingAndOptionalPawns.Count < this.pawnChoiceCount)
			{
				Find.GameInitData.startingAndOptionalPawns.Add(StartingPawnUtility.NewGeneratedStartingPawn());
			}
			return;
			goto IL_EF;
		}

		// Token: 0x04003761 RID: 14177
		public int pawnCount = 3;

		// Token: 0x04003762 RID: 14178
		public int pawnChoiceCount = 10;

		// Token: 0x04003763 RID: 14179
		private string pawnCountBuffer;

		// Token: 0x04003764 RID: 14180
		private string pawnCountChoiceBuffer;

		// Token: 0x04003765 RID: 14181
		private const int MaxPawnCount = 10;

		// Token: 0x04003766 RID: 14182
		private const int MaxPawnChoiceCount = 10;
	}
}
