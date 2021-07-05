using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D00 RID: 3328
	public class JoyGiver_Ingest : JoyGiver
	{
		// Token: 0x06004C6F RID: 19567 RVA: 0x00036446 File Offset: 0x00034646
		public override Job TryGiveJob(Pawn pawn)
		{
			return this.TryGiveJobInternal(pawn, null);
		}

		// Token: 0x06004C70 RID: 19568 RVA: 0x001AA0DC File Offset: 0x001A82DC
		public override Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatheringSpot)
		{
			return this.TryGiveJobInternal(pawn, (Thing x) => !x.Spawned || GatheringsUtility.InGatheringArea(x.Position, gatheringSpot, pawn.Map));
		}

		// Token: 0x06004C71 RID: 19569 RVA: 0x001AA118 File Offset: 0x001A8318
		private Job TryGiveJobInternal(Pawn pawn, Predicate<Thing> extraValidator)
		{
			Thing thing = this.BestIngestItem(pawn, extraValidator);
			if (thing != null)
			{
				return this.CreateIngestJob(thing, pawn);
			}
			return null;
		}

		// Token: 0x06004C72 RID: 19570 RVA: 0x001AA13C File Offset: 0x001A833C
		protected virtual Thing BestIngestItem(Pawn pawn, Predicate<Thing> extraValidator)
		{
			Predicate<Thing> predicate = (Thing t) => this.CanIngestForJoy(pawn, t) && (extraValidator == null || extraValidator(t));
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (this.SearchSetWouldInclude(innerContainer[i]) && predicate(innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			JoyGiver_Ingest.tmpCandidates.Clear();
			this.GetSearchSet(pawn, JoyGiver_Ingest.tmpCandidates);
			if (JoyGiver_Ingest.tmpCandidates.Count == 0)
			{
				return null;
			}
			Thing result = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, JoyGiver_Ingest.tmpCandidates, PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, predicate, null);
			JoyGiver_Ingest.tmpCandidates.Clear();
			return result;
		}

		// Token: 0x06004C73 RID: 19571 RVA: 0x001AA224 File Offset: 0x001A8424
		protected virtual bool CanIngestForJoy(Pawn pawn, Thing t)
		{
			if (!t.def.IsIngestible || t.def.ingestible.joyKind == null || t.def.ingestible.joy <= 0f || !pawn.WillEat(t, null, true))
			{
				return false;
			}
			if (t.Spawned)
			{
				if (!pawn.CanReserve(t, 1, -1, null, false))
				{
					return false;
				}
				if (t.IsForbidden(pawn))
				{
					return false;
				}
				if (!t.IsSociallyProper(pawn))
				{
					return false;
				}
				if (!t.IsPoliticallyProper(pawn))
				{
					return false;
				}
			}
			return !t.def.IsDrug || pawn.drugs == null || pawn.drugs.CurrentPolicy[t.def].allowedForJoy || pawn.story == null || pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) > 0 || pawn.InMentalState;
		}

		// Token: 0x06004C74 RID: 19572 RVA: 0x00036450 File Offset: 0x00034650
		protected virtual bool SearchSetWouldInclude(Thing thing)
		{
			return this.def.thingDefs != null && this.def.thingDefs.Contains(thing.def);
		}

		// Token: 0x06004C75 RID: 19573 RVA: 0x00036477 File Offset: 0x00034677
		protected virtual Job CreateIngestJob(Thing ingestible, Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, ingestible);
			job.count = Mathf.Min(ingestible.stackCount, ingestible.def.ingestible.maxNumToIngestAtOnce);
			return job;
		}

		// Token: 0x04003254 RID: 12884
		private static List<Thing> tmpCandidates = new List<Thing>();
	}
}
