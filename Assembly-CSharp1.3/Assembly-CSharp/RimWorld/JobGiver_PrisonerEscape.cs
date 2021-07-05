using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007CB RID: 1995
	public class JobGiver_PrisonerEscape : ThinkNode_JobGiver
	{
		// Token: 0x060035C4 RID: 13764 RVA: 0x00130794 File Offset: 0x0012E994
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

		// Token: 0x060035C5 RID: 13765 RVA: 0x0013081C File Offset: 0x0012EA1C
		private bool ShouldStartEscaping(Pawn pawn)
		{
			if (!pawn.guest.IsPrisoner || pawn.guest.HostFaction != Faction.OfPlayer || !pawn.guest.PrisonerIsSecure)
			{
				return false;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && lord.LordJob is LordJob_Ritual)
			{
				return false;
			}
			District district = pawn.GetDistrict(RegionType.Set_Passable);
			if (district.TouchesMapEdge)
			{
				return true;
			}
			bool found = false;
			RegionTraverser.BreadthFirstTraverse(district.Regions[0], (Region from, Region reg) => reg.door == null || reg.door.FreePassage, delegate(Region reg)
			{
				if (reg.District.TouchesMapEdge)
				{
					found = true;
					return true;
				}
				return false;
			}, 25, RegionType.Set_Passable);
			return found;
		}

		// Token: 0x04001EC0 RID: 7872
		private const int MaxRegionsToCheckWhenEscapingThroughOpenDoors = 25;
	}
}
