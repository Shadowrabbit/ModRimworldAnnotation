using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015FD RID: 5629
	public class SymbolResolver_Interior_DiningRoom : SymbolResolver
	{
		// Token: 0x060083EA RID: 33770 RVA: 0x002F384C File Offset: 0x002F1A4C
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("indoorLighting", rp, null);
			BaseGen.symbolStack.Push("randomlyPlaceMealsOnTables", rp, null);
			BaseGen.symbolStack.Push("placeChairsNearTables", rp, null);
			int num = Mathf.Max(GenMath.RoundRandom((float)rp.rect.Area / 20f), 1);
			for (int i = 0; i < num; i++)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.Table2x2c;
				BaseGen.symbolStack.Push("thing", resolveParams, null);
			}
		}
	}
}
