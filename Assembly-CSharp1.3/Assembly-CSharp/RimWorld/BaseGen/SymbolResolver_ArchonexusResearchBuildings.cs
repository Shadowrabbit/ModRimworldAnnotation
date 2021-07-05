using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015D7 RID: 5591
	public class SymbolResolver_ArchonexusResearchBuildings : SymbolResolver
	{
		// Token: 0x06008374 RID: 33652 RVA: 0x002EEBB4 File Offset: 0x002ECDB4
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.singleThingDef = ThingDefOf.ArchonexusSuperstructureMajorStudiable;
			resolveParams.rect = CellRect.CenteredOn(rp.rect.CenterCell, ThingDefOf.ArchonexusSuperstructureMajorStudiable.Size.x, ThingDefOf.ArchonexusSuperstructureMajorStudiable.Size.z);
			BaseGen.symbolStack.Push("thing", resolveParams, null);
			int num = rp.minorBuildingCount ?? Rand.Range(4, 8);
			float num2 = 360f / (float)num;
			Vector3 v = IntVec3.North.ToVector3();
			int num3 = Mathf.Max(rp.minorBuildingRadialDistance ?? 10, 10);
			for (int i = 0; i < num; i++)
			{
				float angle = (float)i * num2;
				Vector3 vect = v.RotatedBy(angle) * (float)num3;
				IntVec3 intVec = rp.rect.CenterCell + vect.ToIntVec3();
				CellRect rect = CellRect.CenteredOn(intVec, ThingDefOf.ArchonexusSuperstructureMinor.size.x, ThingDefOf.ArchonexusSuperstructureMinor.size.z);
				if (rect.InBounds(BaseGen.globalSettings.map))
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.singleThingDef = ThingDefOf.ArchonexusSuperstructureMinor;
					resolveParams2.rect = rect;
					BaseGen.symbolStack.Push("thing", resolveParams2, null);
					BaseGenUtility.DoPathwayBetween(resolveParams.rect.CenterCell, intVec, TerrainDefOf.Sandstone_Smooth, 3);
				}
			}
		}
	}
}
