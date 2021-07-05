using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F63 RID: 3939
	public class RitualOutcomeComp_NumActiveLoudspeakers : RitualOutcomeComp_Quality
	{
		// Token: 0x06005D6D RID: 23917 RVA: 0x00200565 File Offset: 0x001FE765
		public override RitualOutcomeComp_Data MakeData()
		{
			return new RitualOutcomeComp_DataThingPresence();
		}

		// Token: 0x06005D6E RID: 23918 RVA: 0x002007F2 File Offset: 0x001FE9F2
		private bool LoudspeakerActive(Thing s, TargetInfo target)
		{
			return s.GetRoom(RegionType.Set_All) == target.Cell.GetRoom(target.Map) && s.TryGetComp<CompPowerTrader>().PowerOn;
		}

		// Token: 0x06005D6F RID: 23919 RVA: 0x00200820 File Offset: 0x001FEA20
		public override void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
		{
			base.Tick(ritual, data, progressAmount);
			TargetInfo selectedTarget = ritual.selectedTarget;
			if (selectedTarget.ThingDestroyed || !selectedTarget.HasThing)
			{
				return;
			}
			RitualOutcomeComp_DataThingPresence ritualOutcomeComp_DataThingPresence = (RitualOutcomeComp_DataThingPresence)data;
			foreach (Thing thing in selectedTarget.Map.listerBuldingOfDefInProximity.GetForCell(selectedTarget.Cell, (float)this.maxDistance, ThingDefOf.Loudspeaker, null))
			{
				if (this.LoudspeakerActive(thing, selectedTarget))
				{
					if (!ritualOutcomeComp_DataThingPresence.presentForTicks.ContainsKey(thing))
					{
						ritualOutcomeComp_DataThingPresence.presentForTicks.Add(thing, 0f);
					}
					Dictionary<Thing, float> presentForTicks = ritualOutcomeComp_DataThingPresence.presentForTicks;
					Thing key = thing;
					float num = presentForTicks[key];
					presentForTicks[key] = num + 1f;
				}
			}
		}

		// Token: 0x06005D70 RID: 23920 RVA: 0x00200900 File Offset: 0x001FEB00
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			int num = 0;
			foreach (KeyValuePair<Thing, float> keyValuePair in ((RitualOutcomeComp_DataThingPresence)data).presentForTicks)
			{
				if (keyValuePair.Value >= (float)(ritual.DurationTicks / 2))
				{
					num++;
				}
			}
			return (float)((int)Math.Min((float)num, this.curve.Points[this.curve.PointsCount - 1].x));
		}

		// Token: 0x06005D71 RID: 23921 RVA: 0x00200998 File Offset: 0x001FEB98
		public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			List<Thing> forCell = ritualTarget.Map.listerBuldingOfDefInProximity.GetForCell(ritualTarget.Cell, (float)this.maxDistance, ThingDefOf.Loudspeaker, null);
			int num = 0;
			foreach (Thing thing in forCell)
			{
				CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
				if (compPowerTrader != null && compPowerTrader.PowerNet != null && compPowerTrader.PowerNet.HasActivePowerSource)
				{
					num++;
				}
			}
			float quality = this.curve.Evaluate((float)num);
			return new ExpectedOutcomeDesc
			{
				label = "RitualPredictedOutcomeDescNumActiveLoudspeakers".Translate(),
				count = Mathf.Min((float)num, base.MaxValue) + " / " + base.MaxValue,
				effect = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				positive = (num > 0),
				priority = 1f
			};
		}

		// Token: 0x04003602 RID: 13826
		public int maxDistance;
	}
}
