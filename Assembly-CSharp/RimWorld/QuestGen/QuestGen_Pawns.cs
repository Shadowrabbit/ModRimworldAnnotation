using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EC6 RID: 7878
	public static class QuestGen_Pawns
	{
		// Token: 0x0600A91C RID: 43292 RVA: 0x00315280 File Offset: 0x00313480
		public static Pawn GeneratePawn(this Quest quest, PawnKindDef kindDef, Faction faction, bool allowAddictions = true, IEnumerable<TraitDef> forcedTraits = null, float biocodeWeaponChance = 0f, bool mustBeCapableOfViolence = true, Pawn extraPawnForExtraRelationChance = null, float relationWithExtraPawnChanceFactor = 0f, float biocodeApparelChance = 0f, bool ensureNonNumericName = false, bool forceGenerateNewPawn = false)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kindDef, faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn, false, false, false, true, mustBeCapableOfViolence, 1f, false, true, true, allowAddictions, false, false, false, false, biocodeWeaponChance, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, null, null, forcedTraits, null, null, null, null, null, null, null, null, null)
			{
				BiocodeApparelChance = biocodeApparelChance
			});
			if (ensureNonNumericName && (pawn.Name == null || pawn.Name.Numerical))
			{
				pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(pawn, NameStyle.Full, null);
			}
			QuestGen.AddToGeneratedPawns(pawn);
			if (!pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			}
			return pawn;
		}

		// Token: 0x0600A91D RID: 43293 RVA: 0x00315354 File Offset: 0x00313554
		public static bool GetPawnTest(QuestGen_Pawns.GetPawnParms parms, out Pawn pawn)
		{
			pawn = null;
			if (parms.mustHaveNoFaction && parms.mustHaveRoyalTitleInCurrentFaction)
			{
				return false;
			}
			if (parms.canGeneratePawn && (parms.mustBeFactionLeader || parms.mustBePlayerPrisoner || parms.mustBeFreeColonist))
			{
				Log.Warning("QuestGen_GetPawn has incompatible flags set, when canGeneratePawn is true these flags cannot be set: mustBeFactionLeader, mustBePlayerPrisoner, mustBeFreeColonist", false);
				return false;
			}
			IEnumerable<Pawn> source = QuestGen_Pawns.ExistingUsablePawns(parms);
			if (source.Count<Pawn>() > 0)
			{
				pawn = source.RandomElement<Pawn>();
				return true;
			}
			if (!parms.canGeneratePawn)
			{
				return false;
			}
			Faction faction;
			if (!parms.mustHaveNoFaction && !QuestGen_Pawns.TryFindFactionForPawnGeneration(parms, out faction))
			{
				return false;
			}
			FloatRange senRange = parms.seniorityRange;
			return !parms.mustHaveRoyalTitleInCurrentFaction || !parms.requireResearchedBedroomFurnitureIfRoyal || DefDatabase<RoyalTitleDef>.AllDefsListForReading.Any((RoyalTitleDef x) => (senRange.max <= 0f || senRange.IncludesEpsilon((float)x.seniority)) && QuestGen_Pawns.PlayerHasResearchedBedroomRequirementsFor(x));
		}

		// Token: 0x0600A91E RID: 43294 RVA: 0x00315418 File Offset: 0x00313618
		public static Pawn GetPawn(this Quest quest, QuestGen_Pawns.GetPawnParms parms)
		{
			Slate slate = QuestGen.slate;
			IEnumerable<Pawn> source = QuestGen_Pawns.ExistingUsablePawns(parms);
			int num = source.Count<Pawn>();
			Faction faction;
			Pawn pawn;
			if (Rand.Chance(parms.canGeneratePawn ? Mathf.Clamp01(1f - (float)num / 10f) : 0f) && (parms.mustHaveNoFaction || QuestGen_Pawns.TryFindFactionForPawnGeneration(parms, out faction)))
			{
				pawn = QuestGen_Pawns.GeneratePawn(parms, null);
			}
			else
			{
				pawn = source.RandomElement<Pawn>();
			}
			if (pawn.Faction != null && !pawn.Faction.Hidden)
			{
				quest.AddPart(new QuestPart_InvolvedFactions
				{
					factions = 
					{
						pawn.Faction
					}
				});
			}
			QuestGen.AddToGeneratedPawns(pawn);
			return pawn;
		}

		// Token: 0x0600A91F RID: 43295 RVA: 0x003154C4 File Offset: 0x003136C4
		public static IEnumerable<Pawn> ExistingUsablePawns(QuestGen_Pawns.GetPawnParms parms)
		{
			return from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where QuestGen_Pawns.IsGoodPawn(x, parms)
			select x;
		}

		// Token: 0x0600A920 RID: 43296 RVA: 0x003154F4 File Offset: 0x003136F4
		private static bool TryFindFactionForPawnGeneration(QuestGen_Pawns.GetPawnParms parms, out Faction faction)
		{
			FactionManager factionManager = Find.FactionManager;
			bool allowTemporaryFactions = parms.allowTemporaryFactions;
			return (from x in factionManager.GetFactions_NewTemp(parms.allowHidden, false, false, TechLevel.Undefined, allowTemporaryFactions)
			where (parms.mustBeOfFaction == null || x == parms.mustBeOfFaction) && (parms.excludeFactionDefs == null || !parms.excludeFactionDefs.Contains(x.def)) && (!parms.mustHaveRoyalTitleInCurrentFaction || x.def.HasRoyalTitles) && (!parms.mustBeNonHostileToPlayer || !x.HostileTo(Faction.OfPlayer)) && ((parms.allowPermanentEnemyFaction ?? false) || !x.def.permanentEnemy) && x.def.techLevel >= parms.minTechLevel
			select x).TryRandomElement(out faction);
		}

		// Token: 0x0600A921 RID: 43297 RVA: 0x0031554C File Offset: 0x0031374C
		private static Pawn GeneratePawn(QuestGen_Pawns.GetPawnParms parms, Faction faction = null)
		{
			PawnKindDef pawnKindDef = parms.mustBeOfKind;
			if (faction == null && !parms.mustHaveNoFaction)
			{
				if (!QuestGen_Pawns.TryFindFactionForPawnGeneration(parms, out faction))
				{
					Log.Error("QuestNode_GetPawn tried generating pawn but couldn't find a proper faction for new pawn.", false);
				}
				else if (pawnKindDef == null)
				{
					pawnKindDef = faction.RandomPawnKind();
				}
			}
			RoyalTitleDef fixedTitle;
			if (parms.mustHaveRoyalTitleInCurrentFaction)
			{
				FloatRange senRange = parms.seniorityRange;
				IEnumerable<RoyalTitleDef> source = from t in DefDatabase<RoyalTitleDef>.AllDefsListForReading
				where faction.def.RoyalTitlesAllInSeniorityOrderForReading.Contains(t) && (senRange.max <= 0f || senRange.IncludesEpsilon((float)t.seniority))
				select t;
				if (parms.requireResearchedBedroomFurnitureIfRoyal)
				{
					if (source.Any((RoyalTitleDef x) => QuestGen_Pawns.PlayerHasResearchedBedroomRequirementsFor(x)))
					{
						source = from x in source
						where QuestGen_Pawns.PlayerHasResearchedBedroomRequirementsFor(x)
						select x;
					}
				}
				fixedTitle = source.RandomElementByWeight((RoyalTitleDef t) => t.commonality);
				if (parms.mustBeOfKind == null && !(from k in DefDatabase<PawnKindDef>.AllDefsListForReading
				where k.titleRequired != null && k.titleRequired == fixedTitle
				select k).TryRandomElement(out pawnKindDef))
				{
					(from k in DefDatabase<PawnKindDef>.AllDefsListForReading
					where k.titleSelectOne != null && k.titleSelectOne.Contains(fixedTitle)
					select k).TryRandomElement(out pawnKindDef);
				}
			}
			else
			{
				fixedTitle = null;
			}
			if (pawnKindDef == null)
			{
				pawnKindDef = (from kind in DefDatabase<PawnKindDef>.AllDefsListForReading
				where kind.race.race.Humanlike
				select kind).RandomElement<PawnKindDef>();
			}
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKindDef, faction, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, fixedTitle));
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			if (pawn.royalty != null && pawn.royalty.AllTitlesForReading.Any<RoyalTitle>())
			{
				QuestPart_Hyperlinks questPart_Hyperlinks = new QuestPart_Hyperlinks();
				questPart_Hyperlinks.pawns.Add(pawn);
				QuestGen.quest.AddPart(questPart_Hyperlinks);
			}
			return pawn;
		}

		// Token: 0x0600A922 RID: 43298 RVA: 0x003157B0 File Offset: 0x003139B0
		private static bool IsGoodPawn(Pawn pawn, QuestGen_Pawns.GetPawnParms parms)
		{
			if (parms.mustBeFactionLeader)
			{
				Faction faction = pawn.Faction;
				if (faction == null || faction.leader != pawn || !faction.def.humanlikeFaction || faction.defeated || faction.Hidden || faction.IsPlayer || pawn.IsPrisoner)
				{
					return false;
				}
			}
			if (parms.mustBeOfFaction != null && pawn.Faction != parms.mustBeOfFaction)
			{
				return false;
			}
			if (pawn.Faction != null && parms.excludeFactionDefs != null && parms.excludeFactionDefs.Contains(pawn.Faction.def))
			{
				return false;
			}
			if (pawn.Faction != null && pawn.Faction.def.techLevel < parms.minTechLevel)
			{
				return false;
			}
			if (parms.mustBeOfKind != null && pawn.kindDef != parms.mustBeOfKind)
			{
				return false;
			}
			if (parms.mustHaveRoyalTitleInCurrentFaction && (pawn.Faction == null || pawn.royalty == null || !pawn.royalty.HasAnyTitleIn(pawn.Faction)))
			{
				return false;
			}
			if (parms.seniorityRange != default(FloatRange) && (pawn.royalty == null || pawn.royalty.MostSeniorTitle == null || !parms.seniorityRange.IncludesEpsilon((float)pawn.royalty.MostSeniorTitle.def.seniority)))
			{
				return false;
			}
			if (parms.mustBeWorldPawn && !pawn.IsWorldPawn())
			{
				return false;
			}
			if (parms.ifWorldPawnThenMustBeFree && pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.Free)
			{
				return false;
			}
			if (parms.ifWorldPawnThenMustBeFreeOrLeader && pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.Free && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.FactionLeader)
			{
				return false;
			}
			if (pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) == WorldPawnSituation.ReservedByQuest)
			{
				return false;
			}
			if (parms.mustHaveNoFaction && pawn.Faction != null)
			{
				return false;
			}
			if (parms.mustBeFreeColonist && !pawn.IsFreeColonist)
			{
				return false;
			}
			if (parms.mustBePlayerPrisoner && !pawn.IsPrisonerOfColony)
			{
				return false;
			}
			if (parms.mustBeNotSuspended && pawn.Suspended)
			{
				return false;
			}
			if (parms.mustBeNonHostileToPlayer && (pawn.HostileTo(Faction.OfPlayer) || (pawn.Faction != null && pawn.Faction != Faction.OfPlayer && pawn.Faction.HostileTo(Faction.OfPlayer))))
			{
				return false;
			}
			if (!(parms.allowPermanentEnemyFaction ?? true) && pawn.Faction != null && pawn.Faction.def.permanentEnemy)
			{
				return false;
			}
			if (parms.requireResearchedBedroomFurnitureIfRoyal)
			{
				RoyalTitle royalTitle = pawn.royalty.HighestTitleWithBedroomRequirements();
				if (royalTitle != null && !QuestGen_Pawns.PlayerHasResearchedBedroomRequirementsFor(royalTitle.def))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600A923 RID: 43299 RVA: 0x00315A60 File Offset: 0x00313C60
		private static bool PlayerHasResearchedBedroomRequirementsFor(RoyalTitleDef title)
		{
			if (title.bedroomRequirements == null)
			{
				return true;
			}
			for (int i = 0; i < title.bedroomRequirements.Count; i++)
			{
				if (!title.bedroomRequirements[i].PlayerHasResearched())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600A924 RID: 43300 RVA: 0x00315AA4 File Offset: 0x00313CA4
		public static QuestPart_ReservePawns ReservePawns(this Quest quest, IEnumerable<Pawn> pawns)
		{
			QuestPart_ReservePawns questPart_ReservePawns = new QuestPart_ReservePawns();
			questPart_ReservePawns.pawns.AddRange(pawns);
			quest.AddPart(questPart_ReservePawns);
			return questPart_ReservePawns;
		}

		// Token: 0x0600A925 RID: 43301 RVA: 0x00315ACC File Offset: 0x00313CCC
		public static QuestPart_FeedPawns FeedPawns(this Quest quest, IEnumerable<Pawn> pawns = null, Thing pawnsInTransporter = null, string inSignal = null)
		{
			QuestPart_FeedPawns questPart_FeedPawns = new QuestPart_FeedPawns();
			questPart_FeedPawns.pawnsInTransporter = pawnsInTransporter;
			questPart_FeedPawns.pawns = ((pawns == null) ? null : pawns.ToList<Pawn>());
			questPart_FeedPawns.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_FeedPawns);
			return questPart_FeedPawns;
		}

		// Token: 0x0400728B RID: 29323
		public const int MaxUsablePawnsToGenerate = 10;

		// Token: 0x02001EC7 RID: 7879
		public struct GetPawnParms
		{
			// Token: 0x0400728C RID: 29324
			public bool mustBeFactionLeader;

			// Token: 0x0400728D RID: 29325
			public bool mustBeWorldPawn;

			// Token: 0x0400728E RID: 29326
			public bool ifWorldPawnThenMustBeFree;

			// Token: 0x0400728F RID: 29327
			public bool ifWorldPawnThenMustBeFreeOrLeader;

			// Token: 0x04007290 RID: 29328
			public bool mustHaveNoFaction;

			// Token: 0x04007291 RID: 29329
			public bool mustBeFreeColonist;

			// Token: 0x04007292 RID: 29330
			public bool mustBePlayerPrisoner;

			// Token: 0x04007293 RID: 29331
			public bool mustBeNotSuspended;

			// Token: 0x04007294 RID: 29332
			public bool mustHaveRoyalTitleInCurrentFaction;

			// Token: 0x04007295 RID: 29333
			public bool mustBeNonHostileToPlayer;

			// Token: 0x04007296 RID: 29334
			public bool? allowPermanentEnemyFaction;

			// Token: 0x04007297 RID: 29335
			public bool canGeneratePawn;

			// Token: 0x04007298 RID: 29336
			public bool requireResearchedBedroomFurnitureIfRoyal;

			// Token: 0x04007299 RID: 29337
			public PawnKindDef mustBeOfKind;

			// Token: 0x0400729A RID: 29338
			public Faction mustBeOfFaction;

			// Token: 0x0400729B RID: 29339
			public FloatRange seniorityRange;

			// Token: 0x0400729C RID: 29340
			public TechLevel minTechLevel;

			// Token: 0x0400729D RID: 29341
			public List<FactionDef> excludeFactionDefs;

			// Token: 0x0400729E RID: 29342
			public bool allowTemporaryFactions;

			// Token: 0x0400729F RID: 29343
			public bool allowHidden;
		}
	}
}
