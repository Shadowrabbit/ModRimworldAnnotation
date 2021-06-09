using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E9B RID: 7835
	public class SymbolResolver_Stockpile : SymbolResolver
	{
		// Token: 0x0600A871 RID: 43121 RVA: 0x003112A4 File Offset: 0x0030F4A4
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			if (rp.stockpileConcreteContents != null)
			{
				this.CalculateFreeCells(rp.rect, 0f);
				int num = 0;
				int num2 = rp.stockpileConcreteContents.Count - 1;
				while (num2 >= 0 && num < this.cells.Count)
				{
					GenSpawn.Spawn(rp.stockpileConcreteContents[num2], this.cells[num], map, WipeMode.Vanish);
					num++;
					num2--;
				}
				for (int i = rp.stockpileConcreteContents.Count - 1; i >= 0; i--)
				{
					if (!rp.stockpileConcreteContents[i].Spawned)
					{
						rp.stockpileConcreteContents[i].Destroy(DestroyMode.Vanish);
					}
				}
				rp.stockpileConcreteContents.Clear();
				return;
			}
			this.CalculateFreeCells(rp.rect, 0.45f);
			ThingSetMakerDef thingSetMakerDef = rp.thingSetMakerDef ?? ThingSetMakerDefOf.MapGen_DefaultStockpile;
			ThingSetMakerParams value;
			if (rp.thingSetMakerParams != null)
			{
				value = rp.thingSetMakerParams.Value;
			}
			else
			{
				value = default(ThingSetMakerParams);
				value.techLevel = new TechLevel?((rp.faction != null) ? rp.faction.def.techLevel : TechLevel.Undefined);
				value.makingFaction = rp.faction;
				value.validator = ((ThingDef x) => rp.faction == null || x.techLevel >= rp.faction.def.techLevel || !x.IsWeapon || x.GetStatValueAbstract(StatDefOf.MarketValue, GenStuff.DefaultStuffFor(x)) >= 100f);
				float num3 = rp.stockpileMarketValue ?? Mathf.Min((float)this.cells.Count * 130f, 1800f);
				value.totalMarketValueRange = new FloatRange?(new FloatRange(num3, num3));
			}
			if (value.countRange == null)
			{
				value.countRange = new IntRange?(new IntRange(this.cells.Count, this.cells.Count));
			}
			ResolveParams rp2 = rp;
			rp2.thingSetMakerDef = thingSetMakerDef;
			rp2.thingSetMakerParams = new ThingSetMakerParams?(value);
			BaseGen.symbolStack.Push("thingSet", rp2, null);
		}

		// Token: 0x0600A872 RID: 43122 RVA: 0x00311514 File Offset: 0x0030F714
		private void CalculateFreeCells(CellRect rect, float freeCellsFraction)
		{
			Map map = BaseGen.globalSettings.map;
			this.cells.Clear();
			foreach (IntVec3 intVec in rect)
			{
				if (intVec.Standable(map) && intVec.GetFirstItem(map) == null)
				{
					this.cells.Add(intVec);
				}
			}
			int num = (int)(freeCellsFraction * (float)this.cells.Count);
			for (int i = 0; i < num; i++)
			{
				this.cells.RemoveAt(Rand.Range(0, this.cells.Count));
			}
			this.cells.Shuffle<IntVec3>();
		}

		// Token: 0x04007241 RID: 29249
		private List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x04007242 RID: 29250
		private const float FreeCellsFraction = 0.45f;
	}
}
