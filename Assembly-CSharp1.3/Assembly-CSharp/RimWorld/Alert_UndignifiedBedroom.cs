using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200126D RID: 4717
	public class Alert_UndignifiedBedroom : Alert
	{
		// Token: 0x060070EB RID: 28907 RVA: 0x00259FDB File Offset: 0x002581DB
		public Alert_UndignifiedBedroom()
		{
			this.defaultLabel = "UndignifiedBedroom".Translate();
			this.defaultExplanation = "UndignifiedBedroomDesc".Translate();
		}

		// Token: 0x170013B2 RID: 5042
		// (get) Token: 0x060070EC RID: 28908 RVA: 0x0025A018 File Offset: 0x00258218
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

		// Token: 0x060070ED RID: 28909 RVA: 0x0025A0C8 File Offset: 0x002582C8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x060070EE RID: 28910 RVA: 0x0025A0D8 File Offset: 0x002582D8
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

		// Token: 0x04003E2E RID: 15918
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
