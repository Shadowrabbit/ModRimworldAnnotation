using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C26 RID: 3110
	public class IncidentWorker_TraderCaravanArrival : IncidentWorker_NeutralGroup
	{
		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x0600490B RID: 18699 RVA: 0x00182B21 File Offset: 0x00180D21
		protected override PawnGroupKindDef PawnGroupKindDef
		{
			get
			{
				return PawnGroupKindDefOf.Trader;
			}
		}

		// Token: 0x0600490C RID: 18700 RVA: 0x00182B28 File Offset: 0x00180D28
		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			return base.FactionCanBeGroupSource(f, map, desperate) && f.def.caravanTraderKinds.Count != 0 && f.def.caravanTraderKinds.Any((TraderKindDef t) => this.TraderKindCommonality(t, map, f) > 0f);
		}

		// Token: 0x0600490D RID: 18701 RVA: 0x00182BA0 File Offset: 0x00180DA0
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			return parms.faction == null || !NeutralGroupIncidentUtility.AnyBlockingHostileLord(map, parms.faction);
		}

		// Token: 0x0600490E RID: 18702 RVA: 0x00182BE0 File Offset: 0x00180DE0
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

		// Token: 0x0600490F RID: 18703 RVA: 0x00182C88 File Offset: 0x00180E88
		protected virtual float TraderKindCommonality(TraderKindDef traderKind, Map map, Faction faction)
		{
			if (traderKind.faction != null && faction.def != traderKind.faction)
			{
				return 0f;
			}
			if (ModsConfig.IdeologyActive && faction.ideos != null && traderKind.category == "Slaver")
			{
				using (IEnumerator<Ideo> enumerator = faction.ideos.AllIdeos.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IdeoApprovesOfSlavery())
						{
							return 0f;
						}
					}
				}
			}
			if (traderKind.permitRequiredForTrading != null && !map.mapPawns.FreeColonists.Any((Pawn p) => p.royalty != null && p.royalty.HasPermit(traderKind.permitRequiredForTrading, faction)))
			{
				return 0f;
			}
			return traderKind.CalculatedCommonality;
		}

		// Token: 0x06004910 RID: 18704 RVA: 0x00182D90 File Offset: 0x00180F90
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

		// Token: 0x06004911 RID: 18705 RVA: 0x00182EAC File Offset: 0x001810AC
		protected virtual void SendLetter(IncidentParms parms, List<Pawn> pawns, TraderKindDef traderKind)
		{
			TaggedString baseLetterLabel = "LetterLabelTraderCaravanArrival".Translate(parms.faction.Name, traderKind.label).CapitalizeFirst();
			TaggedString taggedString = "LetterTraderCaravanArrival".Translate(parms.faction.NameColored, traderKind.label).CapitalizeFirst();
			taggedString += "\n\n" + "LetterCaravanArrivalCommonWarning".Translate();
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(pawns, ref baseLetterLabel, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			base.SendStandardLetter(baseLetterLabel, taggedString, LetterDefOf.PositiveEvent, parms, pawns[0], Array.Empty<NamedArgument>());
		}

		// Token: 0x06004912 RID: 18706 RVA: 0x00182F7E File Offset: 0x0018117E
		protected override void ResolveParmsPoints(IncidentParms parms)
		{
			parms.points = TraderCaravanUtility.GenerateGuardPoints();
		}

		// Token: 0x04002C79 RID: 11385
		public const string SlaverTraderKindCategory = "Slaver";
	}
}
