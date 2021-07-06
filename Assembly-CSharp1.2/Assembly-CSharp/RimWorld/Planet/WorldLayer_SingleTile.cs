using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002058 RID: 8280
	public abstract class WorldLayer_SingleTile : WorldLayer
	{
		// Token: 0x170019F0 RID: 6640
		// (get) Token: 0x0600AF7E RID: 44926
		protected abstract int Tile { get; }

		// Token: 0x170019F1 RID: 6641
		// (get) Token: 0x0600AF7F RID: 44927
		protected abstract Material Material { get; }

		// Token: 0x170019F2 RID: 6642
		// (get) Token: 0x0600AF80 RID: 44928 RVA: 0x000722F4 File Offset: 0x000704F4
		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || this.Tile != this.lastDrawnTile;
			}
		}

		// Token: 0x0600AF81 RID: 44929 RVA: 0x00072311 File Offset: 0x00070511
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			int tile = this.Tile;
			if (tile >= 0)
			{
				LayerSubMesh subMesh = base.GetSubMesh(this.Material);
				Find.WorldGrid.GetTileVertices(tile, this.verts);
				int count = subMesh.verts.Count;
				int i = 0;
				int count2 = this.verts.Count;
				while (i < count2)
				{
					subMesh.verts.Add(this.verts[i] + this.verts[i].normalized * 0.012f);
					subMesh.uvs.Add((GenGeo.RegularPolygonVertexPosition(count2, i) + Vector2.one) / 2f);
					if (i < count2 - 2)
					{
						subMesh.tris.Add(count + i + 2);
						subMesh.tris.Add(count + i + 1);
						subMesh.tris.Add(count);
					}
					i++;
				}
				base.FinalizeMesh(MeshParts.All);
			}
			this.lastDrawnTile = tile;
			yield break;
			yield break;
		}

		// Token: 0x040078A8 RID: 30888
		private int lastDrawnTile = -1;

		// Token: 0x040078A9 RID: 30889
		private List<Vector3> verts = new List<Vector3>();
	}
}
