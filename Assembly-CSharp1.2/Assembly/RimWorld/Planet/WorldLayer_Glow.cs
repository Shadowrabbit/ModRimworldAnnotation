using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002049 RID: 8265
	public class WorldLayer_Glow : WorldLayer
	{
		// Token: 0x0600AF36 RID: 44854 RVA: 0x00072159 File Offset: 0x00070359
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

		// Token: 0x0400786B RID: 30827
		private const int SubdivisionsCount = 4;

		// Token: 0x0400786C RID: 30828
		public const float GlowRadius = 8f;
	}
}
