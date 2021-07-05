using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001766 RID: 5990
	public class WorldLayer_Terrain : WorldLayer
	{
		// Token: 0x06008A36 RID: 35382 RVA: 0x00319AD5 File Offset: 0x00317CD5
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			WorldGrid grid = Find.World.grid;
			int tilesCount = grid.TilesCount;
			List<Tile> tiles = grid.tiles;
			List<int> tileIDToVerts_offsets = grid.tileIDToVerts_offsets;
			List<Vector3> verts = grid.verts;
			this.triangleIndexToTileID.Clear();
			foreach (object obj2 in this.CalculateInterpolatedVerticesParams())
			{
				yield return obj2;
			}
			enumerator = null;
			int num = 0;
			for (int i = 0; i < tilesCount; i++)
			{
				BiomeDef biome = tiles[i].biome;
				int j;
				LayerSubMesh subMesh = base.GetSubMesh(biome.DrawMaterial, out j);
				while (j >= this.triangleIndexToTileID.Count)
				{
					this.triangleIndexToTileID.Add(new List<int>());
				}
				int count = subMesh.verts.Count;
				int num2 = 0;
				int num3 = (i + 1 < tileIDToVerts_offsets.Count) ? tileIDToVerts_offsets[i + 1] : verts.Count;
				for (int k = tileIDToVerts_offsets[i]; k < num3; k++)
				{
					subMesh.verts.Add(verts[k]);
					subMesh.uvs.Add(this.elevationValues[num]);
					num++;
					if (k < num3 - 2)
					{
						subMesh.tris.Add(count + num2 + 2);
						subMesh.tris.Add(count + num2 + 1);
						subMesh.tris.Add(count);
						this.triangleIndexToTileID[j].Add(i);
					}
					num2++;
				}
			}
			base.FinalizeMesh(MeshParts.All);
			foreach (object obj3 in this.RegenerateMeshColliders())
			{
				yield return obj3;
			}
			enumerator = null;
			this.elevationValues.Clear();
			this.elevationValues.TrimExcess();
			yield break;
			yield break;
		}

		// Token: 0x06008A37 RID: 35383 RVA: 0x00319AE8 File Offset: 0x00317CE8
		public int GetTileIDFromRayHit(RaycastHit hit)
		{
			int i = 0;
			int count = this.meshCollidersInOrder.Count;
			while (i < count)
			{
				if (this.meshCollidersInOrder[i] == hit.collider)
				{
					return this.triangleIndexToTileID[i][hit.triangleIndex];
				}
				i++;
			}
			return -1;
		}

		// Token: 0x06008A38 RID: 35384 RVA: 0x00319B41 File Offset: 0x00317D41
		private IEnumerable RegenerateMeshColliders()
		{
			this.meshCollidersInOrder.Clear();
			GameObject gameObject = WorldTerrainColliderManager.GameObject;
			MeshCollider[] components = gameObject.GetComponents<MeshCollider>();
			int j;
			for (j = 0; j < components.Length; j++)
			{
				UnityEngine.Object.Destroy(components[j]);
			}
			for (int i = 0; i < this.subMeshes.Count; i = j + 1)
			{
				MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
				meshCollider.sharedMesh = this.subMeshes[i].mesh;
				this.meshCollidersInOrder.Add(meshCollider);
				yield return null;
				j = i;
			}
			yield break;
		}

		// Token: 0x06008A39 RID: 35385 RVA: 0x00319B51 File Offset: 0x00317D51
		private IEnumerable CalculateInterpolatedVerticesParams()
		{
			this.elevationValues.Clear();
			WorldGrid grid = Find.World.grid;
			int tilesCount = grid.TilesCount;
			List<Vector3> verts = grid.verts;
			List<int> tileIDToVerts_offsets = grid.tileIDToVerts_offsets;
			List<int> tileIDToNeighbors_offsets = grid.tileIDToNeighbors_offsets;
			List<int> tileIDToNeighbors_values = grid.tileIDToNeighbors_values;
			List<Tile> tiles = grid.tiles;
			int num4;
			for (int i = 0; i < tilesCount; i = num4 + 1)
			{
				Tile tile = tiles[i];
				float elevation = tile.elevation;
				int num = (i + 1 < tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_offsets[i + 1] : tileIDToNeighbors_values.Count;
				int num2 = (i + 1 < tilesCount) ? tileIDToVerts_offsets[i + 1] : verts.Count;
				for (int j = tileIDToVerts_offsets[i]; j < num2; j++)
				{
					Vector3 vector = default(Vector3);
					vector.x = elevation;
					bool flag = false;
					for (int k = tileIDToNeighbors_offsets[i]; k < num; k++)
					{
						int num3 = (tileIDToNeighbors_values[k] + 1 < tileIDToVerts_offsets.Count) ? tileIDToVerts_offsets[tileIDToNeighbors_values[k] + 1] : verts.Count;
						int l = tileIDToVerts_offsets[tileIDToNeighbors_values[k]];
						while (l < num3)
						{
							if (verts[l] == verts[j])
							{
								Tile tile2 = tiles[tileIDToNeighbors_values[k]];
								if (flag)
								{
									break;
								}
								if ((tile2.elevation >= 0f && elevation <= 0f) || (tile2.elevation <= 0f && elevation >= 0f))
								{
									flag = true;
									break;
								}
								if (tile2.elevation > vector.x)
								{
									vector.x = tile2.elevation;
									break;
								}
								break;
							}
							else
							{
								l++;
							}
						}
					}
					if (flag)
					{
						vector.x = 0f;
					}
					if (tile.biome.DrawMaterial.shader != ShaderDatabase.WorldOcean && vector.x < 0f)
					{
						vector.x = 0f;
					}
					this.elevationValues.Add(vector);
				}
				if (i % 1000 == 0)
				{
					yield return null;
				}
				num4 = i;
			}
			yield break;
		}

		// Token: 0x040057DA RID: 22490
		private List<MeshCollider> meshCollidersInOrder = new List<MeshCollider>();

		// Token: 0x040057DB RID: 22491
		private List<List<int>> triangleIndexToTileID = new List<List<int>>();

		// Token: 0x040057DC RID: 22492
		private List<Vector3> elevationValues = new List<Vector3>();
	}
}
