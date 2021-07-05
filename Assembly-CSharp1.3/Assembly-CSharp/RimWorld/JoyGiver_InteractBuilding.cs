using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D6 RID: 2006
	public abstract class JoyGiver_InteractBuilding : JoyGiver
	{
		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x060035E7 RID: 13799 RVA: 0x0001276E File Offset: 0x0001096E
		protected virtual bool CanDoDuringGathering
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x0013149C File Offset: 0x0012F69C
		public override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = this.FindBestGame(pawn, false, IntVec3.Invalid);
			if (thing != null)
			{
				return this.TryGivePlayJob(pawn, thing);
			}
			return null;
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x001314C4 File Offset: 0x0012F6C4
		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			Thing thing = this.FindBestGame(pawn, true, IntVec3.Invalid);
			if (thing != null)
			{
				return this.TryGivePlayJobWhileInBed(pawn, thing);
			}
			return null;
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x001314EC File Offset: 0x0012F6EC
		public override Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatheringSpot)
		{
			if (!this.CanDoDuringGathering)
			{
				return null;
			}
			Thing thing = this.FindBestGame(pawn, false, gatheringSpot);
			if (thing != null)
			{
				return this.TryGivePlayJob(pawn, thing);
			}
			return null;
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x0013151C File Offset: 0x0012F71C
		private Thing FindBestGame(Pawn pawn, bool inBed, IntVec3 gatheringSpot)
		{
			JoyGiver_InteractBuilding.tmpCandidates.Clear();
			this.GetSearchSet(pawn, JoyGiver_InteractBuilding.tmpCandidates);
			if (JoyGiver_InteractBuilding.tmpCandidates.Count == 0)
			{
				return null;
			}
			Predicate<Thing> predicate = (Thing t) => this.CanInteractWith(pawn, t, inBed);
			if (gatheringSpot.IsValid)
			{
				Predicate<Thing> oldValidator = predicate;
				predicate = ((Thing x) => GatheringsUtility.InGatheringArea(x.Position, gatheringSpot, pawn.Map) && oldValidator(x));
			}
			Thing result = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, JoyGiver_InteractBuilding.tmpCandidates, PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, predicate, null);
			JoyGiver_InteractBuilding.tmpCandidates.Clear();
			return result;
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x001315F0 File Offset: 0x0012F7F0
		protected virtual bool CanInteractWith(Pawn pawn, Thing t, bool inBed)
		{
			if (!pawn.CanReserve(t, this.def.jobDef.joyMaxParticipants, -1, null, false))
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
			CompPowerTrader compPowerTrader = t.TryGetComp<CompPowerTrader>();
			return (compPowerTrader == null || compPowerTrader.PowerOn) && (!this.def.unroofedOnly || !t.Position.Roofed(t.Map));
		}

		// Token: 0x060035ED RID: 13805
		protected abstract Job TryGivePlayJob(Pawn pawn, Thing bestGame);

		// Token: 0x060035EE RID: 13806 RVA: 0x00131678 File Offset: 0x0012F878
		protected virtual Job TryGivePlayJobWhileInBed(Pawn pawn, Thing bestGame)
		{
			Building_Bed t = pawn.CurrentBed();
			return JobMaker.MakeJob(this.def.jobDef, bestGame, pawn.Position, t);
		}

		// Token: 0x04001EC8 RID: 7880
		private static List<Thing> tmpCandidates = new List<Thing>();
	}
}
