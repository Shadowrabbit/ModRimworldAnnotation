using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200204B RID: 8267
	public class WorldLayer_Hills : WorldLayer
	{
		// Token: 0x0600AF42 RID: 44866 RVA: 0x000721A3 File Offset: 0x000703A3
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			WorldGrid worldGrid = Find.WorldGrid;
			int tilesCount = worldGrid.TilesCount;
			int i = 0;
			while (i < tilesCount)
			{
				Material material;
				FloatRange floatRange;
				switch (worldGrid[i].hilliness)
				{
				case Hilliness.SmallHills:
					material = WorldMaterials.SmallHills;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_SmallHills;
					goto IL_11D;
				case Hilliness.LargeHills:
					material = WorldMaterials.LargeHills;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_LargeHills;
					goto IL_11D;
				case Hilliness.Mountainous:
					material = WorldMaterials.Mountains;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_Mountains;
					goto IL_11D;
				case Hilliness.Impassable:
					material = WorldMaterials.ImpassableMountains;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_ImpassableMountains;
					goto IL_11D;
				}
				IL_1D6:
				i++;
				continue;
				IL_11D:
				LayerSubMesh subMesh = base.GetSubMesh(material);
				Vector3 vector = worldGrid.GetTileCenter(i);
				Vector3 posForTangents = vector;
				float magnitude = vector.magnitude;
				vector = (vector + Rand.UnitVector3 * floatRange.RandomInRange * worldGrid.averageTileSize).normalized * magnitude;
				WorldRendererUtility.PrintQuadTangentialToPlanet(vector, posForTangents, WorldLayer_Hills.BaseSizeRange.RandomInRange * worldGrid.averageTileSize, 0.005f, subMesh, false, true, false);
				WorldRendererUtility.PrintTextureAtlasUVs(Rand.Range(0, WorldLayer_Hills.TexturesInAtlas.x), Rand.Range(0, WorldLayer_Hills.TexturesInAtlas.z), WorldLayer_Hills.TexturesInAtlas.x, WorldLayer_Hills.TexturesInAtlas.z, subMesh);
				goto IL_1D6;
			}
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x04007872 RID: 30834
		private static readonly FloatRange BaseSizeRange = new FloatRange(0.9f, 1.1f);

		// Token: 0x04007873 RID: 30835
		private static readonly IntVec2 TexturesInAtlas = new IntVec2(2, 2);

		// Token: 0x04007874 RID: 30836
		private static readonly FloatRange BasePosOffsetRange_SmallHills = new FloatRange(0f, 0.37f);

		// Token: 0x04007875 RID: 30837
		private static readonly FloatRange BasePosOffsetRange_LargeHills = new FloatRange(0f, 0.2f);

		// Token: 0x04007876 RID: 30838
		private static readonly FloatRange BasePosOffsetRange_Mountains = new FloatRange(0f, 0.08f);

		// Token: 0x04007877 RID: 30839
		private static readonly FloatRange BasePosOffsetRange_ImpassableMountains = new FloatRange(0f, 0.08f);
	}
}
