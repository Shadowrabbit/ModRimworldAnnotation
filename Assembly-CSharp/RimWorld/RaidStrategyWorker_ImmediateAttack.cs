﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E22 RID: 3618
	public class RaidStrategyWorker_ImmediateAttack : RaidStrategyWorker
	{
		// Token: 0x06005218 RID: 21016 RVA: 0x001BD720 File Offset: 0x001BB920
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			//事件中心或第一个角色的坐标
			IntVec3 originCell = parms.spawnCenter.IsValid ? parms.spawnCenter : pawns[0].PositionHeld;
			//事件派系与玩家敌对
			if (parms.faction.HostileTo(Faction.OfPlayer))
			{
				//突击殖民者
				return new LordJob_AssaultColony(parms.faction, true, true, false, false, true);
			}
			//非敌对派系 比如动物 尝试在殖民地外寻找随机点突袭
			IntVec3 fallbackLocation;
			RCellFinder.TryFindRandomSpotJustOutsideColony(originCell, map, out fallbackLocation);
			return new LordJob_AssistColony(parms.faction, fallbackLocation);
		}
	}
}
