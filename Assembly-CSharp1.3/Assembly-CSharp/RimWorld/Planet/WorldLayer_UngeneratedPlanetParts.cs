using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001767 RID: 5991
	public class WorldLayer_UngeneratedPlanetParts : WorldLayer
	{
		// Token: 0x06008A3C RID: 35388 RVA: 0x00319B8A File Offset: 0x00317D8A
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

		// Token: 0x040057DD RID: 22493
		private const int SubdivisionsCount = 4;

		// Token: 0x040057DE RID: 22494
		private const float ViewAngleOffset = 10f;
	}
}
