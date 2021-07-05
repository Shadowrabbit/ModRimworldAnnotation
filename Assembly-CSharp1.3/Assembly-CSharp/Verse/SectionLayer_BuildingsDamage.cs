using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001D3 RID: 467
	public class SectionLayer_BuildingsDamage : SectionLayer
	{
		// Token: 0x06000D73 RID: 3443 RVA: 0x00049674 File Offset: 0x00047874
		public SectionLayer_BuildingsDamage(Section section) : base(section)
		{
			this.relevantChangeTypes = (MapMeshFlag.Buildings | MapMeshFlag.BuildingsDamage);
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00049688 File Offset: 0x00047888
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			foreach (IntVec3 intVec in this.section.CellRect)
			{
				List<Thing> list = base.Map.thingGrid.ThingsListAt(intVec);
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					Building building = list[i] as Building;
					if (building != null && building.def.useHitPoints && building.HitPoints < building.MaxHitPoints && building.def.drawDamagedOverlay && building.Position.x == intVec.x && building.Position.z == intVec.z)
					{
						this.PrintDamageVisualsFrom(building);
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0004978C File Offset: 0x0004798C
		private void PrintDamageVisualsFrom(Building b)
		{
			if (b.def.graphicData != null && b.def.graphicData.damageData != null && !b.def.graphicData.damageData.enabled)
			{
				return;
			}
			this.PrintScratches(b);
			this.PrintCornersAndEdges(b);
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x000497E0 File Offset: 0x000479E0
		private void PrintScratches(Building b)
		{
			int num = 0;
			List<DamageOverlay> overlays = BuildingsDamageSectionLayerUtility.GetOverlays(b);
			for (int i = 0; i < overlays.Count; i++)
			{
				if (overlays[i] == DamageOverlay.Scratch)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return;
			}
			Rect rect = BuildingsDamageSectionLayerUtility.GetDamageRect(b);
			float num2 = Mathf.Min(0.5f * Mathf.Min(rect.width, rect.height), 1f);
			rect = rect.ContractedBy(num2 / 2f);
			if (rect.width <= 0f || rect.height <= 0f)
			{
				return;
			}
			float num3 = Mathf.Max(rect.width, rect.height) * 0.7f;
			SectionLayer_BuildingsDamage.scratches.Clear();
			Rand.PushState();
			Rand.Seed = b.thingIDNumber * 3697;
			for (int j = 0; j < num; j++)
			{
				this.AddScratch(b, rect.width, rect.height, ref num3);
			}
			Rand.PopState();
			float damageTexturesAltitude = this.GetDamageTexturesAltitude(b);
			IList<Material> scratchMats = BuildingsDamageSectionLayerUtility.GetScratchMats(b);
			Rand.PushState();
			Rand.Seed = b.thingIDNumber * 7;
			for (int k = 0; k < SectionLayer_BuildingsDamage.scratches.Count; k++)
			{
				float x = SectionLayer_BuildingsDamage.scratches[k].x;
				float y = SectionLayer_BuildingsDamage.scratches[k].y;
				float rot = Rand.Range(0f, 360f);
				float num4 = num2;
				if (rect.width > 0.95f && rect.height > 0.95f)
				{
					num4 *= Rand.Range(0.85f, 1f);
				}
				Vector3 center = new Vector3(rect.xMin + x, damageTexturesAltitude, rect.yMin + y);
				Material mat = scratchMats.RandomElement<Material>();
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
				Printer_Plane.PrintPlane(this, center, new Vector2(num4, num4), mat, rot, false, uvs, null, 0f, 0f);
			}
			Rand.PopState();
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x000499E8 File Offset: 0x00047BE8
		private void AddScratch(Building b, float rectWidth, float rectHeight, ref float minDist)
		{
			bool flag = false;
			float num = 0f;
			float num2 = 0f;
			while (!flag)
			{
				for (int i = 0; i < 5; i++)
				{
					num = Rand.Value * rectWidth;
					num2 = Rand.Value * rectHeight;
					float num3 = float.MaxValue;
					for (int j = 0; j < SectionLayer_BuildingsDamage.scratches.Count; j++)
					{
						float num4 = (num - SectionLayer_BuildingsDamage.scratches[j].x) * (num - SectionLayer_BuildingsDamage.scratches[j].x) + (num2 - SectionLayer_BuildingsDamage.scratches[j].y) * (num2 - SectionLayer_BuildingsDamage.scratches[j].y);
						if (num4 < num3)
						{
							num3 = num4;
						}
					}
					if (num3 >= minDist * minDist)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					minDist *= 0.85f;
					if (minDist < 0.001f)
					{
						break;
					}
				}
			}
			if (flag)
			{
				SectionLayer_BuildingsDamage.scratches.Add(new Vector2(num, num2));
			}
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00049AE7 File Offset: 0x00047CE7
		private void PrintCornersAndEdges(Building b)
		{
			Rand.PushState();
			Rand.Seed = b.thingIDNumber * 3;
			if (BuildingsDamageSectionLayerUtility.UsesLinkableCornersAndEdges(b))
			{
				this.DrawLinkableCornersAndEdges(b);
			}
			else
			{
				this.DrawFullThingCorners(b);
			}
			Rand.PopState();
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00049B18 File Offset: 0x00047D18
		private void DrawLinkableCornersAndEdges(Building b)
		{
			if (b.def.graphicData == null)
			{
				return;
			}
			DamageGraphicData damageData = b.def.graphicData.damageData;
			if (damageData == null)
			{
				return;
			}
			float damageTexturesAltitude = this.GetDamageTexturesAltitude(b);
			List<DamageOverlay> overlays = BuildingsDamageSectionLayerUtility.GetOverlays(b);
			IntVec3 position = b.Position;
			Vector3 vector = new Vector3((float)position.x + 0.5f, damageTexturesAltitude, (float)position.z + 0.5f);
			float x = Rand.Range(0.4f, 0.6f);
			float z = Rand.Range(0.4f, 0.6f);
			float x2 = Rand.Range(0.4f, 0.6f);
			float z2 = Rand.Range(0.4f, 0.6f);
			Vector2[] uvs = null;
			for (int i = 0; i < overlays.Count; i++)
			{
				switch (overlays[i])
				{
				case DamageOverlay.TopLeftCorner:
				{
					Material mat = damageData.cornerTLMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector, Vector2.one, mat, 0f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.TopRightCorner:
				{
					Material mat = damageData.cornerTRMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector, Vector2.one, mat, 90f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.BotLeftCorner:
				{
					Material mat = damageData.cornerBLMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector, Vector2.one, mat, 270f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.BotRightCorner:
				{
					Material mat = damageData.cornerBRMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector, Vector2.one, mat, 180f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.LeftEdge:
				{
					Material mat = damageData.edgeLeftMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector + new Vector3(0f, 0f, z2), Vector2.one, mat, 270f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.RightEdge:
				{
					Material mat = damageData.edgeRightMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector + new Vector3(0f, 0f, z), Vector2.one, mat, 90f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.TopEdge:
				{
					Material mat = damageData.edgeTopMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector + new Vector3(x, 0f, 0f), Vector2.one, mat, 0f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.BotEdge:
				{
					Material mat = damageData.edgeBotMat;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, vector + new Vector3(x2, 0f, 0f), Vector2.one, mat, 180f, false, uvs, null, 0f, 0f);
					break;
				}
				}
			}
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00049E68 File Offset: 0x00048068
		private void DrawFullThingCorners(Building b)
		{
			if (b.def.graphicData == null)
			{
				return;
			}
			if (b.def.graphicData.damageData == null)
			{
				return;
			}
			Rect damageRect = BuildingsDamageSectionLayerUtility.GetDamageRect(b);
			float damageTexturesAltitude = this.GetDamageTexturesAltitude(b);
			float num = Mathf.Min(Mathf.Min(damageRect.width, damageRect.height), 1.5f);
			Material material;
			Material material2;
			Material material3;
			Material material4;
			BuildingsDamageSectionLayerUtility.GetCornerMats(out material, out material2, out material3, out material4, b);
			float num2 = num * Rand.Range(0.9f, 1f);
			float num3 = num * Rand.Range(0.9f, 1f);
			float num4 = num * Rand.Range(0.9f, 1f);
			float num5 = num * Rand.Range(0.9f, 1f);
			Vector2[] uvs = null;
			List<DamageOverlay> overlays = BuildingsDamageSectionLayerUtility.GetOverlays(b);
			for (int i = 0; i < overlays.Count; i++)
			{
				switch (overlays[i])
				{
				case DamageOverlay.TopLeftCorner:
				{
					Rect rect = new Rect(damageRect.xMin, damageRect.yMax - num2, num2, num2);
					Material mat = material;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, new Vector3(rect.center.x, damageTexturesAltitude, rect.center.y), rect.size, mat, 0f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.TopRightCorner:
				{
					Rect rect2 = new Rect(damageRect.xMax - num3, damageRect.yMax - num3, num3, num3);
					Material mat = material2;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, new Vector3(rect2.center.x, damageTexturesAltitude, rect2.center.y), rect2.size, mat, 90f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.BotLeftCorner:
				{
					Rect rect3 = new Rect(damageRect.xMin, damageRect.yMin, num5, num5);
					Material mat = material4;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, new Vector3(rect3.center.x, damageTexturesAltitude, rect3.center.y), rect3.size, mat, 270f, false, uvs, null, 0f, 0f);
					break;
				}
				case DamageOverlay.BotRightCorner:
				{
					Rect rect4 = new Rect(damageRect.xMax - num4, damageRect.yMin, num4, num4);
					Material mat = material3;
					Color32 color;
					Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, false, false, out mat, out uvs, out color);
					Printer_Plane.PrintPlane(this, new Vector3(rect4.center.x, damageTexturesAltitude, rect4.center.y), rect4.size, mat, 180f, false, uvs, null, 0f, 0f);
					break;
				}
				}
			}
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0004A139 File Offset: 0x00048339
		private float GetDamageTexturesAltitude(Building b)
		{
			return b.def.Altitude + 0.2027027f;
		}

		// Token: 0x04000B18 RID: 2840
		private static List<Vector2> scratches = new List<Vector2>();
	}
}
