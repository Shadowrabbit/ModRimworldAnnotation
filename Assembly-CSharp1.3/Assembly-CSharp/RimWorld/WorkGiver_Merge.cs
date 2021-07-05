using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200084C RID: 2124
	public class WorkGiver_Merge : WorkGiver_Scanner
	{
		// Token: 0x06003831 RID: 14385 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x0013C66E File Offset: 0x0013A86E
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerMergeables.ThingsPotentiallyNeedingMerging();
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x0013C680 File Offset: 0x0013A880
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerMergeables.ThingsPotentiallyNeedingMerging().Count == 0;
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x0013C69C File Offset: 0x0013A89C
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
