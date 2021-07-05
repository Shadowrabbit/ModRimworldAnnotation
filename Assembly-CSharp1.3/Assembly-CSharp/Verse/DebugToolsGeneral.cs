using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.BaseGen;
using RimWorld.SketchGen;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020003A3 RID: 931
	public static class DebugToolsGeneral
	{
		// Token: 0x06001BB3 RID: 7091 RVA: 0x000A214C File Offset: 0x000A034C
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Destroy()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x000A21B0 File Offset: 0x000A03B0
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Discard()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.Destroy(DestroyMode.Vanish);
				Pawn p;
				if ((p = (thing as Pawn)) != null)
				{
					Find.WorldPawns.RemoveAndDiscardPawnViaGC(p);
				}
				else
				{
					thing.Discard(false);
				}
			}
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x000A2234 File Offset: 0x000A0434
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Kill()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.Kill(null, null);
			}
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x000A22A4 File Offset: 0x000A04A4
		[DebugAction("General", "10 damage", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Take10Damage()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x000A2328 File Offset: 0x000A0528
		[DebugAction("General", "5000 damage", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Take5000Damage()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 50000f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x000A23AC File Offset: 0x000A05AC
		[DebugAction("General", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RotateClockwise()
		{
			DebugTools.curTool = new DebugTool("Rotate clockwise", delegate()
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					thing.Rotation = thing.Rotation.Rotated(RotationDirection.Clockwise);
					thing.DirtyMapMesh(thing.Map);
				}
			}, null);
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x000A23DD File Offset: 0x000A05DD
		[DebugAction("General", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RotateCounterClockwise()
		{
			DebugTools.curTool = new DebugTool("Rotate counter clockwise", delegate()
			{
				foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
				{
					thing.Rotation = thing.Rotation.Rotated(RotationDirection.Counterclockwise);
					thing.DirtyMapMesh(thing.Map);
				}
			}, null);
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x000A240E File Offset: 0x000A060E
		[DebugAction("General", "Clear area 21x21", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ClearArea21x21()
		{
			GenDebug.ClearArea(CellRect.CenteredOn(UI.MouseCell(), 10), Find.CurrentMap);
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x000A2428 File Offset: 0x000A0628
		[DebugAction("General", "Make rock 11x11", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Rock21x21()
		{
			foreach (IntVec3 loc in CellRect.CenteredOn(UI.MouseCell(), 5).ClipInsideMap(Find.CurrentMap))
			{
				GenSpawn.Spawn(ThingDefOf.Granite, loc, Find.CurrentMap, WipeMode.Vanish);
			}
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x000A249C File Offset: 0x000A069C
		[DebugAction("General", "Explosion (bomb)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionBomb()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.Bomb, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x000A24EC File Offset: 0x000A06EC
		[DebugAction("General", "Explosion (flame)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionFlame()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x000A253C File Offset: 0x000A073C
		[DebugAction("General", "Explosion (stun)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionStun()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.Stun, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x000A258C File Offset: 0x000A078C
		[DebugAction("General", "Explosion (EMP)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionEMP()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 3.9f, DamageDefOf.EMP, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x000A25DC File Offset: 0x000A07DC
		[DebugAction("General", "Explosion (extinguisher)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionExtinguisher()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 10f, DamageDefOf.Extinguish, null, -1, -1f, null, null, null, null, ThingDefOf.Filth_FireFoam, 1f, 3, true, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x000A2630 File Offset: 0x000A0830
		[DebugAction("General", "Explosion (smoke)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ExplosionSmoke()
		{
			GenExplosion.DoExplosion(UI.MouseCell(), Find.CurrentMap, 10f, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, ThingDefOf.Gas_Smoke, 1f, 1, false, null, 0f, 1, 0f, false, null, null);
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x000A2684 File Offset: 0x000A0884
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void LightningStrike()
		{
			Find.CurrentMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(Find.CurrentMap, UI.MouseCell()));
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x000A26A9 File Offset: 0x000A08A9
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddSnow()
		{
			SnowUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, 1f);
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x000A26C4 File Offset: 0x000A08C4
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveSnow()
		{
			SnowUtility.AddSnowRadial(UI.MouseCell(), Find.CurrentMap, 5f, -1f);
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x000A26E0 File Offset: 0x000A08E0
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ClearAllSnow()
		{
			foreach (IntVec3 c in Find.CurrentMap.AllCells)
			{
				Find.CurrentMap.snowGrid.SetDepth(c, 0f);
			}
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x000A2740 File Offset: 0x000A0940
		[DebugAction("General", "Push heat (10)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PushHeat10()
		{
			foreach (IntVec3 intVec in Find.CurrentMap.AllCells)
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, 10f);
			}
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x000A27A0 File Offset: 0x000A09A0
		[DebugAction("General", "Push heat (1000)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PushHeat1000()
		{
			foreach (IntVec3 intVec in Find.CurrentMap.AllCells)
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, 1000f);
			}
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x000A2800 File Offset: 0x000A0A00
		[DebugAction("General", "Push heat (-1000)", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PushHeatNeg1000()
		{
			foreach (IntVec3 intVec in Find.CurrentMap.AllCells)
			{
				GenTemperature.PushHeat(UI.MouseCell(), Find.CurrentMap, -1000f);
			}
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x000A2860 File Offset: 0x000A0A60
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

		// Token: 0x06001BCA RID: 7114 RVA: 0x000A28C8 File Offset: 0x000A0AC8
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

		// Token: 0x06001BCB RID: 7115 RVA: 0x000A29A0 File Offset: 0x000A0BA0
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

		// Token: 0x06001BCC RID: 7116 RVA: 0x000A2A1C File Offset: 0x000A0C1C
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RegenSection()
		{
			Find.CurrentMap.mapDrawer.SectionAt(UI.MouseCell()).RegenerateAllLayers();
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000A2A38 File Offset: 0x000A0C38
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetColor()
		{
			DebugToolsGeneral.<>c__DisplayClass26_0 CS$<>8__locals1 = new DebugToolsGeneral.<>c__DisplayClass26_0();
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			CS$<>8__locals1.cell = UI.MouseCell();
			using (List<Ideo>.Enumerator enumerator = Find.IdeoManager.IdeosListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ideo i = enumerator.Current;
					list.Add(new FloatMenuOption(i.name, delegate()
					{
						CS$<>8__locals1.<SetColor>g__SetColor_All|0(i.Color);
					}, i.Icon, i.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			using (IEnumerator<ColorDef> enumerator2 = DefDatabase<ColorDef>.AllDefs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ColorDef c = enumerator2.Current;
					list.Add(new FloatMenuOption(c.defName, delegate()
					{
						CS$<>8__locals1.<SetColor>g__SetColor_All|0(c.color);
					}, BaseContent.WhiteTex, c.color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x000A2B9C File Offset: 0x000A0D9C
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

		// Token: 0x06001BCF RID: 7119 RVA: 0x000A2C04 File Offset: 0x000A0E04
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

		// Token: 0x06001BD0 RID: 7120 RVA: 0x000A2C74 File Offset: 0x000A0E74
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

		// Token: 0x06001BD1 RID: 7121 RVA: 0x000A2D20 File Offset: 0x000A0F20
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

		// Token: 0x06001BD2 RID: 7122 RVA: 0x000A2D94 File Offset: 0x000A0F94
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

		// Token: 0x06001BD3 RID: 7123 RVA: 0x000A2E00 File Offset: 0x000A1000
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void UseScatterer()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_MapGen.Options_Scatterers()));
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x000A2E18 File Offset: 0x000A1018
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

		// Token: 0x06001BD5 RID: 7125 RVA: 0x000A2EC4 File Offset: 0x000A10C4
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

		// Token: 0x06001BD6 RID: 7126 RVA: 0x000A3028 File Offset: 0x000A1228
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeRoof()
		{
			foreach (IntVec3 c in CellRect.CenteredOn(UI.MouseCell(), 1))
			{
				Find.CurrentMap.roofGrid.SetRoof(c, RoofDefOf.RoofConstructed);
			}
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x000A3094 File Offset: 0x000A1294
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DeleteRoof()
		{
			foreach (IntVec3 c in CellRect.CenteredOn(UI.MouseCell(), 1))
			{
				Find.CurrentMap.roofGrid.SetRoof(c, null);
			}
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x000A30FC File Offset: 0x000A12FC
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TestFloodUnfog()
		{
			FloodFillerFog.DebugFloodUnfog(UI.MouseCell(), Find.CurrentMap);
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x000A3110 File Offset: 0x000A1310
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashClosewalkCell30()
		{
			IntVec3 c = CellFinder.RandomClosewalkCellNear(UI.MouseCell(), Find.CurrentMap, 30, null);
			Find.CurrentMap.debugDrawer.FlashCell(c, 0f, null, 50);
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x000A3148 File Offset: 0x000A1348
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashWalkPath()
		{
			WalkPathFinder.DebugFlashWalkPath(UI.MouseCell(), 8);
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x000A3158 File Offset: 0x000A1358
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void FlashSkygazeCell()
		{
			Pawn pawn = Find.CurrentMap.mapPawns.FreeColonists.First<Pawn>();
			IntVec3 c;
			RCellFinder.TryFindSkygazeCell(UI.MouseCell(), pawn, out c);
			Find.CurrentMap.debugDrawer.FlashCell(c, 0f, null, 50);
			MoteMaker.ThrowText(c.ToVector3Shifted(), Find.CurrentMap, "for " + pawn.Label, Color.white, -1f);
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x000A31CC File Offset: 0x000A13CC
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

		// Token: 0x06001BDD RID: 7133 RVA: 0x000A325C File Offset: 0x000A145C
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
								allowedSides = SpectatorCellFinder.FindSingleBestSide(spectateRect, Find.CurrentMap, SpectateRectSide.All, 1, null);
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

		// Token: 0x06001BDE RID: 7134 RVA: 0x000A32E4 File Offset: 0x000A14E4
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
									flag = fromPawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, false, traverseMode);
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

		// Token: 0x06001BDF RID: 7135 RVA: 0x000A3364 File Offset: 0x000A1564
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

		// Token: 0x06001BE0 RID: 7136 RVA: 0x000A33D4 File Offset: 0x000A15D4
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

		// Token: 0x06001BE1 RID: 7137 RVA: 0x000A3444 File Offset: 0x000A1644
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

		// Token: 0x06001BE2 RID: 7138 RVA: 0x000A353E File Offset: 0x000A173E
		[DebugAction("General", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ClearAllFog()
		{
			Find.CurrentMap.fogGrid.ClearAllFog();
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x000A3550 File Offset: 0x000A1750
		[DebugAction("General", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ChangeThingStyle()
		{
			Thing thing = Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).FirstOrDefault((Thing x) => x.def.CanBeStyled());
			if (thing == null)
			{
				return;
			}
			DebugToolsGeneral.tmpStyleDefs.Clear();
			if (!thing.def.randomStyle.NullOrEmpty<ThingStyleChance>())
			{
				foreach (ThingStyleChance thingStyleChance in thing.def.randomStyle)
				{
					if (thingStyleChance.StyleDef.graphicData != null)
					{
						DebugToolsGeneral.tmpStyleDefs.Add(thingStyleChance.StyleDef);
					}
				}
			}
			IEnumerable<StyleCategoryDef> allDefs = DefDatabase<StyleCategoryDef>.AllDefs;
			Func<StyleCategoryDef, bool> <>9__1;
			Func<StyleCategoryDef, bool> predicate;
			if ((predicate = <>9__1) == null)
			{
				Predicate<ThingDefStyle> <>9__2;
				predicate = (<>9__1 = delegate(StyleCategoryDef x)
				{
					List<ThingDefStyle> thingDefStyles = x.thingDefStyles;
					Predicate<ThingDefStyle> predicate2;
					if ((predicate2 = <>9__2) == null)
					{
						predicate2 = (<>9__2 = ((ThingDefStyle y) => y.ThingDef == thing.def));
					}
					return thingDefStyles.Any(predicate2);
				});
			}
			foreach (StyleCategoryDef styleCategoryDef in allDefs.Where(predicate))
			{
				DebugToolsGeneral.tmpStyleDefs.Add(styleCategoryDef.GetStyleForThingDef(thing.def, null));
			}
			if (DebugToolsGeneral.tmpStyleDefs.Any<ThingStyleDef>())
			{
				DebugToolsGeneral.<>c__DisplayClass49_1 CS$<>8__locals2;
				CS$<>8__locals2.opts = new List<DebugMenuOption>();
				DebugToolsGeneral.<ChangeThingStyle>g__AddOption|49_3(thing, () => null, "Standard", ref CS$<>8__locals2);
				Func<ThingStyleDef, float> <>9__7;
				DebugToolsGeneral.<ChangeThingStyle>g__AddOption|49_3(thing, delegate
				{
					IEnumerable<ThingStyleDef> source = DebugToolsGeneral.tmpStyleDefs;
					Func<ThingStyleDef, float> weightSelector;
					if ((weightSelector = <>9__7) == null)
					{
						weightSelector = (<>9__7 = delegate(ThingStyleDef x)
						{
							if (x != thing.StyleDef)
							{
								return 1f;
							}
							return 0.01f;
						});
					}
					return source.RandomElementByWeight(weightSelector);
				}, "Random", ref CS$<>8__locals2);
				using (List<ThingStyleDef>.Enumerator enumerator3 = DebugToolsGeneral.tmpStyleDefs.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						ThingStyleDef s = enumerator3.Current;
						DebugToolsGeneral.<ChangeThingStyle>g__AddOption|49_3(thing, () => s, s.defName, ref CS$<>8__locals2);
					}
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(CS$<>8__locals2.opts));
			}
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x000A3798 File Offset: 0x000A1998
		[DebugAction("General", "Force sleep then assault colony", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceSleepThenAssaultColony()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					Lord lord = pawn.GetLord();
					if (lord != null)
					{
						lord.Notify_PawnLost(pawn, PawnLostCondition.ForcedByPlayerAction, null);
					}
					pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					LordJob_SleepThenAssaultColony lordJob = new LordJob_SleepThenAssaultColony(pawn.Faction, true);
					LordMaker.MakeNewLord(pawn.Faction, lordJob, pawn.Map, null).AddPawn(pawn);
				}
			}
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x000A385C File Offset: 0x000A1A5C
		[CompilerGenerated]
		internal static void <ChangeThingStyle>g__AddOption|49_3(Thing t, Func<ThingStyleDef> styleSelector, string label, ref DebugToolsGeneral.<>c__DisplayClass49_1 A_3)
		{
			A_3.opts.Add(new DebugMenuOption(label, DebugMenuOptionMode.Action, delegate()
			{
				t.StyleDef = styleSelector();
				t.DirtyMapMesh(t.Map);
			}));
		}

		// Token: 0x040011A5 RID: 4517
		private static List<ThingStyleDef> tmpStyleDefs = new List<ThingStyleDef>();
	}
}
