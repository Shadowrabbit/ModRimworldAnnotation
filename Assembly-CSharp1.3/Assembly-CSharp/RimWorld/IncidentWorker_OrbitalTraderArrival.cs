using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C12 RID: 3090
	public class IncidentWorker_OrbitalTraderArrival : IncidentWorker
	{
		// Token: 0x06004898 RID: 18584 RVA: 0x0018015D File Offset: 0x0017E35D
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && ((Map)parms.target).passingShipManager.passingShips.Count < 5;
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x0018018C File Offset: 0x0017E38C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (map.passingShipManager.passingShips.Count >= 5)
			{
				return false;
			}
			if (parms.traderKind == null)
			{
				if (!(from x in DefDatabase<TraderKindDef>.AllDefs
				where this.CanSpawn(map, x)
				select x).TryRandomElementByWeight((TraderKindDef traderDef) => traderDef.CalculatedCommonality, out parms.traderKind))
				{
					throw new InvalidOperationException();
				}
			}
			TradeShip tradeShip = new TradeShip(parms.traderKind, this.GetFaction(parms.traderKind));
			if (map.listerBuildings.allBuildingsColonist.Any((Building b) => b.def.IsCommsConsole && (b.GetComp<CompPowerTrader>() == null || b.GetComp<CompPowerTrader>().PowerOn)))
			{
				base.SendStandardLetter(tradeShip.def.LabelCap, "TraderArrival".Translate(tradeShip.name, tradeShip.def.label, (tradeShip.Faction == null) ? "TraderArrivalNoFaction".Translate() : "TraderArrivalFromFaction".Translate(tradeShip.Faction.Named("FACTION"))), LetterDefOf.PositiveEvent, parms, LookTargets.Invalid, Array.Empty<NamedArgument>());
			}
			map.passingShipManager.AddShip(tradeShip);
			tradeShip.GenerateThings();
			return true;
		}

		// Token: 0x0600489A RID: 18586 RVA: 0x00180300 File Offset: 0x0017E500
		private Faction GetFaction(TraderKindDef trader)
		{
			if (trader.faction == null)
			{
				return null;
			}
			Faction result;
			if (!(from f in Find.FactionManager.AllFactions
			where f.def == trader.faction
			select f).TryRandomElement(out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600489B RID: 18587 RVA: 0x00180350 File Offset: 0x0017E550
		private bool CanSpawn(Map map, TraderKindDef trader)
		{
			if (!trader.orbital)
			{
				return false;
			}
			if (trader.faction == null)
			{
				return true;
			}
			Faction faction = this.GetFaction(trader);
			if (faction == null)
			{
				return false;
			}
			using (List<Pawn>.Enumerator enumerator = map.mapPawns.FreeColonists.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CanTradeWith(faction, trader).Accepted)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04002C6B RID: 11371
		private const int MaxShips = 5;
	}
}
