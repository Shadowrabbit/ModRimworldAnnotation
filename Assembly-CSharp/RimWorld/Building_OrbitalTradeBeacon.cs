using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020016C4 RID: 5828
	public class Building_OrbitalTradeBeacon : Building
	{
		// Token: 0x170013DD RID: 5085
		// (get) Token: 0x06007FEC RID: 32748 RVA: 0x00055EE2 File Offset: 0x000540E2
		public IEnumerable<IntVec3> TradeableCells
		{
			get
			{
				return Building_OrbitalTradeBeacon.TradeableCellsAround(base.Position, base.Map);
			}
		}

		// Token: 0x06007FED RID: 32749 RVA: 0x00055EF5 File Offset: 0x000540F5
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>() != null)
			{
				yield return new Command_Action
				{
					action = new Action(this.MakeMatchingStockpile),
					hotKey = KeyBindingDefOf.Misc1,
					defaultDesc = "CommandMakeBeaconStockpileDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true),
					defaultLabel = "CommandMakeBeaconStockpileLabel".Translate()
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06007FEE RID: 32750 RVA: 0x0025EAB0 File Offset: 0x0025CCB0
		private void MakeMatchingStockpile()
		{
			Designator des = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>();
			des.DesignateMultiCell(from c in this.TradeableCells
			where des.CanDesignateCell(c).Accepted
			select c);
		}

		// Token: 0x06007FEF RID: 32751 RVA: 0x0025EAF0 File Offset: 0x0025CCF0
		public static List<IntVec3> TradeableCellsAround(IntVec3 pos, Map map)
		{
			Building_OrbitalTradeBeacon.tradeableCells.Clear();
			if (!pos.InBounds(map))
			{
				return Building_OrbitalTradeBeacon.tradeableCells;
			}
			Region region = pos.GetRegion(map, RegionType.Set_Passable);
			if (region == null)
			{
				return Building_OrbitalTradeBeacon.tradeableCells;
			}
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.door == null, delegate(Region r)
			{
				foreach (IntVec3 item in r.Cells)
				{
					if (item.InHorDistOf(pos, 7.9f))
					{
						Building_OrbitalTradeBeacon.tradeableCells.Add(item);
					}
				}
				return false;
			}, 16, RegionType.Set_Passable);
			return Building_OrbitalTradeBeacon.tradeableCells;
		}

		// Token: 0x06007FF0 RID: 32752 RVA: 0x00055F05 File Offset: 0x00054105
		public static IEnumerable<Building_OrbitalTradeBeacon> AllPowered(Map map)
		{
			foreach (Building_OrbitalTradeBeacon building_OrbitalTradeBeacon in map.listerBuildings.AllBuildingsColonistOfClass<Building_OrbitalTradeBeacon>())
			{
				CompPowerTrader comp = building_OrbitalTradeBeacon.GetComp<CompPowerTrader>();
				if (comp == null || comp.PowerOn)
				{
					yield return building_OrbitalTradeBeacon;
				}
			}
			IEnumerator<Building_OrbitalTradeBeacon> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040052ED RID: 21229
		private const float TradeRadius = 7.9f;

		// Token: 0x040052EE RID: 21230
		private static List<IntVec3> tradeableCells = new List<IntVec3>();
	}
}
