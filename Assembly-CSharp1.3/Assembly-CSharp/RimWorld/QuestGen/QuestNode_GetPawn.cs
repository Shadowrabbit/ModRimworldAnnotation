using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200167E RID: 5758
	public class QuestNode_GetPawn : QuestNode
	{
		// Token: 0x06008603 RID: 34307 RVA: 0x00300E80 File Offset: 0x002FF080
		private IEnumerable<Pawn> ExistingUsablePawns(Slate slate)
		{
			return from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where this.IsGoodPawn(x, slate)
			select x;
		}

		// Token: 0x06008604 RID: 34308 RVA: 0x00300EB8 File Offset: 0x002FF0B8
		protected override bool TestRunInt(Slate slate)
		{
			if (this.mustHaveNoFaction.GetValue(slate) && this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate))
			{
				return false;
			}
			if (this.canGeneratePawn.GetValue(slate) && (this.mustBeFactionLeader.GetValue(slate) || this.mustBeWorldPawn.GetValue(slate) || this.mustBePlayerPrisoner.GetValue(slate) || this.mustBeFreeColonist.GetValue(slate)))
			{
				Log.Warning("QuestNode_GetPawn has incompatible flags set, when canGeneratePawn is true these flags cannot be set: mustBeFactionLeader, mustBeWorldPawn, mustBePlayerPrisoner, mustBeFreeColonist");
				return false;
			}
			Pawn pawn;
			if (slate.TryGet<Pawn>(this.storeAs.GetValue(slate), out pawn, false) && this.IsGoodPawn(pawn, slate))
			{
				return true;
			}
			IEnumerable<Pawn> source = this.ExistingUsablePawns(slate);
			if (source.Count<Pawn>() > 0)
			{
				slate.Set<Pawn>(this.storeAs.GetValue(slate), source.RandomElement<Pawn>(), false);
				return true;
			}
			if (!this.canGeneratePawn.GetValue(slate))
			{
				return false;
			}
			Faction faction;
			if (!this.mustHaveNoFaction.GetValue(slate) && !this.TryFindFactionForPawnGeneration(slate, out faction))
			{
				return false;
			}
			FloatRange senRange = this.seniorityRange.GetValue(slate);
			return !this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate) || !this.requireResearchedBedroomFurnitureIfRoyal.GetValue(slate) || DefDatabase<RoyalTitleDef>.AllDefsListForReading.Any((RoyalTitleDef x) => (senRange.max <= 0f || senRange.IncludesEpsilon((float)x.seniority)) && this.PlayerHasResearchedBedroomRequirementsFor(x));
		}

		// Token: 0x06008605 RID: 34309 RVA: 0x00301008 File Offset: 0x002FF208
		private bool TryFindFactionForPawnGeneration(Slate slate, out Faction faction)
		{
			return (from x in Find.FactionManager.GetFactions(false, false, false, TechLevel.Undefined, false)
			where (this.excludeFactionDefs.GetValue(slate) == null || !this.excludeFactionDefs.GetValue(slate).Contains(x.def)) && (!this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate) || x.def.HasRoyalTitles) && (!this.mustBeNonHostileToPlayer.GetValue(slate) || !x.HostileTo(Faction.OfPlayer)) && ((this.allowPermanentEnemyFaction.GetValue(slate) ?? false) || !x.def.permanentEnemy) && x.def.techLevel >= this.minTechLevel.GetValue(slate)
			select x).TryRandomElementByWeight(delegate(Faction x)
			{
				if (x.HostileTo(Faction.OfPlayer))
				{
					float? value = this.hostileWeight.GetValue(slate);
					if (value == null)
					{
						return 1f;
					}
					return value.GetValueOrDefault();
				}
				else
				{
					float? value = this.nonHostileWeight.GetValue(slate);
					if (value == null)
					{
						return 1f;
					}
					return value.GetValueOrDefault();
				}
			}, out faction);
		}

		// Token: 0x06008606 RID: 34310 RVA: 0x0030105C File Offset: 0x002FF25C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Pawn pawn;
			if (QuestGen.slate.TryGet<Pawn>(this.storeAs.GetValue(slate), out pawn, false) && this.IsGoodPawn(pawn, slate))
			{
				return;
			}
			IEnumerable<Pawn> source = this.ExistingUsablePawns(slate);
			int num = source.Count<Pawn>();
			Faction faction;
			if (Rand.Chance(this.canGeneratePawn.GetValue(slate) ? Mathf.Clamp01(1f - (float)num / (float)this.maxUsablePawnsToGenerate.GetValue(slate)) : 0f) && (this.mustHaveNoFaction.GetValue(slate) || this.TryFindFactionForPawnGeneration(slate, out faction)))
			{
				pawn = this.GeneratePawn(slate, null);
			}
			else
			{
				pawn = source.RandomElementByWeight(delegate(Pawn x)
				{
					if (x.Faction != null && x.Faction.HostileTo(Faction.OfPlayer))
					{
						float? value = this.hostileWeight.GetValue(slate);
						if (value == null)
						{
							return 1f;
						}
						return value.GetValueOrDefault();
					}
					else
					{
						float? value = this.nonHostileWeight.GetValue(slate);
						if (value == null)
						{
							return 1f;
						}
						return value.GetValueOrDefault();
					}
				});
			}
			if (pawn.Faction != null && !pawn.Faction.Hidden)
			{
				QuestPart_InvolvedFactions questPart_InvolvedFactions = new QuestPart_InvolvedFactions();
				questPart_InvolvedFactions.factions.Add(pawn.Faction);
				QuestGen.quest.AddPart(questPart_InvolvedFactions);
			}
			QuestGen.slate.Set<Pawn>(this.storeAs.GetValue(slate), pawn, false);
		}

		// Token: 0x06008607 RID: 34311 RVA: 0x003011A4 File Offset: 0x002FF3A4
		private Pawn GeneratePawn(Slate slate, Faction faction = null)
		{
			PawnKindDef pawnKindDef = this.mustBeOfKind.GetValue(slate);
			if (faction == null && !this.mustHaveNoFaction.GetValue(slate))
			{
				if (!this.TryFindFactionForPawnGeneration(slate, out faction))
				{
					Log.Error("QuestNode_GetPawn tried generating pawn but couldn't find a proper faction for new pawn.");
				}
				else if (pawnKindDef == null)
				{
					pawnKindDef = faction.RandomPawnKind();
				}
			}
			RoyalTitleDef fixedTitle;
			if (this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate))
			{
				FloatRange senRange;
				if (!this.seniorityRange.TryGetValue(slate, out senRange))
				{
					senRange = FloatRange.Zero;
				}
				IEnumerable<RoyalTitleDef> source = from t in DefDatabase<RoyalTitleDef>.AllDefsListForReading
				where faction.def.RoyalTitlesAllInSeniorityOrderForReading.Contains(t) && (senRange.max <= 0f || senRange.IncludesEpsilon((float)t.seniority))
				select t;
				if (this.requireResearchedBedroomFurnitureIfRoyal.GetValue(slate) && source.Any((RoyalTitleDef x) => this.PlayerHasResearchedBedroomRequirementsFor(x)))
				{
					source = from x in source
					where this.PlayerHasResearchedBedroomRequirementsFor(x)
					select x;
				}
				fixedTitle = source.RandomElementByWeight((RoyalTitleDef t) => t.commonality);
				if (this.mustBeOfKind.GetValue(slate) == null && !(from k in DefDatabase<PawnKindDef>.AllDefsListForReading
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
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKindDef, faction, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, fixedTitle, null, false, false));
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			if (pawn.royalty != null && pawn.royalty.AllTitlesForReading.Any<RoyalTitle>())
			{
				QuestPart_Hyperlinks questPart_Hyperlinks = new QuestPart_Hyperlinks();
				questPart_Hyperlinks.pawns.Add(pawn);
				QuestGen.quest.AddPart(questPart_Hyperlinks);
			}
			return pawn;
		}

		// Token: 0x06008608 RID: 34312 RVA: 0x0030142C File Offset: 0x002FF62C
		private bool IsGoodPawn(Pawn pawn, Slate slate)
		{
			if (this.mustBeFactionLeader.GetValue(slate))
			{
				Faction faction = pawn.Faction;
				if (faction == null || faction.leader != pawn || !faction.def.humanlikeFaction || faction.defeated || faction.Hidden || faction.IsPlayer || pawn.IsPrisoner)
				{
					return false;
				}
			}
			if (pawn.Faction != null && this.excludeFactionDefs.GetValue(slate) != null && this.excludeFactionDefs.GetValue(slate).Contains(pawn.Faction.def))
			{
				return false;
			}
			if (pawn.Faction != null && pawn.Faction.def.techLevel < this.minTechLevel.GetValue(slate))
			{
				return false;
			}
			if (this.mustBeOfKind.GetValue(slate) != null && pawn.kindDef != this.mustBeOfKind.GetValue(slate))
			{
				return false;
			}
			if (this.mustHaveRoyalTitleInCurrentFaction.GetValue(slate) && (pawn.Faction == null || pawn.royalty == null || !pawn.royalty.HasAnyTitleIn(pawn.Faction)))
			{
				return false;
			}
			if (this.seniorityRange.GetValue(slate) != default(FloatRange) && (pawn.royalty == null || pawn.royalty.MostSeniorTitle == null || !this.seniorityRange.GetValue(slate).IncludesEpsilon((float)pawn.royalty.MostSeniorTitle.def.seniority)))
			{
				return false;
			}
			if (this.mustBeWorldPawn.GetValue(slate) && !pawn.IsWorldPawn())
			{
				return false;
			}
			if (this.ifWorldPawnThenMustBeFree.GetValue(slate) && pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.Free)
			{
				return false;
			}
			if (this.ifWorldPawnThenMustBeFreeOrLeader.GetValue(slate) && pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.Free && Find.WorldPawns.GetSituation(pawn) != WorldPawnSituation.FactionLeader)
			{
				return false;
			}
			if (pawn.IsWorldPawn() && Find.WorldPawns.GetSituation(pawn) == WorldPawnSituation.ReservedByQuest)
			{
				return false;
			}
			if (this.mustHaveNoFaction.GetValue(slate) && pawn.Faction != null)
			{
				return false;
			}
			if (this.mustBeFreeColonist.GetValue(slate) && !pawn.IsFreeColonist)
			{
				return false;
			}
			if (this.mustBePlayerPrisoner.GetValue(slate) && !pawn.IsPrisonerOfColony)
			{
				return false;
			}
			if (this.mustBeNotSuspended.GetValue(slate) && pawn.Suspended)
			{
				return false;
			}
			if (this.mustBeNonHostileToPlayer.GetValue(slate) && (pawn.HostileTo(Faction.OfPlayer) || (pawn.Faction != null && pawn.Faction != Faction.OfPlayer && pawn.Faction.HostileTo(Faction.OfPlayer))))
			{
				return false;
			}
			if (!(this.allowPermanentEnemyFaction.GetValue(slate) ?? true) && pawn.Faction != null && pawn.Faction.def.permanentEnemy)
			{
				return false;
			}
			if (this.requireResearchedBedroomFurnitureIfRoyal.GetValue(slate))
			{
				RoyalTitle royalTitle = pawn.royalty.HighestTitleWithBedroomRequirements();
				if (royalTitle != null && !this.PlayerHasResearchedBedroomRequirementsFor(royalTitle.def))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008609 RID: 34313 RVA: 0x00301738 File Offset: 0x002FF938
		private bool PlayerHasResearchedBedroomRequirementsFor(RoyalTitleDef title)
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

		// Token: 0x040053C8 RID: 21448
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040053C9 RID: 21449
		public SlateRef<bool> mustBeFactionLeader;

		// Token: 0x040053CA RID: 21450
		public SlateRef<bool> mustBeWorldPawn;

		// Token: 0x040053CB RID: 21451
		public SlateRef<bool> ifWorldPawnThenMustBeFree;

		// Token: 0x040053CC RID: 21452
		public SlateRef<bool> ifWorldPawnThenMustBeFreeOrLeader;

		// Token: 0x040053CD RID: 21453
		public SlateRef<bool> mustHaveNoFaction;

		// Token: 0x040053CE RID: 21454
		public SlateRef<bool> mustBeFreeColonist;

		// Token: 0x040053CF RID: 21455
		public SlateRef<bool> mustBePlayerPrisoner;

		// Token: 0x040053D0 RID: 21456
		public SlateRef<bool> mustBeNotSuspended;

		// Token: 0x040053D1 RID: 21457
		public SlateRef<bool> mustHaveRoyalTitleInCurrentFaction;

		// Token: 0x040053D2 RID: 21458
		public SlateRef<bool> mustBeNonHostileToPlayer;

		// Token: 0x040053D3 RID: 21459
		public SlateRef<bool?> allowPermanentEnemyFaction;

		// Token: 0x040053D4 RID: 21460
		public SlateRef<bool> canGeneratePawn;

		// Token: 0x040053D5 RID: 21461
		public SlateRef<bool> requireResearchedBedroomFurnitureIfRoyal;

		// Token: 0x040053D6 RID: 21462
		public SlateRef<PawnKindDef> mustBeOfKind;

		// Token: 0x040053D7 RID: 21463
		public SlateRef<FloatRange> seniorityRange;

		// Token: 0x040053D8 RID: 21464
		public SlateRef<TechLevel> minTechLevel;

		// Token: 0x040053D9 RID: 21465
		public SlateRef<List<FactionDef>> excludeFactionDefs;

		// Token: 0x040053DA RID: 21466
		public SlateRef<float?> hostileWeight;

		// Token: 0x040053DB RID: 21467
		public SlateRef<float?> nonHostileWeight;

		// Token: 0x040053DC RID: 21468
		public SlateRef<int> maxUsablePawnsToGenerate = 10;
	}
}
