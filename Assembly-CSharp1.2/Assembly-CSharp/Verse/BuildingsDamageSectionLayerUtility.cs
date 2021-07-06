using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000293 RID: 659
	[StaticConstructorOnStartup]
	public static class BuildingsDamageSectionLayerUtility
	{
		// Token: 0x06001117 RID: 4375 RVA: 0x000BD858 File Offset: 0x000BBA58
		public static void Notify_BuildingHitPointsChanged(Building b, int oldHitPoints)
		{
			if (!b.Spawned || !b.def.useHitPoints || b.HitPoints == oldHitPoints || !b.def.drawDamagedOverlay || BuildingsDamageSectionLayerUtility.GetDamageOverlaysCount(b, b.HitPoints) == BuildingsDamageSectionLayerUtility.GetDamageOverlaysCount(b, oldHitPoints))
			{
				return;
			}
			b.Map.mapDrawer.MapMeshDirty(b.Position, MapMeshFlag.BuildingsDamage);
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0001280A File Offset: 0x00010A0A
		public static bool UsesLinkableCornersAndEdges(Building b)
		{
			return b.def.size.x == 1 && b.def.size.z == 1 && b.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x000BD8C4 File Offset: 0x000BBAC4
		public static IList<Material> GetScratchMats(Building b)
		{
			IList<Material> result = BuildingsDamageSectionLayerUtility.DefaultScratchMats;
			if (b.def.graphicData != null && b.def.graphicData.damageData != null && b.def.graphicData.damageData.scratchMats != null)
			{
				result = b.def.graphicData.damageData.scratchMats;
			}
			return result;
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x000BD924 File Offset: 0x000BBB24
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

		// Token: 0x0600111B RID: 4379 RVA: 0x000BDCFC File Offset: 0x000BBEFC
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

		// Token: 0x0600111C RID: 4380 RVA: 0x000BDE08 File Offset: 0x000BC008
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

		// Token: 0x0600111D RID: 4381 RVA: 0x000BDEB8 File Offset: 0x000BC0B8
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

		// Token: 0x0600111E RID: 4382 RVA: 0x000BE25C File Offset: 0x000BC45C
		private static int GetDamageOverlaysCount(Building b, int hp)
		{
			float num = (float)hp / (float)b.MaxHitPoints;
			int count = BuildingsDamageSectionLayerUtility.GetAvailableOverlays(b).Count;
			return count - Mathf.FloorToInt((float)count * num);
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x000BE28C File Offset: 0x000BC48C
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

		// Token: 0x06001120 RID: 4384 RVA: 0x000BE2E8 File Offset: 0x000BC4E8
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

		// Token: 0x06001121 RID: 4385 RVA: 0x000BE360 File Offset: 0x000BC560
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

		// Token: 0x04000DF2 RID: 3570
		private static readonly Material[] DefaultScratchMats = new Material[]
		{
			MaterialPool.MatFrom("Damage/Scratch1", ShaderDatabase.Transparent),
			MaterialPool.MatFrom("Damage/Scratch2", ShaderDatabase.Transparent),
			MaterialPool.MatFrom("Damage/Scratch3", ShaderDatabase.Transparent)
		};

		// Token: 0x04000DF3 RID: 3571
		private static List<DamageOverlay> availableOverlays = new List<DamageOverlay>();

		// Token: 0x04000DF4 RID: 3572
		private static List<DamageOverlay> overlaysWorkingList = new List<DamageOverlay>();

		// Token: 0x04000DF5 RID: 3573
		private static List<DamageOverlay> overlays = new List<DamageOverlay>();
	}
}
