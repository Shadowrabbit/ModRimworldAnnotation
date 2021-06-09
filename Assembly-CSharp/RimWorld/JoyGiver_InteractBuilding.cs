using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CED RID: 3309
	public abstract class JoyGiver_InteractBuilding : JoyGiver
	{
		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x06004C1D RID: 19485 RVA: 0x0000A2E4 File Offset: 0x000084E4
		protected virtual bool CanDoDuringGathering
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004C1E RID: 19486 RVA: 0x001A8B84 File Offset: 0x001A6D84
		public override Job TryGiveJob(Pawn pawn)
		{
			Thing thing = this.FindBestGame(pawn, false, IntVec3.Invalid);
			if (thing != null)
			{
				return this.TryGivePlayJob(pawn, thing);
			}
			return null;
		}

		// Token: 0x06004C1F RID: 19487 RVA: 0x001A8BAC File Offset: 0x001A6DAC
		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			Thing thing = this.FindBestGame(pawn, true, IntVec3.Invalid);
			if (thing != null)
			{
				return this.TryGivePlayJobWhileInBed(pawn, thing);
			}
			return null;
		}

		// Token: 0x06004C20 RID: 19488 RVA: 0x001A8BD4 File Offset: 0x001A6DD4
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

		// Token: 0x06004C21 RID: 19489 RVA: 0x001A8C04 File Offset: 0x001A6E04
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
			Thing result = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, JoyGiver_InteractBuilding.tmpCandidates, PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, predicate, null);
			JoyGiver_InteractBuilding.tmpCandidates.Clear();
			return result;
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x001A8CD8 File Offset: 0x001A6ED8
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

		// Token: 0x06004C23 RID: 19491
		protected abstract Job TryGivePlayJob(Pawn pawn, Thing bestGame);

		// Token: 0x06004C24 RID: 19492 RVA: 0x001A8D60 File Offset: 0x001A6F60
		protected virtual Job TryGivePlayJobWhileInBed(Pawn pawn, Thing bestGame)
		{
			Building_Bed t = pawn.CurrentBed();
			return JobMaker.MakeJob(this.def.jobDef, bestGame, pawn.Position, t);
		}

		// Token: 0x0400322A RID: 12842
		private static List<Thing> tmpCandidates = new List<Thing>();
	}
}
