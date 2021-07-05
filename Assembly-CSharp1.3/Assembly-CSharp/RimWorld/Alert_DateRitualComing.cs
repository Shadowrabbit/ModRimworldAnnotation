using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200128A RID: 4746
	public class Alert_DateRitualComing : Alert
	{
		// Token: 0x0600715E RID: 29022 RVA: 0x0025CBB2 File Offset: 0x0025ADB2
		public Alert_DateRitualComing()
		{
			this.defaultLabel = "AlertRitualComing".Translate();
		}

		// Token: 0x0600715F RID: 29023 RVA: 0x0025CBDC File Offset: 0x0025ADDC
		private void UpcomingRituals()
		{
			this.ritualEntries.Clear();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept_Ritual precept_Ritual in ideo.PreceptsListForReading.OfType<Precept_Ritual>())
				{
					if (!precept_Ritual.isAnytime)
					{
						RitualObligationTrigger ritualObligationTrigger = precept_Ritual.obligationTriggers.FirstOrDefault((RitualObligationTrigger o) => o is RitualObligationTrigger_Date);
						if (ritualObligationTrigger != null)
						{
							RitualObligationTrigger_Date ritualObligationTrigger_Date = (RitualObligationTrigger_Date)ritualObligationTrigger;
							int num = ritualObligationTrigger_Date.OccursOnTick();
							int num2 = ritualObligationTrigger_Date.CurrentTickRelative();
							if ((float)(num - num2) < 180000f && num2 < num)
							{
								this.ritualEntries.Add("AlertRitualComingEntry".Translate(precept_Ritual.LabelCap, ritualObligationTrigger_Date.DateString, (num - num2).ToStringTicksToPeriod(true, false, true, true)));
							}
						}
					}
				}
			}
		}

		// Token: 0x06007160 RID: 29024 RVA: 0x0025CD24 File Offset: 0x0025AF24
		public override TaggedString GetExplanation()
		{
			return "AlertRitualComingDesc".Translate() + ":\n\n" + this.ritualEntries.ToLineList(" -  ") + "\n\n" + "AlertRitualComingExtra".Translate(RitualObligation.DaysToExpire);
		}

		// Token: 0x06007161 RID: 29025 RVA: 0x0025CD7D File Offset: 0x0025AF7D
		public override AlertReport GetReport()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			this.UpcomingRituals();
			return this.ritualEntries.Count > 0;
		}

		// Token: 0x04003E5A RID: 15962
		private const float TicksBeforeWarning = 180000f;

		// Token: 0x04003E5B RID: 15963
		private List<string> ritualEntries = new List<string>();
	}
}
