using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000769 RID: 1897
	public static class SelfDefenseUtility
	{
		// Token: 0x06003460 RID: 13408 RVA: 0x00128FB8 File Offset: 0x001271B8
		public static bool ShouldStartFleeing(Pawn pawn)
		{
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.AlwaysFlee);
			for (int i = 0; i < list.Count; i++)
			{
				if (SelfDefenseUtility.ShouldFleeFrom(list[i], pawn, true, false))
				{
					return true;
				}
			}
			bool foundThreat = false;
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				return false;
			}
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region reg) => reg.door == null || reg.door.Open, delegate(Region reg)
			{
				List<Thing> list2 = reg.ListerThings.ThingsInGroup(ThingRequestGroup.AttackTarget);
				for (int j = 0; j < list2.Count; j++)
				{
					if (SelfDefenseUtility.ShouldFleeFrom(list2[j], pawn, true, true))
					{
						foundThreat = true;
						break;
					}
				}
				return foundThreat;
			}, 9, RegionType.Set_Passable);
			return foundThreat;
		}

		// Token: 0x06003461 RID: 13409 RVA: 0x00129068 File Offset: 0x00127268
		public static bool ShouldFleeFrom(Thing t, Pawn pawn, bool checkDistance, bool checkLOS)
		{
			if (t == pawn || (checkDistance && !t.Position.InHorDistOf(pawn.Position, 8f)))
			{
				return false;
			}
			if (t.def.alwaysFlee)
			{
				return true;
			}
			if (!t.HostileTo(pawn))
			{
				return false;
			}
			IAttackTarget attackTarget = t as IAttackTarget;
			return attackTarget != null && !attackTarget.ThreatDisabled(pawn) && t is IAttackTargetSearcher && (!checkLOS || GenSight.LineOfSight(pawn.Position, t.Position, pawn.Map, false, null, 0, 0));
		}

		// Token: 0x04001E46 RID: 7750
		public const float FleeWhenDistToHostileLessThan = 8f;
	}
}
