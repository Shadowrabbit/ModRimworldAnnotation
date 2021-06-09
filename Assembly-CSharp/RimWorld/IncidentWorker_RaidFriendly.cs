using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011E8 RID: 4584
	public class IncidentWorker_RaidFriendly : IncidentWorker_Raid
	{
		// Token: 0x0600645E RID: 25694 RVA: 0x001F2A90 File Offset: 0x001F0C90
		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			IEnumerable<Faction> source = (from x in map.attackTargetsCache.TargetsHostileToColony
			where GenHostility.IsActiveThreatToPlayer(x)
			select x into p
			select ((Thing)p).Faction).Distinct<Faction>();
			return base.FactionCanBeGroupSource(f, map, desperate) && !f.Hidden && f.PlayerRelationKind == FactionRelationKind.Ally && (!source.Any<Faction>() || source.Any((Faction hf) => hf.HostileTo(f)));
		}

		// Token: 0x0600645F RID: 25695 RVA: 0x001F2B4C File Offset: 0x001F0D4C
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

		// Token: 0x06006460 RID: 25696 RVA: 0x001F2BC8 File Offset: 0x001F0DC8
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

		// Token: 0x06006461 RID: 25697 RVA: 0x00044CB5 File Offset: 0x00042EB5
		public override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			if (parms.raidStrategy != null)
			{
				return;
			}
			parms.raidStrategy = RaidStrategyDefOf.ImmediateAttackFriendly;
		}

		// Token: 0x06006462 RID: 25698 RVA: 0x00044CCB File Offset: 0x00042ECB
		protected override void ResolveRaidPoints(IncidentParms parms)
		{
			if (parms.points <= 0f)
			{
				parms.points = StorytellerUtility.DefaultThreatPointsNow(parms.target);
			}
		}

		// Token: 0x06006463 RID: 25699 RVA: 0x00044CEB File Offset: 0x00042EEB
		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelFriendly + ": " + parms.faction.Name;
		}

		// Token: 0x06006464 RID: 25700 RVA: 0x001F2C30 File Offset: 0x001F0E30
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

		// Token: 0x06006465 RID: 25701 RVA: 0x00044D0D File Offset: 0x00042F0D
		protected override LetterDef GetLetterDef()
		{
			return LetterDefOf.PositiveEvent;
		}

		// Token: 0x06006466 RID: 25702 RVA: 0x00044D14 File Offset: 0x00042F14
		protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsRaidFriendly".Translate(Faction.OfPlayer.def.pawnsPlural, parms.faction.def.pawnsPlural);
		}
	}
}
