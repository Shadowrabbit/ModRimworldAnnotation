using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CBC RID: 3260
	public class GenStep_Outpost : GenStep
	{
		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x06004BE9 RID: 19433 RVA: 0x0019471C File Offset: 0x0019291C
		public override int SeedPart
		{
			get
			{
				return 398638181;
			}
		}

		// Token: 0x06004BEA RID: 19434 RVA: 0x00194724 File Offset: 0x00192924
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect rectToDefend;
			if (!MapGenerator.TryGetVar<CellRect>("RectOfInterest", out rectToDefend))
			{
				rectToDefend = CellRect.SingleCell(map.Center);
			}
			List<CellRect> list;
			if (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list))
			{
				list = new List<CellRect>();
				MapGenerator.SetVar<List<CellRect>>("UsedRects", list);
			}
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = map.ParentFaction;
			}
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = this.GetOutpostRect(rectToDefend, list, map);
			resolveParams.faction = faction;
			resolveParams.edgeDefenseWidth = new int?(2);
			resolveParams.edgeDefenseTurretsCount = new int?(Rand.RangeInclusive(0, 1));
			resolveParams.edgeDefenseMortarsCount = new int?(0);
			resolveParams.settlementDontGeneratePawns = new bool?(this.settlementDontGeneratePawns);
			resolveParams.bedCount = ((parms.sitePart.expectedEnemyCount == -1) ? null : new int?(parms.sitePart.expectedEnemyCount));
			resolveParams.sitePart = parms.sitePart;
			resolveParams.attackWhenPlayerBecameEnemy = new bool?(this.attackWhenPlayerBecameEnemy);
			resolveParams.pawnGroupKindDef = this.pawnGroupKindDef;
			if (parms.sitePart != null)
			{
				resolveParams.settlementPawnGroupPoints = new float?(parms.sitePart.parms.threatPoints);
				resolveParams.settlementPawnGroupSeed = new int?(OutpostSitePartUtility.GetPawnGroupMakerSeed(parms.sitePart.parms));
			}
			else
			{
				resolveParams.settlementPawnGroupPoints = new float?(this.defaultPawnGroupPointsRange.RandomInRange);
			}
			resolveParams.allowGeneratingThronerooms = new bool?(this.allowGeneratingThronerooms);
			if (this.generateLoot)
			{
				if (parms.sitePart != null)
				{
					resolveParams.lootMarketValue = new float?(parms.sitePart.parms.lootMarketValue);
				}
				else
				{
					resolveParams.lootMarketValue = null;
				}
			}
			else
			{
				resolveParams.lootMarketValue = new float?(0f);
			}
			BaseGen.globalSettings.map = map;
			BaseGen.globalSettings.minBuildings = this.requiredWorshippedTerminalRooms + 1;
			BaseGen.globalSettings.minBarracks = 1;
			BaseGen.globalSettings.requiredWorshippedTerminalRooms = this.requiredWorshippedTerminalRooms;
			BaseGen.globalSettings.maxFarms = (this.allowGeneratingFarms ? -1 : 0);
			BaseGen.symbolStack.Push("settlement", resolveParams, null);
			if (faction != null && faction == Faction.OfEmpire)
			{
				BaseGen.globalSettings.minThroneRooms = (this.allowGeneratingThronerooms ? 1 : 0);
				BaseGen.globalSettings.minLandingPads = 1;
			}
			BaseGen.Generate();
			if (faction != null && faction == Faction.OfEmpire && BaseGen.globalSettings.landingPadsGenerated == 0)
			{
				CellRect item;
				GenStep_Settlement.GenerateLandingPadNearby(resolveParams.rect, map, faction, out item);
				list.Add(item);
			}
			if (this.unfogged)
			{
				foreach (IntVec3 item2 in resolveParams.rect)
				{
					MapGenerator.rootsToUnfog.Add(item2);
				}
			}
			list.Add(resolveParams.rect);
		}

		// Token: 0x06004BEB RID: 19435 RVA: 0x00194A2C File Offset: 0x00192C2C
		private CellRect GetOutpostRect(CellRect rectToDefend, List<CellRect> usedRects, Map map)
		{
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.minX - 1 - this.size, rectToDefend.CenterCell.z - this.size / 2, this.size, this.size));
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.maxX + 1, rectToDefend.CenterCell.z - this.size / 2, this.size, this.size));
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2, rectToDefend.minZ - 1 - this.size, this.size, this.size));
			GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2, rectToDefend.maxZ + 1, this.size, this.size));
			CellRect mapRect = new CellRect(0, 0, map.Size.x, map.Size.z);
			GenStep_Outpost.possibleRects.RemoveAll((CellRect x) => !x.FullyContainedWithin(mapRect));
			if (!GenStep_Outpost.possibleRects.Any<CellRect>())
			{
				return rectToDefend;
			}
			IEnumerable<CellRect> source = from x in GenStep_Outpost.possibleRects
			where !usedRects.Any((CellRect y) => x.Overlaps(y))
			select x;
			if (!source.Any<CellRect>())
			{
				GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.minX - 1 - this.size * 2, rectToDefend.CenterCell.z - this.size / 2, this.size, this.size));
				GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.maxX + 1 + this.size, rectToDefend.CenterCell.z - this.size / 2, this.size, this.size));
				GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2, rectToDefend.minZ - 1 - this.size * 2, this.size, this.size));
				GenStep_Outpost.possibleRects.Add(new CellRect(rectToDefend.CenterCell.x - this.size / 2, rectToDefend.maxZ + 1 + this.size, this.size, this.size));
			}
			if (source.Any<CellRect>())
			{
				return source.RandomElement<CellRect>();
			}
			return GenStep_Outpost.possibleRects.RandomElement<CellRect>();
		}

		// Token: 0x04002DEC RID: 11756
		public int size = 16;

		// Token: 0x04002DED RID: 11757
		public int requiredWorshippedTerminalRooms;

		// Token: 0x04002DEE RID: 11758
		public bool allowGeneratingThronerooms = true;

		// Token: 0x04002DEF RID: 11759
		public bool settlementDontGeneratePawns;

		// Token: 0x04002DF0 RID: 11760
		public bool allowGeneratingFarms = true;

		// Token: 0x04002DF1 RID: 11761
		public bool generateLoot = true;

		// Token: 0x04002DF2 RID: 11762
		public bool unfogged;

		// Token: 0x04002DF3 RID: 11763
		public bool attackWhenPlayerBecameEnemy;

		// Token: 0x04002DF4 RID: 11764
		public FloatRange defaultPawnGroupPointsRange = SymbolResolver_Settlement.DefaultPawnsPoints;

		// Token: 0x04002DF5 RID: 11765
		public PawnGroupKindDef pawnGroupKindDef;

		// Token: 0x04002DF6 RID: 11766
		private static List<CellRect> possibleRects = new List<CellRect>();
	}
}
