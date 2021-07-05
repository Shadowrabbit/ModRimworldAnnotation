using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200126C RID: 4716
	public class Alert_UndignifiedThroneroom : Alert
	{
		// Token: 0x060070E7 RID: 28903 RVA: 0x00259E85 File Offset: 0x00258085
		public Alert_UndignifiedThroneroom()
		{
			this.defaultLabel = "UndignifiedThroneroom".Translate();
			this.defaultExplanation = "UndignifiedThroneroomDesc".Translate();
		}

		// Token: 0x170013B1 RID: 5041
		// (get) Token: 0x060070E8 RID: 28904 RVA: 0x00259EC4 File Offset: 0x002580C4
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
						if (pawn.royalty != null && !pawn.Suspended && pawn.royalty.GetUnmetThroneroomRequirements(true, false).Any<string>())
						{
							this.targetsResult.Add(pawn);
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x060070E9 RID: 28905 RVA: 0x00259F74 File Offset: 0x00258174
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x060070EA RID: 28906 RVA: 0x00259F84 File Offset: 0x00258184
		public override TaggedString GetExplanation()
		{
			return this.defaultExplanation + "\n" + this.Targets.Select(delegate(Pawn t)
			{
				RoyalTitle royalTitle = t.royalty.HighestTitleWithThroneRoomRequirements();
				RoyalTitleDef royalTitleDef = royalTitle.RoomRequirementGracePeriodActive(t) ? royalTitle.def.GetPreviousTitle(royalTitle.faction) : royalTitle.def;
				string[] array = t.royalty.GetUnmetThroneroomRequirements(false, false).ToArray<string>();
				string[] array2 = t.royalty.GetUnmetThroneroomRequirements(true, true).ToArray<string>();
				StringBuilder stringBuilder = new StringBuilder();
				if (array.Length != 0)
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
					if (array.Length != 0)
					{
						stringBuilder.Append("\n\n");
					}
					stringBuilder.Append(t.LabelShort + " (" + royalTitle.def.GetLabelFor(t.gender) + ", " + "RoomRequirementGracePeriodDesc".Translate(royalTitle.RoomRequirementGracePeriodDaysLeft.ToString("0.0")) + ")" + ":\n" + array2.ToLineList("- "));
				}
				return stringBuilder.ToString();
			}).ToLineList("\n", false);
		}

		// Token: 0x04003E2D RID: 15917
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
