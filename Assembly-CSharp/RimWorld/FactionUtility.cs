using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020015A5 RID: 5541
	public static class FactionUtility
	{
		// Token: 0x06007851 RID: 30801 RVA: 0x0005104D File Offset: 0x0004F24D
		public static bool HostileTo(this Faction fac, Faction other)
		{
			return fac != null && other != null && other != fac && fac.RelationWith(other, false).kind == FactionRelationKind.Hostile;
		}

		// Token: 0x06007852 RID: 30802 RVA: 0x0005106B File Offset: 0x0004F26B
		public static bool AllyOrNeutralTo(this Faction fac, Faction other)
		{
			return !fac.HostileTo(other);
		}

		// Token: 0x06007853 RID: 30803 RVA: 0x002498B0 File Offset: 0x00247AB0
		public static AcceptanceReport CanTradeWith_NewTemp(this Pawn p, Faction faction, TraderKindDef traderKind = null)
		{
			if (p.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
			{
				return AcceptanceReport.WasRejected;
			}
			if (faction != null)
			{
				if (faction.HostileTo(p.Faction))
				{
					return AcceptanceReport.WasRejected;
				}
				if (traderKind == null || traderKind.permitRequiredForTrading == null)
				{
					return AcceptanceReport.WasAccepted;
				}
				if (p.royalty == null || !p.royalty.HasPermit(traderKind.permitRequiredForTrading, faction))
				{
					return new AcceptanceReport("MessageNeedRoyalTitleToCallWithShip".Translate(traderKind.TitleRequiredToTrade));
				}
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06007854 RID: 30804 RVA: 0x00249944 File Offset: 0x00247B44
		public static bool CanTradeWith(this Pawn p, Faction faction, TraderKindDef traderKind = null)
		{
			return p.CanTradeWith_NewTemp(faction, traderKind).Accepted;
		}

		// Token: 0x06007855 RID: 30805 RVA: 0x00249964 File Offset: 0x00247B64
		public static Faction DefaultFactionFrom(FactionDef ft)
		{
			if (ft == null)
			{
				return null;
			}
			if (ft.isPlayer)
			{
				return Faction.OfPlayer;
			}
			Faction result;
			if ((from fac in Find.FactionManager.AllFactions
			where fac.def == ft
			select fac).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06007856 RID: 30806 RVA: 0x00051077 File Offset: 0x0004F277
		public static bool IsPoliticallyProper(this Thing thing, Pawn pawn)
		{
			return thing.Faction == null || pawn.Faction == null || thing.Faction == pawn.Faction || thing.Faction == pawn.HostFaction;
		}
	}
}
