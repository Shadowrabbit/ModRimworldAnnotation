﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200204E RID: 8270
	public abstract class WorldLayer_Paths : WorldLayer
	{
		// Token: 0x0600AF52 RID: 44882 RVA: 0x0032F120 File Offset: 0x0032D320
		public void GeneratePaths(LayerSubMesh subMesh, int tileID, List<WorldLayer_Paths.OutputDirection> nodes, Color32 color, bool allowSmoothTransition)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			worldGrid.GetTileVertices(tileID, this.tmpVerts);
			worldGrid.GetTileNeighbors(tileID, this.tmpNeighbors);
			if (nodes.Count == 1 && this.pointyEnds)
			{
				int count = subMesh.verts.Count;
				this.AddPathEndpoint(subMesh, this.tmpVerts, this.tmpNeighbors.IndexOf(nodes[0].neighbor), color, tileID, nodes[0]);
				subMesh.verts.Add(this.FinalizePoint(worldGrid.GetTileCenter(tileID), nodes[0].distortionFrequency, nodes[0].distortionIntensity));
				subMesh.colors.Add(color.MutateAlpha(0));
				subMesh.tris.Add(count);
				subMesh.tris.Add(count + 3);
				subMesh.tris.Add(count + 1);
				subMesh.tris.Add(count + 1);
				subMesh.tris.Add(count + 3);
				subMesh.tris.Add(count + 2);
				return;
			}
			if (nodes.Count == 2)
			{
				int count2 = subMesh.verts.Count;
				int num = this.tmpNeighbors.IndexOf(nodes[0].neighbor);
				int num2 = this.tmpNeighbors.IndexOf(nodes[1].neighbor);
				if (allowSmoothTransition && Mathf.Abs(num - num2) > 1 && Mathf.Abs((num - num2 + this.tmpVerts.Count) % this.tmpVerts.Count) > 1)
				{
					this.AddPathEndpoint(subMesh, this.tmpVerts, num, color, tileID, nodes[0]);
					this.AddPathEndpoint(subMesh, this.tmpVerts, num2, color, tileID, nodes[1]);
					subMesh.tris.Add(count2);
					subMesh.tris.Add(count2 + 5);
					subMesh.tris.Add(count2 + 1);
					subMesh.tris.Add(count2 + 5);
					subMesh.tris.Add(count2 + 4);
					subMesh.tris.Add(count2 + 1);
					subMesh.tris.Add(count2 + 1);
					subMesh.tris.Add(count2 + 4);
					subMesh.tris.Add(count2 + 2);
					subMesh.tris.Add(count2 + 4);
					subMesh.tris.Add(count2 + 3);
					subMesh.tris.Add(count2 + 2);
					return;
				}
			}
			float num3 = 0f;
			for (int i = 0; i < nodes.Count; i++)
			{
				num3 = Mathf.Max(num3, nodes[i].width);
			}
			Vector3 vector = worldGrid.GetTileCenter(tileID);
			this.tmpHexVerts.Clear();
			for (int j = 0; j < this.tmpVerts.Count; j++)
			{
				this.tmpHexVerts.Add(this.FinalizePoint(Vector3.LerpUnclamped(vector, this.tmpVerts[j], num3 * 0.5f * 2f), 0f, 0f));
			}
			vector = this.FinalizePoint(vector, 0f, 0f);
			int count3 = subMesh.verts.Count;
			subMesh.verts.Add(vector);
			subMesh.colors.Add(color);
			int count4 = subMesh.verts.Count;
			for (int k = 0; k < this.tmpHexVerts.Count; k++)
			{
				subMesh.verts.Add(this.tmpHexVerts[k]);
				subMesh.colors.Add(color.MutateAlpha(0));
				subMesh.tris.Add(count3);
				subMesh.tris.Add(count4 + (k + 1) % this.tmpHexVerts.Count);
				subMesh.tris.Add(count4 + k);
			}
			for (int l = 0; l < nodes.Count; l++)
			{
				if (nodes[l].width != 0f)
				{
					int count5 = subMesh.verts.Count;
					int num4 = this.tmpNeighbors.IndexOf(nodes[l].neighbor);
					this.AddPathEndpoint(subMesh, this.tmpVerts, num4, color, tileID, nodes[l]);
					subMesh.tris.Add(count5);
					subMesh.tris.Add(count4 + (num4 + this.tmpHexVerts.Count - 1) % this.tmpHexVerts.Count);
					subMesh.tris.Add(count3);
					subMesh.tris.Add(count5);
					subMesh.tris.Add(count3);
					subMesh.tris.Add(count5 + 1);
					subMesh.tris.Add(count5 + 1);
					subMesh.tris.Add(count3);
					subMesh.tris.Add(count5 + 2);
					subMesh.tris.Add(count3);
					subMesh.tris.Add(count4 + (num4 + 2) % this.tmpHexVerts.Count);
					subMesh.tris.Add(count5 + 2);
				}
			}
		}

		// Token: 0x0600AF53 RID: 44883 RVA: 0x0032F648 File Offset: 0x0032D848
		private void AddPathEndpoint(LayerSubMesh subMesh, List<Vector3> verts, int index, Color32 color, int tileID, WorldLayer_Paths.OutputDirection data)
		{
			int index2 = (index + 1) % verts.Count;
			Find.WorldGrid.GetTileNeighbors(tileID, WorldLayer_Paths.lhsID);
			Find.WorldGrid.GetTileNeighbors(data.neighbor, WorldLayer_Paths.rhsID);
			float num = WorldLayer_Paths.lhsID.Intersect(WorldLayer_Paths.rhsID).Any((int id) => Find.WorldGrid[id].WaterCovered) ? 0.5f : 1f;
			Vector3 a = this.FinalizePoint(verts[index], data.distortionFrequency, data.distortionIntensity * num);
			Vector3 b = this.FinalizePoint(verts[index2], data.distortionFrequency, data.distortionIntensity * num);
			subMesh.verts.Add(Vector3.LerpUnclamped(a, b, 0.5f - data.width));
			subMesh.colors.Add(color.MutateAlpha(0));
			subMesh.verts.Add(Vector3.LerpUnclamped(a, b, 0.5f));
			subMesh.colors.Add(color);
			subMesh.verts.Add(Vector3.LerpUnclamped(a, b, 0.5f + data.width));
			subMesh.colors.Add(color.MutateAlpha(0));
		}

		// Token: 0x0600AF54 RID: 44884
		public abstract Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity);

		// Token: 0x0400787D RID: 30845
		protected bool pointyEnds;

		// Token: 0x0400787E RID: 30846
		private List<Vector3> tmpVerts = new List<Vector3>();

		// Token: 0x0400787F RID: 30847
		private List<Vector3> tmpHexVerts = new List<Vector3>();

		// Token: 0x04007880 RID: 30848
		private List<int> tmpNeighbors = new List<int>();

		// Token: 0x04007881 RID: 30849
		private static List<int> lhsID = new List<int>();

		// Token: 0x04007882 RID: 30850
		private static List<int> rhsID = new List<int>();

		// Token: 0x0200204F RID: 8271
		public struct OutputDirection
		{
			// Token: 0x04007883 RID: 30851
			public int neighbor;

			// Token: 0x04007884 RID: 30852
			public float width;

			// Token: 0x04007885 RID: 30853
			public float distortionFrequency;

			// Token: 0x04007886 RID: 30854
			public float distortionIntensity;
		}
	}
}
