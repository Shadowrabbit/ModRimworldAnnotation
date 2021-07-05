using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02001760 RID: 5984
	public class WorldLayer_Roads : WorldLayer_Paths
	{
		// Token: 0x06008A18 RID: 35352 RVA: 0x003197E9 File Offset: 0x003179E9
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Roads);
			WorldGrid grid = Find.WorldGrid;
			List<RoadWorldLayerDef> roadLayerDefs = (from rwld in DefDatabase<RoadWorldLayerDef>.AllDefs
			orderby rwld.order
			select rwld).ToList<RoadWorldLayerDef>();
			int num;
			for (int i = 0; i < grid.TilesCount; i = num)
			{
				if (i % 1000 == 0)
				{
					yield return null;
				}
				if (subMesh.verts.Count > 60000)
				{
					subMesh = base.GetSubMesh(WorldMaterials.Roads);
				}
				Tile tile = grid[i];
				if (!tile.WaterCovered)
				{
					List<WorldLayer_Paths.OutputDirection> list = new List<WorldLayer_Paths.OutputDirection>();
					if (tile.potentialRoads != null)
					{
						bool allowSmoothTransition = true;
						for (int j = 0; j < tile.potentialRoads.Count - 1; j++)
						{
							if (tile.potentialRoads[j].road.worldTransitionGroup != tile.potentialRoads[j + 1].road.worldTransitionGroup)
							{
								allowSmoothTransition = false;
							}
						}
						for (int k = 0; k < roadLayerDefs.Count; k++)
						{
							bool flag = false;
							list.Clear();
							for (int l = 0; l < tile.potentialRoads.Count; l++)
							{
								RoadDef road = tile.potentialRoads[l].road;
								float layerWidth = road.GetLayerWidth(roadLayerDefs[k]);
								if (layerWidth > 0f)
								{
									flag = true;
								}
								list.Add(new WorldLayer_Paths.OutputDirection
								{
									neighbor = tile.potentialRoads[l].neighbor,
									width = layerWidth,
									distortionFrequency = road.distortionFrequency,
									distortionIntensity = road.distortionIntensity
								});
							}
							if (flag)
							{
								base.GeneratePaths(subMesh, i, list, roadLayerDefs[k].color, allowSmoothTransition);
							}
						}
					}
				}
				num = i + 1;
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x06008A19 RID: 35353 RVA: 0x003197FC File Offset: 0x003179FC
		public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
		{
			Vector3 coordinate = inp * distortionFrequency;
			float magnitude = inp.magnitude;
			Vector3 a = new Vector3(this.roadDisplacementX.GetValue(coordinate), this.roadDisplacementY.GetValue(coordinate), this.roadDisplacementZ.GetValue(coordinate));
			if ((double)a.magnitude > 0.0001)
			{
				float d = (1f / (1f + Mathf.Exp(-a.magnitude / 1f * 2f)) * 2f - 1f) * 1f;
				a = a.normalized * d;
			}
			inp = (inp + a * distortionIntensity).normalized * magnitude;
			return inp + inp.normalized * 0.012f;
		}

		// Token: 0x040057CE RID: 22478
		private ModuleBase roadDisplacementX = new Perlin(1.0, 2.0, 0.5, 3, 74173887, QualityMode.Medium);

		// Token: 0x040057CF RID: 22479
		private ModuleBase roadDisplacementY = new Perlin(1.0, 2.0, 0.5, 3, 67515931, QualityMode.Medium);

		// Token: 0x040057D0 RID: 22480
		private ModuleBase roadDisplacementZ = new Perlin(1.0, 2.0, 0.5, 3, 87116801, QualityMode.Medium);
	}
}
