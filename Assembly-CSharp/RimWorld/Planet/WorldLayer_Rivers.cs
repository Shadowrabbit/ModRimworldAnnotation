using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02002051 RID: 8273
	public class WorldLayer_Rivers : WorldLayer_Paths
	{
		// Token: 0x0600AF5A RID: 44890 RVA: 0x0032F790 File Offset: 0x0032D990
		public WorldLayer_Rivers()
		{
			this.pointyEnds = true;
		}

		// Token: 0x0600AF5B RID: 44891 RVA: 0x00072241 File Offset: 0x00070441
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

		// Token: 0x0600AF5C RID: 44892 RVA: 0x0032F848 File Offset: 0x0032DA48
		public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
		{
			float magnitude = inp.magnitude;
			inp = (inp + new Vector3(this.riverDisplacementX.GetValue(inp), this.riverDisplacementY.GetValue(inp), this.riverDisplacementZ.GetValue(inp)) * 0.1f).normalized * magnitude;
			return inp + inp.normalized * 0.008f;
		}

		// Token: 0x04007889 RID: 30857
		private Color32 riverColor = new Color32(73, 82, 100, byte.MaxValue);

		// Token: 0x0400788A RID: 30858
		private const float PerlinFrequency = 0.6f;

		// Token: 0x0400788B RID: 30859
		private const float PerlinMagnitude = 0.1f;

		// Token: 0x0400788C RID: 30860
		private ModuleBase riverDisplacementX = new Perlin(0.6000000238418579, 2.0, 0.5, 3, 84905524, QualityMode.Medium);

		// Token: 0x0400788D RID: 30861
		private ModuleBase riverDisplacementY = new Perlin(0.6000000238418579, 2.0, 0.5, 3, 37971116, QualityMode.Medium);

		// Token: 0x0400788E RID: 30862
		private ModuleBase riverDisplacementZ = new Perlin(0.6000000238418579, 2.0, 0.5, 3, 91572032, QualityMode.Medium);
	}
}
