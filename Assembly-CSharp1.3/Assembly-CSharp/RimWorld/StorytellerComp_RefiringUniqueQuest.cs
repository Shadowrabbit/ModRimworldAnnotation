using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C4B RID: 3147
	public class StorytellerComp_RefiringUniqueQuest : StorytellerComp
	{
		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x060049AD RID: 18861 RVA: 0x0017B75F File Offset: 0x0017995F
		private int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x060049AE RID: 18862 RVA: 0x00185812 File Offset: 0x00183A12
		private StorytellerCompProperties_RefiringUniqueQuest Props
		{
			get
			{
				return (StorytellerCompProperties_RefiringUniqueQuest)this.props;
			}
		}

		// Token: 0x060049AF RID: 18863 RVA: 0x0018581F File Offset: 0x00183A1F
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
				if (this.Props.minColonyWealth > 0 && WealthUtility.PlayerWealth < (float)this.Props.minColonyWealth)
				{
					flag = false;
				}
				else if (this.generateSkipped)
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
				if (this.Props.incident.Worker.CanFireNow(parms))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x060049B0 RID: 18864 RVA: 0x00185836 File Offset: 0x00183A36
		public override void Initialize()
		{
			base.Initialize();
			if ((float)GenTicks.TicksGame >= this.Props.minDaysPassed * 60000f)
			{
				this.generateSkipped = true;
			}
		}

		// Token: 0x060049B1 RID: 18865 RVA: 0x0018585E File Offset: 0x00183A5E
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.incident;
		}

		// Token: 0x04002CC7 RID: 11463
		private bool generateSkipped;
	}
}
