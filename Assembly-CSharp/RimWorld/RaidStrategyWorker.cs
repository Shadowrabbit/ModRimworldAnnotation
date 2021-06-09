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

		// Token: 0x06005869 RID: 22633
		protected abstract LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed);

		// Token: 0x0600586A RID: 22634 RVA: 0x001D0028 File Offset: 0x001CE228
		public virtual void MakeLords(IncidentParms parms, List<Pawn> pawns)
		{
			Map map = (Map)parms.target;
			List<List<Pawn>> list = IncidentParmsUtility.SplitIntoGroups(pawns, parms.pawnGroups);
			int @int = Rand.Int;
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

		// Token: 0x06005871 RID: 22641 RVA: 0x001D012C File Offset: 0x001CE32C
		public virtual List<Pawn> SpawnThreats(IncidentParms parms)
		{
			if (parms.pawnKind != null)
			{
				List<Pawn> list = new List<Pawn>();
				for (int i = 0; i < parms.pawnCount; i++)
				{
					PawnKindDef pawnKind = parms.pawnKind;
					Faction faction = parms.faction;
					PawnGenerationContext context = PawnGenerationContext.NonPlayer;
					int tile = -1;
					bool forceGenerateNewPawn = false;
					bool newborn = false;
					bool allowDead = false;
					bool allowDowned = false;
					bool canGeneratePawnRelations = true;
					bool mustBeCapableOfViolence = true;
					float colonistRelationChanceFactor = 1f;
					bool forceAddFreeWarmLayerIfNeeded = false;
					bool allowGay = true;
					float biocodeWeaponsChance = parms.biocodeWeaponsChance;
					Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind, faction, context, tile, forceGenerateNewPawn, newborn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, this.def.pawnsCanBringFood, true, false, false, false, false, biocodeWeaponsChance, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null)
					{
						BiocodeApparelChance = 1f
					});
					if (pawn != null)
					{
						list.Add(pawn);
					}
				}
				if (list.Any<Pawn>())
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
