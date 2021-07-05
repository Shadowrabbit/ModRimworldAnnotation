using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000BF3 RID: 3059
	public abstract class IncidentWorker_Ambush : IncidentWorker
	{
		// Token: 0x06004800 RID: 18432
		protected abstract List<Pawn> GeneratePawns(IncidentParms parms);

		// Token: 0x06004801 RID: 18433 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x00002688 File Offset: 0x00000888
		protected virtual LordJob CreateLordJob(List<Pawn> generatedPawns, IncidentParms parms)
		{
			return null;
		}

		// Token: 0x06004803 RID: 18435 RVA: 0x0017C454 File Offset: 0x0017A654
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = parms.target as Map;
			if (map != null)
			{
				IntVec3 intVec;
				return this.TryFindEntryCell(map, out intVec);
			}
			return CaravanIncidentUtility.CanFireIncidentWhichWantsToGenerateMapAt(parms.target.Tile);
		}

		// Token: 0x06004804 RID: 18436 RVA: 0x0017C48C File Offset: 0x0017A68C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = parms.target as Map;
			IntVec3 existingMapEdgeCell = IntVec3.Invalid;
			if (map != null && !this.TryFindEntryCell(map, out existingMapEdgeCell))
			{
				return false;
			}
			List<Pawn> generatedEnemies = this.GeneratePawns(parms);
			if (!generatedEnemies.Any<Pawn>())
			{
				return false;
			}
			if (map != null)
			{
				return this.DoExecute(parms, generatedEnemies, existingMapEdgeCell);
			}
			LongEventHandler.QueueLongEvent(delegate()
			{
				this.DoExecute(parms, generatedEnemies, existingMapEdgeCell);
			}, "GeneratingMapForNewEncounter", false, null, true);
			return true;
		}

		// Token: 0x06004805 RID: 18437 RVA: 0x0017C534 File Offset: 0x0017A734
		private bool DoExecute(IncidentParms parms, List<Pawn> generatedEnemies, IntVec3 existingMapEdgeCell)
		{
			Map map = parms.target as Map;
			bool flag = false;
			if (map == null)
			{
				map = CaravanIncidentUtility.SetupCaravanAttackMap((Caravan)parms.target, generatedEnemies, false);
				flag = true;
			}
			else
			{
				for (int i = 0; i < generatedEnemies.Count; i++)
				{
					IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(existingMapEdgeCell, map, 4);
					GenSpawn.Spawn(generatedEnemies[i], loc, map, Rot4.Random, WipeMode.Vanish, false);
				}
			}
			this.PostProcessGeneratedPawnsAfterSpawning(generatedEnemies);
			LordJob lordJob = this.CreateLordJob(generatedEnemies, parms);
			if (lordJob != null)
			{
				LordMaker.MakeNewLord(parms.faction, lordJob, map, generatedEnemies);
			}
			TaggedString baseLetterLabel = this.GetLetterLabel(generatedEnemies[0], parms);
			TaggedString baseLetterText = this.GetLetterText(generatedEnemies[0], parms);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(generatedEnemies, ref baseLetterLabel, ref baseLetterText, this.GetRelatedPawnsInfoLetterText(parms), true, true);
			base.SendStandardLetter(baseLetterLabel, baseLetterText, this.GetLetterDef(generatedEnemies[0], parms), parms, generatedEnemies[0], Array.Empty<NamedArgument>());
			if (flag)
			{
				Find.TickManager.Notify_GeneratedPotentiallyHostileMap();
			}
			return true;
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x0017C638 File Offset: 0x0017A838
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && map.reachability.CanReachColony(x), map, CellFinder.EdgeRoadChance_Hostile, out cell);
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x0017C66F File Offset: 0x0017A86F
		protected virtual string GetLetterLabel(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterLabel;
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x0017C67C File Offset: 0x0017A87C
		protected virtual string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterText;
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x0017C689 File Offset: 0x0017A889
		protected virtual LetterDef GetLetterDef(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterDef;
		}

		// Token: 0x0600480A RID: 18442 RVA: 0x0017C696 File Offset: 0x0017A896
		protected virtual string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsGroupGeneric".Translate(Faction.OfPlayer.def.pawnsPlural);
		}
	}
}
