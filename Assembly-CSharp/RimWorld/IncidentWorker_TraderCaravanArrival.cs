using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020011FE RID: 4606
	public class IncidentWorker_TraderCaravanArrival : IncidentWorker_NeutralGroup
	{
		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x060064CB RID: 25803 RVA: 0x000450F7 File Offset: 0x000432F7
		protected override PawnGroupKindDef PawnGroupKindDef
		{
			get
			{
				return PawnGroupKindDefOf.Trader;
			}
		}

		// Token: 0x060064CC RID: 25804 RVA: 0x001F4104 File Offset: 0x001F2304
		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			return base.FactionCanBeGroupSource(f, map, desperate) && f.def.caravanTraderKinds.Count != 0 && f.def.caravanTraderKinds.Any((TraderKindDef t) => this.TraderKindCommonality(t, map, f) > 0f);
		}

		// Token: 0x060064CD RID: 25805 RVA: 0x001F417C File Offset: 0x001F237C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			return parms.faction == null || !NeutralGroupIncidentUtility.AnyBlockingHostileLord(map, parms.faction);
		}

		// Token: 0x060064CE RID: 25806 RVA: 0x001F41BC File Offset: 0x001F23BC
		protected override bool TryResolveParmsGeneral(IncidentParms parms)
		{
			if (!base.TryResolveParmsGeneral(parms))
			{
				return false;
			}
			if (parms.traderKind == null)
			{
				Map map = (Map)parms.target;
				if (!parms.faction.def.caravanTraderKinds.TryRandomElementByWeight((TraderKindDef traderDef) => this.TraderKindCommonality(traderDef, map, parms.faction), out parms.traderKind))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060064CF RID: 25807 RVA: 0x001F4264 File Offset: 0x001F2464
		protected virtual float TraderKindCommonality(TraderKindDef traderKind, Map map, Faction faction)
		{
			if (traderKind.faction == null)
			{
				return traderKind.CalculatedCommonality;
			}
			if (faction.def != traderKind.faction)
			{
				return 0f;
			}
			if (traderKind.permitRequiredForTrading != null && !map.mapPawns.FreeColonists.Any((Pawn p) => p.royalty != null && p.royalty.HasPermit(traderKind.permitRequiredForTrading, faction)))
			{
				return 0f;
			}
			return traderKind.CalculatedCommonality;
		}

		// Token: 0x060064D0 RID: 25808 RVA: 0x001F42F8 File Offset: 0x001F24F8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!base.TryResolveParms(parms))
			{
				return false;
			}
			if (parms.faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			List<Pawn> list = base.SpawnPawns(parms);
			if (list.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].needs != null && list[i].needs.food != null)
				{
					list[i].needs.food.CurLevel = list[i].needs.food.MaxLevel;
				}
			}
			TraderKindDef traderKind = null;
			for (int j = 0; j < list.Count; j++)
			{
				Pawn pawn = list[j];
				if (pawn.TraderKind != null)
				{
					traderKind = pawn.TraderKind;
					break;
				}
			}
			this.SendLetter(parms, list, traderKind);
			IntVec3 chillSpot;
			RCellFinder.TryFindRandomSpotJustOutsideColony(list[0], out chillSpot);
			LordJob_TradeWithColony lordJob = new LordJob_TradeWithColony(parms.faction, chillSpot);
			LordMaker.MakeNewLord(parms.faction, lordJob, map, list);
			return true;
		}

		// Token: 0x060064D1 RID: 25809 RVA: 0x001F4414 File Offset: 0x001F2614
		protected virtual void SendLetter(IncidentParms parms, List<Pawn> pawns, TraderKindDef traderKind)
		{
			TaggedString baseLetterLabel = "LetterLabelTraderCaravanArrival".Translate(parms.faction.Name, traderKind.label).CapitalizeFirst();
			TaggedString taggedString = "LetterTraderCaravanArrival".Translate(parms.faction.NameColored, traderKind.label).CapitalizeFirst();
			taggedString += "\n\n" + "LetterCaravanArrivalCommonWarning".Translate();
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(pawns, ref baseLetterLabel, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			base.SendStandardLetter(baseLetterLabel, taggedString, LetterDefOf.PositiveEvent, parms, pawns[0], Array.Empty<NamedArgument>());
		}

		// Token: 0x060064D2 RID: 25810 RVA: 0x000450FE File Offset: 0x000432FE
		protected override void ResolveParmsPoints(IncidentParms parms)
		{
			parms.points = TraderCaravanUtility.GenerateGuardPoints();
		}
	}
}
