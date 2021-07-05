using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001617 RID: 5655
	public static class QuestGen_Pawns
	{
		// Token: 0x0600848A RID: 33930 RVA: 0x002F8C30 File Offset: 0x002F6E30
		public static Pawn GeneratePawn(this Quest quest, PawnKindDef kindDef, Faction faction, bool allowAddictions = true, IEnumerable<TraitDef> forcedTraits = null, float biocodeWeaponChance = 0f, bool mustBeCapableOfViolence = true, Pawn extraPawnForExtraRelationChance = null, float relationWithExtraPawnChanceFactor = 0f, float biocodeApparelChance = 0f, bool ensureNonNumericName = false, bool forceGenerateNewPawn = false)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kindDef, faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn, false, false, false, true, mustBeCapableOfViolence, 1f, false, true, true, allowAddictions, false, false, false, false, biocodeWeaponChance, 0f, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, null, null, forcedTraits, null, null, null, null, null, null, null, null, null, null, false, false)
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

		// Token: 0x0600848B RID: 33931 RVA: 0x002F8D0C File Offset: 0x002F6F0C
		public static bool GetPawnTest(QuestGen_Pawns.GetPawnParms parms, out Pawn pawn)
		{
			pawn = null;
			if (parms.mustHaveNoFaction && parms.mustHaveRoyalTitleInCurrentFaction)
			{
				return false;
			}
			if (parms.canGeneratePawn && (parms.mustBeFactionLeader || parms.mustBePlayerPrisoner || parms.mustBeFreeColonist))
			{
				Log.Warning("QuestGen_GetPawn has incompatible flags set, when canGeneratePawn is true these flags cannot be set: mustBeFactionLeader, mustBePlayerPrisoner, mustBeFreeColonist");
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

		// Token: 0x0600848C RID: 33932 RVA: 0x002F8DD0 File Offset: 0x002F6FD0
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

		// Token: 0x0600848D RID: 33933 RVA: 0x002F8E7C File Offset: 0x002F707C
		public static IEnumerable<Pawn> ExistingUsablePawns(QuestGen_Pawns.GetPawnParms parms)
		{
			return from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where QuestGen_Pawns.IsGoodPawn(x, parms)
			select x;
		}

		// Token: 0x0600848E RID: 33934 RVA: 0x002F8EAC File Offset: 0x002F70AC
		private static bool TryFindFactionForPawnGeneration(QuestGen_Pawns.GetPawnParms parms, out Faction faction)
		{
			FactionManager factionManager = Find.FactionManager;
			bool allowTemporaryFactions = parms.allowTemporaryFactions;
			return (from x in factionManager.GetFactions(parms.allowHidden, false, false, TechLevel.Undefined, allowTemporaryFactions)
			where (parms.mustBeOfFaction == null || x == parms.mustBeOfFaction) && (parms.excludeFactionDefs == null || !parms.excludeFactionDefs.Contains(x.def)) && (!parms.mustHaveRoyalTitleInCurrentFaction || x.def.HasRoyalTitles) && (!parms.mustBeNonHostileToPlayer || !x.HostileTo(Faction.OfPlayer)) && ((parms.allowPermanentEnemyFaction ?? false) || !x.def.permanentEnemy) && x.def.techLevel >= parms.minTechLevel
			select x).TryRandomElement(out faction);
		}

		// Token: 0x0600848F RID: 33935 RVA: 0x002F8F04 File Offset: 0x002F7104
		private static Pawn GeneratePawn(QuestGen_Pawns.GetPawnParms parms, Faction faction = null)
		{
			PawnKindDef pawnKindDef = parms.mustBeOfKind;
			if (faction == null && !parms.mustHaveNoFaction)
			{
				if (!QuestGen_Pawns.TryFindFactionForPawnGeneration(parms, out faction))
				{
					Log.Error("QuestNode_GetPawn tried generating pawn but couldn't find a proper faction for new pawn.");
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
			PawnKindDef kind2 = pawnKindDef;
			Faction faction2 = faction;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			int tile = -1;
			bool forceGenerateNewPawn = true;
			bool newborn = false;
			bool allowDead = false;
			bool allowDowned = false;
			bool canGeneratePawnRelations = true;
			RoyalTitleDef fixedTitle2 = fixedTitle;
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind2, faction2, context, tile, forceGenerateNewPawn, newborn, allowDead, allowDowned, canGeneratePawnRelations, parms.mustBeCapableOfViolence, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, fixedTitle2, null, false, false));
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			if (pawn.royalty != null && pawn.royalty.AllTitlesForReading.Any<RoyalTitle>())
			{
				QuestPart_Hyperlinks questPart_Hyperlinks = new QuestPart_Hyperlinks();
				questPart_Hyperlinks.pawns.Add(pawn);
				QuestGen.quest.AddPart(questPart_Hyperlinks);
			}
			return pawn;
		}

		// Token: 0x06008490 RID: 33936 RVA: 0x002F9178 File Offset: 0x002F7378
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
			return !parms.mustBeCapableOfViolence || !pawn.WorkTagIsDisabled(WorkTags.Violent);
		}

		// Token: 0x06008491 RID: 33937 RVA: 0x002F943C File Offset: 0x002F763C
		private static bool PlayerHasResearchedBedroomRequirementsFor(RoyalTitleDef title)
		{
			if (title.bedroomRequirements == null)
			{
				return true;
			}
			for (int i = 0; i < title.bedroomRequirements.Count; i++)
			{
				if (!title.bedroomRequirements[i].PlayerCanBuildNow())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008492 RID: 33938 RVA: 0x002F9480 File Offset: 0x002F7680
		public static QuestPart_ReservePawns ReservePawns(this Quest quest, IEnumerable<Pawn> pawns)
		{
			QuestPart_ReservePawns questPart_ReservePawns = new QuestPart_ReservePawns();
			questPart_ReservePawns.pawns.AddRange(pawns);
			quest.AddPart(questPart_ReservePawns);
			return questPart_ReservePawns;
		}

		// Token: 0x06008493 RID: 33939 RVA: 0x002F94A8 File Offset: 0x002F76A8
		public static QuestPart_TransporterPawns_Feed FeedPawns(this Quest quest, IEnumerable<Pawn> pawns = null, Thing pawnsInTransporter = null, string inSignal = null)
		{
			QuestPart_TransporterPawns_Feed questPart_TransporterPawns_Feed = new QuestPart_TransporterPawns_Feed();
			questPart_TransporterPawns_Feed.pawnsInTransporter = pawnsInTransporter;
			questPart_TransporterPawns_Feed.pawns = ((pawns == null) ? null : pawns.ToList<Pawn>());
			questPart_TransporterPawns_Feed.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_TransporterPawns_Feed);
			return questPart_TransporterPawns_Feed;
		}

		// Token: 0x06008494 RID: 33940 RVA: 0x002F9500 File Offset: 0x002F7700
		public static QuestPart_TransporterPawns_Tend TendPawns(this Quest quest, IEnumerable<Pawn> pawns = null, Thing pawnsInTransporter = null, string inSignal = null)
		{
			QuestPart_TransporterPawns_Tend questPart_TransporterPawns_Tend = new QuestPart_TransporterPawns_Tend();
			questPart_TransporterPawns_Tend.pawnsInTransporter = pawnsInTransporter;
			questPart_TransporterPawns_Tend.pawns = ((pawns == null) ? null : pawns.ToList<Pawn>());
			questPart_TransporterPawns_Tend.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_TransporterPawns_Tend);
			return questPart_TransporterPawns_Tend;
		}

		// Token: 0x06008495 RID: 33941 RVA: 0x002F9558 File Offset: 0x002F7758
		public static QuestPart_TransporterPawns_TendWithMedicine TendPawnsWithMedicine(this Quest quest, ThingDef medicineDef, bool allowSelfTend = true, IEnumerable<Pawn> pawns = null, Thing pawnsInTransporter = null, string inSignal = null)
		{
			QuestPart_TransporterPawns_TendWithMedicine questPart_TransporterPawns_TendWithMedicine = new QuestPart_TransporterPawns_TendWithMedicine();
			questPart_TransporterPawns_TendWithMedicine.pawnsInTransporter = pawnsInTransporter;
			questPart_TransporterPawns_TendWithMedicine.pawns = ((pawns == null) ? null : pawns.ToList<Pawn>());
			questPart_TransporterPawns_TendWithMedicine.medicineDef = medicineDef;
			questPart_TransporterPawns_TendWithMedicine.allowSelfTend = allowSelfTend;
			questPart_TransporterPawns_TendWithMedicine.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_TransporterPawns_TendWithMedicine);
			return questPart_TransporterPawns_TendWithMedicine;
		}

		// Token: 0x06008496 RID: 33942 RVA: 0x002F95C0 File Offset: 0x002F77C0
		public static QuestPart_EnsureNotDowned EnsureNotDowned(this Quest quest, IEnumerable<Pawn> pawns, string inSignal = null)
		{
			QuestPart_EnsureNotDowned questPart_EnsureNotDowned = new QuestPart_EnsureNotDowned();
			questPart_EnsureNotDowned.pawns = ((pawns == null) ? null : pawns.ToList<Pawn>());
			questPart_EnsureNotDowned.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_EnsureNotDowned);
			return questPart_EnsureNotDowned;
		}

		// Token: 0x06008497 RID: 33943 RVA: 0x002F9610 File Offset: 0x002F7810
		public static QuestPart_Bestowing_TargetChangedTitle Bestowing_TargetChangedTitle(this Quest quest, Pawn target, Pawn bestower, RoyalTitleDef currentTitle, string inSignal)
		{
			QuestPart_Bestowing_TargetChangedTitle questPart_Bestowing_TargetChangedTitle = new QuestPart_Bestowing_TargetChangedTitle();
			questPart_Bestowing_TargetChangedTitle.pawn = target;
			questPart_Bestowing_TargetChangedTitle.bestower = bestower;
			questPart_Bestowing_TargetChangedTitle.currentTitle = currentTitle;
			questPart_Bestowing_TargetChangedTitle.inSignal = inSignal;
			questPart_Bestowing_TargetChangedTitle.signalListenMode = QuestPart.SignalListenMode.OngoingOrNotYetAccepted;
			quest.AddPart(questPart_Bestowing_TargetChangedTitle);
			return questPart_Bestowing_TargetChangedTitle;
		}

		// Token: 0x06008498 RID: 33944 RVA: 0x002F9650 File Offset: 0x002F7850
		public static QuestPart_SetAllApparelLocked SetAllApparelLocked(this Quest quest, IEnumerable<Pawn> pawns, string inSignal = null)
		{
			QuestPart_SetAllApparelLocked questPart_SetAllApparelLocked = new QuestPart_SetAllApparelLocked();
			questPart_SetAllApparelLocked.pawns.AddRange(pawns);
			questPart_SetAllApparelLocked.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(inSignal) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			quest.AddPart(questPart_SetAllApparelLocked);
			return questPart_SetAllApparelLocked;
		}

		// Token: 0x0400526F RID: 21103
		public const int MaxUsablePawnsToGenerate = 10;

		// Token: 0x02002900 RID: 10496
		public struct GetPawnParms
		{
			// Token: 0x04009A8D RID: 39565
			public bool mustBeFactionLeader;

			// Token: 0x04009A8E RID: 39566
			public bool mustBeWorldPawn;

			// Token: 0x04009A8F RID: 39567
			public bool ifWorldPawnThenMustBeFree;

			// Token: 0x04009A90 RID: 39568
			public bool ifWorldPawnThenMustBeFreeOrLeader;

			// Token: 0x04009A91 RID: 39569
			public bool mustHaveNoFaction;

			// Token: 0x04009A92 RID: 39570
			public bool mustBeFreeColonist;

			// Token: 0x04009A93 RID: 39571
			public bool mustBePlayerPrisoner;

			// Token: 0x04009A94 RID: 39572
			public bool mustBeNotSuspended;

			// Token: 0x04009A95 RID: 39573
			public bool mustHaveRoyalTitleInCurrentFaction;

			// Token: 0x04009A96 RID: 39574
			public bool mustBeNonHostileToPlayer;

			// Token: 0x04009A97 RID: 39575
			public bool? allowPermanentEnemyFaction;

			// Token: 0x04009A98 RID: 39576
			public bool canGeneratePawn;

			// Token: 0x04009A99 RID: 39577
			public bool requireResearchedBedroomFurnitureIfRoyal;

			// Token: 0x04009A9A RID: 39578
			public PawnKindDef mustBeOfKind;

			// Token: 0x04009A9B RID: 39579
			public Faction mustBeOfFaction;

			// Token: 0x04009A9C RID: 39580
			public FloatRange seniorityRange;

			// Token: 0x04009A9D RID: 39581
			public TechLevel minTechLevel;

			// Token: 0x04009A9E RID: 39582
			public List<FactionDef> excludeFactionDefs;

			// Token: 0x04009A9F RID: 39583
			public bool allowTemporaryFactions;

			// Token: 0x04009AA0 RID: 39584
			public bool allowHidden;

			// Token: 0x04009AA1 RID: 39585
			public bool mustBeCapableOfViolence;
		}
	}
}
