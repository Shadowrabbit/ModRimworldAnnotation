using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001EA4 RID: 7844
	public class SymbolResolver_Interior_DiningRoom : SymbolResolver
	{
		// Token: 0x0600A88B RID: 43147 RVA: 0x00311DCC File Offset: 0x0030FFCC
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
