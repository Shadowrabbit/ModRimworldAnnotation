using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007E3 RID: 2019
	public class JoyGiver_Ingest : JoyGiver
	{
		// Token: 0x06003623 RID: 13859 RVA: 0x0013289F File Offset: 0x00130A9F
		public override Job TryGiveJob(Pawn pawn)
		{
			return this.TryGiveJobInternal(pawn, null);
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x001328AC File Offset: 0x00130AAC
		public override Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatheringSpot)
		{
			return this.TryGiveJobInternal(pawn, (Thing x) => !x.Spawned || GatheringsUtility.InGatheringArea(x.Position, gatheringSpot, pawn.Map));
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x001328E8 File Offset: 0x00130AE8
		private Job TryGiveJobInternal(Pawn pawn, Predicate<Thing> extraValidator)
		{
			Thing thing = this.BestIngestItem(pawn, extraValidator);
			if (thing != null)
			{
				return this.CreateIngestJob(thing, pawn);
			}
			return null;
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x0013290C File Offset: 0x00130B0C
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
			Thing result = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, JoyGiver_Ingest.tmpCandidates, PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, predicate, null);
			JoyGiver_Ingest.tmpCandidates.Clear();
			return result;
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x001329F4 File Offset: 0x00130BF4
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

		// Token: 0x06003628 RID: 13864 RVA: 0x00132ADE File Offset: 0x00130CDE
		protected virtual bool SearchSetWouldInclude(Thing thing)
		{
			return this.def.thingDefs != null && this.def.thingDefs.Contains(thing.def);
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x00132B05 File Offset: 0x00130D05
		protected virtual Job CreateIngestJob(Thing ingestible, Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, ingestible);
			job.count = Mathf.Min(ingestible.stackCount, ingestible.def.ingestible.maxNumToIngestAtOnce);
			return job;
		}

		// Token: 0x04001ED8 RID: 7896
		private static List<Thing> tmpCandidates = new List<Thing>();
	}
}
