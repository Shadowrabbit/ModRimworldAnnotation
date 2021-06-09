using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020000B6 RID: 182
	public static class CameraJumper
	{
		// Token: 0x060005B5 RID: 1461 RVA: 0x0000ADB1 File Offset: 0x00008FB1
		public static void TryJumpAndSelect(GlobalTargetInfo target)
		{
			if (!target.IsValid)
			{
				return;
			}
			CameraJumper.TryJump(target);
			CameraJumper.TrySelect(target);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0000ADC9 File Offset: 0x00008FC9
		public static void TrySelect(GlobalTargetInfo target)
		{
			if (!target.IsValid)
			{
				return;
			}
			target = CameraJumper.GetAdjustedTarget(target);
			if (target.HasThing)
			{
				CameraJumper.TrySelectInternal(target.Thing);
				return;
			}
			if (target.HasWorldObject)
			{
				CameraJumper.TrySelectInternal(target.WorldObject);
			}
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0008C334 File Offset: 0x0008A534
		private static void TrySelectInternal(Thing thing)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (thing.Spawned && thing.def.selectable)
			{
				bool flag = CameraJumper.TryHideWorld();
				bool flag2 = false;
				if (thing.Map != Find.CurrentMap)
				{
					Current.Game.CurrentMap = thing.Map;
					flag2 = true;
					if (!flag)
					{
						SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
					}
				}
				if (flag || flag2)
				{
					Find.CameraDriver.JumpToCurrentMapLoc(thing.Position);
				}
				Find.Selector.ClearSelection();
				Find.Selector.Select(thing, true, true);
			}
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0000AE08 File Offset: 0x00009008
		private static void TrySelectInternal(WorldObject worldObject)
		{
			if (Find.World == null)
			{
				return;
			}
			if (worldObject.Spawned && worldObject.SelectableNow)
			{
				CameraJumper.TryShowWorld();
				Find.WorldSelector.ClearSelection();
				Find.WorldSelector.Select(worldObject, true);
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0008C3C0 File Offset: 0x0008A5C0
		public static void TryJump(GlobalTargetInfo target)
		{
			if (!target.IsValid)
			{
				return;
			}
			target = CameraJumper.GetAdjustedTarget(target);
			if (target.HasThing)
			{
				CameraJumper.TryJumpInternal(target.Thing);
				return;
			}
			if (target.HasWorldObject)
			{
				CameraJumper.TryJumpInternal(target.WorldObject);
				return;
			}
			if (target.Cell.IsValid)
			{
				CameraJumper.TryJumpInternal(target.Cell, target.Map);
				return;
			}
			CameraJumper.TryJumpInternal(target.Tile);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0000AE3E File Offset: 0x0000903E
		public static void TryJump(IntVec3 cell, Map map)
		{
			CameraJumper.TryJump(new GlobalTargetInfo(cell, map, false));
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0000AE4D File Offset: 0x0000904D
		public static void TryJump(int tile)
		{
			CameraJumper.TryJump(new GlobalTargetInfo(tile));
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0008C43C File Offset: 0x0008A63C
		private static void TryJumpInternal(Thing thing)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			Map mapHeld = thing.MapHeld;
			if (mapHeld != null && Find.Maps.Contains(mapHeld) && thing.PositionHeld.IsValid && thing.PositionHeld.InBounds(mapHeld))
			{
				bool flag = CameraJumper.TryHideWorld();
				if (Find.CurrentMap != mapHeld)
				{
					Current.Game.CurrentMap = mapHeld;
					if (!flag)
					{
						SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
					}
				}
				Find.CameraDriver.JumpToCurrentMapLoc(thing.PositionHeld);
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0008C4C0 File Offset: 0x0008A6C0
		private static void TryJumpInternal(IntVec3 cell, Map map)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (!cell.IsValid)
			{
				return;
			}
			if (map == null || !Find.Maps.Contains(map))
			{
				return;
			}
			if (!cell.InBounds(map))
			{
				return;
			}
			bool flag = CameraJumper.TryHideWorld();
			if (Find.CurrentMap != map)
			{
				Current.Game.CurrentMap = map;
				if (!flag)
				{
					SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
				}
			}
			Find.CameraDriver.JumpToCurrentMapLoc(cell);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0000AE5A File Offset: 0x0000905A
		private static void TryJumpInternal(WorldObject worldObject)
		{
			if (Find.World == null)
			{
				return;
			}
			if (worldObject.Tile < 0)
			{
				return;
			}
			CameraJumper.TryShowWorld();
			Find.WorldCameraDriver.JumpTo(worldObject.Tile);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0000AE84 File Offset: 0x00009084
		private static void TryJumpInternal(int tile)
		{
			if (Find.World == null)
			{
				return;
			}
			if (tile < 0)
			{
				return;
			}
			CameraJumper.TryShowWorld();
			Find.WorldCameraDriver.JumpTo(tile);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0008C530 File Offset: 0x0008A730
		public static bool CanJump(GlobalTargetInfo target)
		{
			if (!target.IsValid)
			{
				return false;
			}
			target = CameraJumper.GetAdjustedTarget(target);
			if (target.HasThing)
			{
				return target.Thing.MapHeld != null && Find.Maps.Contains(target.Thing.MapHeld) && target.Thing.PositionHeld.IsValid && target.Thing.PositionHeld.InBounds(target.Thing.MapHeld);
			}
			if (target.HasWorldObject)
			{
				return target.WorldObject.Spawned;
			}
			if (target.Cell.IsValid)
			{
				return target.Map != null && Find.Maps.Contains(target.Map) && target.Cell.IsValid && target.Cell.InBounds(target.Map);
			}
			return target.Tile >= 0;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0008C62C File Offset: 0x0008A82C
		public static GlobalTargetInfo GetAdjustedTarget(GlobalTargetInfo target)
		{
			if (target.HasThing)
			{
				Thing thing = target.Thing;
				if (thing.Spawned)
				{
					return thing;
				}
				GlobalTargetInfo result = GlobalTargetInfo.Invalid;
				for (IThingHolder parentHolder = thing.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
				{
					Thing thing2 = parentHolder as Thing;
					if (thing2 != null && thing2.Spawned)
					{
						result = thing2;
						break;
					}
					ThingComp thingComp = parentHolder as ThingComp;
					if (thingComp != null && thingComp.parent.Spawned)
					{
						result = thingComp.parent;
						break;
					}
					WorldObject worldObject = parentHolder as WorldObject;
					if (worldObject != null && worldObject.Spawned)
					{
						result = worldObject;
						break;
					}
				}
				if (result.IsValid)
				{
					return result;
				}
				if (target.Thing.TryGetComp<CompCauseGameCondition>() != null)
				{
					List<Site> sites = Find.WorldObjects.Sites;
					for (int i = 0; i < sites.Count; i++)
					{
						for (int j = 0; j < sites[i].parts.Count; j++)
						{
							if (sites[i].parts[j].conditionCauser == target.Thing)
							{
								return sites[i];
							}
						}
					}
				}
				if (thing.Tile >= 0)
				{
					return new GlobalTargetInfo(thing.Tile);
				}
			}
			else if (target.Cell.IsValid && target.Tile >= 0 && target.Map != null && !Find.Maps.Contains(target.Map))
			{
				MapParent parent = target.Map.Parent;
				if (parent != null && parent.Spawned)
				{
					return parent;
				}
				return GlobalTargetInfo.Invalid;
			}
			return target;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0008C7E0 File Offset: 0x0008A9E0
		public static GlobalTargetInfo GetWorldTarget(GlobalTargetInfo target)
		{
			GlobalTargetInfo adjustedTarget = CameraJumper.GetAdjustedTarget(target);
			if (!adjustedTarget.IsValid)
			{
				return GlobalTargetInfo.Invalid;
			}
			if (adjustedTarget.IsWorldTarget)
			{
				return adjustedTarget;
			}
			return CameraJumper.GetWorldTargetOfMap(adjustedTarget.Map);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0000AEA4 File Offset: 0x000090A4
		public static GlobalTargetInfo GetWorldTargetOfMap(Map map)
		{
			if (map == null)
			{
				return GlobalTargetInfo.Invalid;
			}
			if (map.Parent != null && map.Parent.Spawned)
			{
				return map.Parent;
			}
			return GlobalTargetInfo.Invalid;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0008C81C File Offset: 0x0008AA1C
		public static bool TryHideWorld()
		{
			if (!WorldRendererUtility.WorldRenderedNow)
			{
				return false;
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			if (Find.World.renderer.wantedMode != WorldRenderMode.None)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.None;
				SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				return true;
			}
			return false;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0008C86C File Offset: 0x0008AA6C
		public static bool TryShowWorld()
		{
			if (WorldRendererUtility.WorldRenderedNow)
			{
				return true;
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			if (Find.World.renderer.wantedMode == WorldRenderMode.None)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				return true;
			}
			return false;
		}
	}
}
