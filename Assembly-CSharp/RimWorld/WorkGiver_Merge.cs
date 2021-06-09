using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D8A RID: 3466
	public class WorkGiver_Merge : WorkGiver_Scanner
	{
		// Token: 0x06004F08 RID: 20232 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06004F09 RID: 20233 RVA: 0x00037A38 File Offset: 0x00035C38
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerMergeables.ThingsPotentiallyNeedingMerging();
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x00037A4A File Offset: 0x00035C4A
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerMergeables.ThingsPotentiallyNeedingMerging().Count == 0;
		}

		// Token: 0x06004F0B RID: 20235 RVA: 0x001B3D0C File Offset: 0x001B1F0C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.stackCount == t.def.stackLimit)
			{
				return null;
			}
			if (!HaulAIUtility.PawnCanAutomaticallyHaul(pawn, t, forced))
			{
				return null;
			}
			SlotGroup slotGroup = t.GetSlotGroup();
			if (slotGroup == null)
			{
				return null;
			}
			if (!pawn.CanReserve(t.Position, 1, -1, null, forced))
			{
				return null;
			}
			foreach (Thing thing in slotGroup.HeldThings)
			{
				if (thing != t && thing.CanStackWith(t) && (forced || thing.stackCount >= t.stackCount) && thing.stackCount < thing.def.stackLimit && pawn.CanReserve(thing.Position, 1, -1, null, forced) && pawn.CanReserve(thing, 1, -1, null, false) && thing.Position.IsValidStorageFor(thing.Map, t))
				{
					Job job = JobMaker.MakeJob(JobDefOf.HaulToCell, t, thing.Position);
					job.count = Mathf.Min(thing.def.stackLimit - thing.stackCount, t.stackCount);
					job.haulMode = HaulMode.ToCellStorage;
					return job;
				}
			}
			JobFailReason.Is("NoMergeTarget".Translate(), null);
			return null;
		}
	}
}
