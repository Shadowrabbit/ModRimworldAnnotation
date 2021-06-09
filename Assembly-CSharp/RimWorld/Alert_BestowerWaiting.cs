using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001942 RID: 6466
	public class Alert_BestowerWaiting : Alert
	{
		// Token: 0x170016A4 RID: 5796
		// (get) Token: 0x06008F4B RID: 36683 RVA: 0x00293EB0 File Offset: 0x002920B0
		private List<Pawn> BestowersWaiting
		{
			get
			{
				this.bestowersWaitingResult.Clear();
				foreach (Map map in Find.Maps)
				{
					foreach (Lord lord in map.lordManager.lords)
					{
						if (lord.CurLordToil is LordToil_BestowingCeremony_Wait)
						{
							this.bestowersWaitingResult.Add(lord.ownedPawns[0]);
						}
					}
				}
				return this.bestowersWaitingResult;
			}
		}

		// Token: 0x06008F4C RID: 36684 RVA: 0x0005FF15 File Offset: 0x0005E115
		public Alert_BestowerWaiting()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06008F4D RID: 36685 RVA: 0x0005FF2F File Offset: 0x0005E12F
		public override string GetLabel()
		{
			return "BestowerWaitingAlert".Translate();
		}

		// Token: 0x06008F4E RID: 36686 RVA: 0x0005FF40 File Offset: 0x0005E140
		public override TaggedString GetExplanation()
		{
			return "BestowerWaitingAlertDesc".Translate();
		}

		// Token: 0x06008F4F RID: 36687 RVA: 0x0005FF4C File Offset: 0x0005E14C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BestowersWaiting);
		}

		// Token: 0x04005B5D RID: 23389
		private List<Pawn> bestowersWaitingResult = new List<Pawn>();
	}
}
