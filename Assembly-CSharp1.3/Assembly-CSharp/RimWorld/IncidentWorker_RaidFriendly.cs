using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C1E RID: 3102
	public class IncidentWorker_RaidFriendly : IncidentWorker_Raid
	{
		// Token: 0x060048CF RID: 18639 RVA: 0x001812A0 File Offset: 0x0017F4A0
		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			IEnumerable<Faction> source = (from x in map.attackTargetsCache.TargetsHostileToColony
			where GenHostility.IsActiveThreatToPlayer(x)
			select x into p
			select ((Thing)p).Faction).Distinct<Faction>();
			return base.FactionCanBeGroupSource(f, map, desperate) && !f.Hidden && f.PlayerRelationKind == FactionRelationKind.Ally && (!source.Any<Faction>() || source.Any((Faction hf) => hf.HostileTo(f)));
		}

		// Token: 0x060048D0 RID: 18640 RVA: 0x0018135C File Offset: 0x0017F55C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			return (from p in ((Map)parms.target).attackTargetsCache.TargetsHostileToColony
			where GenHostility.IsActiveThreatToPlayer(p)
			select p).Sum(delegate(IAttackTarget p)
			{
				Pawn pawn = p as Pawn;
				if (pawn != null)
				{
					return pawn.kindDef.combatPower;
				}
				return 0f;
			}) > 120f;
		}

		// Token: 0x060048D1 RID: 18641 RVA: 0x001813D8 File Offset: 0x0017F5D8
		protected override bool TryResolveRaidFaction(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (parms.faction != null)
			{
				return true;
			}
			if (!base.CandidateFactions(map, false).Any<Faction>())
			{
				return false;
			}
			parms.faction = base.CandidateFactions(map, false).RandomElementByWeight((Faction fac) => (float)fac.PlayerGoodwill + 120.00001f);
			return true;
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x0018143F File Offset: 0x0017F63F
		public override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			if (parms.raidStrategy != null)
			{
				return;
			}
			parms.raidStrategy = RaidStrategyDefOf.ImmediateAttackFriendly;
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x00181455 File Offset: 0x0017F655
		protected override void ResolveRaidPoints(IncidentParms parms)
		{
			if (parms.points <= 0f)
			{
				parms.points = StorytellerUtility.DefaultThreatPointsNow(parms.target);
			}
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x00181475 File Offset: 0x0017F675
		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelFriendly + ": " + parms.faction.Name;
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x00181498 File Offset: 0x0017F698
		protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
		{
			string text = string.Format(parms.raidArrivalMode.textFriendly, parms.faction.def.pawnsPlural, parms.faction.Name.ApplyTag(parms.faction));
			text += "\n\n";
			text += parms.raidStrategy.arrivalTextFriendly;
			Pawn pawn = pawns.Find((Pawn x) => x.Faction.leader == x);
			if (pawn != null)
			{
				text += "\n\n";
				text += "FriendlyRaidLeaderPresent".Translate(pawn.Faction.def.pawnsPlural, pawn.LabelShort, pawn.Named("LEADER"));
			}
			return text;
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x00181576 File Offset: 0x0017F776
		protected override LetterDef GetLetterDef()
		{
			return LetterDefOf.PositiveEvent;
		}

		// Token: 0x060048D7 RID: 18647 RVA: 0x0018157D File Offset: 0x0017F77D
		protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsRaidFriendly".Translate(Faction.OfPlayer.def.pawnsPlural, parms.faction.def.pawnsPlural);
		}
	}
}
