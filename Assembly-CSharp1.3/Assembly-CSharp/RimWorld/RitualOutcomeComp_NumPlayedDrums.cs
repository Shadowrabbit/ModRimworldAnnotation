using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F64 RID: 3940
	public class RitualOutcomeComp_NumPlayedDrums : RitualOutcomeComp_Quality
	{
		// Token: 0x06005D73 RID: 23923 RVA: 0x00200565 File Offset: 0x001FE765
		public override RitualOutcomeComp_Data MakeData()
		{
			return new RitualOutcomeComp_DataThingPresence();
		}

		// Token: 0x06005D74 RID: 23924 RVA: 0x00200AA8 File Offset: 0x001FECA8
		public override void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
		{
			base.Tick(ritual, data, progressAmount);
			TargetInfo selectedTarget = ritual.selectedTarget;
			if (selectedTarget.ThingDestroyed || !selectedTarget.HasThing)
			{
				return;
			}
			RitualOutcomeComp_DataThingPresence ritualOutcomeComp_DataThingPresence = (RitualOutcomeComp_DataThingPresence)data;
			foreach (Thing thing in selectedTarget.Map.listerBuldingOfDefInProximity.GetForCell(selectedTarget.Cell, (float)this.maxDistance, ThingDefOf.Drum, null))
			{
				Building_MusicalInstrument building_MusicalInstrument = thing as Building_MusicalInstrument;
				if (building_MusicalInstrument != null && thing.GetRoom(RegionType.Set_All) == selectedTarget.Cell.GetRoom(selectedTarget.Map) && building_MusicalInstrument.IsBeingPlayed)
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

		// Token: 0x06005D75 RID: 23925 RVA: 0x00200BB4 File Offset: 0x001FEDB4
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

		// Token: 0x06005D76 RID: 23926 RVA: 0x00200C4C File Offset: 0x001FEE4C
		public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			int count = ritualTarget.Map.listerBuldingOfDefInProximity.GetForCell(ritualTarget.Cell, (float)this.maxDistance, ThingDefOf.Drum, null).Count;
			float quality = this.curve.Evaluate((float)count);
			return new ExpectedOutcomeDesc
			{
				label = "RitualPredictedOutcomeDescNumDrums".Translate(),
				count = Mathf.Min((float)count, base.MaxValue) + " / " + base.MaxValue,
				effect = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				positive = (count > 0),
				priority = 1f
			};
		}

		// Token: 0x04003603 RID: 13827
		public int maxDistance;
	}
}
