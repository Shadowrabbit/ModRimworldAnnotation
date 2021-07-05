using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001701 RID: 5889
	public class QuestNode_Root_Mission_BanditCamp : QuestNode_Root_Mission
	{
		// Token: 0x17001624 RID: 5668
		// (get) Token: 0x060087F8 RID: 34808 RVA: 0x0030C7BB File Offset: 0x0030A9BB
		protected override string QuestTag
		{
			get
			{
				return "BanditCamp";
			}
		}

		// Token: 0x17001625 RID: 5669
		// (get) Token: 0x060087F9 RID: 34809 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool AddCampLootReward
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060087FA RID: 34810 RVA: 0x0030C7C4 File Offset: 0x0030A9C4
		protected override Pawn GetAsker(Quest quest)
		{
			if (Rand.Chance(0.1f))
			{
				return (from f in Find.FactionManager.AllFactions
				where this.factionsToDrawLeaderFrom.Contains(f.def)
				select f).RandomElement<Faction>().leader;
			}
			return quest.GetPawn(new QuestGen_Pawns.GetPawnParms
			{
				mustBeOfKind = PawnKindDefOf.Empire_Royal_NobleWimp,
				mustHaveRoyalTitleInCurrentFaction = true,
				canGeneratePawn = true
			});
		}

		// Token: 0x060087FB RID: 34811 RVA: 0x0030C82E File Offset: 0x0030AA2E
		private float GetSiteThreatPoints(float threatPoints, int population, int pawnCount)
		{
			return threatPoints * ((float)pawnCount / (float)population) * QuestNode_Root_Mission_BanditCamp.PawnCountToSitePointsFactorCurve.Evaluate((float)pawnCount);
		}

		// Token: 0x060087FC RID: 34812 RVA: 0x0030C844 File Offset: 0x0030AA44
		protected override int GetRequiredPawnCount(int population, float threatPoints)
		{
			if (population == 0)
			{
				return -1;
			}
			int num = -1;
			for (int i = 1; i <= population; i++)
			{
				if (this.GetSiteThreatPoints(threatPoints, population, i) >= 200f)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return -1;
			}
			return Rand.RangeInclusive(num, population);
		}

		// Token: 0x060087FD RID: 34813 RVA: 0x0030C888 File Offset: 0x0030AA88
		protected override Site GenerateSite(Pawn asker, float threatPoints, int pawnCount, int population, int tile)
		{
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(SitePartDefOf.BanditCamp, new SitePartParams
				{
					threatPoints = this.GetSiteThreatPoints(threatPoints, population, pawnCount)
				})
			}, tile, (from f in Find.FactionManager.AllFactions
			where !f.temporary && f.def == this.siteFaction
			select f).FirstOrDefault<Faction>(), false, null);
			site.factionMustRemainHostile = true;
			site.desiredThreatPoints = site.ActualThreatPoints;
			return site;
		}

		// Token: 0x060087FE RID: 34814 RVA: 0x0030C8FC File Offset: 0x0030AAFC
		protected override bool DoesPawnCountAsAvailableForFight(Pawn p)
		{
			return !p.Downed && p.health.hediffSet.BleedRateTotal <= 0f && !p.health.hediffSet.HasTendableNonInjuryNonMissingPartHediff(false) && !p.IsQuestLodger() && !p.IsSlave;
		}

		// Token: 0x040055EE RID: 21998
		private const float LeaderChance = 0.1f;

		// Token: 0x040055EF RID: 21999
		private static readonly SimpleCurve PawnCountToSitePointsFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.33f),
				true
			},
			{
				new CurvePoint(3f, 0.37f),
				true
			},
			{
				new CurvePoint(5f, 0.45f),
				true
			},
			{
				new CurvePoint(10f, 0.5f),
				true
			}
		};

		// Token: 0x040055F0 RID: 22000
		private const float MinSiteThreatPoints = 200f;

		// Token: 0x040055F1 RID: 22001
		public List<FactionDef> factionsToDrawLeaderFrom;

		// Token: 0x040055F2 RID: 22002
		public FactionDef siteFaction;
	}
}
