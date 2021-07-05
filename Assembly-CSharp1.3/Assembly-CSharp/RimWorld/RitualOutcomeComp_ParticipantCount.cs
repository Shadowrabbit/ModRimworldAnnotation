using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F62 RID: 3938
	public class RitualOutcomeComp_ParticipantCount : RitualOutcomeComp_Quality
	{
		// Token: 0x06005D67 RID: 23911 RVA: 0x00200565 File Offset: 0x001FE765
		public override RitualOutcomeComp_Data MakeData()
		{
			return new RitualOutcomeComp_DataThingPresence();
		}

		// Token: 0x06005D68 RID: 23912 RVA: 0x0020056C File Offset: 0x001FE76C
		public override void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
		{
			base.Tick(ritual, data, progressAmount);
			RitualOutcomeComp_DataThingPresence ritualOutcomeComp_DataThingPresence = (RitualOutcomeComp_DataThingPresence)data;
			foreach (Pawn pawn in ritual.PawnsToCountTowardsPresence)
			{
				if (ritual.Ritual != null)
				{
					RitualRole ritualRole = ritual.RoleFor(pawn, true);
					if (ritualRole != null && !ritualRole.countsAsParticipant)
					{
						continue;
					}
				}
				if (GatheringsUtility.InGatheringArea(pawn.Position, ritual.Spot, pawn.MapHeld))
				{
					if (!ritualOutcomeComp_DataThingPresence.presentForTicks.ContainsKey(pawn))
					{
						ritualOutcomeComp_DataThingPresence.presentForTicks.Add(pawn, 0f);
					}
					Dictionary<Thing, float> presentForTicks = ritualOutcomeComp_DataThingPresence.presentForTicks;
					Thing key = pawn;
					presentForTicks[key] += progressAmount;
				}
			}
		}

		// Token: 0x06005D69 RID: 23913 RVA: 0x00200638 File Offset: 0x001FE838
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			int num = 0;
			foreach (KeyValuePair<Thing, float> keyValuePair in ((RitualOutcomeComp_DataThingPresence)data).presentForTicks)
			{
				Pawn p = (Pawn)keyValuePair.Key;
				if (this.Counts(ritual.assignments, p) && keyValuePair.Value >= (float)ritual.DurationTicks / 2f)
				{
					num++;
				}
			}
			return (float)((int)Math.Min((float)num, this.curve.Points[this.curve.PointsCount - 1].x));
		}

		// Token: 0x06005D6A RID: 23914 RVA: 0x002006F0 File Offset: 0x001FE8F0
		private bool Counts(RitualRoleAssignments assignments, Pawn p)
		{
			if (assignments != null && assignments.Ritual == null && assignments.Required(p))
			{
				return false;
			}
			RitualRole ritualRole = (assignments != null) ? assignments.RoleForPawn(p, true) : null;
			return (ritualRole == null || ritualRole.countsAsParticipant) && p.RaceProps.Humanlike;
		}

		// Token: 0x06005D6B RID: 23915 RVA: 0x0020073C File Offset: 0x001FE93C
		public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			int num = assignments.Participants.Count((Pawn p) => this.Counts(assignments, p));
			float quality = this.curve.Evaluate((float)num);
			return new ExpectedOutcomeDesc
			{
				label = "RitualPredictedOutcomeDescParticipantCount".Translate(),
				count = num + " / " + Mathf.Max(base.MaxValue, (float)num),
				effect = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				positive = true,
				priority = 4f
			};
		}
	}
}
