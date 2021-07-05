using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004B4 RID: 1204
	[StaticConstructorOnStartup]
	public static class GenDraw
	{
		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x0600249C RID: 9372 RVA: 0x000E39F2 File Offset: 0x000E1BF2
		public static Material CurTargetingMat
		{
			get
			{
				GenDraw.TargetSquareMatSingle.color = GenDraw.CurTargetingColor;
				return GenDraw.TargetSquareMatSingle;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x0600249D RID: 9373 RVA: 0x000E3A08 File Offset: 0x000E1C08
		public static Color CurTargetingColor
		{
			get
			{
				float num = (float)Math.Sin((double)(Time.time * 8f));
				num *= 0.2f;
				num += 0.8f;
				return new Color(1f, num, num);
			}
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x000E3A44 File Offset: 0x000E1C44
		public static void DrawNoBuildEdgeLines()
		{
			GenDraw.DrawMapEdgeLines(10);
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x000E3A4D File Offset: 0x000E1C4D
		public static void DrawNoZoneEdgeLines()
		{
			GenDraw.DrawMapEdgeLines(5);
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x000E3A58 File Offset: 0x000E1C58
		private static void DrawMapEdgeLines(int edgeDist)
		{
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			IntVec3 size = Find.CurrentMap.Size;
			Vector3 vector = new Vector3((float)edgeDist, y, (float)edgeDist);
			Vector3 vector2 = new Vector3((float)edgeDist, y, (float)(size.z - edgeDist));
			Vector3 vector3 = new Vector3((float)(size.x - edgeDist), y, (float)(size.z - edgeDist));
			Vector3 vector4 = new Vector3((float)(size.x - edgeDist), y, (float)edgeDist);
			GenDraw.DrawLineBetween(vector, vector2, GenDraw.LineMatMetaOverlay, 0.2f);
			GenDraw.DrawLineBetween(vector2, vector3, GenDraw.LineMatMetaOverlay, 0.2f);
			GenDraw.DrawLineBetween(vector3, vector4, GenDraw.LineMatMetaOverlay, 0.2f);
			GenDraw.DrawLineBetween(vector4, vector, GenDraw.LineMatMetaOverlay, 0.2f);
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x000E3B0C File Offset: 0x000E1D0C
		public static void DrawLineBetween(Vector3 A, Vector3 B)
		{
			GenDraw.DrawLineBetween(A, B, GenDraw.LineMatWhite, 0.2f);
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x000E3B1F File Offset: 0x000E1D1F
		public static void DrawLineBetween(Vector3 A, Vector3 B, float layer)
		{
			GenDraw.DrawLineBetween(A, B, layer, GenDraw.LineMatWhite, 0.2f);
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x000E3B33 File Offset: 0x000E1D33
		public static void DrawLineBetween(Vector3 A, Vector3 B, float layer, Material mat, float lineWidth = 0.2f)
		{
			GenDraw.DrawLineBetween(A + Vector3.up * layer, B + Vector3.up * layer, mat, lineWidth);
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x000E3B5F File Offset: 0x000E1D5F
		public static void DrawLineBetween(Vector3 A, Vector3 B, SimpleColor color, float lineWidth = 0.2f)
		{
			GenDraw.DrawLineBetween(A, B, GenDraw.GetLineMat(color), lineWidth);
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x000E3B70 File Offset: 0x000E1D70
		public static void DrawLineBetween(Vector3 A, Vector3 B, Material mat, float lineWidth = 0.2f)
		{
			if (Mathf.Abs(A.x - B.x) < 0.01f && Mathf.Abs(A.z - B.z) < 0.01f)
			{
				return;
			}
			Vector3 pos = (A + B) / 2f;
			if (A == B)
			{
				return;
			}
			A.y = B.y;
			float z = (A - B).MagnitudeHorizontal();
			Quaternion q = Quaternion.LookRotation(A - B);
			Vector3 s = new Vector3(lineWidth, 1f, z);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, q, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, mat, 0);
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x000E3C21 File Offset: 0x000E1E21
		public static void DrawCircleOutline(Vector3 center, float radius)
		{
			GenDraw.DrawCircleOutline(center, radius, GenDraw.LineMatWhite);
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x000E3C2F File Offset: 0x000E1E2F
		public static void DrawCircleOutline(Vector3 center, float radius, SimpleColor color)
		{
			GenDraw.DrawCircleOutline(center, radius, GenDraw.GetLineMat(color));
		}

		// Token: 0x060024A8 RID: 9384 RVA: 0x000E3C40 File Offset: 0x000E1E40
		public static void DrawCircleOutline(Vector3 center, float radius, Material material)
		{
			int num = Mathf.Clamp(Mathf.RoundToInt(24f * radius), 12, 48);
			float num2 = 0f;
			float num3 = 6.2831855f / (float)num;
			Vector3 vector = center;
			Vector3 a = center;
			for (int i = 0; i < num + 2; i++)
			{
				if (i >= 2)
				{
					GenDraw.DrawLineBetween(a, vector, material, 0.2f);
				}
				a = vector;
				vector = center;
				vector.x += Mathf.Cos(num2) * radius;
				vector.z += Mathf.Sin(num2) * radius;
				num2 += num3;
			}
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x000E3CCC File Offset: 0x000E1ECC
		private static Material GetLineMat(SimpleColor color)
		{
			switch (color)
			{
			case SimpleColor.White:
				return GenDraw.LineMatWhite;
			case SimpleColor.Red:
				return GenDraw.LineMatRed;
			case SimpleColor.Green:
				return GenDraw.LineMatGreen;
			case SimpleColor.Blue:
				return GenDraw.LineMatBlue;
			case SimpleColor.Magenta:
				return GenDraw.LineMatMagenta;
			case SimpleColor.Yellow:
				return GenDraw.LineMatYellow;
			case SimpleColor.Cyan:
				return GenDraw.LineMatCyan;
			case SimpleColor.Orange:
				return GenDraw.LineMatOrange;
			default:
				return GenDraw.LineMatWhite;
			}
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x000E3D36 File Offset: 0x000E1F36
		public static void DrawWorldLineBetween(Vector3 A, Vector3 B)
		{
			GenDraw.DrawWorldLineBetween(A, B, GenDraw.WorldLineMatWhite, 1f);
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x000E3D4C File Offset: 0x000E1F4C
		public static void DrawWorldLineBetween(Vector3 A, Vector3 B, Material material, float widthFactor = 1f)
		{
			if (Mathf.Abs(A.x - B.x) < 0.005f && Mathf.Abs(A.y - B.y) < 0.005f && Mathf.Abs(A.z - B.z) < 0.005f)
			{
				return;
			}
			Vector3 pos = (A + B) / 2f;
			float magnitude = (A - B).magnitude;
			Quaternion q = Quaternion.LookRotation(A - B, pos.normalized);
			Vector3 s = new Vector3(0.2f * Find.WorldGrid.averageTileSize * widthFactor, 1f, magnitude);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, q, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, WorldCameraManager.WorldLayer);
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x000E3E20 File Offset: 0x000E2020
		public static void DrawWorldRadiusRing(int center, int radius)
		{
			if (radius < 0)
			{
				return;
			}
			if (GenDraw.cachedEdgeTilesForCenter != center || GenDraw.cachedEdgeTilesForRadius != radius || GenDraw.cachedEdgeTilesForWorldSeed != Find.World.info.Seed)
			{
				GenDraw.cachedEdgeTilesForCenter = center;
				GenDraw.cachedEdgeTilesForRadius = radius;
				GenDraw.cachedEdgeTilesForWorldSeed = Find.World.info.Seed;
				GenDraw.cachedEdgeTiles.Clear();
				Find.WorldFloodFiller.FloodFill(center, (int tile) => true, delegate(int tile, int dist)
				{
					if (dist > radius + 1)
					{
						return true;
					}
					if (dist == radius + 1)
					{
						GenDraw.cachedEdgeTiles.Add(tile);
					}
					return false;
				}, int.MaxValue, null);
				WorldGrid worldGrid = Find.WorldGrid;
				Vector3 c = worldGrid.GetTileCenter(center);
				Vector3 n = c.normalized;
				GenDraw.cachedEdgeTiles.Sort(delegate(int a, int b)
				{
					float num = Vector3.Dot(n, Vector3.Cross(worldGrid.GetTileCenter(a) - c, worldGrid.GetTileCenter(b) - c));
					if (Mathf.Abs(num) < 0.0001f)
					{
						return 0;
					}
					if (num < 0f)
					{
						return -1;
					}
					return 1;
				});
			}
			GenDraw.DrawWorldLineStrip(GenDraw.cachedEdgeTiles, GenDraw.OneSidedWorldLineMatWhite, 5f);
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x000E3F3C File Offset: 0x000E213C
		public static void DrawWorldLineStrip(List<int> edgeTiles, Material material, float widthFactor)
		{
			if (edgeTiles.Count < 3)
			{
				return;
			}
			WorldGrid worldGrid = Find.WorldGrid;
			float d = 0.05f;
			for (int i = 0; i < edgeTiles.Count; i++)
			{
				int index = (i == 0) ? (edgeTiles.Count - 1) : (i - 1);
				int num = edgeTiles[index];
				int num2 = edgeTiles[i];
				if (worldGrid.IsNeighbor(num, num2))
				{
					Vector3 a = worldGrid.GetTileCenter(num);
					Vector3 vector = worldGrid.GetTileCenter(num2);
					a += a.normalized * d;
					vector += vector.normalized * d;
					GenDraw.DrawWorldLineBetween(a, vector, material, widthFactor);
				}
			}
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x000E3FED File Offset: 0x000E21ED
		public static void DrawTargetHighlight(LocalTargetInfo targ)
		{
			if (targ.Thing != null)
			{
				GenDraw.DrawTargetingHighlight_Thing(targ.Thing);
				return;
			}
			GenDraw.DrawTargetingHighlight_Cell(targ.Cell);
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x000E4011 File Offset: 0x000E2211
		private static void DrawTargetingHighlight_Cell(IntVec3 c)
		{
			GenDraw.DrawTargetHighlightWithLayer(c, AltitudeLayer.Building);
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x000E401C File Offset: 0x000E221C
		public static void DrawTargetHighlightWithLayer(IntVec3 c, AltitudeLayer layer)
		{
			Vector3 position = c.ToVector3ShiftedWithAltitude(layer);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x000E4048 File Offset: 0x000E2248
		public static void DrawTargetHighlightWithLayer(Vector3 c, AltitudeLayer layer)
		{
			Vector3 position = new Vector3(c.x, layer.AltitudeFor(), c.z);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x000E4084 File Offset: 0x000E2284
		private static void DrawTargetingHighlight_Thing(Thing t)
		{
			Graphics.DrawMesh(MeshPool.plane10, t.TrueCenter() + Altitudes.AltIncVect, t.Rotation.AsQuat, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x000E40C0 File Offset: 0x000E22C0
		public static void DrawStencilCell(Vector3 c, Material material, float width = 1f, float height = 1f)
		{
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(new Vector3(c.x, -1f, c.z), Quaternion.identity, new Vector3(width, 1f, height));
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000E4110 File Offset: 0x000E2310
		public static void DrawTargetingHightlight_Explosion(IntVec3 c, float Radius)
		{
			GenDraw.DrawRadiusRing(c, Radius);
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x000E411C File Offset: 0x000E231C
		public static void DrawInteractionCell(ThingDef tDef, IntVec3 center, Rot4 placingRot)
		{
			if (tDef.hasInteractionCell)
			{
				IntVec3 c = ThingUtility.InteractionCellWhenAt(tDef, center, placingRot, Find.CurrentMap);
				Vector3 vector = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
				if (c.InBounds(Find.CurrentMap))
				{
					Building edifice = c.GetEdifice(Find.CurrentMap);
					if (edifice != null && edifice.def.building != null && edifice.def.building.isSittable)
					{
						return;
					}
				}
				if (tDef.interactionCellGraphic == null && tDef.interactionCellIcon != null)
				{
					ThingDef thingDef = tDef.interactionCellIcon;
					if (thingDef.blueprintDef != null)
					{
						thingDef = thingDef.blueprintDef;
					}
					tDef.interactionCellGraphic = thingDef.graphic.GetColoredVersion(ShaderTypeDefOf.EdgeDetect.Shader, GenDraw.InteractionCellIntensity, Color.white);
				}
				if (tDef.interactionCellGraphic != null)
				{
					Rot4 rot = tDef.interactionCellIconReverse ? placingRot.Opposite : placingRot;
					tDef.interactionCellGraphic.DrawFromDef(vector, rot, tDef.interactionCellIcon, 0f);
					return;
				}
				Graphics.DrawMesh(MeshPool.plane10, vector, Quaternion.identity, GenDraw.InteractionCellMaterial, 0);
			}
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x000E4220 File Offset: 0x000E2420
		public static void DrawRadiusRing(IntVec3 center, float radius, Color color, Func<IntVec3, bool> predicate = null)
		{
			if (radius > GenRadial.MaxRadialPatternRadius)
			{
				if (!GenDraw.maxRadiusMessaged)
				{
					Log.Error("Cannot draw radius ring of radius " + radius + ": not enough squares in the precalculated list.");
					GenDraw.maxRadiusMessaged = true;
				}
				return;
			}
			GenDraw.ringDrawCells.Clear();
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (predicate == null || predicate(intVec))
				{
					GenDraw.ringDrawCells.Add(intVec);
				}
			}
			GenDraw.DrawFieldEdges(GenDraw.ringDrawCells, color, null);
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x000E42B7 File Offset: 0x000E24B7
		public static void DrawRadiusRing(IntVec3 center, float radius)
		{
			GenDraw.DrawRadiusRing(center, radius, Color.white, null);
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x000E42C8 File Offset: 0x000E24C8
		public static void DrawFieldEdges(List<IntVec3> cells)
		{
			GenDraw.DrawFieldEdges(cells, Color.white, null);
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x000E42EC File Offset: 0x000E24EC
		public static void DrawFieldEdges(List<IntVec3> cells, Color color, float? altOffset = null)
		{
			Map currentMap = Find.CurrentMap;
			Material material = MaterialPool.MatFrom(new MaterialRequest
			{
				shader = ShaderDatabase.Transparent,
				color = color,
				BaseTexPath = "UI/Overlays/TargetHighlight_Side"
			});
			material.GetTexture("_MainTex").wrapMode = TextureWrapMode.Clamp;
			if (GenDraw.fieldGrid == null)
			{
				GenDraw.fieldGrid = new BoolGrid(currentMap);
			}
			else
			{
				GenDraw.fieldGrid.ClearAndResizeTo(currentMap);
			}
			int x = currentMap.Size.x;
			int z = currentMap.Size.z;
			int count = cells.Count;
			float y = altOffset ?? (Rand.ValueSeeded(color.ToOpaque().GetHashCode()) * 0.04054054f / 10f);
			for (int i = 0; i < count; i++)
			{
				if (cells[i].InBounds(currentMap))
				{
					GenDraw.fieldGrid[cells[i].x, cells[i].z] = true;
				}
			}
			for (int j = 0; j < count; j++)
			{
				IntVec3 intVec = cells[j];
				if (intVec.InBounds(currentMap))
				{
					GenDraw.rotNeeded[0] = (intVec.z < z - 1 && !GenDraw.fieldGrid[intVec.x, intVec.z + 1]);
					GenDraw.rotNeeded[1] = (intVec.x < x - 1 && !GenDraw.fieldGrid[intVec.x + 1, intVec.z]);
					GenDraw.rotNeeded[2] = (intVec.z > 0 && !GenDraw.fieldGrid[intVec.x, intVec.z - 1]);
					GenDraw.rotNeeded[3] = (intVec.x > 0 && !GenDraw.fieldGrid[intVec.x - 1, intVec.z]);
					for (int k = 0; k < 4; k++)
					{
						if (GenDraw.rotNeeded[k])
						{
							Graphics.DrawMesh(MeshPool.plane10, intVec.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays) + new Vector3(0f, y, 0f), new Rot4(k).AsQuat, material, 0);
						}
					}
				}
			}
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x000E4550 File Offset: 0x000E2750
		public static void DrawAimPie(Thing shooter, LocalTargetInfo target, int degreesWide, float offsetDist)
		{
			float facing = 0f;
			if (target.Cell != shooter.Position)
			{
				if (target.Thing != null)
				{
					facing = (target.Thing.DrawPos - shooter.Position.ToVector3Shifted()).AngleFlat();
				}
				else
				{
					facing = (target.Cell - shooter.Position).AngleFlat;
				}
			}
			GenDraw.DrawAimPieRaw(shooter.DrawPos + new Vector3(0f, offsetDist, 0f), facing, degreesWide);
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x000E45E4 File Offset: 0x000E27E4
		public static void DrawAimPieRaw(Vector3 center, float facing, int degreesWide)
		{
			if (degreesWide <= 0)
			{
				return;
			}
			if (degreesWide > 360)
			{
				degreesWide = 360;
			}
			center += Quaternion.AngleAxis(facing, Vector3.up) * Vector3.forward * 0.8f;
			Graphics.DrawMesh(MeshPool.pies[degreesWide], center, Quaternion.AngleAxis(facing + (float)(degreesWide / 2) - 90f, Vector3.up), GenDraw.AimPieMaterial, 0);
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x000E4658 File Offset: 0x000E2858
		public static void DrawCooldownCircle(Vector3 center, float radius)
		{
			Vector3 s = new Vector3(radius, 1f, radius);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(center, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.circle, matrix, GenDraw.AimPieMaterial, 0);
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x000E469C File Offset: 0x000E289C
		public static void DrawFillableBar(GenDraw.FillableBarRequest r)
		{
			Vector2 vector = r.preRotationOffset.RotatedBy(r.rotation.AsAngle);
			r.center += new Vector3(vector.x, 0f, vector.y);
			if (r.rotation == Rot4.South)
			{
				r.rotation = Rot4.North;
			}
			if (r.rotation == Rot4.West)
			{
				r.rotation = Rot4.East;
			}
			Vector3 s = new Vector3(r.size.x + r.margin, 1f, r.size.y + r.margin);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(r.center, r.rotation.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, r.unfilledMat, 0);
			if (r.fillPercent > 0.001f)
			{
				s = new Vector3(r.size.x * r.fillPercent, 1f, r.size.y);
				matrix = default(Matrix4x4);
				Vector3 pos = r.center + Vector3.up * 0.01f;
				if (!r.rotation.IsHorizontal)
				{
					pos.x -= r.size.x * 0.5f;
					pos.x += 0.5f * r.size.x * r.fillPercent;
				}
				else
				{
					pos.z -= r.size.x * 0.5f;
					pos.z += 0.5f * r.size.x * r.fillPercent;
				}
				matrix.SetTRS(pos, r.rotation.AsQuat, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, r.filledMat, 0);
			}
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x000E48A0 File Offset: 0x000E2AA0
		public static void DrawMeshNowOrLater(Mesh mesh, Vector3 loc, Quaternion quat, Material mat, bool drawNow)
		{
			if (drawNow)
			{
				if (!mat.SetPass(0))
				{
					Log.Error("SetPass(0) call failed on material " + mat.name + " with shader " + mat.shader.name);
				}
				Graphics.DrawMeshNow(mesh, loc, quat);
				return;
			}
			Graphics.DrawMesh(mesh, loc, quat, mat, 0);
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x000E48F2 File Offset: 0x000E2AF2
		public static void DrawMeshNowOrLater(Mesh mesh, Matrix4x4 matrix, Material mat, bool drawNow)
		{
			if (drawNow)
			{
				mat.SetPass(0);
				Graphics.DrawMeshNow(mesh, matrix);
				return;
			}
			Graphics.DrawMesh(mesh, matrix, mat, 0);
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x000E4910 File Offset: 0x000E2B10
		public static void DrawArrowPointingAt(Vector3 mapTarget, bool offscreenOnly = false)
		{
			Vector3 vector = UI.UIToMapPosition((float)(UI.screenWidth / 2), (float)(UI.screenHeight / 2));
			if ((vector - mapTarget).MagnitudeHorizontalSquared() < 81f)
			{
				if (!offscreenOnly)
				{
					Vector3 position = mapTarget;
					position.y = AltitudeLayer.MetaOverlays.AltitudeFor();
					position.z -= 1.5f;
					Graphics.DrawMesh(MeshPool.plane20, position, Quaternion.identity, GenDraw.ArrowMatWhite, 0);
					return;
				}
			}
			else
			{
				Vector3 normalized = (mapTarget - vector).Yto0().normalized;
				Vector3 position2 = vector + normalized * 7f;
				position2.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				Quaternion rotation = Quaternion.LookRotation(normalized);
				Graphics.DrawMesh(MeshPool.plane20, position2, rotation, GenDraw.ArrowMatWhite, 0);
			}
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000E49D4 File Offset: 0x000E2BD4
		public static void DrawArrowRotated(Vector3 pos, float rotationAngle, bool ghost)
		{
			Quaternion rotation = Quaternion.AngleAxis(rotationAngle, new Vector3(0f, 1f, 0f));
			Vector3 position = pos;
			position.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Graphics.DrawMesh(MeshPool.plane10, position, rotation, ghost ? GenDraw.ArrowMatGhost : GenDraw.ArrowMatWhite, 0);
		}

		// Token: 0x040016DD RID: 5853
		private static readonly Material TargetSquareMatSingle = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Square", ShaderDatabase.Transparent);

		// Token: 0x040016DE RID: 5854
		private const float TargetPulseFrequency = 8f;

		// Token: 0x040016DF RID: 5855
		public static readonly string LineTexPath = "UI/Overlays/ThingLine";

		// Token: 0x040016E0 RID: 5856
		public static readonly string OneSidedLineTexPath = "UI/Overlays/OneSidedLine";

		// Token: 0x040016E1 RID: 5857
		private static readonly Material LineMatWhite = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.white);

		// Token: 0x040016E2 RID: 5858
		private static readonly Material LineMatRed = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.red);

		// Token: 0x040016E3 RID: 5859
		private static readonly Material LineMatGreen = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.green);

		// Token: 0x040016E4 RID: 5860
		private static readonly Material LineMatBlue = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.blue);

		// Token: 0x040016E5 RID: 5861
		private static readonly Material LineMatMagenta = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.magenta);

		// Token: 0x040016E6 RID: 5862
		private static readonly Material LineMatYellow = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.yellow);

		// Token: 0x040016E7 RID: 5863
		private static readonly Material LineMatCyan = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.cyan);

		// Token: 0x040016E8 RID: 5864
		private static readonly Material LineMatOrange = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, ColorLibrary.Orange);

		// Token: 0x040016E9 RID: 5865
		private static readonly Material LineMatMetaOverlay = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.MetaOverlay);

		// Token: 0x040016EA RID: 5866
		private static readonly Material WorldLineMatWhite = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.WorldOverlayTransparent, Color.white, WorldMaterials.WorldLineRenderQueue);

		// Token: 0x040016EB RID: 5867
		private static readonly Material OneSidedWorldLineMatWhite = MaterialPool.MatFrom(GenDraw.OneSidedLineTexPath, ShaderDatabase.WorldOverlayTransparent, Color.white, WorldMaterials.WorldLineRenderQueue);

		// Token: 0x040016EC RID: 5868
		public static readonly Material RitualStencilMat = MaterialPool.MatFrom(ShaderDatabase.RitualStencil);

		// Token: 0x040016ED RID: 5869
		private const float LineWidth = 0.2f;

		// Token: 0x040016EE RID: 5870
		private const float BaseWorldLineWidth = 0.2f;

		// Token: 0x040016EF RID: 5871
		public static readonly Material InteractionCellMaterial = MaterialPool.MatFrom("UI/Overlays/InteractionCell", ShaderDatabase.Transparent);

		// Token: 0x040016F0 RID: 5872
		private static readonly Color InteractionCellIntensity = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x040016F1 RID: 5873
		private static List<int> cachedEdgeTiles = new List<int>();

		// Token: 0x040016F2 RID: 5874
		private static int cachedEdgeTilesForCenter = -1;

		// Token: 0x040016F3 RID: 5875
		private static int cachedEdgeTilesForRadius = -1;

		// Token: 0x040016F4 RID: 5876
		private static int cachedEdgeTilesForWorldSeed = -1;

		// Token: 0x040016F5 RID: 5877
		private static List<IntVec3> ringDrawCells = new List<IntVec3>();

		// Token: 0x040016F6 RID: 5878
		private static bool maxRadiusMessaged = false;

		// Token: 0x040016F7 RID: 5879
		private static BoolGrid fieldGrid;

		// Token: 0x040016F8 RID: 5880
		private static bool[] rotNeeded = new bool[4];

		// Token: 0x040016F9 RID: 5881
		private static readonly Material AimPieMaterial = SolidColorMaterials.SimpleSolidColorMaterial(new Color(1f, 1f, 1f, 0.3f), false);

		// Token: 0x040016FA RID: 5882
		private static readonly Material ArrowMatWhite = MaterialPool.MatFrom("UI/Overlays/Arrow", ShaderDatabase.CutoutFlying, Color.white);

		// Token: 0x040016FB RID: 5883
		private static readonly Material ArrowMatGhost = MaterialPool.MatFrom("UI/Overlays/ArrowGhost", ShaderDatabase.Transparent, Color.white);

		// Token: 0x02001CB8 RID: 7352
		public struct FillableBarRequest
		{
			// Token: 0x04006ECD RID: 28365
			public Vector3 center;

			// Token: 0x04006ECE RID: 28366
			public Vector2 size;

			// Token: 0x04006ECF RID: 28367
			public float fillPercent;

			// Token: 0x04006ED0 RID: 28368
			public Material filledMat;

			// Token: 0x04006ED1 RID: 28369
			public Material unfilledMat;

			// Token: 0x04006ED2 RID: 28370
			public float margin;

			// Token: 0x04006ED3 RID: 28371
			public Rot4 rotation;

			// Token: 0x04006ED4 RID: 28372
			public Vector2 preRotationOffset;
		}
	}
}
