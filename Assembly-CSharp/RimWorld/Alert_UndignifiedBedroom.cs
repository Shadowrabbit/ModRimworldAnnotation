using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200195A RID: 6490
	public class Alert_UndignifiedBedroom : Alert
	{
		// Token: 0x06008FAF RID: 36783 RVA: 0x0006049C File Offset: 0x0005E69C
		public Alert_UndignifiedBedroom()
		{
			this.defaultLabel = "UndignifiedBedroom".Translate();
			this.defaultExplanation = "UndignifiedBedroomDesc".Translate();
		}

		// Token: 0x170016B3 RID: 5811
		// (get) Token: 0x06008FB0 RID: 36784 RVA: 0x00295E88 File Offset: 0x00294088
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn pawn in maps[i].mapPawns.FreeColonists)
					{
						if (pawn.royalty != null && !pawn.Suspended && pawn.royalty.GetUnmetBedroomRequirements(true, false).Any<string>())
						{
							this.targetsResult.Add(pawn);
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x06008FB1 RID: 36785 RVA: 0x000604D9 File Offset: 0x0005E6D9
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x06008FB2 RID: 36786 RVA: 0x00295F38 File Offset: 0x00294138
		public override TaggedString GetExplanation()
		{
			return this.defaultExplanation + "\n" + this.Targets.Select(delegate(Pawn t)
			{
				RoyalTitle royalTitle = t.royalty.HighestTitleWithBedroomRequirements();
				RoyalTitleDef royalTitleDef = royalTitle.RoomRequirementGracePeriodActive(t) ? royalTitle.def.GetPreviousTitle(royalTitle.faction) : royalTitle.def;
				string[] array = t.royalty.GetUnmetBedroomRequirements(false, false).ToArray<string>();
				string[] array2 = t.royalty.GetUnmetBedroomRequirements(true, true).ToArray<string>();
				bool flag = royalTitleDef != null && array.Length != 0;
				StringBuilder stringBuilder = new StringBuilder();
				if (flag)
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						t.LabelShort,
						" (",
						royalTitleDef.GetLabelFor(t.gender),
						"):\n",
						array.ToLineList("- ")
					}));
				}
				if (array2.Length != 0)
				{
					if (flag)
					{
						stringBuilder.Append("\n\n");
					}
					stringBuilder.Append(t.LabelShort + " (" + royalTitle.def.GetLabelFor(t.gender) + ", " + "RoomRequirementGracePeriodDesc".Translate(royalTitle.RoomRequirementGracePeriodDaysLeft.ToString("0.0")) + ")" + ":\n" + array2.ToLineList("- "));
				}
				return stringBuilder.ToString();
			}).ToLineList("\n", false);
		}

		// Token: 0x04005B7A RID: 23418
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
