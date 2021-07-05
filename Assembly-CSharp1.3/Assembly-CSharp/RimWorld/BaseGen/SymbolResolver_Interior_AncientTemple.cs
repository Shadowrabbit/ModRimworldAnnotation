using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015F8 RID: 5624
	public class SymbolResolver_Interior_AncientTemple : SymbolResolver
	{
		// Token: 0x060083DE RID: 33758 RVA: 0x002F345C File Offset: 0x002F165C
		public override void Resolve(ResolveParams rp)
		{
			List<Thing> list = ThingSetMakerDefOf.MapGen_AncientTempleContents.root.Generate();
			list.SortByDescending((Thing t) => t.MarketValue * (float)t.stackCount);
			for (int i = 0; i < list.Count; i++)
			{
				ResolveParams resolveParams = rp;
				if (ModsConfig.IdeologyActive && i == 0)
				{
					resolveParams.singleThingDef = ThingDefOf.AncientHermeticCrate;
					resolveParams.singleThingInnerThings = new List<Thing>
					{
						list[0]
					};
				}
				else
				{
					resolveParams.singleThingToSpawn = list[i];
				}
				BaseGen.symbolStack.Push("thing", resolveParams, null);
			}
			if (!Find.Storyteller.difficulty.peacefulTemples)
			{
				if (Rand.Chance(0.65f))
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.mechanoidsCount = new int?(rp.mechanoidsCount ?? SymbolResolver_Interior_AncientTemple.MechanoidCountRange.RandomInRange);
					BaseGen.symbolStack.Push("randomMechanoidGroup", resolveParams2, null);
				}
				else if (Faction.OfInsects != null)
				{
					ResolveParams resolveParams3 = rp;
					resolveParams3.hivesCount = new int?(rp.hivesCount ?? SymbolResolver_Interior_AncientTemple.HivesCountRange.RandomInRange);
					BaseGen.symbolStack.Push("hives", resolveParams3, null);
				}
			}
			if (rp.rect.Width >= SymbolResolver_Interior_AncientTemple.MinSizeForShrines.x && rp.rect.Height >= SymbolResolver_Interior_AncientTemple.MinSizeForShrines.z)
			{
				BaseGen.symbolStack.Push("ancientShrinesGroup", rp, null);
			}
			if (ModsConfig.IdeologyActive)
			{
				ResolveParams resolveParams4 = rp;
				resolveParams4.singleThingDef = ThingDefOf.AncientBarrel;
				BaseGen.symbolStack.Push("edgeThing", resolveParams4, null);
			}
		}

		// Token: 0x0400524B RID: 21067
		private const float MechanoidsChance = 0.65f;

		// Token: 0x0400524C RID: 21068
		private static readonly IntRange MechanoidCountRange = new IntRange(1, 5);

		// Token: 0x0400524D RID: 21069
		private static readonly IntRange HivesCountRange = new IntRange(1, 2);

		// Token: 0x0400524E RID: 21070
		private static readonly IntVec2 MinSizeForShrines = new IntVec2(4, 3);
	}
}
