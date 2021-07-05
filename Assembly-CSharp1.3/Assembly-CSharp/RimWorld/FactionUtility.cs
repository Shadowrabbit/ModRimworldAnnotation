using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC0 RID: 3776
	public static class FactionUtility
	{
		// Token: 0x06005916 RID: 22806 RVA: 0x001E6271 File Offset: 0x001E4471
		public static bool HostileTo(this Faction fac, Faction other)
		{
			return fac != null && other != null && other != fac && fac.RelationWith(other, false).kind == FactionRelationKind.Hostile;
		}

		// Token: 0x06005917 RID: 22807 RVA: 0x001E628F File Offset: 0x001E448F
		public static bool AllyOrNeutralTo(this Faction fac, Faction other)
		{
			return !fac.HostileTo(other);
		}

		// Token: 0x06005918 RID: 22808 RVA: 0x001E629C File Offset: 0x001E449C
		public static AcceptanceReport CanTradeWith(this Pawn p, Faction faction, TraderKindDef traderKind = null)
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

		// Token: 0x06005919 RID: 22809 RVA: 0x001E6330 File Offset: 0x001E4530
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

		// Token: 0x0600591A RID: 22810 RVA: 0x001E638E File Offset: 0x001E458E
		public static bool IsPoliticallyProper(this Thing thing, Pawn pawn)
		{
			return thing.Faction == null || pawn.Faction == null || thing.Faction == pawn.Faction || thing.Faction == pawn.HostFaction;
		}

		// Token: 0x0600591B RID: 22811 RVA: 0x001E63C5 File Offset: 0x001E45C5
		public static bool IsPlayerSafe(this Faction faction)
		{
			return faction != null && faction.IsPlayer;
		}

		// Token: 0x0600591C RID: 22812 RVA: 0x001E63D4 File Offset: 0x001E45D4
		public static void ResetAllFactionRelations()
		{
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				faction.RemoveAllRelations();
				foreach (Faction faction2 in Find.FactionManager.AllFactionsListForReading)
				{
					if (faction != faction2)
					{
						faction.TryMakeInitialRelationsWith(faction2);
					}
				}
			}
		}

		// Token: 0x0600591D RID: 22813 RVA: 0x001E6474 File Offset: 0x001E4674
		public static int GetSlavesInFactionCount(Faction faction)
		{
			if (faction == null)
			{
				return 0;
			}
			int num = 0;
			using (List<Pawn>.Enumerator enumerator = PawnsFinder.AllMaps_SpawnedPawnsInFaction(faction).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsSlave)
					{
						num++;
					}
				}
			}
			return num;
		}
	}
}
