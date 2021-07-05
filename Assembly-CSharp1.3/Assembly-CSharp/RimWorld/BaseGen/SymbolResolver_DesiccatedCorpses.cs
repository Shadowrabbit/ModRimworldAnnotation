using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015D9 RID: 5593
	public class SymbolResolver_DesiccatedCorpses : SymbolResolver
	{
		// Token: 0x06008379 RID: 33657 RVA: 0x002EEE50 File Offset: 0x002ED050
		public override void Resolve(ResolveParams rp)
		{
			if (rp.desiccatedCorpsePawnKind != null && !rp.desiccatedCorpsePawnKind.RaceProps.IsFlesh)
			{
				Log.Error("Cannot create desiccated corpses for non-flesh based pawns.");
				return;
			}
			Map map = BaseGen.globalSettings.map;
			IntRange? desiccatedCorpseRandomAgeRange = rp.desiccatedCorpseRandomAgeRange;
			if (desiccatedCorpseRandomAgeRange == null)
			{
				IntRange defaultCorpseAge = SymbolResolver_DesiccatedCorpses.DefaultCorpseAge;
			}
			else
			{
				desiccatedCorpseRandomAgeRange.GetValueOrDefault();
			}
			SymbolResolver_DesiccatedCorpses.tmpSeenRooms.Clear();
			foreach (IntVec3 loc in rp.rect)
			{
				Room room = loc.GetRoom(map);
				if (room != null && !SymbolResolver_DesiccatedCorpses.tmpSeenRooms.Contains(room) && !room.PsychologicallyOutdoors && room.CellCount < 1000 && Rand.Chance(0.04f))
				{
					int cellCount = room.CellCount;
					float chance = SymbolResolver_DesiccatedCorpses.CorpseSpawnChanceOverRoomSizeCurve.Evaluate((float)cellCount);
					foreach (IntVec3 intVec in room.Cells)
					{
						if (Rand.Chance(chance) && SymbolResolver_DesiccatedCorpses.CanSpawnAt(intVec, map))
						{
							PawnKindDef pawnKindDef = rp.desiccatedCorpsePawnKind ?? this.GetRandomPawnKindForCorpse();
							this.SpawnCorpse(pawnKindDef, intVec, SymbolResolver_DesiccatedCorpses.DefaultCorpseAge.RandomInRange, map);
						}
					}
				}
				SymbolResolver_DesiccatedCorpses.tmpSeenRooms.Add(room);
			}
			SymbolResolver_DesiccatedCorpses.tmpSeenRooms.Clear();
			int num = Mathf.Max(1, Mathf.RoundToInt((rp.desiccatedCorpseDensityRange ?? SymbolResolver_DesiccatedCorpses.DefaultCorpseDensity).RandomInRange * (float)rp.rect.Area));
			for (int i = 0; i < num; i++)
			{
				IntVec3 spawnPosition = SymbolResolver_DesiccatedCorpses.FindRandomCorpseSpawnPosition(rp.rect, map);
				if (spawnPosition.IsValid)
				{
					PawnKindDef pawnKindDef2 = rp.desiccatedCorpsePawnKind ?? this.GetRandomPawnKindForCorpse();
					this.SpawnCorpse(pawnKindDef2, spawnPosition, SymbolResolver_DesiccatedCorpses.DefaultCorpseAge.RandomInRange, map);
				}
			}
		}

		// Token: 0x0600837A RID: 33658 RVA: 0x002EF080 File Offset: 0x002ED280
		private void SpawnCorpse(PawnKindDef pawnKindDef, IntVec3 spawnPosition, int age, Map map)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKindDef, null, PawnGenerationContext.NonPlayer, -1, false, false, true, false, false, false, 0f, false, true, false, true, false, false, false, false, 0f, 0f, null, 0f, null, null, null, null, null, null, null, null, null, null, null, null, null, true, true));
			if (!pawn.Dead)
			{
				pawn.Kill(null, null);
			}
			if (pawn.inventory != null)
			{
				pawn.inventory.DestroyAll(DestroyMode.Vanish);
			}
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
			if (pawn.equipment != null)
			{
				pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
			}
			pawn.Corpse.Age = age + Rand.Range(0, 900000);
			pawn.relations.hidePawnRelations = true;
			GenSpawn.Spawn(pawn.Corpse, spawnPosition, map, WipeMode.Vanish);
			pawn.Corpse.GetComp<CompRottable>().RotProgress += (float)pawn.Corpse.Age;
		}

		// Token: 0x0600837B RID: 33659 RVA: 0x002EF1A1 File Offset: 0x002ED3A1
		private PawnKindDef GetRandomPawnKindForCorpse()
		{
			return (from pk in DefDatabase<PawnKindDef>.AllDefs
			where pk.RaceProps.IsFlesh
			select pk).RandomElement<PawnKindDef>();
		}

		// Token: 0x0600837C RID: 33660 RVA: 0x002EF1D4 File Offset: 0x002ED3D4
		private static IntVec3 FindRandomCorpseSpawnPosition(CellRect rect, Map map)
		{
			foreach (IntVec3 intVec in rect.Cells.InRandomOrder(null))
			{
				if (SymbolResolver_DesiccatedCorpses.CanSpawnAt(intVec, map))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x0600837D RID: 33661 RVA: 0x002EF238 File Offset: 0x002ED438
		private static bool CanSpawnAt(IntVec3 cell, Map map)
		{
			return !cell.Impassable(map) && cell.GetThingList(map).Count == 0;
		}

		// Token: 0x0400521D RID: 21021
		private static FloatRange DefaultCorpseDensity = new FloatRange(0.001f, 0.002f);

		// Token: 0x0400521E RID: 21022
		private static IntRange DefaultCorpseAge = new IntRange(180000000, 720000000);

		// Token: 0x0400521F RID: 21023
		private const float ChanceToFillRoom = 0.04f;

		// Token: 0x04005220 RID: 21024
		private const int MaxRoomSizeToFill = 1000;

		// Token: 0x04005221 RID: 21025
		private static readonly SimpleCurve CorpseSpawnChanceOverRoomSizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.25f),
				true
			},
			{
				new CurvePoint(100f, 0.25f),
				true
			},
			{
				new CurvePoint(400f, 0.15f),
				true
			}
		};

		// Token: 0x04005222 RID: 21026
		private static HashSet<Room> tmpSeenRooms = new HashSet<Room>();
	}
}
