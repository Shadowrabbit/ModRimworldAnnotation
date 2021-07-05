using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CDF RID: 3295
	public class JobGiver_PrisonerEscape : ThinkNode_JobGiver
	{
		// Token: 0x06004BF5 RID: 19445 RVA: 0x001A8188 File Offset: 0x001A6388
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (this.ShouldStartEscaping(pawn) && RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn))
			{
				if (!pawn.guest.Released)
				{
					Messages.Message("MessagePrisonerIsEscaping".Translate(pawn.LabelShort, pawn), pawn, MessageTypeDefOf.ThreatSmall, true);
					Find.TickManager.slower.SignalForceNormalSpeed();
				}
				Job job = JobMaker.MakeJob(JobDefOf.Goto, c);
				job.exitMapOnArrival = true;
				return job;
			}
			return null;
		}

		// Token: 0x06004BF6 RID: 19446 RVA: 0x001A8210 File Offset: 0x001A6410
		private bool ShouldStartEscaping(Pawn pawn)
		{
			if (!pawn.guest.IsPrisoner || pawn.guest.HostFaction != Faction.OfPlayer || !pawn.guest.PrisonerIsSecure)
			{
				return false;
			}
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			if (room.TouchesMapEdge)
			{
				return true;
			}
			bool found = false;
			RegionTraverser.BreadthFirstTraverse(room.Regions[0], (Region from, Region reg) => reg.door == null || reg.door.FreePassage, delegate(Region reg)
			{
				if (reg.Room.TouchesMapEdge)
				{
					found = true;
					return true;
				}
				return false;
			}, 25, RegionType.Set_Passable);
			return found;
		}

		// Token: 0x0400321C RID: 12828
		private const int MaxRegionsToCheckWhenEscapingThroughOpenDoors = 25;
	}
}
