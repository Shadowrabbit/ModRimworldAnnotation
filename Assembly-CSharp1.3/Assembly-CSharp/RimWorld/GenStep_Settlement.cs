using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CAE RID: 3246
	public class GenStep_Settlement : GenStep_Scatterer
	{
		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06004BAD RID: 19373 RVA: 0x00193434 File Offset: 0x00191634
		public override int SeedPart
		{
			get
			{
				return 1806208471;
			}
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x0019343C File Offset: 0x0019163C
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.Standable(map))
			{
				return false;
			}
			if (c.Roofed(map))
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false)))
			{
				return false;
			}
			int min = GenStep_Settlement.SettlementSizeRange.min;
			CellRect cellRect = new CellRect(c.x - min / 2, c.z - min / 2, min, min);
			return cellRect.FullyContainedWithin(new CellRect(0, 0, map.Size.x, map.Size.z));
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x001934D8 File Offset: 0x001916D8
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			int randomInRange = GenStep_Settlement.SettlementSizeRange.RandomInRange;
			int randomInRange2 = GenStep_Settlement.SettlementSizeRange.RandomInRange;
			CellRect rect = new CellRect(c.x - randomInRange / 2, c.z - randomInRange2 / 2, randomInRange, randomInRange2);
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = map.ParentFaction;
			}
			rect.ClipInsideMap(map);
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = rect;
			resolveParams.faction = faction;
			BaseGen.globalSettings.map = map;
			BaseGen.globalSettings.minBuildings = 1;
			BaseGen.globalSettings.minBarracks = 1;
			BaseGen.symbolStack.Push("settlement", resolveParams, null);
			if (faction != null && faction == Faction.OfEmpire)
			{
				BaseGen.globalSettings.minThroneRooms = 1;
				BaseGen.globalSettings.minLandingPads = 1;
			}
			BaseGen.Generate();
			if (faction != null && faction == Faction.OfEmpire && BaseGen.globalSettings.landingPadsGenerated == 0)
			{
				CellRect cellRect;
				GenStep_Settlement.GenerateLandingPadNearby(resolveParams.rect, map, faction, out cellRect);
			}
		}

		// Token: 0x06004BB0 RID: 19376 RVA: 0x001935F0 File Offset: 0x001917F0
		public static void GenerateLandingPadNearby(CellRect rect, Map map, Faction faction, out CellRect usedRect)
		{
			ResolveParams resolveParams = default(ResolveParams);
			List<CellRect> usedRects;
			MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out usedRects);
			GenStep_Settlement.tmpCandidates.Clear();
			int size = 9;
			GenStep_Settlement.tmpCandidates.Add(new IntVec3(rect.maxX + 1, 0, rect.CenterCell.z));
			GenStep_Settlement.tmpCandidates.Add(new IntVec3(rect.minX - size, 0, rect.CenterCell.z));
			GenStep_Settlement.tmpCandidates.Add(new IntVec3(rect.CenterCell.x, 0, rect.maxZ + 1));
			GenStep_Settlement.tmpCandidates.Add(new IntVec3(rect.CenterCell.x, 0, rect.minZ - size));
			IntVec3 intVec;
			if (!GenStep_Settlement.tmpCandidates.Where(delegate(IntVec3 x)
			{
				CellRect r = new CellRect(x.x, x.z, size, size);
				return r.InBounds(map) && (usedRects == null || !usedRects.Any((CellRect y) => y.Overlaps(r)));
			}).TryRandomElement(out intVec))
			{
				usedRect = CellRect.Empty;
				return;
			}
			resolveParams.rect = new CellRect(intVec.x, intVec.z, size, size);
			resolveParams.faction = faction;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("landingPad", resolveParams, null);
			BaseGen.Generate();
			usedRect = resolveParams.rect;
		}

		// Token: 0x04002DDC RID: 11740
		private static readonly IntRange SettlementSizeRange = new IntRange(34, 38);

		// Token: 0x04002DDD RID: 11741
		private static List<IntVec3> tmpCandidates = new List<IntVec3>();
	}
}
