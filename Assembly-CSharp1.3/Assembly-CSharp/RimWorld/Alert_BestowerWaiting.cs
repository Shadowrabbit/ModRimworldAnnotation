using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001253 RID: 4691
	public class Alert_BestowerWaiting : Alert
	{
		// Token: 0x170013A0 RID: 5024
		// (get) Token: 0x0600707B RID: 28795 RVA: 0x00257528 File Offset: 0x00255728
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

		// Token: 0x0600707C RID: 28796 RVA: 0x002575E8 File Offset: 0x002557E8
		public Alert_BestowerWaiting()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600707D RID: 28797 RVA: 0x00257602 File Offset: 0x00255802
		public override string GetLabel()
		{
			return "BestowerWaitingAlert".Translate();
		}

		// Token: 0x0600707E RID: 28798 RVA: 0x00257613 File Offset: 0x00255813
		public override TaggedString GetExplanation()
		{
			return "BestowerWaitingAlertDesc".Translate();
		}

		// Token: 0x0600707F RID: 28799 RVA: 0x0025761F File Offset: 0x0025581F
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BestowersWaiting);
		}

		// Token: 0x04003E10 RID: 15888
		private List<Pawn> bestowersWaitingResult = new List<Pawn>();
	}
}
