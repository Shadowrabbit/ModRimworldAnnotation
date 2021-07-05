using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001763 RID: 5987
	public abstract class WorldLayer_SingleTile : WorldLayer
	{
		// Token: 0x17001686 RID: 5766
		// (get) Token: 0x06008A23 RID: 35363
		protected abstract int Tile { get; }

		// Token: 0x17001687 RID: 5767
		// (get) Token: 0x06008A24 RID: 35364
		protected abstract Material Material { get; }

		// Token: 0x17001688 RID: 5768
		// (get) Token: 0x06008A25 RID: 35365 RVA: 0x003199DC File Offset: 0x00317BDC
		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || this.Tile != this.lastDrawnTile;
			}
		}

		// Token: 0x06008A26 RID: 35366 RVA: 0x003199F9 File Offset: 0x00317BF9
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

		// Token: 0x040057D1 RID: 22481
		private int lastDrawnTile = -1;

		// Token: 0x040057D2 RID: 22482
		private List<Vector3> verts = new List<Vector3>();
	}
}
