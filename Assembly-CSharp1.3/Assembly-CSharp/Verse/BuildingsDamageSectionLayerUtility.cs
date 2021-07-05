using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001D2 RID: 466
	[StaticConstructorOnStartup]
	public static class BuildingsDamageSectionLayerUtility
	{
		// Token: 0x06000D66 RID: 3430 RVA: 0x000489D8 File Offset: 0x00046BD8
		public static void TryInsertIntoAtlas()
		{
			for (int i = 0; i < BuildingsDamageSectionLayerUtility.DefaultScratchMats.Length; i++)
			{
				GlobalTextureAtlasManager.TryInsertStatic(TextureAtlasGroup.Building, (Texture2D)BuildingsDamageSectionLayerUtility.DefaultScratchMats[i].mainTexture, null);
			}
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00048A10 File Offset: 0x00046C10
		public static void Notify_BuildingHitPointsChanged(Building b, int oldHitPoints)
		{
			if (!b.Spawned || !b.def.useHitPoints || b.HitPoints == oldHitPoints || !b.def.drawDamagedOverlay || BuildingsDamageSectionLayerUtility.GetDamageOverlaysCount(b, b.HitPoints) == BuildingsDamageSectionLayerUtility.GetDamageOverlaysCount(b, oldHitPoints))
			{
				return;
			}
			b.Map.mapDrawer.MapMeshDirty(b.Position, MapMeshFlag.BuildingsDamage);
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x00048A79 File Offset: 0x00046C79
		public static bool UsesLinkableCornersAndEdges(Building b)
		{
			return b.def.size.x == 1 && b.def.size.z == 1 && b.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x00048AB4 File Offset: 0x00046CB4
		public static IList<Material> GetScratchMats(Building b)
		{
			IList<Material> result = BuildingsDamageSectionLayerUtility.DefaultScratchMats;
			if (b.def.graphicData != null && b.def.graphicData.damageData != null && b.def.graphicData.damageData.scratchMats != null)
			{
				result = b.def.graphicData.damageData.scratchMats;
			}
			return result;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x00048B14 File Offset: 0x00046D14
		public static List<DamageOverlay> GetAvailableOverlays(Building b)
		{
			BuildingsDamageSectionLayerUtility.availableOverlays.Clear();
			if (BuildingsDamageSectionLayerUtility.GetScratchMats(b).Any<Material>())
			{
				int num = 3;
				Rect damageRect = BuildingsDamageSectionLayerUtility.GetDamageRect(b);
				float num2 = damageRect.width * damageRect.height;
				if (num2 > 4f)
				{
					num += Mathf.RoundToInt((num2 - 4f) * 0.54f);
				}
				for (int i = 0; i < num; i++)
				{
					BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.Scratch);
				}
			}
			if (BuildingsDamageSectionLayerUtility.UsesLinkableCornersAndEdges(b))
			{
				if (b.def.graphicData != null && b.def.graphicData.damageData != null)
				{
					IntVec3 position = b.Position;
					DamageGraphicData damageData = b.def.graphicData.damageData;
					if (damageData.edgeTopMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z + 1) && BuildingsDamageSectionLayerUtility.SameAndDamagedAt(b, position.x + 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z + 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.TopEdge);
					}
					if (damageData.edgeRightMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z) && BuildingsDamageSectionLayerUtility.SameAndDamagedAt(b, position.x, position.z + 1) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z + 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.RightEdge);
					}
					if (damageData.edgeBotMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z - 1) && BuildingsDamageSectionLayerUtility.SameAndDamagedAt(b, position.x + 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z - 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.BotEdge);
					}
					if (damageData.edgeLeftMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x - 1, position.z) && BuildingsDamageSectionLayerUtility.SameAndDamagedAt(b, position.x, position.z + 1) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x - 1, position.z + 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.LeftEdge);
					}
					if (damageData.cornerTLMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x - 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z + 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.TopLeftCorner);
					}
					if (damageData.cornerTRMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z + 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.TopRightCorner);
					}
					if (damageData.cornerBRMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x + 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z - 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.BotRightCorner);
					}
					if (damageData.cornerBLMat != null && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x - 1, position.z) && BuildingsDamageSectionLayerUtility.DifferentAt(b, position.x, position.z - 1))
					{
						BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.BotLeftCorner);
					}
				}
			}
			else
			{
				Material x;
				Material x2;
				Material x3;
				Material x4;
				BuildingsDamageSectionLayerUtility.GetCornerMats(out x, out x2, out x3, out x4, b);
				if (x != null)
				{
					BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.TopLeftCorner);
				}
				if (x2 != null)
				{
					BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.TopRightCorner);
				}
				if (x4 != null)
				{
					BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.BotLeftCorner);
				}
				if (x3 != null)
				{
					BuildingsDamageSectionLayerUtility.availableOverlays.Add(DamageOverlay.BotRightCorner);
				}
			}
			return BuildingsDamageSectionLayerUtility.availableOverlays;
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00048EEC File Offset: 0x000470EC
		public static void GetCornerMats(out Material topLeft, out Material topRight, out Material botRight, out Material botLeft, Building b)
		{
			if (b.def.graphicData == null || b.def.graphicData.damageData == null)
			{
				topLeft = null;
				topRight = null;
				botRight = null;
				botLeft = null;
				return;
			}
			DamageGraphicData damageData = b.def.graphicData.damageData;
			if (b.Rotation == Rot4.North)
			{
				topLeft = damageData.cornerTLMat;
				topRight = damageData.cornerTRMat;
				botRight = damageData.cornerBRMat;
				botLeft = damageData.cornerBLMat;
				return;
			}
			if (b.Rotation == Rot4.East)
			{
				topLeft = damageData.cornerBLMat;
				topRight = damageData.cornerTLMat;
				botRight = damageData.cornerTRMat;
				botLeft = damageData.cornerBRMat;
				return;
			}
			if (b.Rotation == Rot4.South)
			{
				topLeft = damageData.cornerBRMat;
				topRight = damageData.cornerBLMat;
				botRight = damageData.cornerTLMat;
				botLeft = damageData.cornerTRMat;
				return;
			}
			topLeft = damageData.cornerTRMat;
			topRight = damageData.cornerBRMat;
			botRight = damageData.cornerBLMat;
			botLeft = damageData.cornerTLMat;
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00048FF8 File Offset: 0x000471F8
		public static List<DamageOverlay> GetOverlays(Building b)
		{
			BuildingsDamageSectionLayerUtility.overlays.Clear();
			BuildingsDamageSectionLayerUtility.overlaysWorkingList.Clear();
			BuildingsDamageSectionLayerUtility.overlaysWorkingList.AddRange(BuildingsDamageSectionLayerUtility.GetAvailableOverlays(b));
			if (!BuildingsDamageSectionLayerUtility.overlaysWorkingList.Any<DamageOverlay>())
			{
				return BuildingsDamageSectionLayerUtility.overlays;
			}
			Rand.PushState();
			Rand.Seed = Gen.HashCombineInt(b.thingIDNumber, 1958376471);
			int damageOverlaysCount = BuildingsDamageSectionLayerUtility.GetDamageOverlaysCount(b, b.HitPoints);
			int num = 0;
			while (num < damageOverlaysCount && BuildingsDamageSectionLayerUtility.overlaysWorkingList.Any<DamageOverlay>())
			{
				DamageOverlay item = BuildingsDamageSectionLayerUtility.overlaysWorkingList.RandomElement<DamageOverlay>();
				BuildingsDamageSectionLayerUtility.overlaysWorkingList.Remove(item);
				BuildingsDamageSectionLayerUtility.overlays.Add(item);
				num++;
			}
			Rand.PopState();
			return BuildingsDamageSectionLayerUtility.overlays;
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x000490A8 File Offset: 0x000472A8
		public static Rect GetDamageRect(Building b)
		{
			DamageGraphicData damageGraphicData = null;
			if (b.def.graphicData != null)
			{
				damageGraphicData = b.def.graphicData.damageData;
			}
			CellRect cellRect = b.OccupiedRect();
			Rect result = new Rect((float)cellRect.minX, (float)cellRect.minZ, (float)cellRect.Width, (float)cellRect.Height);
			if (damageGraphicData != null)
			{
				if (b.Rotation == Rot4.North && damageGraphicData.rectN != default(Rect))
				{
					result.position += damageGraphicData.rectN.position;
					result.size = damageGraphicData.rectN.size;
				}
				else if (b.Rotation == Rot4.East && damageGraphicData.rectE != default(Rect))
				{
					result.position += damageGraphicData.rectE.position;
					result.size = damageGraphicData.rectE.size;
				}
				else if (b.Rotation == Rot4.South && damageGraphicData.rectS != default(Rect))
				{
					result.position += damageGraphicData.rectS.position;
					result.size = damageGraphicData.rectS.size;
				}
				else if (b.Rotation == Rot4.West && damageGraphicData.rectW != default(Rect))
				{
					result.position += damageGraphicData.rectW.position;
					result.size = damageGraphicData.rectW.size;
				}
				else if (damageGraphicData.rect != default(Rect))
				{
					Rect rect = damageGraphicData.rect;
					if (b.Rotation == Rot4.North)
					{
						result.x += rect.x;
						result.y += rect.y;
						result.width = rect.width;
						result.height = rect.height;
					}
					else if (b.Rotation == Rot4.South)
					{
						result.x += (float)cellRect.Width - rect.x - rect.width;
						result.y += (float)cellRect.Height - rect.y - rect.height;
						result.width = rect.width;
						result.height = rect.height;
					}
					else if (b.Rotation == Rot4.West)
					{
						result.x += (float)cellRect.Width - rect.y - rect.height;
						result.y += rect.x;
						result.width = rect.height;
						result.height = rect.width;
					}
					else if (b.Rotation == Rot4.East)
					{
						result.x += rect.y;
						result.y += (float)cellRect.Height - rect.x - rect.width;
						result.width = rect.height;
						result.height = rect.width;
					}
				}
			}
			return result;
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0004944C File Offset: 0x0004764C
		private static int GetDamageOverlaysCount(Building b, int hp)
		{
			float num = (float)hp / (float)b.MaxHitPoints;
			int count = BuildingsDamageSectionLayerUtility.GetAvailableOverlays(b).Count;
			return count - Mathf.FloorToInt((float)count * num);
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0004947C File Offset: 0x0004767C
		private static bool DifferentAt(Building b, int x, int z)
		{
			IntVec3 c = new IntVec3(x, 0, z);
			if (!c.InBounds(b.Map))
			{
				return true;
			}
			List<Thing> thingList = c.GetThingList(b.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == b.def)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x000494D8 File Offset: 0x000476D8
		private static bool SameAndDamagedAt(Building b, int x, int z)
		{
			IntVec3 c = new IntVec3(x, 0, z);
			if (!c.InBounds(b.Map))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(b.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == b.def && thingList[i].HitPoints < thingList[i].MaxHitPoints)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x00049550 File Offset: 0x00047750
		public static void DebugDraw()
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawDamageRects || Find.CurrentMap == null)
			{
				return;
			}
			Building building = Find.Selector.FirstSelectedObject as Building;
			if (building == null)
			{
				return;
			}
			Material material = DebugSolidColorMats.MaterialOf(Color.red);
			Rect damageRect = BuildingsDamageSectionLayerUtility.GetDamageRect(building);
			float y = 14.99f;
			Vector3 pos = new Vector3(damageRect.x + damageRect.width / 2f, y, damageRect.y + damageRect.height / 2f);
			Vector3 s = new Vector3(damageRect.width, 1f, damageRect.height);
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(pos, Quaternion.identity, s), material, 0);
		}

		// Token: 0x04000B14 RID: 2836
		private static readonly Material[] DefaultScratchMats = new Material[]
		{
			MaterialPool.MatFrom("Damage/Scratch1", ShaderDatabase.Transparent),
			MaterialPool.MatFrom("Damage/Scratch2", ShaderDatabase.Transparent),
			MaterialPool.MatFrom("Damage/Scratch3", ShaderDatabase.Transparent)
		};

		// Token: 0x04000B15 RID: 2837
		private static List<DamageOverlay> availableOverlays = new List<DamageOverlay>();

		// Token: 0x04000B16 RID: 2838
		private static List<DamageOverlay> overlaysWorkingList = new List<DamageOverlay>();

		// Token: 0x04000B17 RID: 2839
		private static List<DamageOverlay> overlays = new List<DamageOverlay>();
	}
}
