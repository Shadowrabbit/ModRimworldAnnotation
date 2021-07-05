using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000069 RID: 105
	public static class CameraJumper
	{
		// Token: 0x06000447 RID: 1095 RVA: 0x0001672B File Offset: 0x0001492B
		public static void TryJumpAndSelect(GlobalTargetInfo target)
		{
			if (!target.IsValid)
			{
				return;
			}
			CameraJumper.TryJump(target);
			CameraJumper.TrySelect(target);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00016743 File Offset: 0x00014943
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

		// Token: 0x06000449 RID: 1097 RVA: 0x00016784 File Offset: 0x00014984
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

		// Token: 0x0600044A RID: 1098 RVA: 0x00016810 File Offset: 0x00014A10
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

		// Token: 0x0600044B RID: 1099 RVA: 0x00016848 File Offset: 0x00014A48
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

		// Token: 0x0600044C RID: 1100 RVA: 0x000168C4 File Offset: 0x00014AC4
		public static void TryJump(IntVec3 cell, Map map)
		{
			CameraJumper.TryJump(new GlobalTargetInfo(cell, map, false));
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x000168D3 File Offset: 0x00014AD3
		public static void TryJump(int tile)
		{
			CameraJumper.TryJump(new GlobalTargetInfo(tile));
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x000168E0 File Offset: 0x00014AE0
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

		// Token: 0x0600044F RID: 1103 RVA: 0x00016964 File Offset: 0x00014B64
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

		// Token: 0x06000450 RID: 1104 RVA: 0x000169D1 File Offset: 0x00014BD1
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

		// Token: 0x06000451 RID: 1105 RVA: 0x000169FB File Offset: 0x00014BFB
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

		// Token: 0x06000452 RID: 1106 RVA: 0x00016A1C File Offset: 0x00014C1C
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

		// Token: 0x06000453 RID: 1107 RVA: 0x00016B18 File Offset: 0x00014D18
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

		// Token: 0x06000454 RID: 1108 RVA: 0x00016CCC File Offset: 0x00014ECC
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

		// Token: 0x06000455 RID: 1109 RVA: 0x00016D06 File Offset: 0x00014F06
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

		// Token: 0x06000456 RID: 1110 RVA: 0x00016D38 File Offset: 0x00014F38
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

		// Token: 0x06000457 RID: 1111 RVA: 0x00016D88 File Offset: 0x00014F88
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
