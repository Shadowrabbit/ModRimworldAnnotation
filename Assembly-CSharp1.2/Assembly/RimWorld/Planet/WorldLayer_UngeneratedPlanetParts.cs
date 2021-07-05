using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002062 RID: 8290
	public class WorldLayer_UngeneratedPlanetParts : WorldLayer
	{
		// Token: 0x0600AFCD RID: 45005 RVA: 0x00072542 File Offset: 0x00070742
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			Vector3 viewCenter = Find.WorldGrid.viewCenter;
			float viewAngle = Find.WorldGrid.viewAngle;
			if (viewAngle < 180f)
			{
				List<Vector3> collection;
				List<int> collection2;
				SphereGenerator.Generate(4, 99.85f, -viewCenter, 180f - Mathf.Min(viewAngle, 180f) + 10f, out collection, out collection2);
				LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.UngeneratedPlanetParts);
				subMesh.verts.AddRange(collection);
				subMesh.tris.AddRange(collection2);
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x040078DD RID: 30941
		private const int SubdivisionsCount = 4;

		// Token: 0x040078DE RID: 30942
		private const float ViewAngleOffset = 10f;
	}
}
