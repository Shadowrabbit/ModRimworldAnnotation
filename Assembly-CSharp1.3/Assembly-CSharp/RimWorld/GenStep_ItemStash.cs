using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CB8 RID: 3256
	public class GenStep_ItemStash : GenStep_Scatterer
	{
		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x06004BDE RID: 19422 RVA: 0x0019424A File Offset: 0x0019244A
		public override int SeedPart
		{
			get
			{
				return 913432591;
			}
		}

		// Token: 0x06004BDF RID: 19423 RVA: 0x00194254 File Offset: 0x00192454
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy))
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false)))
			{
				return false;
			}
			CellRect rect = CellRect.CenteredOn(c, 7, 7);
			List<CellRect> list;
			if (MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list) && list.Any((CellRect x) => x.Overlaps(rect)))
			{
				return false;
			}
			foreach (IntVec3 c2 in rect)
			{
				if (!c2.InBounds(map) || c2.GetEdifice(map) != null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004BE0 RID: 19424 RVA: 0x0019432C File Offset: 0x0019252C
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			CellRect cellRect = CellRect.CenteredOn(loc, 7, 7).ClipInsideMap(map);
			List<CellRect> list;
			if (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list))
			{
				list = new List<CellRect>();
				MapGenerator.SetVar<List<CellRect>>("UsedRects", list);
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = cellRect;
			resolveParams.faction = map.ParentFaction;
			if (parms.sitePart != null && parms.sitePart.things != null && parms.sitePart.things.Any)
			{
				resolveParams.stockpileConcreteContents = parms.sitePart.things;
			}
			else
			{
				ItemStashContentsComp component = map.Parent.GetComponent<ItemStashContentsComp>();
				if (component != null && component.contents.Any)
				{
					resolveParams.stockpileConcreteContents = component.contents;
				}
				else
				{
					resolveParams.thingSetMakerDef = (this.thingSetMakerDef ?? ThingSetMakerDefOf.MapGen_DefaultStockpile);
				}
			}
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("storage", resolveParams, null);
			BaseGen.Generate();
			MapGenerator.SetVar<CellRect>("RectOfInterest", cellRect);
			list.Add(cellRect);
		}

		// Token: 0x04002DE5 RID: 11749
		public ThingSetMakerDef thingSetMakerDef;

		// Token: 0x04002DE6 RID: 11750
		private const int Size = 7;
	}
}
