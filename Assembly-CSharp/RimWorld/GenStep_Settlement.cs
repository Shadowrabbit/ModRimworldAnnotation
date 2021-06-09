using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020012B7 RID: 4791
	public class GenStep_Settlement : GenStep_Scatterer
	{
		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x060067FB RID: 26619 RVA: 0x00046D67 File Offset: 0x00044F67
		public override int SeedPart
		{
			get
			{
				return 1806208471;
			}
		}

		// Token: 0x060067FC RID: 26620 RVA: 0x0020176C File Offset: 0x001FF96C
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
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
			{
				return false;
			}
			int min = GenStep_Settlement.SettlementSizeRange.min;
			CellRect cellRect = new CellRect(c.x - min / 2, c.z - min / 2, min, min);
			return cellRect.FullyContainedWithin(new CellRect(0, 0, map.Size.x, map.Size.z));
		}

		// Token: 0x060067FD RID: 26621 RVA: 0x00201804 File Offset: 0x001FFA04
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
			if (faction != null && faction == Faction.Empire)
			{
				BaseGen.globalSettings.minThroneRooms = 1;
				BaseGen.globalSettings.minLandingPads = 1;
			}
			BaseGen.Generate();
			if (faction != null && faction == Faction.Empire && BaseGen.globalSettings.landingPadsGenerated == 0)
			{
				CellRect cellRect;
				GenStep_Settlement.GenerateLandingPadNearby(resolveParams.rect, map, faction, out cellRect);
			}
		}

		// Token: 0x060067FE RID: 26622 RVA: 0x0020191C File Offset: 0x001FFB1C
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

		// Token: 0x04004545 RID: 17733
		private static readonly IntRange SettlementSizeRange = new IntRange(34, 38);

		// Token: 0x04004546 RID: 17734
		private static List<IntVec3> tmpCandidates = new List<IntVec3>();
	}
}
