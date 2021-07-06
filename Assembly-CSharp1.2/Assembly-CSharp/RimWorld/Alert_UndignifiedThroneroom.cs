using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001958 RID: 6488
	public class Alert_UndignifiedThroneroom : Alert
	{
		// Token: 0x06008FA8 RID: 36776 RVA: 0x00060446 File Offset: 0x0005E646
		public Alert_UndignifiedThroneroom()
		{
			this.defaultLabel = "UndignifiedThroneroom".Translate();
			this.defaultExplanation = "UndignifiedThroneroomDesc".Translate();
		}

		// Token: 0x170016B2 RID: 5810
		// (get) Token: 0x06008FA9 RID: 36777 RVA: 0x00295C2C File Offset: 0x00293E2C
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

		// Token: 0x06008FAA RID: 36778 RVA: 0x00060483 File Offset: 0x0005E683
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x06008FAB RID: 36779 RVA: 0x00295CDC File Offset: 0x00293EDC
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

		// Token: 0x04005B77 RID: 23415
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
