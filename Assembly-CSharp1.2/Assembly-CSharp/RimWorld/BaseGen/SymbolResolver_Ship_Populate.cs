using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001EAB RID: 7851
	public class SymbolResolver_Ship_Populate : SymbolResolver
	{
		// Token: 0x0600A8A5 RID: 43173 RVA: 0x003124A8 File Offset: 0x003106A8
		public override void Resolve(ResolveParams rp)
		{
			if (rp.thrustAxis == null)
			{
				Log.ErrorOnce("No thrust axis when generating ship parts", 50627817, false);
			}
			foreach (KeyValuePair<ThingDef, int> keyValuePair in ShipUtility.RequiredParts())
			{
				for (int i = 0; i < keyValuePair.Value; i++)
				{
					Rot4 rotation = Rot4.Random;
					if (keyValuePair.Key == ThingDefOf.Ship_Engine && rp.thrustAxis != null)
					{
						rotation = rp.thrustAxis.Value;
					}
					this.AttemptToPlace(keyValuePair.Key, rp.rect, rotation, rp.faction);
				}
			}
		}

		// Token: 0x0600A8A6 RID: 43174 RVA: 0x0031256C File Offset: 0x0031076C
		public void AttemptToPlace(ThingDef thingDef, CellRect rect, Rot4 rotation, Faction faction)
		{
			Map map = BaseGen.globalSettings.map;
			Func<IntVec3, bool> <>9__1;
			Thing thing;
			IntVec3 loc = rect.Cells.InRandomOrder(null).Where(delegate(IntVec3 cell)
			{
				if (GenConstruct.CanPlaceBlueprintAt(thingDef, cell, rotation, map, false, null, null, null).Accepted)
				{
					IEnumerable<IntVec3> adjacentCellsCardinal = GenAdj.OccupiedRect(cell, rotation, thingDef.Size).AdjacentCellsCardinal;
					Func<IntVec3, bool> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = delegate(IntVec3 edgeCell)
						{
							if (edgeCell.InBounds(map))
							{
								return edgeCell.GetThingList(map).Any((Thing thing) => thing.def == ThingDefOf.Ship_Beam);
							}
							return false;
						});
					}
					return adjacentCellsCardinal.Any(predicate);
				}
				return false;
			}).FirstOrFallback(IntVec3.Invalid);
			if (loc.IsValid)
			{
				thing = ThingMaker.MakeThing(thingDef, null);
				thing.SetFaction(faction, null);
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null)
				{
					compHibernatable.State = HibernatableStateDefOf.Hibernating;
				}
				GenSpawn.Spawn(thing, loc, BaseGen.globalSettings.map, rotation, WipeMode.Vanish, false);
			}
		}
	}
}
