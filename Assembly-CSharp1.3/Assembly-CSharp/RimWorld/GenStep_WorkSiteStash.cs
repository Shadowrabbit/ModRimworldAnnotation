using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CC2 RID: 3266
	public class GenStep_WorkSiteStash : GenStep_Scatterer
	{
		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x06004C06 RID: 19462 RVA: 0x0019424A File Offset: 0x0019244A
		public override int SeedPart
		{
			get
			{
				return 913432591;
			}
		}

		// Token: 0x06004C07 RID: 19463 RVA: 0x001959F0 File Offset: 0x00193BF0
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
			CellRect rect = CellRect.CenteredOn(c, 10, 10);
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

		// Token: 0x06004C08 RID: 19464 RVA: 0x00195AC8 File Offset: 0x00193CC8
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			CellRect cellRect = CellRect.CenteredOn(loc, 10, 10).ClipInsideMap(map);
			List<CellRect> list;
			if (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list))
			{
				list = new List<CellRect>();
				MapGenerator.SetVar<List<CellRect>>("UsedRects", list);
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = cellRect;
			resolveParams.faction = map.ParentFaction;
			resolveParams.workSitePoints = parms.sitePart.site.desiredThreatPoints;
			if (parms.sitePart != null && parms.sitePart.things != null && parms.sitePart.things.Any)
			{
				resolveParams.stockpileConcreteContents = parms.sitePart.things;
			}
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push(this.symbolName, resolveParams, null);
			BaseGen.Generate();
			MapGenerator.SetVar<CellRect>("RectOfInterest", cellRect);
			list.Add(cellRect);
		}

		// Token: 0x04002E01 RID: 11777
		public string symbolName;

		// Token: 0x04002E02 RID: 11778
		private const int Size = 10;
	}
}
