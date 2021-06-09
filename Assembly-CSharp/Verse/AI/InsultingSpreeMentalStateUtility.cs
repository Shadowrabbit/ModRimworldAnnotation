using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A13 RID: 2579
	public static class InsultingSpreeMentalStateUtility
	{
		// Token: 0x06003D9F RID: 15775 RVA: 0x00175F6C File Offset: 0x0017416C
		public static bool CanChaseAndInsult(Pawn bully, Pawn insulted, bool skipReachabilityCheck = false, bool allowPrisoners = true)
		{
			return insulted.RaceProps.Humanlike && (insulted.Faction == bully.Faction || (allowPrisoners && insulted.HostFaction == bully.Faction)) && insulted != bully && !insulted.Dead && !insulted.Downed && insulted.Spawned && insulted.Awake() && insulted.Position.InHorDistOf(bully.Position, 40f) && InteractionUtility.CanReceiveInteraction(insulted, null) && !insulted.HostileTo(bully) && Find.TickManager.TicksGame - insulted.mindState.lastHarmTick >= 833 && (skipReachabilityCheck || bully.CanReach(insulted, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn));
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x00176038 File Offset: 0x00174238
		public static void GetInsultCandidatesFor(Pawn bully, List<Pawn> outCandidates, bool allowPrisoners = true)
		{
			outCandidates.Clear();
			Region region = bully.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				return;
			}
			TraverseParms traverseParams = TraverseParms.For(bully, Danger.Deadly, TraverseMode.ByPawn, false);
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

		// Token: 0x04002AB4 RID: 10932
		private const int MaxRegionsToSearch = 40;

		// Token: 0x04002AB5 RID: 10933
		public const int MaxDistance = 40;

		// Token: 0x04002AB6 RID: 10934
		public const int MinTicksBetweenInsults = 1200;
	}
}
