using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200118F RID: 4495
	public abstract class IncidentWorker_Ambush : IncidentWorker
	{
		// Token: 0x06006315 RID: 25365
		protected abstract List<Pawn> GeneratePawns(IncidentParms parms);

		// Token: 0x06006316 RID: 25366 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void PostProcessGeneratedPawnsAfterSpawning(List<Pawn> generatedPawns)
		{
		}

		// Token: 0x06006317 RID: 25367 RVA: 0x0000C32E File Offset: 0x0000A52E
		protected virtual LordJob CreateLordJob(List<Pawn> generatedPawns, IncidentParms parms)
		{
			return null;
		}

		// Token: 0x06006318 RID: 25368 RVA: 0x001EDE1C File Offset: 0x001EC01C
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

		// Token: 0x06006319 RID: 25369 RVA: 0x001EDE54 File Offset: 0x001EC054
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

		// Token: 0x0600631A RID: 25370 RVA: 0x001EDEFC File Offset: 0x001EC0FC
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

		// Token: 0x0600631B RID: 25371 RVA: 0x001EE000 File Offset: 0x001EC200
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && map.reachability.CanReachColony(x), map, CellFinder.EdgeRoadChance_Hostile, out cell);
		}

		// Token: 0x0600631C RID: 25372 RVA: 0x00044297 File Offset: 0x00042497
		protected virtual string GetLetterLabel(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterLabel;
		}

		// Token: 0x0600631D RID: 25373 RVA: 0x000442A4 File Offset: 0x000424A4
		protected virtual string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterText;
		}

		// Token: 0x0600631E RID: 25374 RVA: 0x000442B1 File Offset: 0x000424B1
		protected virtual LetterDef GetLetterDef(Pawn anyPawn, IncidentParms parms)
		{
			return this.def.letterDef;
		}

		// Token: 0x0600631F RID: 25375 RVA: 0x000442BE File Offset: 0x000424BE
		protected virtual string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsGroupGeneric".Translate(Faction.OfPlayer.def.pawnsPlural);
		}
	}
}
