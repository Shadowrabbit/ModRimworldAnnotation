using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200176F RID: 5999
	public static class WorldRendererUtility
	{
		// Token: 0x17001694 RID: 5780
		// (get) Token: 0x06008A62 RID: 35426 RVA: 0x0031A91F File Offset: 0x00318B1F
		public static WorldRenderMode CurrentWorldRenderMode
		{
			get
			{
				if (Find.World == null)
				{
					return WorldRenderMode.None;
				}
				if (Current.ProgramState == ProgramState.Playing && Find.CurrentMap == null)
				{
					return WorldRenderMode.Planet;
				}
				return Find.World.renderer.wantedMode;
			}
		}

		// Token: 0x17001695 RID: 5781
		// (get) Token: 0x06008A63 RID: 35427 RVA: 0x0031A94A File Offset: 0x00318B4A
		public static bool WorldRenderedNow
		{
			get
			{
				return WorldRendererUtility.CurrentWorldRenderMode > WorldRenderMode.None;
			}
		}

		// Token: 0x06008A64 RID: 35428 RVA: 0x0031A954 File Offset: 0x00318B54
		public static void UpdateWorldShadersParams()
		{
			Vector3 v = -GenCelestial.CurSunPositionInWorldSpace();
			float value = Find.PlaySettings.usePlanetDayNightSystem ? 1f : 0f;
			Shader.SetGlobalVector(ShaderPropertyIDs.PlanetSunLightDirection, v);
			Shader.SetGlobalFloat(ShaderPropertyIDs.PlanetSunLightEnabled, value);
			WorldMaterials.PlanetGlow.SetFloat(ShaderPropertyIDs.PlanetRadius, 100f);
			WorldMaterials.PlanetGlow.SetFloat(ShaderPropertyIDs.GlowRadius, 8f);
		}

		// Token: 0x06008A65 RID: 35429 RVA: 0x0031A9C8 File Offset: 0x00318BC8
		public static void PrintQuadTangentialToPlanet(Vector3 pos, float size, float altOffset, LayerSubMesh subMesh, bool counterClockwise = false, bool randomizeRotation = false, bool printUVs = true)
		{
			WorldRendererUtility.PrintQuadTangentialToPlanet(pos, pos, size, altOffset, subMesh, counterClockwise, randomizeRotation, printUVs);
		}

		// Token: 0x06008A66 RID: 35430 RVA: 0x0031A9DC File Offset: 0x00318BDC
		public static void PrintQuadTangentialToPlanet(Vector3 pos, Vector3 posForTangents, float size, float altOffset, LayerSubMesh subMesh, bool counterClockwise = false, bool randomizeRotation = false, bool printUVs = true)
		{
			Vector3 a;
			Vector3 a2;
			WorldRendererUtility.GetTangentsToPlanet(posForTangents, out a, out a2, randomizeRotation);
			Vector3 normalized = posForTangents.normalized;
			float d = size * 0.5f;
			Vector3 item = pos - a * d - a2 * d + normalized * altOffset;
			Vector3 item2 = pos - a * d + a2 * d + normalized * altOffset;
			Vector3 item3 = pos + a * d + a2 * d + normalized * altOffset;
			Vector3 item4 = pos + a * d - a2 * d + normalized * altOffset;
			int count = subMesh.verts.Count;
			subMesh.verts.Add(item);
			subMesh.verts.Add(item2);
			subMesh.verts.Add(item3);
			subMesh.verts.Add(item4);
			if (printUVs)
			{
				subMesh.uvs.Add(new Vector2(0f, 0f));
				subMesh.uvs.Add(new Vector2(0f, 1f));
				subMesh.uvs.Add(new Vector2(1f, 1f));
				subMesh.uvs.Add(new Vector2(1f, 0f));
			}
			if (counterClockwise)
			{
				subMesh.tris.Add(count + 2);
				subMesh.tris.Add(count + 1);
				subMesh.tris.Add(count);
				subMesh.tris.Add(count + 3);
				subMesh.tris.Add(count + 2);
				subMesh.tris.Add(count);
				return;
			}
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 1);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count + 3);
		}

		// Token: 0x06008A67 RID: 35431 RVA: 0x0031AC2C File Offset: 0x00318E2C
		public static void DrawQuadTangentialToPlanet(Vector3 pos, float size, float altOffset, Material material, bool counterClockwise = false, bool useSkyboxLayer = false, MaterialPropertyBlock propertyBlock = null)
		{
			if (material == null)
			{
				Log.Warning("Tried to draw quad with null material.");
				return;
			}
			Vector3 normalized = pos.normalized;
			Vector3 vector;
			if (counterClockwise)
			{
				vector = -normalized;
			}
			else
			{
				vector = normalized;
			}
			Quaternion q = Quaternion.LookRotation(Vector3.Cross(vector, Vector3.up), vector);
			Vector3 s = new Vector3(size, 1f, size);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos + normalized * altOffset, q, s);
			int layer = useSkyboxLayer ? WorldCameraManager.WorldSkyboxLayer : WorldCameraManager.WorldLayer;
			if (propertyBlock != null)
			{
				Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer, null, 0, propertyBlock);
				return;
			}
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer);
		}

		// Token: 0x06008A68 RID: 35432 RVA: 0x0031ACDC File Offset: 0x00318EDC
		public static void GetTangentsToPlanet(Vector3 pos, out Vector3 first, out Vector3 second, bool randomizeRotation = false)
		{
			Vector3 upwards;
			if (randomizeRotation)
			{
				upwards = Rand.UnitVector3;
			}
			else
			{
				upwards = Vector3.up;
			}
			Quaternion rotation = Quaternion.LookRotation(pos.normalized, upwards);
			first = rotation * Vector3.up;
			second = rotation * Vector3.right;
		}

		// Token: 0x06008A69 RID: 35433 RVA: 0x0031AD2C File Offset: 0x00318F2C
		public static Vector3 ProjectOnQuadTangentialToPlanet(Vector3 center, Vector2 point)
		{
			Vector3 a;
			Vector3 a2;
			WorldRendererUtility.GetTangentsToPlanet(center, out a, out a2, false);
			return point.x * a + point.y * a2;
		}

		// Token: 0x06008A6A RID: 35434 RVA: 0x0031AD64 File Offset: 0x00318F64
		public static void GetTangentialVectorFacing(Vector3 root, Vector3 pointToFace, out Vector3 forward, out Vector3 right)
		{
			Quaternion rotation = Quaternion.LookRotation(root, pointToFace);
			forward = rotation * Vector3.up;
			right = rotation * Vector3.left;
		}

		// Token: 0x06008A6B RID: 35435 RVA: 0x0031AD9C File Offset: 0x00318F9C
		public static void PrintTextureAtlasUVs(int indexX, int indexY, int numX, int numY, LayerSubMesh subMesh)
		{
			float num = 1f / (float)numX;
			float num2 = 1f / (float)numY;
			float num3 = (float)indexX * num;
			float num4 = (float)indexY * num2;
			subMesh.uvs.Add(new Vector2(num3, num4));
			subMesh.uvs.Add(new Vector2(num3, num4 + num2));
			subMesh.uvs.Add(new Vector2(num3 + num, num4 + num2));
			subMesh.uvs.Add(new Vector2(num3 + num, num4));
		}

		// Token: 0x06008A6C RID: 35436 RVA: 0x0031AE30 File Offset: 0x00319030
		public static bool HiddenBehindTerrainNow(Vector3 pos)
		{
			Vector3 normalized = pos.normalized;
			Vector3 currentlyLookingAtPointOnSphere = Find.WorldCameraDriver.CurrentlyLookingAtPointOnSphere;
			return Vector3.Angle(normalized, currentlyLookingAtPointOnSphere) > 73f;
		}
	}
}
