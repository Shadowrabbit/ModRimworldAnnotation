using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020003A6 RID: 934
	public static class DebugToolsSpawning
	{
		// Token: 0x06001C44 RID: 7236 RVA: 0x000A6A1C File Offset: 0x000A4C1C
		private static IEnumerable<float> PointsMechCluster()
		{
			for (float points = 50f; points <= 10000f; points += 50f)
			{
				yield return points;
			}
			yield break;
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x000A6A28 File Offset: 0x000A4C28
		[DebugAction("Spawning", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (PawnKindDef localKindDef2 in from kd in DefDatabase<PawnKindDef>.AllDefs
			orderby kd.defName
			select kd)
			{
				PawnKindDef localKindDef = localKindDef2;
				list.Add(new DebugMenuOption(localKindDef.defName, DebugMenuOptionMode.Tool, delegate()
				{
					Faction faction = FactionUtility.DefaultFactionFrom(localKindDef.defaultFactionType);
					Pawn newPawn = PawnGenerator.GeneratePawn(localKindDef, faction);
					GenSpawn.Spawn(newPawn, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
					if (faction != null && faction != Faction.OfPlayer)
					{
						Lord lord = null;
						if (newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction).Any((Pawn p) => p != newPawn))
						{
							lord = ((Pawn)GenClosest.ClosestThing_Global(newPawn.Position, newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction), 99999f, (Thing p) => p != newPawn && ((Pawn)p).GetLord() != null, null)).GetLord();
						}
						if (lord == null)
						{
							LordJob_DefendPoint lordJob = new LordJob_DefendPoint(newPawn.Position, null, false, true);
							lord = LordMaker.MakeNewLord(faction, lordJob, Find.CurrentMap, null);
						}
						lord.AddPawn(newPawn);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x000A6AD8 File Offset: 0x000A4CD8
		[DebugAction("Spawning", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnWeapon()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where def.equipmentType == EquipmentType.Primary
			select def into d
			orderby d.defName
			select d)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Tool, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false, null);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x000A6BAC File Offset: 0x000A4DAC
		[DebugAction("Spawning", "Spawn apparel...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnApparel()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where def.IsApparel
			select def into d
			orderby d.defName
			select d)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Tool, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), -1, false, null);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x000A6C80 File Offset: 0x000A4E80
		[DebugAction("Spawning", "Try place near thing...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryPlaceNearThing()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, false)));
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x000A6C98 File Offset: 0x000A4E98
		[DebugAction("Spawning", "Try place near thing with style...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryPlaceNearThingWithStyle()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingDef localDef2 in from d in DefDatabase<ThingDef>.AllDefs
			orderby d.defName
			select d)
			{
				ThingDef localDef = localDef2;
				if (!localDef.randomStyle.NullOrEmpty<ThingStyleChance>() || DefDatabase<StyleCategoryDef>.AllDefs.Any((StyleCategoryDef s) => s.GetStyleForThingDef(localDef, null) != null))
				{
					Predicate<ThingDefStyle> <>9__6;
					Func<StyleCategoryDef, bool> <>9__5;
					list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Action, delegate()
					{
						DebugToolsSpawning.<>c__DisplayClass5_1 CS$<>8__locals2;
						CS$<>8__locals2.styleOptions = new List<DebugMenuOption>();
						base.<TryPlaceNearThingWithStyle>g__AddOption|3("Standard", null, ref CS$<>8__locals2);
						IEnumerable<StyleCategoryDef> allDefs = DefDatabase<StyleCategoryDef>.AllDefs;
						Func<StyleCategoryDef, bool> predicate;
						if ((predicate = <>9__5) == null)
						{
							predicate = (<>9__5 = delegate(StyleCategoryDef x)
							{
								List<ThingDefStyle> thingDefStyles = x.thingDefStyles;
								Predicate<ThingDefStyle> predicate2;
								if ((predicate2 = <>9__6) == null)
								{
									predicate2 = (<>9__6 = ((ThingDefStyle y) => y.ThingDef == localDef));
								}
								return thingDefStyles.Any(predicate2);
							});
						}
						foreach (StyleCategoryDef styleCategoryDef in allDefs.Where(predicate))
						{
							base.<TryPlaceNearThingWithStyle>g__AddOption|3(styleCategoryDef.LabelCap, styleCategoryDef.GetStyleForThingDef(localDef, null), ref CS$<>8__locals2);
						}
						if (localDef.randomStyle != null)
						{
							foreach (ThingStyleChance thingStyleChance in localDef.randomStyle)
							{
								base.<TryPlaceNearThingWithStyle>g__AddOption|3(thingStyleChance.StyleDef.overrideLabel ?? localDef.label, thingStyleChance.StyleDef, ref CS$<>8__locals2);
							}
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(CS$<>8__locals2.styleOptions));
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x000A6D78 File Offset: 0x000A4F78
		[DebugAction("Spawning", "Try place near stack of 25...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryPlaceNearStacksOf25()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, false)));
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x000A6D91 File Offset: 0x000A4F91
		[DebugAction("Spawning", "Try place near stack of 75...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryPlaceNearStacksOf75()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(75, false)));
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x000A6DAA File Offset: 0x000A4FAA
		[DebugAction("Spawning", "Try place direct thing...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryPlaceDirectThing()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(1, true)));
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x000A6DC2 File Offset: 0x000A4FC2
		[DebugAction("Spawning", "Try place direct stack of 25...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryPlaceDirectStackOf25()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForStackCount(25, true)));
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x000A6DDC File Offset: 0x000A4FDC
		[DebugAction("Spawning", "Try place stack of market value...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryPlaceNearMarketValue()
		{
			float[] array = new float[]
			{
				1000f,
				10000f,
				100000f
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			float[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				float marketValue = array2[i];
				list.Add(new DebugMenuOption(marketValue.ToStringMoney(null), DebugMenuOptionMode.Action, delegate()
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.TryPlaceOptionsForBaseMarketValue(marketValue, false)));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x000A6E54 File Offset: 0x000A5054
		[DebugAction("Spawning", "Try add to inventory...", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryAddToInventory(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<ThingDef> enumerator = DefDatabase<ThingDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef def = enumerator.Current;
					if (def.category == ThingCategory.Item)
					{
						list.Add(new DebugMenuOption(def.label, DebugMenuOptionMode.Action, delegate()
						{
							p.inventory.TryAddItemNotForSale(ThingMaker.MakeThing(def, null));
						}));
					}
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x000A6F00 File Offset: 0x000A5100
		[DebugAction("Spawning", "Spawn thing with wipe mode...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnThingWithWipeMode()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			WipeMode[] array = (WipeMode[])Enum.GetValues(typeof(WipeMode));
			for (int i = 0; i < array.Length; i++)
			{
				WipeMode localWipeMode2 = array[i];
				WipeMode localWipeMode = localWipeMode2;
				list.Add(new DebugMenuOption(localWipeMode2.ToString(), DebugMenuOptionMode.Action, delegate()
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugThingPlaceHelper.SpawnOptions(localWipeMode)));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x000A6F80 File Offset: 0x000A5180
		[DebugAction("Spawning", "Set terrain...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetTerrain()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (TerrainDef localDef2 in DefDatabase<TerrainDef>.AllDefs)
			{
				TerrainDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Tool, delegate()
				{
					if (UI.MouseCell().InBounds(Find.CurrentMap))
					{
						Find.CurrentMap.terrainGrid.SetTerrain(UI.MouseCell(), localDef);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x000A7010 File Offset: 0x000A5210
		[DebugAction("Spawning", "Set terrain (rect)...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetTerrainRect()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (TerrainDef localDef2 in DefDatabase<TerrainDef>.AllDefs)
			{
				TerrainDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					DebugTool tool = null;
					IntVec3 firstCorner;
					Action <>9__2;
					tool = new DebugTool("first corner...", delegate()
					{
						firstCorner = UI.MouseCell();
						string label = "second corner...";
						Action clickAction;
						if ((clickAction = <>9__2) == null)
						{
							clickAction = (<>9__2 = delegate()
							{
								IntVec3 second = UI.MouseCell();
								foreach (IntVec3 c in CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap))
								{
									Find.CurrentMap.terrainGrid.SetTerrain(c, localDef);
								}
								DebugTools.curTool = tool;
							});
						}
						DebugTools.curTool = new DebugTool(label, clickAction, firstCorner);
					}, null);
					DebugTools.curTool = tool;
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x000A70A0 File Offset: 0x000A52A0
		[DebugAction("Spawning", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnMechCluster()
		{
			if (Faction.OfMechanoids == null)
			{
				Messages.Message("No mechanoid world faction found.", MessageTypeDefOf.RejectInput, false);
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (float num in DebugToolsSpawning.PointsMechCluster())
			{
				float localPoints = num;
				Action <>9__1;
				Action <>9__2;
				Action <>9__3;
				Action <>9__4;
				list.Add(new DebugMenuOption(num + " points", DebugMenuOptionMode.Action, delegate()
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					List<DebugMenuOption> list3 = list2;
					string label = "In pods, click place";
					DebugMenuOptionMode mode = DebugMenuOptionMode.Tool;
					Action method;
					if ((method = <>9__1) == null)
					{
						method = (<>9__1 = delegate()
						{
							MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true, false);
							MechClusterUtility.SpawnCluster(UI.MouseCell(), Find.CurrentMap, sketch, true, false, null);
						});
					}
					list3.Add(new DebugMenuOption(label, mode, method));
					List<DebugMenuOption> list4 = list2;
					string label2 = "In pods, autoplace";
					DebugMenuOptionMode mode2 = DebugMenuOptionMode.Action;
					Action method2;
					if ((method2 = <>9__2) == null)
					{
						method2 = (<>9__2 = delegate()
						{
							MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true, false);
							MechClusterUtility.SpawnCluster(MechClusterUtility.FindClusterPosition(Find.CurrentMap, sketch, 100, 0f), Find.CurrentMap, sketch, true, false, null);
						});
					}
					list4.Add(new DebugMenuOption(label2, mode2, method2));
					List<DebugMenuOption> list5 = list2;
					string label3 = "Direct spawn, click place";
					DebugMenuOptionMode mode3 = DebugMenuOptionMode.Tool;
					Action method3;
					if ((method3 = <>9__3) == null)
					{
						method3 = (<>9__3 = delegate()
						{
							MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true, false);
							MechClusterUtility.SpawnCluster(UI.MouseCell(), Find.CurrentMap, sketch, false, false, null);
						});
					}
					list5.Add(new DebugMenuOption(label3, mode3, method3));
					List<DebugMenuOption> list6 = list2;
					string label4 = "Direct spawn, autoplace";
					DebugMenuOptionMode mode4 = DebugMenuOptionMode.Action;
					Action method4;
					if ((method4 = <>9__4) == null)
					{
						method4 = (<>9__4 = delegate()
						{
							MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(localPoints, Find.CurrentMap, true, false);
							MechClusterUtility.SpawnCluster(MechClusterUtility.FindClusterPosition(Find.CurrentMap, sketch, 100, 0f), Find.CurrentMap, sketch, false, false, null);
						});
					}
					list6.Add(new DebugMenuOption(label4, mode4, method4));
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x000A7148 File Offset: 0x000A5348
		[DebugAction("Spawning", "Make filth x100", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeFilthx100()
		{
			for (int i = 0; i < 100; i++)
			{
				IntVec3 c = UI.MouseCell() + GenRadial.RadialPattern[i];
				if (c.InBounds(Find.CurrentMap) && c.WalkableByAny(Find.CurrentMap))
				{
					FilthMaker.TryMakeFilth(c, Find.CurrentMap, ThingDefOf.Filth_Dirt, 2, FilthSourceFlags.None);
					FleckMaker.ThrowMetaPuff(c.ToVector3Shifted(), Find.CurrentMap);
				}
			}
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x000A71B8 File Offset: 0x000A53B8
		[DebugAction("Spawning", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnFactionLeader()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (Faction localFac2 in Find.FactionManager.AllFactions)
			{
				Faction localFac = localFac2;
				if (localFac.leader != null)
				{
					list.Add(new FloatMenuOption(localFac.Name + " - " + localFac.leader.Name.ToStringFull, delegate()
					{
						GenSpawn.Spawn(localFac.leader, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x000A7280 File Offset: 0x000A5480
		[DebugAction("Spawning", "Spawn world pawn...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnWorldPawn()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			Action<Pawn> act = delegate(Pawn p)
			{
				List<DebugMenuOption> list2 = new List<DebugMenuOption>();
				IEnumerable<PawnKindDef> allDefs = DefDatabase<PawnKindDef>.AllDefs;
				Func<PawnKindDef, bool> <>9__1;
				Func<PawnKindDef, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((PawnKindDef x) => x.race == p.def));
				}
				foreach (PawnKindDef kLocal2 in allDefs.Where(predicate))
				{
					PawnKindDef kLocal = kLocal2;
					list2.Add(new DebugMenuOption(kLocal.defName, DebugMenuOptionMode.Tool, delegate()
					{
						PawnGenerationRequest request = new PawnGenerationRequest(kLocal, p.Faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false);
						PawnGenerator.RedressPawn(p, request);
						GenSpawn.Spawn(p, UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
						DebugTools.curTool = null;
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
			};
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAlive)
			{
				Pawn pLocal = pawn;
				list.Add(new DebugMenuOption(pawn.LabelShort, DebugMenuOptionMode.Action, delegate()
				{
					act(pLocal);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x000A7348 File Offset: 0x000A5548
		[DebugAction("Spawning", "Spawn thing set...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnThingSet()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<ThingSetMakerDef> allDefsListForReading = DefDatabase<ThingSetMakerDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ThingSetMakerDef localGenerator = allDefsListForReading[i];
				list.Add(new DebugMenuOption(localGenerator.defName, DebugMenuOptionMode.Tool, delegate()
				{
					if (!UI.MouseCell().InBounds(Find.CurrentMap))
					{
						return;
					}
					StringBuilder stringBuilder = new StringBuilder();
					string nonNullFieldsDebugInfo = Gen.GetNonNullFieldsDebugInfo(localGenerator.debugParams);
					List<Thing> list2 = localGenerator.root.Generate(localGenerator.debugParams);
					stringBuilder.Append(string.Concat(new object[]
					{
						localGenerator.defName,
						" generated ",
						list2.Count,
						" things"
					}));
					if (!nonNullFieldsDebugInfo.NullOrEmpty())
					{
						stringBuilder.Append(" (used custom debug params: " + nonNullFieldsDebugInfo + ")");
					}
					stringBuilder.AppendLine(":");
					float num = 0f;
					float num2 = 0f;
					for (int j = 0; j < list2.Count; j++)
					{
						stringBuilder.AppendLine("   - " + list2[j].LabelCap);
						num += list2[j].MarketValue * (float)list2[j].stackCount;
						if (!(list2[j] is Pawn))
						{
							num2 += list2[j].GetStatValue(StatDefOf.Mass, true) * (float)list2[j].stackCount;
						}
						if (!GenPlace.TryPlaceThing(list2[j], UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, default(Rot4)))
						{
							list2[j].Destroy(DestroyMode.Vanish);
						}
					}
					stringBuilder.AppendLine("Total market value: " + num.ToString("0.##"));
					stringBuilder.AppendLine("Total mass: " + num2.ToStringMass());
					Log.Message(stringBuilder.ToString());
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x000A73B8 File Offset: 0x000A55B8
		[DebugAction("Spawning", "Trigger effecter...", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TriggerEffecter()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<EffecterDef> allDefsListForReading = DefDatabase<EffecterDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				EffecterDef localDef = allDefsListForReading[i];
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Tool, delegate()
				{
					Effecter effecter = localDef.Spawn();
					effecter.Trigger(new TargetInfo(UI.MouseCell(), Find.CurrentMap, false), new TargetInfo(UI.MouseCell(), Find.CurrentMap, false));
					effecter.Cleanup();
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x000A7428 File Offset: 0x000A5628
		[DebugAction("Spawning", null, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnShuttle()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Incoming", DebugMenuOptionMode.Tool, delegate()
			{
				GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleIncoming, ThingMaker.MakeThing(ThingDefOf.Shuttle, null)), UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, default(Rot4));
			}));
			list.Add(new DebugMenuOption("Crashing", DebugMenuOptionMode.Tool, delegate()
			{
				GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleCrashing, ThingMaker.MakeThing(ThingDefOf.ShuttleCrashed, null)), UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, default(Rot4));
			}));
			list.Add(new DebugMenuOption("Stationary", DebugMenuOptionMode.Tool, delegate()
			{
				GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.Shuttle, null), UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near, null, null, default(Rot4));
			}));
			List<DebugMenuOption> options = list;
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(options));
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x000A74DC File Offset: 0x000A56DC
		[DebugAction("Spawning", null, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnRandomCaravan()
		{
			int num = GenWorld.MouseTile(false);
			if (Find.WorldGrid[num].biome.impassable)
			{
				return;
			}
			List<Pawn> list = new List<Pawn>();
			int num2 = Rand.RangeInclusive(1, 10);
			for (int i = 0; i < num2; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(Faction.OfPlayer.def.basicMemberKind, Faction.OfPlayer);
				list.Add(pawn);
				if (!pawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					ThingDef thingDef = (from def in DefDatabase<ThingDef>.AllDefs
					where def.IsWeapon && !def.weaponTags.NullOrEmpty<string>() && (def.weaponTags.Contains("SimpleGun") || def.weaponTags.Contains("IndustrialGunAdvanced") || def.weaponTags.Contains("SpacerGun") || def.weaponTags.Contains("MedievalMeleeAdvanced") || def.weaponTags.Contains("NeolithicRangedBasic") || def.weaponTags.Contains("NeolithicRangedDecent") || def.weaponTags.Contains("NeolithicRangedHeavy"))
					select def).RandomElementWithFallback(null);
					pawn.equipment.AddEquipment((ThingWithComps)ThingMaker.MakeThing(thingDef, GenStuff.RandomStuffFor(thingDef)));
				}
			}
			int num3 = Rand.RangeInclusive(-4, 10);
			for (int j = 0; j < num3; j++)
			{
				Pawn item = PawnGenerator.GeneratePawn((from d in DefDatabase<PawnKindDef>.AllDefs
				where d.RaceProps.Animal && d.RaceProps.wildness < 1f
				select d).RandomElement<PawnKindDef>(), Faction.OfPlayer);
				list.Add(item);
			}
			Caravan caravan = CaravanMaker.MakeCaravan(list, Faction.OfPlayer, num, true);
			List<Thing> list2 = ThingSetMakerDefOf.DebugCaravanInventory.root.Generate();
			for (int k = 0; k < list2.Count; k++)
			{
				Thing thing = list2[k];
				if (thing.GetStatValue(StatDefOf.Mass, true) * (float)thing.stackCount > caravan.MassCapacity - caravan.MassUsage)
				{
					break;
				}
				CaravanInventoryUtility.GiveThing(caravan, thing);
			}
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x000A7678 File Offset: 0x000A5878
		[DebugAction("Spawning", null, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnRandomFactionBase()
		{
			Faction faction;
			if ((from x in Find.FactionManager.AllFactions
			where !x.IsPlayer && !x.Hidden
			select x).TryRandomElement(out faction))
			{
				int num = GenWorld.MouseTile(false);
				if (Find.WorldGrid[num].biome.impassable)
				{
					return;
				}
				Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
				settlement.SetFaction(faction);
				settlement.Tile = num;
				settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, null);
				Find.WorldObjects.Add(settlement);
			}
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x000A7714 File Offset: 0x000A5914
		[DebugAction("Spawning", null, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnSite()
		{
			DebugToolsSpawning.<>c__DisplayClass24_0 CS$<>8__locals1 = new DebugToolsSpawning.<>c__DisplayClass24_0();
			CS$<>8__locals1.tile = GenWorld.MouseTile(false);
			if (CS$<>8__locals1.tile < 0 || Find.World.Impassable(CS$<>8__locals1.tile))
			{
				Messages.Message("Impassable", MessageTypeDefOf.RejectInput, false);
				return;
			}
			List<SitePartDef> parts = new List<SitePartDef>();
			Action addPart = null;
			Action <>9__1;
			addPart = delegate()
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<DebugMenuOption> list2 = list;
				string label = "-Done (" + parts.Count + " parts)-";
				DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
				Action method;
				if ((method = <>9__1) == null)
				{
					method = (<>9__1 = delegate()
					{
						Site site = SiteMaker.TryMakeSite(parts, CS$<>8__locals1.tile, true, null, true, null);
						if (site == null)
						{
							Messages.Message("Could not find any valid faction for this site.", MessageTypeDefOf.RejectInput, false);
							return;
						}
						Find.WorldObjects.Add(site);
					});
				}
				list2.Add(new DebugMenuOption(label, mode, method));
				foreach (SitePartDef sitePartDef in DefDatabase<SitePartDef>.AllDefs)
				{
					SitePartDef localPart = sitePartDef;
					list.Add(new DebugMenuOption(sitePartDef.defName, DebugMenuOptionMode.Action, delegate()
					{
						parts.Add(localPart);
						addPart();
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			};
			addPart();
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x000A779C File Offset: 0x000A599C
		[DebugAction("Spawning", null, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void DestroySite()
		{
			int tileID = GenWorld.MouseTile(false);
			foreach (WorldObject worldObject in Find.WorldObjects.ObjectsAt(tileID).ToList<WorldObject>())
			{
				worldObject.Destroy();
			}
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x000A7800 File Offset: 0x000A5A00
		[DebugAction("Spawning", null, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnSiteWithPoints()
		{
			DebugToolsSpawning.<>c__DisplayClass26_0 CS$<>8__locals1 = new DebugToolsSpawning.<>c__DisplayClass26_0();
			CS$<>8__locals1.tile = GenWorld.MouseTile(false);
			if (CS$<>8__locals1.tile < 0 || Find.World.Impassable(CS$<>8__locals1.tile))
			{
				Messages.Message("Impassable", MessageTypeDefOf.RejectInput, false);
				return;
			}
			List<SitePartDef> parts = new List<SitePartDef>();
			Action addPart = null;
			Action <>9__1;
			addPart = delegate()
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<DebugMenuOption> list2 = list;
				string label = "-Done (" + parts.Count + " parts)-";
				DebugMenuOptionMode mode = DebugMenuOptionMode.Action;
				Action method;
				if ((method = <>9__1) == null)
				{
					method = (<>9__1 = delegate()
					{
						List<DebugMenuOption> list3 = new List<DebugMenuOption>();
						foreach (float localPoints2 in DebugActionsUtility.PointsOptions(true))
						{
							float localPoints = localPoints2;
							list3.Add(new DebugMenuOption(localPoints2.ToString("F0"), DebugMenuOptionMode.Action, delegate()
							{
								Site site = SiteMaker.TryMakeSite(parts, CS$<>8__locals1.tile, true, null, true, new float?(localPoints));
								if (site == null)
								{
									Messages.Message("Could not find any valid faction for this site.", MessageTypeDefOf.RejectInput, false);
									return;
								}
								Find.WorldObjects.Add(site);
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
					});
				}
				list2.Add(new DebugMenuOption(label, mode, method));
				foreach (SitePartDef sitePartDef in DefDatabase<SitePartDef>.AllDefs)
				{
					SitePartDef localPart = sitePartDef;
					list.Add(new DebugMenuOption(sitePartDef.defName, DebugMenuOptionMode.Action, delegate()
					{
						parts.Add(localPart);
						addPart();
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			};
			addPart();
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x000A7888 File Offset: 0x000A5A88
		[DebugAction("Spawning", null, actionType = DebugActionType.ToolWorld, allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void SpawnWorldObject()
		{
			int tile = GenWorld.MouseTile(false);
			if (tile < 0 || Find.World.Impassable(tile))
			{
				Messages.Message("Impassable", MessageTypeDefOf.RejectInput, false);
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (WorldObjectDef localDef2 in DefDatabase<WorldObjectDef>.AllDefs)
			{
				WorldObjectDef localDef = localDef2;
				Action method = delegate()
				{
					WorldObject worldObject = WorldObjectMaker.MakeWorldObject(localDef);
					worldObject.Tile = tile;
					Find.WorldObjects.Add(worldObject);
				};
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, method));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x000A7964 File Offset: 0x000A5B64
		[DebugAction("General", "Change camera config...", allowedGameStates = AllowedGameStates.PlayingOnWorld)]
		private static void ChangeCameraConfigWorld()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localType2 in typeof(WorldCameraConfig).AllSubclasses())
			{
				Type localType = localType2;
				string text = localType.Name;
				if (text.StartsWith("WorldCameraConfig_"))
				{
					text = text.Substring("WorldCameraConfig_".Length);
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
				{
					Find.WorldCameraDriver.config = (WorldCameraConfig)Activator.CreateInstance(localType);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x000A7A24 File Offset: 0x000A5C24
		[DebugAction("Spawning", "Spawn relic", allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SpawnRelic()
		{
			FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
			Ideo ideo = (ideos != null) ? ideos.PrimaryIdeo : null;
			if (ideo == null)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Precept precept in from x in ideo.PreceptsListForReading
			where x is Precept_Relic
			select x)
			{
				Precept_Relic relic = precept as Precept_Relic;
				list.Add(new DebugMenuOption(relic.LabelCap + " (" + relic.ThingDef.label + ")", DebugMenuOptionMode.Tool, delegate()
				{
					GenSpawn.Spawn(relic.GenerateRelic(), UI.MouseCell(), Find.CurrentMap, WipeMode.Vanish);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}
	}
}
