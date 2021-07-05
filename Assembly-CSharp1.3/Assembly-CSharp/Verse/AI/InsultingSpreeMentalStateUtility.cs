using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005BF RID: 1471
	public static class InsultingSpreeMentalStateUtility
	{
		// Token: 0x06002AE9 RID: 10985 RVA: 0x00101398 File Offset: 0x000FF598
		public static bool CanChaseAndInsult(Pawn bully, Pawn insulted, bool skipReachabilityCheck = false, bool allowPrisoners = true)
		{
			return insulted.RaceProps.Humanlike && (insulted.Faction == bully.Faction || (allowPrisoners && insulted.HostFaction == bully.Faction)) && insulted != bully && !insulted.Dead && !insulted.Downed && insulted.Spawned && insulted.Awake() && insulted.Position.InHorDistOf(bully.Position, 40f) && InteractionUtility.CanReceiveInteraction(insulted, null) && !insulted.HostileTo(bully) && Find.TickManager.TicksGame - insulted.mindState.lastHarmTick >= 833 && (skipReachabilityCheck || bully.CanReach(insulted, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn));
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x00101464 File Offset: 0x000FF664
		public static void GetInsultCandidatesFor(Pawn bully, List<Pawn> outCandidates, bool allowPrisoners = true)
		{
			outCandidates.Clear();
			Region region = bully.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				return;
			}
			TraverseParms traverseParams = TraverseParms.For(bully, Danger.Deadly, TraverseMode.ByPawn, false, false, false);
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region to) => to.Allows(traverseParams, false), delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = (Pawn)list[i];
					if (InsultingSpreeMentalStateUtility.CanChaseAndInsult(bully, pawn, true, allowPrisoners))
					{
						outCandidates.Add(pawn);
					}
				}
				return false;
			}, 40, RegionType.Set_Passable);
		}

		// Token: 0x04001A56 RID: 6742
		private const int MaxRegionsToSearch = 40;

		// Token: 0x04001A57 RID: 6743
		public const int MaxDistance = 40;

		// Token: 0x04001A58 RID: 6744
		public const int MinTicksBetweenInsults = 1200;
	}
}
