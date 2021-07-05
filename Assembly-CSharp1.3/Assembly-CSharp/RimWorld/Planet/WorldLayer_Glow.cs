using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200175B RID: 5979
	public class WorldLayer_Glow : WorldLayer
	{
		// Token: 0x06008A05 RID: 35333 RVA: 0x00318EF9 File Offset: 0x003170F9
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			List<Vector3> collection;
			List<int> collection2;
			SphereGenerator.Generate(4, 108.1f, Vector3.forward, 360f, out collection, out collection2);
			LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.PlanetGlow);
			subMesh.verts.AddRange(collection);
			subMesh.tris.AddRange(collection2);
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x040057BA RID: 22458
		private const int SubdivisionsCount = 4;

		// Token: 0x040057BB RID: 22459
		public const float GlowRadius = 8f;
	}
}
