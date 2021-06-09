using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FD7 RID: 8151
	public class QuestNode_Root_Mission_BanditCamp : QuestNode_Root_Mission
	{
		// Token: 0x17001983 RID: 6531
		// (get) Token: 0x0600ACE6 RID: 44262 RVA: 0x00070BAA File Offset: 0x0006EDAA
		protected override string QuestTag
		{
			get
			{
				return "BanditCamp";
			}
		}

		// Token: 0x17001984 RID: 6532
		// (get) Token: 0x0600ACE7 RID: 44263 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool AddCampLootReward
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600ACE8 RID: 44264 RVA: 0x00325338 File Offset: 0x00323538
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

		// Token: 0x0600ACE9 RID: 44265 RVA: 0x00070BB1 File Offset: 0x0006EDB1
		private float GetSiteThreatPoints(float threatPoints, int population, int pawnCount)
		{
			return threatPoints * ((float)pawnCount / (float)population) * QuestNode_Root_Mission_BanditCamp.PawnCountToSitePointsFactorCurve.Evaluate((float)pawnCount);
		}

		// Token: 0x0600ACEA RID: 44266 RVA: 0x003253A4 File Offset: 0x003235A4
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

		// Token: 0x0600ACEB RID: 44267 RVA: 0x003253E8 File Offset: 0x003235E8
		protected override Site GenerateSite(Pawn asker, float threatPoints, int pawnCount, int population, int tile)
		{
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(SitePartDefOf.BanditCamp, new SitePartParams
				{
					threatPoints = this.GetSiteThreatPoints(threatPoints, population, pawnCount)
				})
			}, tile, (from f in Find.FactionManager.AllFactions
			where f.def == this.siteFaction
			select f).FirstOrDefault<Faction>(), false, null);
			site.factionMustRemainHostile = true;
			site.desiredThreatPoints = site.ActualThreatPoints;
			return site;
		}

		// Token: 0x0600ACEC RID: 44268 RVA: 0x0032545C File Offset: 0x0032365C
		protected override bool DoesPawnCountAsAvailableForFight(Pawn p)
		{
			return !p.Downed && p.health.hediffSet.BleedRateTotal <= 0f && !p.health.hediffSet.HasTendableNonInjuryNonMissingPartHediff(false) && !p.IsQuestLodger();
		}

		// Token: 0x04007667 RID: 30311
		private const float LeaderChance = 0.1f;

		// Token: 0x04007668 RID: 30312
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

		// Token: 0x04007669 RID: 30313
		private const float MinSiteThreatPoints = 200f;

		// Token: 0x0400766A RID: 30314
		public List<FactionDef> factionsToDrawLeaderFrom;

		// Token: 0x0400766B RID: 30315
		public FactionDef siteFaction;
	}
}
