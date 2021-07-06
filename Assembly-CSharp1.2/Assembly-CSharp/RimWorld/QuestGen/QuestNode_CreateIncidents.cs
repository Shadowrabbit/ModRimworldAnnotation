using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FEF RID: 8175
	public class QuestNode_CreateIncidents : QuestNode
	{
		// Token: 0x0600AD4C RID: 44364 RVA: 0x00070DAB File Offset: 0x0006EFAB
		protected override bool TestRunInt(Slate slate)
		{
			return this.incidentDef.GetValue(slate) != null && this.points.GetValue(slate) >= this.incidentDef.GetValue(slate).minThreatPoints;
		}

		// Token: 0x0600AD4D RID: 44365 RVA: 0x00327170 File Offset: 0x00325370
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

		// Token: 0x0600AD4E RID: 44366 RVA: 0x003272B0 File Offset: 0x003254B0
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

		// Token: 0x040076C9 RID: 30409
		[NoTranslate]
		public SlateRef<string> inSignalEnable;

		// Token: 0x040076CA RID: 30410
		[NoTranslate]
		public SlateRef<string> inSignalDisable;

		// Token: 0x040076CB RID: 30411
		public SlateRef<IncidentDef> incidentDef;

		// Token: 0x040076CC RID: 30412
		public SlateRef<int?> intervalTicks;

		// Token: 0x040076CD RID: 30413
		public SlateRef<int?> randomIncidents;

		// Token: 0x040076CE RID: 30414
		public SlateRef<int> startOffsetTicks;

		// Token: 0x040076CF RID: 30415
		public SlateRef<int> duration;

		// Token: 0x040076D0 RID: 30416
		public SlateRef<float> points;

		// Token: 0x040076D1 RID: 30417
		public SlateRef<Faction> faction;
	}
}
