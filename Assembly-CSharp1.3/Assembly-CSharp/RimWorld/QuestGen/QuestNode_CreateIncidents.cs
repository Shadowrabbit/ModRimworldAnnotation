using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200171B RID: 5915
	public class QuestNode_CreateIncidents : QuestNode
	{
		// Token: 0x06008878 RID: 34936 RVA: 0x0031052D File Offset: 0x0030E72D
		protected override bool TestRunInt(Slate slate)
		{
			return this.incidentDef.GetValue(slate) != null && this.points.GetValue(slate) >= this.incidentDef.GetValue(slate).minThreatPoints;
		}

		// Token: 0x06008879 RID: 34937 RVA: 0x00310560 File Offset: 0x0030E760
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int value = this.duration.GetValue(slate);
			Quest quest = QuestGen.quest;
			int value2 = this.startOffsetTicks.GetValue(slate);
			IncidentDef value3 = this.incidentDef.GetValue(slate);
			Map map = slate.Get<Map>("map", null, false);
			float value4 = this.points.GetValue(slate);
			Faction value5 = this.faction.GetValue(slate);
			string delayInSignal = slate.Get<string>("inSignal", null, false);
			string disableSignal = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignalDisable.GetValue(slate));
			int? value6 = this.randomIncidents.GetValue(slate);
			if (value6 != null)
			{
				int num = 0;
				for (;;)
				{
					int num2 = num;
					int? num3 = value6;
					if (!(num2 < num3.GetValueOrDefault() & num3 != null))
					{
						break;
					}
					this.CreateDelayedIncident(Rand.Range(value2, value), delayInSignal, disableSignal, value3, map, value4, value5);
					num++;
				}
			}
			int? value7 = this.intervalTicks.GetValue(slate);
			if (value7 != null)
			{
				int num4 = Mathf.FloorToInt((float)value / (float)value7.Value);
				for (int i = 0; i < num4; i++)
				{
					int delayTicks = Mathf.Max(i * value7.Value, value2);
					this.CreateDelayedIncident(delayTicks, delayInSignal, disableSignal, value3, map, value4, value5);
				}
			}
		}

		// Token: 0x0600887A RID: 34938 RVA: 0x003106A0 File Offset: 0x0030E8A0
		private void CreateDelayedIncident(int delayTicks, string delayInSignal, string disableSignal, IncidentDef incident, Map map, float points, Faction faction)
		{
			Quest quest = QuestGen.quest;
			QuestPart_Delay questPart_Delay = new QuestPart_Delay();
			questPart_Delay.delayTicks = delayTicks;
			questPart_Delay.inSignalEnable = delayInSignal;
			questPart_Delay.inSignalDisable = disableSignal;
			questPart_Delay.debugLabel = questPart_Delay.delayTicks.ToStringTicksToDays("F1") + "_" + this.incidentDef.ToString();
			quest.AddPart(questPart_Delay);
			QuestPart_Incident questPart_Incident = new QuestPart_Incident();
			questPart_Incident.incident = incident;
			questPart_Incident.inSignal = questPart_Delay.OutSignalCompleted;
			questPart_Incident.SetIncidentParmsAndRemoveTarget(new IncidentParms
			{
				forced = true,
				target = map,
				points = points,
				faction = faction
			});
			quest.AddPart(questPart_Incident);
		}

		// Token: 0x04005659 RID: 22105
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x0400565A RID: 22106
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x0400565B RID: 22107
		public SlateRef<IncidentDef> incidentDef;

		// Token: 0x0400565C RID: 22108
		public SlateRef<int?> intervalTicks;

		// Token: 0x0400565D RID: 22109
		public SlateRef<int?> randomIncidents;

		// Token: 0x0400565E RID: 22110
		public SlateRef<int> startOffsetTicks;

		// Token: 0x0400565F RID: 22111
		public SlateRef<int> duration;

		// Token: 0x04005660 RID: 22112
		public SlateRef<float> points;

		// Token: 0x04005661 RID: 22113
		public SlateRef<Faction> faction;
	}
}
