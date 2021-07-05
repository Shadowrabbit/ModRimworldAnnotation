using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200148A RID: 5258
	public static class GenWorld
	{
		// Token: 0x06007DCA RID: 32202 RVA: 0x002C91C0 File Offset: 0x002C73C0
		public static int MouseTile(bool snapToExpandableWorldObjects = false)
		{
			if (snapToExpandableWorldObjects)
			{
				if (GenWorld.cachedFrame_snap == Time.frameCount)
				{
					return GenWorld.cachedTile_snap;
				}
				GenWorld.cachedTile_snap = GenWorld.TileAt(UI.MousePositionOnUI, true);
				GenWorld.cachedFrame_snap = Time.frameCount;
				return GenWorld.cachedTile_snap;
			}
			else
			{
				if (GenWorld.cachedFrame_noSnap == Time.frameCount)
				{
					return GenWorld.cachedTile_noSnap;
				}
				GenWorld.cachedTile_noSnap = GenWorld.TileAt(UI.MousePositionOnUI, false);
				GenWorld.cachedFrame_noSnap = Time.frameCount;
				return GenWorld.cachedTile_noSnap;
			}
		}

		// Token: 0x06007DCB RID: 32203 RVA: 0x002C9234 File Offset: 0x002C7434
		public static int TileAt(Vector2 clickPos, bool snapToExpandableWorldObjects = false)
		{
			Camera worldCamera = Find.WorldCamera;
			if (!worldCamera.gameObject.activeInHierarchy)
			{
				return -1;
			}
			if (snapToExpandableWorldObjects)
			{
				ExpandableWorldObjectsUtility.GetExpandedWorldObjectUnderMouse(UI.MousePositionOnUI, GenWorld.tmpWorldObjectsUnderMouse);
				if (GenWorld.tmpWorldObjectsUnderMouse.Any<WorldObject>())
				{
					int tile = GenWorld.tmpWorldObjectsUnderMouse[0].Tile;
					GenWorld.tmpWorldObjectsUnderMouse.Clear();
					return tile;
				}
			}
			Ray ray = worldCamera.ScreenPointToRay(clickPos * Prefs.UIScale);
			int worldLayerMask = WorldCameraManager.WorldLayerMask;
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 1500f, worldLayerMask))
			{
				return Find.World.renderer.GetTileIDFromRayHit(hit);
			}
			return -1;
		}

		// Token: 0x04004E6C RID: 20076
		private static int cachedTile_noSnap = -1;

		// Token: 0x04004E6D RID: 20077
		private static int cachedFrame_noSnap = -1;

		// Token: 0x04004E6E RID: 20078
		private static int cachedTile_snap = -1;

		// Token: 0x04004E6F RID: 20079
		private static int cachedFrame_snap = -1;

		// Token: 0x04004E70 RID: 20080
		public const float MaxRayLength = 1500f;

		// Token: 0x04004E71 RID: 20081
		private static List<WorldObject> tmpWorldObjectsUnderMouse = new List<WorldObject>();
	}
}
