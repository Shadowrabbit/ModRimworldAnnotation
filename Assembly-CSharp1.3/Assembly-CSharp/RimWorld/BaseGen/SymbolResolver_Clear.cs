using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015B1 RID: 5553
	public class SymbolResolver_Clear : SymbolResolver
	{
		// Token: 0x060082EF RID: 33519 RVA: 0x002E9070 File Offset: 0x002E7270
		public override void Resolve(ResolveParams rp)
		{
			foreach (IntVec3 c in rp.rect)
			{
				if (rp.clearEdificeOnly != null && rp.clearEdificeOnly.Value)
				{
					Building edifice = c.GetEdifice(BaseGen.globalSettings.map);
					if (edifice != null && edifice.def.destroyable)
					{
						edifice.Destroy(DestroyMode.Vanish);
					}
				}
				else if (rp.clearFillageOnly != null && rp.clearFillageOnly.Value)
				{
					SymbolResolver_Clear.tmpThingsToDestroy.Clear();
					SymbolResolver_Clear.tmpThingsToDestroy.AddRange(c.GetThingList(BaseGen.globalSettings.map));
					for (int i = 0; i < SymbolResolver_Clear.tmpThingsToDestroy.Count; i++)
					{
						if (SymbolResolver_Clear.tmpThingsToDestroy[i].def.destroyable && SymbolResolver_Clear.tmpThingsToDestroy[i].def.Fillage != FillCategory.None)
						{
							SymbolResolver_Clear.tmpThingsToDestroy[i].Destroy(DestroyMode.Vanish);
						}
					}
				}
				else
				{
					SymbolResolver_Clear.tmpThingsToDestroy.Clear();
					SymbolResolver_Clear.tmpThingsToDestroy.AddRange(c.GetThingList(BaseGen.globalSettings.map));
					for (int j = 0; j < SymbolResolver_Clear.tmpThingsToDestroy.Count; j++)
					{
						if (SymbolResolver_Clear.tmpThingsToDestroy[j].def.destroyable)
						{
							SymbolResolver_Clear.tmpThingsToDestroy[j].Destroy(DestroyMode.Vanish);
						}
					}
				}
				if (rp.clearRoof != null && rp.clearRoof.Value)
				{
					BaseGen.globalSettings.map.roofGrid.SetRoof(c, null);
				}
			}
		}

		// Token: 0x040051ED RID: 20973
		private static List<Thing> tmpThingsToDestroy = new List<Thing>();
	}
}
