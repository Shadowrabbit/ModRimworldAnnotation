using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001E92 RID: 7826
	public class SymbolResolver_PlaceChairsNearTables : SymbolResolver
	{
		// Token: 0x0600A85A RID: 43098 RVA: 0x003108B8 File Offset: 0x0030EAB8
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_PlaceChairsNearTables.tables.Clear();
			foreach (IntVec3 c in rp.rect)
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def.IsTable && !SymbolResolver_PlaceChairsNearTables.tables.Contains(thingList[i]))
					{
						SymbolResolver_PlaceChairsNearTables.tables.Add(thingList[i]);
					}
				}
			}
			for (int j = 0; j < SymbolResolver_PlaceChairsNearTables.tables.Count; j++)
			{
				CellRect cellRect = SymbolResolver_PlaceChairsNearTables.tables[j].OccupiedRect().ExpandedBy(1);
				bool flag = false;
				foreach (IntVec3 intVec in cellRect.EdgeCells.InRandomOrder(null))
				{
					if (!cellRect.IsCorner(intVec) && rp.rect.Contains(intVec) && intVec.Standable(map) && intVec.GetEdifice(map) == null && (!flag || !Rand.Bool))
					{
						Rot4 value;
						if (intVec.x == cellRect.minX)
						{
							value = Rot4.East;
						}
						else if (intVec.x == cellRect.maxX)
						{
							value = Rot4.West;
						}
						else if (intVec.z == cellRect.minZ)
						{
							value = Rot4.North;
						}
						else
						{
							value = Rot4.South;
						}
						ResolveParams resolveParams = rp;
						resolveParams.rect = CellRect.SingleCell(intVec);
						resolveParams.singleThingDef = ThingDefOf.DiningChair;
						resolveParams.singleThingStuff = (rp.singleThingStuff ?? ThingDefOf.WoodLog);
						resolveParams.thingRot = new Rot4?(value);
						BaseGen.symbolStack.Push("thing", resolveParams, null);
						flag = true;
					}
				}
			}
			SymbolResolver_PlaceChairsNearTables.tables.Clear();
		}

		// Token: 0x04007236 RID: 29238
		private static List<Thing> tables = new List<Thing>();
	}
}
