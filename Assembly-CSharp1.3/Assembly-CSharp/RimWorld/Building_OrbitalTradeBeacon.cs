using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001080 RID: 4224
	public class Building_OrbitalTradeBeacon : Building
	{
		// Token: 0x17001136 RID: 4406
		// (get) Token: 0x06006488 RID: 25736 RVA: 0x0021E497 File Offset: 0x0021C697
		public IEnumerable<IntVec3> TradeableCells
		{
			get
			{
				return Building_OrbitalTradeBeacon.TradeableCellsAround(base.Position, base.Map);
			}
		}

		// Token: 0x06006489 RID: 25737 RVA: 0x0021E4AA File Offset: 0x0021C6AA
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

		// Token: 0x0600648A RID: 25738 RVA: 0x0021E4BC File Offset: 0x0021C6BC
		private void MakeMatchingStockpile()
		{
			Designator des = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>();
			des.DesignateMultiCell(from c in this.TradeableCells
			where des.CanDesignateCell(c).Accepted
			select c);
		}

		// Token: 0x0600648B RID: 25739 RVA: 0x0021E4FC File Offset: 0x0021C6FC
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

		// Token: 0x0600648C RID: 25740 RVA: 0x0021E586 File Offset: 0x0021C786
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

		// Token: 0x040038A5 RID: 14501
		private const float TradeRadius = 7.9f;

		// Token: 0x040038A6 RID: 14502
		private static List<IntVec3> tradeableCells = new List<IntVec3>();
	}
}
