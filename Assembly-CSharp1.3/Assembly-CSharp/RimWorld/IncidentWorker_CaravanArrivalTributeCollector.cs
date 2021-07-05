using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C23 RID: 3107
	public class IncidentWorker_CaravanArrivalTributeCollector : IncidentWorker_TraderCaravanArrival
	{
		// Token: 0x060048FD RID: 18685 RVA: 0x00182728 File Offset: 0x00180928
		protected override bool TryResolveParmsGeneral(IncidentParms parms)
		{
			if (!base.TryResolveParmsGeneral(parms))
			{
				return false;
			}
			if (Faction.OfEmpire == null)
			{
				return false;
			}
			Map map = (Map)parms.target;
			parms.faction = Faction.OfEmpire;
			parms.traderKind = (from t in DefDatabase<TraderKindDef>.AllDefsListForReading
			where t.category == "TributeCollector"
			select t).RandomElementByWeight((TraderKindDef t) => this.TraderKindCommonality(t, map, parms.faction));
			return true;
		}

		// Token: 0x060048FE RID: 18686 RVA: 0x001827CE File Offset: 0x001809CE
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && Faction.OfEmpire != null && this.FactionCanBeGroupSource(Faction.OfEmpire, (Map)parms.target, false);
		}

		// Token: 0x060048FF RID: 18687 RVA: 0x001827F9 File Offset: 0x001809F9
		protected override float TraderKindCommonality(TraderKindDef traderKind, Map map, Faction faction)
		{
			return traderKind.CalculatedCommonality;
		}

		// Token: 0x06004900 RID: 18688 RVA: 0x00182804 File Offset: 0x00180A04
		protected override void SendLetter(IncidentParms parms, List<Pawn> pawns, TraderKindDef traderKind)
		{
			TaggedString baseLetterLabel = "LetterLabelTributeCollectorArrival".Translate().CapitalizeFirst();
			TaggedString taggedString = "LetterTributeCollectorArrival".Translate(parms.faction.Named("FACTION")).CapitalizeFirst();
			taggedString += "\n\n" + "LetterCaravanArrivalCommonWarning".Translate();
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(pawns, ref baseLetterLabel, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			base.SendStandardLetter(baseLetterLabel, taggedString, LetterDefOf.PositiveEvent, parms, pawns[0], Array.Empty<NamedArgument>());
		}

		// Token: 0x04002C78 RID: 11384
		public const string TributeCollectorTraderKindCategory = "TributeCollector";
	}
}
