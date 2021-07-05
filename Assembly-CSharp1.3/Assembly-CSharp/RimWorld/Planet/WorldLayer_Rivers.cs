using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x0200175F RID: 5983
	public class WorldLayer_Rivers : WorldLayer_Paths
	{
		// Token: 0x06008A14 RID: 35348 RVA: 0x003196AC File Offset: 0x003178AC
		public WorldLayer_Rivers()
		{
			this.pointyEnds = true;
		}

		// Token: 0x06008A15 RID: 35349 RVA: 0x00319763 File Offset: 0x00317963
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Rivers);
			LayerSubMesh subMeshBorder = base.GetSubMesh(WorldMaterials.RiversBorder);
			WorldGrid grid = Find.WorldGrid;
			List<WorldLayer_Paths.OutputDirection> outputs = new List<WorldLayer_Paths.OutputDirection>();
			List<WorldLayer_Paths.OutputDirection> outputsBorder = new List<WorldLayer_Paths.OutputDirection>();
			int num;
			for (int i = 0; i < grid.TilesCount; i = num)
			{
				if (i % 1000 == 0)
				{
					yield return null;
				}
				if (subMesh.verts.Count > 60000)
				{
					subMesh = base.GetSubMesh(WorldMaterials.Rivers);
					subMeshBorder = base.GetSubMesh(WorldMaterials.RiversBorder);
				}
				Tile tile = grid[i];
				if (tile.potentialRivers != null)
				{
					outputs.Clear();
					outputsBorder.Clear();
					for (int j = 0; j < tile.potentialRivers.Count; j++)
					{
						outputs.Add(new WorldLayer_Paths.OutputDirection
						{
							neighbor = tile.potentialRivers[j].neighbor,
							width = tile.potentialRivers[j].river.widthOnWorld - 0.2f
						});
						outputsBorder.Add(new WorldLayer_Paths.OutputDirection
						{
							neighbor = tile.potentialRivers[j].neighbor,
							width = tile.potentialRivers[j].river.widthOnWorld
						});
					}
					base.GeneratePaths(subMesh, i, outputs, this.riverColor, true);
					base.GeneratePaths(subMeshBorder, i, outputsBorder, this.riverColor, true);
				}
				num = i + 1;
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x06008A16 RID: 35350 RVA: 0x00319774 File Offset: 0x00317974
		public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
		{
			float magnitude = inp.magnitude;
			inp = (inp + new Vector3(this.riverDisplacementX.GetValue(inp), this.riverDisplacementY.GetValue(inp), this.riverDisplacementZ.GetValue(inp)) * 0.1f).normalized * magnitude;
			return inp + inp.normalized * 0.008f;
		}

		// Token: 0x040057C8 RID: 22472
		private Color32 riverColor = new Color32(73, 82, 100, byte.MaxValue);

		// Token: 0x040057C9 RID: 22473
		private const float PerlinFrequency = 0.6f;

		// Token: 0x040057CA RID: 22474
		private const float PerlinMagnitude = 0.1f;

		// Token: 0x040057CB RID: 22475
		private ModuleBase riverDisplacementX = new Perlin(0.6000000238418579, 2.0, 0.5, 3, 84905524, QualityMode.Medium);

		// Token: 0x040057CC RID: 22476
		private ModuleBase riverDisplacementY = new Perlin(0.6000000238418579, 2.0, 0.5, 3, 37971116, QualityMode.Medium);

		// Token: 0x040057CD RID: 22477
		private ModuleBase riverDisplacementZ = new Perlin(0.6000000238418579, 2.0, 0.5, 3, 91572032, QualityMode.Medium);
	}
}
