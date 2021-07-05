using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.BaseGen;
using RimWorld.SketchGen;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020005AF RID: 1455
	public static class DebugToolsGeneral
	{
		// Token: 0x06002479 RID: 9337 RVA: 0x00112330 File Offset: 0x00110530
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Destroy()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x00112394 File Offset: 0x00110594
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Kill()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.Kill(null, null);
			}
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x00112404 File Offset: 0x00110604
		[DebugAction("General", "10 damage", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Take10Damage()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x00112488 File Offset: 0x00110688
		[DebugAction("General", "5000 damage", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Take5000Damage()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 50000f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x0001E7CB File Offset: 0x0001C9CB
		[DebugAction("General", "Clear area 21x21", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ClearArea21x21()
		{
			GenDebug.ClearArea(CellRect.CenteredOn(UI.MouseCell(), 10), Find.CurrentMap);
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x0011250C File Offset: 0x0011070C
		[DebugAction("General", "Make rock 11x11", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Rock21x21()
		{
			foreach (IntVec3 loc in CellRect.CenteredOn(UI.MouseCell(), 5).ClipInsideMap(Find.CurrentMap))
			{
				GenSpawn.Spawn(ThingDefOf.Granite, loc, Find.CurrentMap, WipeMode.Vanish);
			}
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x00112580 File Offset: 0x00110780
		[DebugAction("General", "Explosion (bomb)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionBomb()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.Bomb, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x001125D0 File Offset: 0x001107D0
		[DebugAction("General", "Explosion (flame)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionFlame()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x00112620 File Offset: 0x00110820
		[DebugAction("General", "Explosion (stun)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionStun()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.Stun, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x00112670 File Offset: 0x00110870
		[DebugAction("General", "Explosion (EMP)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionEMP()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.EMP, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x001126C0 File Offset: 0x001108C0
		[DebugAction("General", "Explosion (extinguisher)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionExtinguisher()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 10f, DamageDefOf.Extinguish, null, -1, -1f, null, null, null, null, ThingDefOf.Filth_FireFoam, 1f, 3, true, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x00112714 File Offset: 0x00110914
		[DebugAction("General", "Explosion (smoke)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionSmoke()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 10f, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, ThingDefOf.Gas_Smoke, 1f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x0001E7E3 File Offset: 0x0001C9E3
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void LightningStrike()
		{
			Find.CurrentMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(Find.CurrentMap, UI.MouseCell()));
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x0001E808 File Offset: 0x0001CA08
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddSnow()
		{
			SnowUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, 1f);
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x0001E823 File Offset: 0x0001CA23
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveSnow()
		{
			SnowUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, -1f);
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x00112768 File Offset: 0x00110968
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ClearAllSnow()
		{
			foreach (IntVec3 c in Find.CurrentMap.AllCells)
			{
				Find.CurrentMap.snowGrid.SetDepth(c, 0f);
			}
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x001127C8 File Offset: 0x001109C8
		[DebugAction("General", "Push heat (10)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PushHeat10()
		{
			foreach (IntVec3 intVec in Find.CurrentMap.AllCells)
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, 10f);
			}
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x00112828 File Offset: 0x00110A28
		[DebugAction("General", "Push heat (1000)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PushHeat1000()
		{
			foreach (IntVec3 intVec in Find.CurrentMap.AllCells)
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, 1000f);
			}
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x00112888 File Offset: 0x00110A88
		[DebugAction("General", "Push heat (-1000)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PushHeatNeg1000()
		{
			foreach (IntVec3 intVec in Find.CurrentMap.AllCells)
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, -1000f);
			}
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x001128E8 File Offset: 0x00110AE8
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FinishPlantGrowth()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				Plant plant = thing as Plant;
				if (plant != null)
				{
					plant.Growth = 1f;
				}
			}
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x00112950 File Offset: 0x00110B50
		[DebugAction("General", "Grow 1 day", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Grow1Day()
		{
			IntVec3 intVec = UI.MouseCell();
			Plant plant = intVec.GetPlant(Find.CurrentMap);
			if (plant != null && plant.def.plant != null)
			{
				int num = (int)((1f - plant.Growth) * plant.def.plant.growDays);
				if (num >= 60000)
				{
					plant.Age += 60000;
				}
				else if (num > 0)
				{
					plant.Age += num;
				}
				plant.Growth += 1f / plant.def.plant.growDays;
				if ((double)plant.Growth > 1.0)
				{
					plant.Growth = 1f;
				}
				Find.CurrentMap.mapDrawer.SectionAt(intVec).RegenerateAllLayers();
			}
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x00112A28 File Offset: 0x00110C28
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GrowToMaturity()
		{
			IntVec3 intVec = UI.MouseCell();
			Plant plant = intVec.GetPlant(Find.CurrentMap);
			if (plant != null && plant.def.plant != null)
			{
				int num = (int)((1f - plant.Growth) * plant.def.plant.growDays);
				plant.Age += num;
				plant.Growth = 1f;
				Find.CurrentMap.mapDrawer.SectionAt(intVec).RegenerateAllLayers();
			}
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x0001E83E File Offset: 0x0001CA3E
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RegenSection()
		{
			Find.CurrentMap.mapDrawer.SectionAt(UI.MouseCell()).RegenerateAllLayers();
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x00112AA4 File Offset: 0x00110CA4
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RandomizeColor()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				if (thing.TryGetComp<CompColorable>() != null)
				{
					thing.SetColor(GenColor.RandomColorOpaque(), true);
				}
			}
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x00112B0C File Offset: 0x00110D0C
		[DebugAction("General", "Rot 1 day", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Rot1Day()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				CompRottable compRottable = thing.TryGetComp<CompRottable>();
				if (compRottable != null)
				{
					compRottable.RotProgress += 60000f;
				}
			}
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x00112B7C File Offset: 0x00110D7C
		[DebugAction("General", "Force sleep", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceSleep()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				CompCanBeDormant compCanBeDormant = thing.TryGetComp<CompCanBeDormant>();
				if (compCanBeDormant != null)
				{
					compCanBeDormant.ToSleep();
				}
				else
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.LayDown, pawn.Position), JobCondition.None, null, false, true, null, null, false, false);
					}
				}
			}
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x00112C28 File Offset: 0x00110E28
		[DebugAction("General", "Fuel -20%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FuelRemove20Percent()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
				if (compRefuelable != null)
				{
					compRefuelable.ConsumeFuel(compRefuelable.Props.fuelCapacity * 0.2f);
				}
			}
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x00112C9C File Offset: 0x00110E9C
		[DebugAction("General", "Break down...", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BreakDown()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()))
			{
				CompBreakdownable compBreakdownable = thing.TryGetComp<CompBreakdownable>();
				if (compBreakdownable != null && !compBreakdownable.BrokenDown)
				{
					compBreakdownable.DoBreakdown();
				}
			}
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x0001E859 File Offset: 0x0001CA59
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void UseScatterer()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_MapGen.Options_Scatterers()));
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x00112D08 File Offset: 0x00110F08
		[DebugAction("General", "BaseGen", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void BaseGen()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (string text in (from x in DefDatabase<RuleDef>.AllDefs
			select x.symbol).Distinct<string>())
			{
				string localSymbol = text;
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Action, delegate()
				{
					DebugTool tool = null;
					IntVec3 firstCorner;
					Action <>9__3;
					tool = new DebugTool("first corner...", delegate()
					{
						firstCorner = UI.MouseCell();
						string label = "second corner...";
						Action clickAction;
						if ((clickAction = <>9__3) == null)
						{
							clickAction = (<>9__3 = delegate()
							{
								IntVec3 second = UI.MouseCell();
								CellRect rect = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap);
								RimWorld.BaseGen.BaseGen.globalSettings.map = Find.CurrentMap;
								RimWorld.BaseGen.BaseGen.symbolStack.Push(localSymbol, rect, null);
								RimWorld.BaseGen.BaseGen.Generate();
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

		// Token: 0x06002497 RID: 9367 RVA: 0x00112DB4 File Offset: 0x00110FB4
		[DebugAction("General", "SketchGen", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SketchGen()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (SketchResolverDef sketchResolverDef in from x in DefDatabase<SketchResolverDef>.AllDefs
			where x.isRoot
			select x)
			{
				SketchResolverDef localResolver = sketchResolverDef;
				if (localResolver == SketchResolverDefOf.Monument || localResolver == SketchResolverDefOf.MonumentRuin)
				{
					List<DebugMenuOption> sizeOpts = new List<DebugMenuOption>();
					for (int i = 1; i <= 60; i++)
					{
						int localIndex = i;
						sizeOpts.Add(new DebugMenuOption(localIndex.ToString(), DebugMenuOptionMode.Tool, delegate()
						{
							RimWorld.SketchGen.ResolveParams parms = default(RimWorld.SketchGen.ResolveParams);
							parms.sketch = new Sketch();
							parms.monumentSize = new IntVec2?(new IntVec2(localIndex, localIndex));
							RimWorld.SketchGen.SketchGen.Generate(localResolver, parms).Spawn(Find.CurrentMap, UI.MouseCell(), null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, null, false, true, null, null);
						}));
					}
					list.Add(new DebugMenuOption(sketchResolverDef.defName, DebugMenuOptionMode.Action, delegate()
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(sizeOpts));
					}));
				}
				else
				{
					list.Add(new DebugMenuOption(sketchResolverDef.defName, DebugMenuOptionMode.Tool, delegate()
					{
						RimWorld.SketchGen.ResolveParams parms = default(RimWorld.SketchGen.ResolveParams);
						parms.sketch = new Sketch();
						RimWorld.SketchGen.SketchGen.Generate(localResolver, parms).Spawn(Find.CurrentMap, UI.MouseCell(), null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, null, false, true, null, null);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x00112F18 File Offset: 0x00111118
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeRoof()
		{
			foreach (IntVec3 c in CellRect.CenteredOn(UI.MouseCell(), 1))
			{
				Find.CurrentMap.roofGrid.SetRoof(c, RoofDefOf.RoofConstructed);
			}
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x00112F84 File Offset: 0x00111184
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DeleteRoof()
		{
			foreach (IntVec3 c in CellRect.CenteredOn(UI.MouseCell(), 1))
			{
				Find.CurrentMap.roofGrid.SetRoof(c, null);
			}
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x0001E86F File Offset: 0x0001CA6F
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestFloodUnfog()
		{
			FloodFillerFog.DebugFloodUnfog(UI.MouseCell(), Find.CurrentMap);
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x00112FEC File Offset: 0x001111EC
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashClosewalkCell30()
		{
			IntVec3 c = CellFinder.RandomClosewalkCellNear(UI.MouseCell(), Find.CurrentMap, 30, null);
			Find.CurrentMap.debugDrawer.FlashCell(c, 0f, null, 50);
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x0001E880 File Offset: 0x0001CA80
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashWalkPath()
		{
			WalkPathFinder.DebugFlashWalkPath(UI.MouseCell(), 8);
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x00113024 File Offset: 0x00111224
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashSkygazeCell()
		{
			Pawn pawn = Find.CurrentMap.mapPawns.FreeColonists.First<Pawn>();
			IntVec3 c;
			RCellFinder.TryFindSkygazeCell(UI.MouseCell(), pawn, out c);
			Find.CurrentMap.debugDrawer.FlashCell(c, 0f, null, 50);
			MoteMaker.ThrowText(c.ToVector3Shifted(), Find.CurrentMap, "for " + pawn.Label, Color.white, -1f);
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x00113098 File Offset: 0x00111298
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashDirectFleeDest()
		{
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn == null)
			{
				Find.CurrentMap.debugDrawer.FlashCell(UI.MouseCell(), 0f, "select a pawn", 50);
				return;
			}
			IntVec3 c;
			if (RCellFinder.TryFindDirectFleeDestination(UI.MouseCell(), 9f, pawn, out c))
			{
				Find.CurrentMap.debugDrawer.FlashCell(c, 0.5f, null, 50);
				return;
			}
			Find.CurrentMap.debugDrawer.FlashCell(UI.MouseCell(), 0.8f, "not found", 50);
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x00113128 File Offset: 0x00111328
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashSpectatorsCells()
		{
			Action<bool> act = delegate(bool bestSideOnly)
			{
				DebugTool tool = null;
				IntVec3 firstCorner;
				Action <>9__4;
				tool = new DebugTool("first watch rect corner...", delegate()
				{
					firstCorner = UI.MouseCell();
					string label = "second watch rect corner...";
					Action clickAction;
					if ((clickAction = <>9__4) == null)
					{
						clickAction = (<>9__4 = delegate()
						{
							IntVec3 second = UI.MouseCell();
							CellRect spectateRect = CellRect.FromLimits(firstCorner, second).ClipInsideMap(Find.CurrentMap);
							SpectateRectSide allowedSides = SpectateRectSide.All;
							if (bestSideOnly)
							{
								allowedSides = SpectatorCellFinder.FindSingleBestSide(spectateRect, Find.CurrentMap, SpectateRectSide.All, 1);
							}
							SpectatorCellFinder.DebugFlashPotentialSpectatorCells(spectateRect, Find.CurrentMap, allowedSides, 1);
							DebugTools.curTool = tool;
						});
					}
					DebugTools.curTool = new DebugTool(label, clickAction, firstCorner);
				}, null);
				DebugTools.curTool = tool;
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("All sides", DebugMenuOptionMode.Action, delegate()
			{
				act(false);
			}));
			list.Add(new DebugMenuOption("Best side only", DebugMenuOptionMode.Action, delegate()
			{
				act(true);
			}));
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x001131B0 File Offset: 0x001113B0
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CheckReachability()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			TraverseMode[] array = (TraverseMode[])Enum.GetValues(typeof(TraverseMode));
			for (int i = 0; i < array.Length; i++)
			{
				TraverseMode traverseMode2 = array[i];
				TraverseMode traverseMode = traverseMode2;
				list.Add(new DebugMenuOption(traverseMode2.ToString(), DebugMenuOptionMode.Action, delegate()
				{
					DebugTool tool = null;
					IntVec3 from;
					Pawn fromPawn;
					Action <>9__2;
					Action <>9__3;
					tool = new DebugTool("from...", delegate()
					{
						from = UI.MouseCell();
						fromPawn = from.GetFirstPawn(Find.CurrentMap);
						string text = "to...";
						if (fromPawn != null)
						{
							text = text + " (pawn=" + fromPawn.LabelShort + ")";
						}
						string label = text;
						Action clickAction;
						if ((clickAction = <>9__2) == null)
						{
							clickAction = (<>9__2 = delegate()
							{
								DebugTools.curTool = tool;
							});
						}
						Action onGUIAction;
						if ((onGUIAction = <>9__3) == null)
						{
							onGUIAction = (<>9__3 = delegate()
							{
								IntVec3 c = UI.MouseCell();
								bool flag;
								IntVec3 intVec;
								if (fromPawn != null)
								{
									flag = fromPawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, traverseMode);
									intVec = fromPawn.Position;
								}
								else
								{
									flag = Find.CurrentMap.reachability.CanReach(from, c, PathEndMode.OnCell, traverseMode, Danger.Deadly);
									intVec = from;
								}
								Color color = flag ? Color.green : Color.red;
								Widgets.DrawLine(intVec.ToUIPosition(), c.ToUIPosition(), color, 2f);
							});
						}
						DebugTools.curTool = new DebugTool(label, clickAction, onGUIAction);
					}, null);
					DebugTools.curTool = tool;
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x00113230 File Offset: 0x00111430
		[DebugAction("General", "Flash TryFindRandomPawnExitCell", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashTryFindRandomPawnExitCell(Pawn p)
		{
			IntVec3 intVec;
			if (CellFinder.TryFindRandomPawnExitCell(p, out intVec))
			{
				p.Map.debugDrawer.FlashCell(intVec, 0.5f, null, 50);
				p.Map.debugDrawer.FlashLine(p.Position, intVec, 50, SimpleColor.White);
				return;
			}
			p.Map.debugDrawer.FlashCell(p.Position, 0.2f, "no exit cell", 50);
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x001132A0 File Offset: 0x001114A0
		[DebugAction("General", "RandomSpotJustOutsideColony", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RandomSpotJustOutsideColony(Pawn p)
		{
			IntVec3 intVec;
			if (RCellFinder.TryFindRandomSpotJustOutsideColony(p, out intVec))
			{
				p.Map.debugDrawer.FlashCell(intVec, 0.5f, null, 50);
				p.Map.debugDrawer.FlashLine(p.Position, intVec, 50, SimpleColor.White);
				return;
			}
			p.Map.debugDrawer.FlashCell(p.Position, 0.2f, "no cell", 50);
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x00113310 File Offset: 0x00111510
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RandomSpotNearThingAvoidingHostiles()
		{
			List<Thing> list = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>();
			if (list.Count == 0)
			{
				return;
			}
			Thing thing = (from t in list
			where t is Pawn && t.Faction != null
			select t).FirstOrDefault<Thing>();
			if (thing == null)
			{
				thing = list.First<Thing>();
			}
			IntVec3 intVec;
			if (RCellFinder.TryFindRandomSpotNearAvoidingHostilePawns(thing, thing.Map, (IntVec3 s) => true, out intVec, 100f, 10f, 50f, true))
			{
				thing.Map.debugDrawer.FlashCell(intVec, 0.5f, null, 50);
				thing.Map.debugDrawer.FlashLine(thing.Position, intVec, 50, SimpleColor.White);
				return;
			}
			thing.Map.debugDrawer.FlashCell(thing.Position, 0.2f, "no cell", 50);
		}
	}
}
