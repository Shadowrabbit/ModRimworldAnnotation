using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015EC RID: 5612
	public class SymbolResolver_Refuel : SymbolResolver
	{
		// Token: 0x060083B6 RID: 33718 RVA: 0x002F1224 File Offset: 0x002EF424
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_Refuel.refuelables.Clear();
			foreach (IntVec3 c in rp.rect)
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					CompRefuelable compRefuelable = thingList[i].TryGetComp<CompRefuelable>();
					if (compRefuelable != null && !SymbolResolver_Refuel.refuelables.Contains(compRefuelable))
					{
						SymbolResolver_Refuel.refuelables.Add(compRefuelable);
					}
				}
			}
			for (int j = 0; j < SymbolResolver_Refuel.refuelables.Count; j++)
			{
				float fuelCapacity = SymbolResolver_Refuel.refuelables[j].Props.fuelCapacity;
				float amount = Rand.Range(fuelCapacity / 2f, fuelCapacity);
				SymbolResolver_Refuel.refuelables[j].Refuel(amount);
			}
			SymbolResolver_Refuel.refuelables.Clear();
		}

		// Token: 0x04005231 RID: 21041
		private static List<CompRefuelable> refuelables = new List<CompRefuelable>();
	}
}
