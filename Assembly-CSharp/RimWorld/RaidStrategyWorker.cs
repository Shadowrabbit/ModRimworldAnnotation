using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000FCC RID: 4044
	public abstract class RaidStrategyWorker
	{
		// Token: 0x06005868 RID: 22632 RVA: 0x0003D72F File Offset: 0x0003B92F
		public virtual float SelectionWeight(Map map, float basePoints)
		{
			return this.def.selectionWeightPerPointsCurve.Evaluate(basePoints);
		}

		/// <summary>
		/// 集群AI的行为
		/// </summary>
		/// <param name="parms"></param>
		/// <param name="map"></param>
		/// <param name="pawns"></param>
		/// <param name="raidSeed"></param>
		/// <returns></returns>
		protected abstract LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed);

		/// <summary>
		/// 创建集群AI
		/// </summary>
		/// <param name="parms"></param>
		/// <param name="pawns"></param>
		public virtual void MakeLords(IncidentParms parms, List<Pawn> pawns)
		{
			Map map = (Map)parms.target;
			//分组
			List<List<Pawn>> list = IncidentParmsUtility.SplitIntoGroups(pawns, parms.pawnGroups);
			int @int = Rand.Int;
			//每一组生成一个集群AI
			for (int i = 0; i < list.Count; i++)
			{
				List<Pawn> list2 = list[i];
				Lord lord = LordMaker.MakeNewLord(parms.faction, this.MakeLordJob(parms, map, list2, @int), map, list2);
				lord.inSignalLeave = parms.inSignalEnd;
				QuestUtility.AddQuestTag(lord, parms.questTag);
				if (DebugViewSettings.drawStealDebug && parms.faction.HostileTo(Faction.OfPlayer))
				{
					Log.Message(string.Concat(new object[]
					{
						"Market value threshold to start stealing (raiders=",
						lord.ownedPawns.Count,
						"): ",
						StealAIUtility.StartStealingMarketValueThreshold(lord),
						" (colony wealth=",
						map.wealthWatcher.WealthTotal,
						")"
					}), false);
				}
			}
		}

		// Token: 0x0600586B RID: 22635 RVA: 0x0003D742 File Offset: 0x0003B942
		public virtual bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return parms.points >= this.MinimumPoints(parms.faction, groupKind);
		}

		// Token: 0x0600586C RID: 22636 RVA: 0x0003D75C File Offset: 0x0003B95C
		public virtual float MinimumPoints(Faction faction, PawnGroupKindDef groupKind)
		{
			return faction.def.MinPointsToGeneratePawnGroup(groupKind);
		}

		// Token: 0x0600586D RID: 22637 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float MinMaxAllowedPawnGenOptionCost(Faction faction, PawnGroupKindDef groupKind)
		{
			return 0f;
		}

		// Token: 0x0600586E RID: 22638 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool CanUsePawnGenOption(PawnGenOption g, List<PawnGenOption> chosenGroups)
		{
			return true;
		}

		// Token: 0x0600586F RID: 22639 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool CanUsePawn(Pawn p, List<Pawn> otherPawns)
		{
			return true;
		}

		// Token: 0x06005870 RID: 22640 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void TryGenerateThreats(IncidentParms parms)
		{
		}

		/// <summary>
		/// 生成威胁
		/// </summary>
		/// <param name="parms"></param>
		/// <returns></returns>
		public virtual List<Pawn> SpawnThreats(IncidentParms parms)
		{
			if (parms.pawnKind != null)
			{
				List<Pawn> list = new List<Pawn>();
				for (int i = 0; i < parms.pawnCount; i++)
				{
					PawnKindDef pawnKind = parms.pawnKind;
					var faction = parms.faction;
					var context = PawnGenerationContext.NonPlayer;
					var tile = -1;
					var forceGenerateNewPawn = false; //是否强制生成新角色
					var newborn = false; //是否新出生
					var allowDead = false; //是否允许死亡角色
					var allowDowned = false; //是否允许倒地角色
					var canGeneratePawnRelations = true; //是否可以生成角色关系
					var mustBeCapableOfViolence = true; //是否必须具备暴力能力
					var colonistRelationChanceFactor = 1f; //殖民者关系因数
					var forceAddFreeWarmLayerIfNeeded = false; //
					var allowGay = true; //是否允许同性恋
					var biocodeWeaponsChance = parms.biocodeWeaponsChance; //生物代码武器几率
					var pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind, faction, context, tile, forceGenerateNewPawn, newborn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, this.def.pawnsCanBringFood, true, false, false, false, false, biocodeWeaponsChance, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null)
					{
						//生物服装概率
						BiocodeApparelChance = 1f
					});
					if (pawn != null)
					{
						list.Add(pawn);
					}
				}
				//列表存在 设置到达
				if (list.Any())
				{
					parms.raidArrivalMode.Worker.Arrive(list, parms);
					return list;
				}
			}
			return null;
		}

		// Token: 0x04003A86 RID: 14982
		public RaidStrategyDef def;
	}
}
