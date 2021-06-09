using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CC9 RID: 7369
	public static class GenWorld
	{
		// Token: 0x0600A046 RID: 41030 RVA: 0x002EE6AC File Offset: 0x002EC8AC
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

		// Token: 0x0600A047 RID: 41031 RVA: 0x002EE720 File Offset: 0x002EC920
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

		// Token: 0x04006CE8 RID: 27880
		private static int cachedTile_noSnap = -1;

		// Token: 0x04006CE9 RID: 27881
		private static int cachedFrame_noSnap = -1;

		// Token: 0x04006CEA RID: 27882
		private static int cachedTile_snap = -1;

		// Token: 0x04006CEB RID: 27883
		private static int cachedFrame_snap = -1;

		// Token: 0x04006CEC RID: 27884
		public const float MaxRayLength = 1500f;

		// Token: 0x04006CED RID: 27885
		private static List<WorldObject> tmpWorldObjectsUnderMouse = new List<WorldObject>();
	}
}
