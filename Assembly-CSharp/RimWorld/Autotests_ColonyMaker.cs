﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BF7 RID: 7159
	public static class Autotests_ColonyMaker
	{
		// Token: 0x170018BA RID: 6330
		// (get) Token: 0x06009D8F RID: 40335 RVA: 0x0001DA80 File Offset: 0x0001BC80
		private static Map Map
		{
			get
			{
				return Find.CurrentMap;
			}
		}

		// Token: 0x06009D90 RID: 40336 RVA: 0x00068E87 File Offset: 0x00067087
		public static void MakeColony_Full()
		{
			Autotests_ColonyMaker.MakeColony(new ColonyMakerFlag[]
			{
				ColonyMakerFlag.ConduitGrid,
				ColonyMakerFlag.PowerPlants,
				ColonyMakerFlag.Batteries,
				ColonyMakerFlag.WorkTables,
				ColonyMakerFlag.AllBuildings,
				ColonyMakerFlag.AllItems,
				ColonyMakerFlag.Filth,
				ColonyMakerFlag.ColonistsMany,
				ColonyMakerFlag.ColonistsHungry,
				ColonyMakerFlag.ColonistsTired,
				ColonyMakerFlag.ColonistsInjured,
				ColonyMakerFlag.ColonistsDiseased,
				ColonyMakerFlag.Beds,
				ColonyMakerFlag.Stockpiles,
				ColonyMakerFlag.GrowingZones
			});
		}

		// Token: 0x06009D91 RID: 40337 RVA: 0x00068EA0 File Offset: 0x000670A0
		public static void MakeColony_Animals()
		{
			Autotests_ColonyMaker.MakeColony(new ColonyMakerFlag[1]);
		}

		// Token: 0x06009D92 RID: 40338 RVA: 0x002E1510 File Offset: 0x002DF710
		public static void MakeColony(params ColonyMakerFlag[] flags)
		{
			bool godMode = DebugSettings.godMode;
			DebugSettings.godMode = true;
			Thing.allowDestroyNonDestroyable = true;
			if (Autotests_ColonyMaker.usedCells == null)
			{
				Autotests_ColonyMaker.usedCells = new BoolGrid(Autotests_ColonyMaker.Map);
			}
			else
			{
				Autotests_ColonyMaker.usedCells.ClearAndResizeTo(Autotests_ColonyMaker.Map);
			}
			Autotests_ColonyMaker.overRect = new CellRect(Autotests_ColonyMaker.Map.Center.x - 50, Autotests_ColonyMaker.Map.Center.z - 50, 100, 100);
			Autotests_ColonyMaker.DeleteAllSpawnedPawns();
			GenDebug.ClearArea(Autotests_ColonyMaker.overRect, Find.CurrentMap);
			if (flags.Contains(ColonyMakerFlag.Animals))
			{
				foreach (PawnKindDef pawnKindDef in from k in DefDatabase<PawnKindDef>.AllDefs
				where k.RaceProps.Animal
				select k)
				{
					CellRect cellRect;
					if (!Autotests_ColonyMaker.TryGetFreeRect(6, 3, out cellRect))
					{
						return;
					}
					cellRect = cellRect.ContractedBy(1);
					foreach (IntVec3 c in cellRect)
					{
						Autotests_ColonyMaker.Map.terrainGrid.SetTerrain(c, TerrainDefOf.Concrete);
					}
					GenSpawn.Spawn(PawnGenerator.GeneratePawn(pawnKindDef, null), cellRect.Cells.ElementAt(0), Autotests_ColonyMaker.Map, WipeMode.Vanish);
					IntVec3 intVec = cellRect.Cells.ElementAt(1);
					HealthUtility.DamageUntilDead((Pawn)GenSpawn.Spawn(PawnGenerator.GeneratePawn(pawnKindDef, null), intVec, Autotests_ColonyMaker.Map, WipeMode.Vanish));
					CompRottable compRottable = ((Corpse)intVec.GetThingList(Find.CurrentMap).First((Thing t) => t is Corpse)).TryGetComp<CompRottable>();
					if (compRottable != null)
					{
						compRottable.RotProgress += 1200000f;
					}
					if (pawnKindDef.RaceProps.leatherDef != null)
					{
						GenSpawn.Spawn(pawnKindDef.RaceProps.leatherDef, cellRect.Cells.ElementAt(2), Autotests_ColonyMaker.Map, WipeMode.Vanish);
					}
					if (pawnKindDef.RaceProps.meatDef != null)
					{
						GenSpawn.Spawn(pawnKindDef.RaceProps.meatDef, cellRect.Cells.ElementAt(3), Autotests_ColonyMaker.Map, WipeMode.Vanish);
					}
				}
			}
			if (flags.Contains(ColonyMakerFlag.ConduitGrid))
			{
				Designator_Build designator_Build = new Designator_Build(ThingDefOf.PowerConduit);
				for (int i = Autotests_ColonyMaker.overRect.minX; i < Autotests_ColonyMaker.overRect.maxX; i++)
				{
					for (int j = Autotests_ColonyMaker.overRect.minZ; j < Autotests_ColonyMaker.overRect.maxZ; j += 7)
					{
						designator_Build.DesignateSingleCell(new IntVec3(i, 0, j));
					}
				}
				for (int k2 = Autotests_ColonyMaker.overRect.minZ; k2 < Autotests_ColonyMaker.overRect.maxZ; k2++)
				{
					for (int l = Autotests_ColonyMaker.overRect.minX; l < Autotests_ColonyMaker.overRect.maxX; l += 7)
					{
						designator_Build.DesignateSingleCell(new IntVec3(l, 0, k2));
					}
				}
			}
			if (flags.Contains(ColonyMakerFlag.PowerPlants))
			{
				List<ThingDef> list = new List<ThingDef>
				{
					ThingDefOf.SolarGenerator,
					ThingDefOf.WindTurbine
				};
				for (int m = 0; m < 8; m++)
				{
					if (Autotests_ColonyMaker.TryMakeBuilding(list[m % list.Count]) == null)
					{
						Log.Message("Could not make solar generator.", false);
						break;
					}
				}
			}
			if (flags.Contains(ColonyMakerFlag.Batteries))
			{
				for (int n = 0; n < 6; n++)
				{
					Thing thing = Autotests_ColonyMaker.TryMakeBuilding(ThingDefOf.Battery);
					if (thing == null)
					{
						Log.Message("Could not make battery.", false);
						break;
					}
					((Building_Battery)thing).GetComp<CompPowerBattery>().AddEnergy(999999f);
				}
			}
			if (flags.Contains(ColonyMakerFlag.WorkTables))
			{
				foreach (ThingDef thingDef in from def in DefDatabase<ThingDef>.AllDefs
				where typeof(Building_WorkTable).IsAssignableFrom(def.thingClass)
				select def)
				{
					Thing thing2 = Autotests_ColonyMaker.TryMakeBuilding(thingDef);
					if (thing2 == null)
					{
						Log.Message("Could not make worktable: " + thingDef.defName, false);
						break;
					}
					Building_WorkTable building_WorkTable = thing2 as Building_WorkTable;
					if (building_WorkTable != null)
					{
						foreach (RecipeDef recipe in building_WorkTable.def.AllRecipes)
						{
							building_WorkTable.billStack.AddBill(recipe.MakeNewBill());
						}
					}
				}
			}
			if (flags.Contains(ColonyMakerFlag.AllBuildings))
			{
				foreach (ThingDef thingDef2 in from def in DefDatabase<ThingDef>.AllDefs
				where def.category == ThingCategory.Building && def.BuildableByPlayer
				select def)
				{
					if (thingDef2 != ThingDefOf.PowerConduit && Autotests_ColonyMaker.TryMakeBuilding(thingDef2) == null)
					{
						Log.Message("Could not make building: " + thingDef2.defName, false);
						break;
					}
				}
			}
			CellRect rect;
			if (!Autotests_ColonyMaker.TryGetFreeRect(33, 33, out rect))
			{
				Log.Error("Could not get wallable rect", false);
			}
			rect = rect.ContractedBy(1);
			if (flags.Contains(ColonyMakerFlag.AllItems))
			{
				List<ThingDef> itemDefs = (from def in DefDatabase<ThingDef>.AllDefs
				where DebugThingPlaceHelper.IsDebugSpawnable(def, false) && def.category == ThingCategory.Item
				select def).ToList<ThingDef>();
				Autotests_ColonyMaker.FillWithItems(rect, itemDefs);
			}
			else if (flags.Contains(ColonyMakerFlag.ItemsRawFood))
			{
				List<ThingDef> list2 = new List<ThingDef>();
				list2.Add(ThingDefOf.RawPotatoes);
				Autotests_ColonyMaker.FillWithItems(rect, list2);
			}
			if (flags.Contains(ColonyMakerFlag.Filth))
			{
				foreach (IntVec3 loc in rect)
				{
					GenSpawn.Spawn(ThingDefOf.Filth_Dirt, loc, Autotests_ColonyMaker.Map, WipeMode.Vanish);
				}
			}
			if (flags.Contains(ColonyMakerFlag.ItemsWall))
			{
				CellRect cellRect2 = rect.ExpandedBy(1);
				Designator_Build designator_Build2 = new Designator_Build(ThingDefOf.Wall);
				designator_Build2.SetStuffDef(ThingDefOf.WoodLog);
				foreach (IntVec3 c2 in cellRect2.EdgeCells)
				{
					designator_Build2.DesignateSingleCell(c2);
				}
			}
			if (flags.Contains(ColonyMakerFlag.ColonistsMany))
			{
				Autotests_ColonyMaker.MakeColonists(15, Autotests_ColonyMaker.overRect.CenterCell);
			}
			else if (flags.Contains(ColonyMakerFlag.ColonistOne))
			{
				Autotests_ColonyMaker.MakeColonists(1, Autotests_ColonyMaker.overRect.CenterCell);
			}
			if (flags.Contains(ColonyMakerFlag.Fire))
			{
				CellRect cellRect3;
				if (!Autotests_ColonyMaker.TryGetFreeRect(30, 30, out cellRect3))
				{
					Log.Error("Could not get free rect for fire.", false);
				}
				ThingDef plant_TreeOak = ThingDefOf.Plant_TreeOak;
				foreach (IntVec3 loc2 in cellRect3)
				{
					GenSpawn.Spawn(plant_TreeOak, loc2, Autotests_ColonyMaker.Map, WipeMode.Vanish);
				}
				foreach (IntVec3 intVec2 in cellRect3)
				{
					if (intVec2.x % 7 == 0 && intVec2.z % 7 == 0)
					{
						GenExplosion.DoExplosion(intVec2, Find.CurrentMap, 3.9f, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
					}
				}
			}
			if (flags.Contains(ColonyMakerFlag.ColonistsHungry))
			{
				Autotests_ColonyMaker.DoToColonists(0.4f, delegate(Pawn col)
				{
					col.needs.food.CurLevel = Mathf.Max(0f, Rand.Range(-0.05f, 0.05f));
				});
			}
			if (flags.Contains(ColonyMakerFlag.ColonistsTired))
			{
				Autotests_ColonyMaker.DoToColonists(0.4f, delegate(Pawn col)
				{
					col.needs.rest.CurLevel = Mathf.Max(0f, Rand.Range(-0.05f, 0.05f));
				});
			}
			if (flags.Contains(ColonyMakerFlag.ColonistsInjured))
			{
				Autotests_ColonyMaker.DoToColonists(0.4f, delegate(Pawn col)
				{
					DamageDef def3 = (from d in DefDatabase<DamageDef>.AllDefs
					where d.ExternalViolenceFor(null)
					select d).RandomElement<DamageDef>();
					col.TakeDamage(new DamageInfo(def3, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				});
			}
			if (flags.Contains(ColonyMakerFlag.ColonistsDiseased))
			{
				foreach (HediffDef def2 in from d in DefDatabase<HediffDef>.AllDefs
				where d.hediffClass != typeof(Hediff_AddedPart) && (d.HasComp(typeof(HediffComp_Immunizable)) || d.HasComp(typeof(HediffComp_GrowthMode)))
				select d)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
					CellRect cellRect4;
					Autotests_ColonyMaker.TryGetFreeRect(1, 1, out cellRect4);
					GenSpawn.Spawn(pawn, cellRect4.CenterCell, Autotests_ColonyMaker.Map, WipeMode.Vanish);
					pawn.health.AddHediff(def2, null, null, null);
				}
			}
			if (flags.Contains(ColonyMakerFlag.Beds))
			{
				IEnumerable<ThingDef> source = from def in DefDatabase<ThingDef>.AllDefs
				where def.thingClass == typeof(Building_Bed)
				select def;
				int freeColonistsCount = Autotests_ColonyMaker.Map.mapPawns.FreeColonistsCount;
				for (int num = 0; num < freeColonistsCount; num++)
				{
					if (Autotests_ColonyMaker.TryMakeBuilding(source.RandomElement<ThingDef>()) == null)
					{
						Log.Message("Could not make beds.", false);
						break;
					}
				}
			}
			if (flags.Contains(ColonyMakerFlag.Stockpiles))
			{
				Designator_ZoneAddStockpile_Resources designator_ZoneAddStockpile_Resources = new Designator_ZoneAddStockpile_Resources();
				foreach (object obj in Enum.GetValues(typeof(StoragePriority)))
				{
					StoragePriority priority = (StoragePriority)obj;
					CellRect cellRect5;
					Autotests_ColonyMaker.TryGetFreeRect(7, 7, out cellRect5);
					cellRect5 = cellRect5.ContractedBy(1);
					designator_ZoneAddStockpile_Resources.DesignateMultiCell(cellRect5.Cells);
					((Zone_Stockpile)Autotests_ColonyMaker.Map.zoneManager.ZoneAt(cellRect5.CenterCell)).settings.Priority = priority;
				}
			}
			if (flags.Contains(ColonyMakerFlag.GrowingZones))
			{
				Zone_Growing dummyZone = new Zone_Growing(Autotests_ColonyMaker.Map.zoneManager);
				Autotests_ColonyMaker.Map.zoneManager.RegisterZone(dummyZone);
				IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
				Func<ThingDef, bool> predicate;
				Func<ThingDef, bool> <>9__11;
				if ((predicate = <>9__11) == null)
				{
					predicate = (<>9__11 = ((ThingDef d) => d.plant != null && PlantUtility.CanSowOnGrower(d, dummyZone)));
				}
				foreach (ThingDef plantDefToGrow in allDefs.Where(predicate))
				{
					CellRect cellRect6;
					if (!Autotests_ColonyMaker.TryGetFreeRect(6, 6, out cellRect6))
					{
						Log.Error("Could not get growing zone rect.", false);
					}
					cellRect6 = cellRect6.ContractedBy(1);
					foreach (IntVec3 c3 in cellRect6)
					{
						Autotests_ColonyMaker.Map.terrainGrid.SetTerrain(c3, TerrainDefOf.Soil);
					}
					new Designator_ZoneAdd_Growing().DesignateMultiCell(cellRect6.Cells);
					Zone_Growing zone_Growing = Autotests_ColonyMaker.Map.zoneManager.ZoneAt(cellRect6.CenterCell) as Zone_Growing;
					if (zone_Growing != null)
					{
						zone_Growing.SetPlantDefToGrow(plantDefToGrow);
					}
				}
				dummyZone.Delete();
			}
			Autotests_ColonyMaker.ClearAllHomeArea();
			Autotests_ColonyMaker.FillWithHomeArea(Autotests_ColonyMaker.overRect);
			DebugSettings.godMode = godMode;
			Thing.allowDestroyNonDestroyable = false;
		}

		// Token: 0x06009D93 RID: 40339 RVA: 0x002E2180 File Offset: 0x002E0380
		private static void FillWithItems(CellRect rect, List<ThingDef> itemDefs)
		{
			int num = 0;
			foreach (IntVec3 intVec in rect)
			{
				if (intVec.x % 6 != 0 && intVec.z % 6 != 0)
				{
					DebugThingPlaceHelper.DebugSpawn(itemDefs[num], intVec, -1, true);
					num++;
					if (num >= itemDefs.Count)
					{
						num = 0;
					}
				}
			}
		}

		// Token: 0x06009D94 RID: 40340 RVA: 0x002E21FC File Offset: 0x002E03FC
		private static Thing TryMakeBuilding(ThingDef def)
		{
			CellRect cellRect;
			if (!Autotests_ColonyMaker.TryGetFreeRect(def.size.x + 2, def.size.z + 2, out cellRect))
			{
				return null;
			}
			foreach (IntVec3 c in cellRect)
			{
				Autotests_ColonyMaker.Map.terrainGrid.SetTerrain(c, TerrainDefOf.Concrete);
			}
			new Designator_Build(def).DesignateSingleCell(cellRect.CenterCell);
			return cellRect.CenterCell.GetEdifice(Find.CurrentMap);
		}

		// Token: 0x06009D95 RID: 40341 RVA: 0x002E22A4 File Offset: 0x002E04A4
		private static bool TryGetFreeRect(int width, int height, out CellRect result)
		{
			for (int i = Autotests_ColonyMaker.overRect.minZ; i <= Autotests_ColonyMaker.overRect.maxZ - height; i++)
			{
				for (int j = Autotests_ColonyMaker.overRect.minX; j <= Autotests_ColonyMaker.overRect.maxX - width; j++)
				{
					CellRect cellRect = new CellRect(j, i, width, height);
					bool flag = true;
					for (int k = cellRect.minZ; k <= cellRect.maxZ; k++)
					{
						for (int l = cellRect.minX; l <= cellRect.maxX; l++)
						{
							if (Autotests_ColonyMaker.usedCells[l, k])
							{
								flag = false;
								break;
							}
						}
						if (!flag)
						{
							break;
						}
					}
					if (flag)
					{
						result = cellRect;
						for (int m = cellRect.minZ; m <= cellRect.maxZ; m++)
						{
							for (int n = cellRect.minX; n <= cellRect.maxX; n++)
							{
								IntVec3 c = new IntVec3(n, 0, m);
								Autotests_ColonyMaker.usedCells.Set(c, true);
								if (c.GetTerrain(Find.CurrentMap).passability == Traversability.Impassable)
								{
									Autotests_ColonyMaker.Map.terrainGrid.SetTerrain(c, TerrainDefOf.Concrete);
								}
							}
						}
						return true;
					}
				}
			}
			result = new CellRect(0, 0, width, height);
			return false;
		}

		// Token: 0x06009D96 RID: 40342 RVA: 0x002E23EC File Offset: 0x002E05EC
		private static void DoToColonists(float fraction, Action<Pawn> funcToDo)
		{
			int num = Rand.RangeInclusive(1, Mathf.RoundToInt((float)Autotests_ColonyMaker.Map.mapPawns.FreeColonistsCount * fraction));
			int num2 = 0;
			foreach (Pawn obj in Autotests_ColonyMaker.Map.mapPawns.FreeColonists.InRandomOrder(null))
			{
				funcToDo(obj);
				num2++;
				if (num2 >= num)
				{
					break;
				}
			}
		}

		// Token: 0x06009D97 RID: 40343 RVA: 0x002E2474 File Offset: 0x002E0674
		private static void MakeColonists(int count, IntVec3 center)
		{
			for (int i = 0; i < count; i++)
			{
				CellRect cellRect;
				Autotests_ColonyMaker.TryGetFreeRect(1, 1, out cellRect);
				Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
				foreach (WorkTypeDef w in DefDatabase<WorkTypeDef>.AllDefs)
				{
					if (!pawn.WorkTypeIsDisabled(w))
					{
						pawn.workSettings.SetPriority(w, 3);
					}
				}
				GenSpawn.Spawn(pawn, cellRect.CenterCell, Autotests_ColonyMaker.Map, WipeMode.Vanish);
			}
		}

		// Token: 0x06009D98 RID: 40344 RVA: 0x002E251C File Offset: 0x002E071C
		private static void DeleteAllSpawnedPawns()
		{
			foreach (Pawn pawn in Autotests_ColonyMaker.Map.mapPawns.AllPawnsSpawned.ToList<Pawn>())
			{
				pawn.Destroy(DestroyMode.Vanish);
				pawn.relations.ClearAllRelations();
			}
			Find.GameEnder.gameEnding = false;
		}

		// Token: 0x06009D99 RID: 40345 RVA: 0x002E2594 File Offset: 0x002E0794
		private static void ClearAllHomeArea()
		{
			foreach (IntVec3 c in Autotests_ColonyMaker.Map.AllCells)
			{
				Autotests_ColonyMaker.Map.areaManager.Home[c] = false;
			}
		}

		// Token: 0x06009D9A RID: 40346 RVA: 0x00068EAD File Offset: 0x000670AD
		private static void FillWithHomeArea(CellRect r)
		{
			new Designator_AreaHomeExpand().DesignateMultiCell(r.Cells);
		}

		// Token: 0x0400645A RID: 25690
		private static CellRect overRect;

		// Token: 0x0400645B RID: 25691
		private static BoolGrid usedCells;

		// Token: 0x0400645C RID: 25692
		private const int OverRectSize = 100;
	}
}
