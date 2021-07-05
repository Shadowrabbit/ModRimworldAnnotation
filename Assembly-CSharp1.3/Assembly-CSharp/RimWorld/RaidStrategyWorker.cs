using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000AAE RID: 2734
	public abstract class RaidStrategyWorker
	{
		// Token: 0x060040EC RID: 16620 RVA: 0x0015E524 File Offset: 0x0015C724
		public virtual float SelectionWeight(Map map, float basePoints)
		{
			return this.def.selectionWeightPerPointsCurve.Evaluate(basePoints);
		}

		// Token: 0x060040ED RID: 16621
		protected abstract LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed);

		// Token: 0x060040EE RID: 16622 RVA: 0x0015E538 File Offset: 0x0015C738
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
					}));
				}
			}
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x0015E638 File Offset: 0x0015C838
		public virtual bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return parms.points >= this.MinimumPoints(parms.faction, groupKind);
		}

		// Token: 0x060040F0 RID: 16624 RVA: 0x0015E652 File Offset: 0x0015C852
		public virtual float MinimumPoints(Faction faction, PawnGroupKindDef groupKind)
		{
			return faction.def.MinPointsToGeneratePawnGroup(groupKind, null);
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float MinMaxAllowedPawnGenOptionCost(Faction faction, PawnGroupKindDef groupKind)
		{
			return 0f;
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanUsePawnGenOption(float pointsTotal, PawnGenOption g, List<PawnGenOption> chosenGroups)
		{
			return true;
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanUsePawn(float pointsTotal, Pawn p, List<Pawn> otherPawns)
		{
			return true;
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void TryGenerateThreats(IncidentParms parms)
		{
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x0015E664 File Offset: 0x0015C864
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
					float biocodeApparelChance = parms.biocodeApparelChance;
					Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind, faction, context, tile, forceGenerateNewPawn, newborn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, this.def.pawnsCanBringFood, true, false, false, false, false, biocodeWeaponsChance, biocodeApparelChance, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false)
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

		// Token: 0x04002648 RID: 9800
		public RaidStrategyDef def;
	}
}
