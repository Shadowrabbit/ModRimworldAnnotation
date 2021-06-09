using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000842 RID: 2114
	[StaticConstructorOnStartup]
	public static class GenDraw
	{
		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x060034DA RID: 13530 RVA: 0x000293CC File Offset: 0x000275CC
		public static Material CurTargetingMat
		{
			get
			{
				GenDraw.TargetSquareMatSingle.color = GenDraw.CurTargetingColor;
				return GenDraw.TargetSquareMatSingle;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x060034DB RID: 13531 RVA: 0x00155420 File Offset: 0x00153620
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

		// Token: 0x060034DC RID: 13532 RVA: 0x000293E2 File Offset: 0x000275E2
		public static void DrawNoBuildEdgeLines()
		{
			GenDraw.DrawMapEdgeLines(10);
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x000293EB File Offset: 0x000275EB
		public static void DrawNoZoneEdgeLines()
		{
			GenDraw.DrawMapEdgeLines(5);
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x0015545C File Offset: 0x0015365C
		private static void DrawMapEdgeLines(int edgeDist)
		{
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			IntVec3 size = Find.CurrentMap.Size;
			Vector3 vector = new Vector3((float)edgeDist, y, (float)edgeDist);
			Vector3 vector2 = new Vector3((float)edgeDist, y, (float)(size.z - edgeDist));
			Vector3 vector3 = new Vector3((float)(size.x - edgeDist), y, (float)(size.z - edgeDist));
			Vector3 vector4 = new Vector3((float)(size.x - edgeDist), y, (float)edgeDist);
			GenDraw.DrawLineBetween(vector, vector2, GenDraw.LineMatMetaOverlay);
			GenDraw.DrawLineBetween(vector2, vector3, GenDraw.LineMatMetaOverlay);
			GenDraw.DrawLineBetween(vector3, vector4, GenDraw.LineMatMetaOverlay);
			GenDraw.DrawLineBetween(vector4, vector, GenDraw.LineMatMetaOverlay);
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x000293F3 File Offset: 0x000275F3
		public static void DrawLineBetween(Vector3 A, Vector3 B)
		{
			GenDraw.DrawLineBetween(A, B, GenDraw.LineMatWhite);
		}

		// Token: 0x060034E0 RID: 13536 RVA: 0x00029401 File Offset: 0x00027601
		public static void DrawLineBetween(Vector3 A, Vector3 B, float layer)
		{
			GenDraw.DrawLineBetween(A + Vector3.up * layer, B + Vector3.up * layer, GenDraw.LineMatWhite);
		}

		// Token: 0x060034E1 RID: 13537 RVA: 0x0002942F File Offset: 0x0002762F
		public static void DrawLineBetween(Vector3 A, Vector3 B, SimpleColor color)
		{
			GenDraw.DrawLineBetween(A, B, GenDraw.GetLineMat(color));
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x001554FC File Offset: 0x001536FC
		public static void DrawLineBetween(Vector3 A, Vector3 B, Material mat)
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
			Vector3 s = new Vector3(0.2f, 1f, z);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, q, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, mat, 0);
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x0002943E File Offset: 0x0002763E
		public static void DrawCircleOutline(Vector3 center, float radius)
		{
			GenDraw.DrawCircleOutline(center, radius, GenDraw.LineMatWhite);
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x0002944C File Offset: 0x0002764C
		public static void DrawCircleOutline(Vector3 center, float radius, SimpleColor color)
		{
			GenDraw.DrawCircleOutline(center, radius, GenDraw.GetLineMat(color));
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x001555B4 File Offset: 0x001537B4
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
					GenDraw.DrawLineBetween(a, vector, material);
				}
				a = vector;
				vector = center;
				vector.x += Mathf.Cos(num2) * radius;
				vector.z += Mathf.Sin(num2) * radius;
				num2 += num3;
			}
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x0015563C File Offset: 0x0015383C
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
			default:
				return GenDraw.LineMatWhite;
			}
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x0002945B File Offset: 0x0002765B
		public static void DrawWorldLineBetween(Vector3 A, Vector3 B)
		{
			GenDraw.DrawWorldLineBetween(A, B, GenDraw.WorldLineMatWhite, 1f);
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x0015569C File Offset: 0x0015389C
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

		// Token: 0x060034E9 RID: 13545 RVA: 0x00155770 File Offset: 0x00153970
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

		// Token: 0x060034EA RID: 13546 RVA: 0x0015588C File Offset: 0x00153A8C
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

		// Token: 0x060034EB RID: 13547 RVA: 0x0002946E File Offset: 0x0002766E
		public static void DrawTargetHighlight(LocalTargetInfo targ)
		{
			if (targ.Thing != null)
			{
				GenDraw.DrawTargetingHighlight_Thing(targ.Thing);
				return;
			}
			GenDraw.DrawTargetingHighlight_Cell(targ.Cell);
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x00029492 File Offset: 0x00027692
		private static void DrawTargetingHighlight_Cell(IntVec3 c)
		{
			GenDraw.DrawTargetHighlightWithLayer(c, AltitudeLayer.Building);
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x00155940 File Offset: 0x00153B40
		public static void DrawTargetHighlightWithLayer(IntVec3 c, AltitudeLayer layer)
		{
			Vector3 position = c.ToVector3ShiftedWithAltitude(layer);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x0015596C File Offset: 0x00153B6C
		public static void DrawTargetHighlightWithLayer(Vector3 c, AltitudeLayer layer)
		{
			Vector3 position = new Vector3(c.x, layer.AltitudeFor(), c.z);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x060034EF RID: 13551 RVA: 0x001559A8 File Offset: 0x00153BA8
		private static void DrawTargetingHighlight_Thing(Thing t)
		{
			Graphics.DrawMesh(MeshPool.plane10, t.TrueCenter() + Altitudes.AltIncVect, t.Rotation.AsQuat, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x0002949C File Offset: 0x0002769C
		public static void DrawTargetingHightlight_Explosion(IntVec3 c, float Radius)
		{
			GenDraw.DrawRadiusRing(c, Radius);
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x001559E4 File Offset: 0x00153BE4
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

		// Token: 0x060034F2 RID: 13554 RVA: 0x00155AE8 File Offset: 0x00153CE8
		public static void DrawRadiusRing(IntVec3 center, float radius, Color color, Func<IntVec3, bool> predicate = null)
		{
			if (radius > GenRadial.MaxRadialPatternRadius)
			{
				if (!GenDraw.maxRadiusMessaged)
				{
					Log.Error("Cannot draw radius ring of radius " + radius + ": not enough squares in the precalculated list.", false);
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
			GenDraw.DrawFieldEdges(GenDraw.ringDrawCells, color);
		}

		// Token: 0x060034F3 RID: 13555 RVA: 0x000294A5 File Offset: 0x000276A5
		public static void DrawRadiusRing(IntVec3 center, float radius)
		{
			GenDraw.DrawRadiusRing(center, radius, Color.white, null);
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x000294B4 File Offset: 0x000276B4
		public static void DrawFieldEdges(List<IntVec3> cells)
		{
			GenDraw.DrawFieldEdges(cells, Color.white);
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x00155B78 File Offset: 0x00153D78
		public static void DrawFieldEdges(List<IntVec3> cells, Color color)
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
							Graphics.DrawMesh(MeshPool.plane10, intVec.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), new Rot4(k).AsQuat, material, 0);
						}
					}
				}
			}
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x00155D88 File Offset: 0x00153F88
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

		// Token: 0x060034F7 RID: 13559 RVA: 0x00155E1C File Offset: 0x0015401C
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

		// Token: 0x060034F8 RID: 13560 RVA: 0x00155E90 File Offset: 0x00154090
		public static void DrawCooldownCircle(Vector3 center, float radius)
		{
			Vector3 s = new Vector3(radius, 1f, radius);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(center, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.circle, matrix, GenDraw.AimPieMaterial, 0);
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x00155ED4 File Offset: 0x001540D4
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

		// Token: 0x060034FA RID: 13562 RVA: 0x000294C1 File Offset: 0x000276C1
		public static void DrawMeshNowOrLater(Mesh mesh, Vector3 loc, Quaternion quat, Material mat, bool drawNow)
		{
			if (drawNow)
			{
				mat.SetPass(0);
				Graphics.DrawMeshNow(mesh, loc, quat);
				return;
			}
			Graphics.DrawMesh(mesh, loc, quat, mat, 0);
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x000294E2 File Offset: 0x000276E2
		public static void DrawMeshNowOrLater_NewTemp(Mesh mesh, Matrix4x4 matrix, Material mat, bool drawNow)
		{
			if (drawNow)
			{
				mat.SetPass(0);
				Graphics.DrawMeshNow(mesh, matrix);
				return;
			}
			Graphics.DrawMesh(mesh, matrix, mat, 0);
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x001560D8 File Offset: 0x001542D8
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

		// Token: 0x040024AC RID: 9388
		private static readonly Material TargetSquareMatSingle = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Square", ShaderDatabase.Transparent);

		// Token: 0x040024AD RID: 9389
		private const float TargetPulseFrequency = 8f;

		// Token: 0x040024AE RID: 9390
		public static readonly string LineTexPath = "UI/Overlays/ThingLine";

		// Token: 0x040024AF RID: 9391
		public static readonly string OneSidedLineTexPath = "UI/Overlays/OneSidedLine";

		// Token: 0x040024B0 RID: 9392
		private static readonly Material LineMatWhite = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.white);

		// Token: 0x040024B1 RID: 9393
		private static readonly Material LineMatRed = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.red);

		// Token: 0x040024B2 RID: 9394
		private static readonly Material LineMatGreen = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.green);

		// Token: 0x040024B3 RID: 9395
		private static readonly Material LineMatBlue = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.blue);

		// Token: 0x040024B4 RID: 9396
		private static readonly Material LineMatMagenta = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.magenta);

		// Token: 0x040024B5 RID: 9397
		private static readonly Material LineMatYellow = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.yellow);

		// Token: 0x040024B6 RID: 9398
		private static readonly Material LineMatCyan = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.cyan);

		// Token: 0x040024B7 RID: 9399
		private static readonly Material LineMatMetaOverlay = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.MetaOverlay);

		// Token: 0x040024B8 RID: 9400
		private static readonly Material WorldLineMatWhite = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.WorldOverlayTransparent, Color.white, WorldMaterials.WorldLineRenderQueue);

		// Token: 0x040024B9 RID: 9401
		private static readonly Material OneSidedWorldLineMatWhite = MaterialPool.MatFrom(GenDraw.OneSidedLineTexPath, ShaderDatabase.WorldOverlayTransparent, Color.white, WorldMaterials.WorldLineRenderQueue);

		// Token: 0x040024BA RID: 9402
		private const float LineWidth = 0.2f;

		// Token: 0x040024BB RID: 9403
		private const float BaseWorldLineWidth = 0.2f;

		// Token: 0x040024BC RID: 9404
		public static readonly Material InteractionCellMaterial = MaterialPool.MatFrom("UI/Overlays/InteractionCell", ShaderDatabase.Transparent);

		// Token: 0x040024BD RID: 9405
		private static readonly Color InteractionCellIntensity = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x040024BE RID: 9406
		private static List<int> cachedEdgeTiles = new List<int>();

		// Token: 0x040024BF RID: 9407
		private static int cachedEdgeTilesForCenter = -1;

		// Token: 0x040024C0 RID: 9408
		private static int cachedEdgeTilesForRadius = -1;

		// Token: 0x040024C1 RID: 9409
		private static int cachedEdgeTilesForWorldSeed = -1;

		// Token: 0x040024C2 RID: 9410
		private static List<IntVec3> ringDrawCells = new List<IntVec3>();

		// Token: 0x040024C3 RID: 9411
		private static bool maxRadiusMessaged = false;

		// Token: 0x040024C4 RID: 9412
		private static BoolGrid fieldGrid;

		// Token: 0x040024C5 RID: 9413
		private static bool[] rotNeeded = new bool[4];

		// Token: 0x040024C6 RID: 9414
		private static readonly Material AimPieMaterial = SolidColorMaterials.SimpleSolidColorMaterial(new Color(1f, 1f, 1f, 0.3f), false);

		// Token: 0x040024C7 RID: 9415
		private static readonly Material ArrowMatWhite = MaterialPool.MatFrom("UI/Overlays/Arrow", ShaderDatabase.CutoutFlying, Color.white);

		// Token: 0x02000843 RID: 2115
		public struct FillableBarRequest
		{
			// Token: 0x040024C8 RID: 9416
			public Vector3 center;

			// Token: 0x040024C9 RID: 9417
			public Vector2 size;

			// Token: 0x040024CA RID: 9418
			public float fillPercent;

			// Token: 0x040024CB RID: 9419
			public Material filledMat;

			// Token: 0x040024CC RID: 9420
			public Material unfilledMat;

			// Token: 0x040024CD RID: 9421
			public float margin;

			// Token: 0x040024CE RID: 9422
			public Rot4 rotation;

			// Token: 0x040024CF RID: 9423
			public Vector2 preRotationOffset;
		}
	}
}
