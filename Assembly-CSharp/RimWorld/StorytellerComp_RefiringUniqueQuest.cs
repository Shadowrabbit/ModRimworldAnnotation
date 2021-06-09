using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001237 RID: 4663
	public class StorytellerComp_RefiringUniqueQuest : StorytellerComp
	{
		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x060065D3 RID: 26067 RVA: 0x00044070 File Offset: 0x00042270
		private int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x060065D4 RID: 26068 RVA: 0x000459F2 File Offset: 0x00043BF2
		private StorytellerCompProperties_RefiringUniqueQuest Props
		{
			get
			{
				return (StorytellerCompProperties_RefiringUniqueQuest)this.props;
			}
		}

		// Token: 0x060065D5 RID: 26069 RVA: 0x000459FF File Offset: 0x00043BFF
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (!this.Props.incident.TargetAllowed(target))
			{
				yield break;
			}
			Quest quest = null;
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].root == this.Props.incident.questScriptDef && (quest == null || questsListForReading[i].appearanceTick > quest.appearanceTick))
				{
					quest = questsListForReading[i];
					break;
				}
			}
			bool flag;
			if (quest == null)
			{
				if (this.generateSkipped)
				{
					flag = ((float)GenTicks.TicksGame >= this.Props.minDaysPassed * 60000f);
				}
				else
				{
					flag = (this.IntervalsPassed == (int)(this.Props.minDaysPassed * 60f) + 1);
				}
			}
			else
			{
				flag = (this.Props.refireEveryDays >= 0f && (quest.State != QuestState.EndedSuccess && quest.State != QuestState.Ongoing && quest.State != QuestState.NotYetAccepted && quest.cleanupTick >= 0 && this.IntervalsPassed == (int)((float)quest.cleanupTick + this.Props.refireEveryDays * 60000f) / 1000));
			}
			if (flag)
			{
				IncidentParms parms = this.GenerateParms(this.Props.incident.category, target);
				if (this.Props.incident.Worker.CanFireNow(parms, false))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x060065D6 RID: 26070 RVA: 0x00045A16 File Offset: 0x00043C16
		public override void Initialize()
		{
			base.Initialize();
			if ((float)GenTicks.TicksGame >= this.Props.minDaysPassed * 60000f)
			{
				this.generateSkipped = true;
			}
		}

		// Token: 0x060065D7 RID: 26071 RVA: 0x00045A3E File Offset: 0x00043C3E
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incident;
		}

		// Token: 0x040043CD RID: 17357
		private bool generateSkipped;
	}
}
