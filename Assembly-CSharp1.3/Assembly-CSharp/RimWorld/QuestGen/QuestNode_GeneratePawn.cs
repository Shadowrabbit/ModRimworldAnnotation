using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200164E RID: 5710
	public class QuestNode_GeneratePawn : QuestNode
	{
		// Token: 0x06008552 RID: 34130 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008553 RID: 34131 RVA: 0x002FDD90 File Offset: 0x002FBF90
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			PawnKindDef value = this.kindDef.GetValue(slate);
			Faction value2 = this.faction.GetValue(slate);
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			int tile = -1;
			bool forceGenerateNewPawn = false;
			bool newborn = false;
			bool allowDead = false;
			bool allowDowned = false;
			bool canGeneratePawnRelations = true;
			bool flag = this.allowAddictions.GetValue(slate) ?? true;
			IEnumerable<TraitDef> value3 = this.forcedTraits.GetValue(slate);
			IEnumerable<TraitDef> value4 = this.prohibitedTraits.GetValue(slate);
			float value5 = this.biocodeWeaponChance.GetValue(slate);
			bool value6 = this.mustBeCapableOfViolence.GetValue(slate);
			float colonistRelationChanceFactor = 1f;
			bool forceAddFreeWarmLayerIfNeeded = false;
			bool allowGay = true;
			bool allowFood = true;
			bool flag2 = flag;
			bool inhabitant = false;
			bool certainlyBeenInCryptosleep = false;
			bool forceRedressWorldPawnIfFormerColonist = false;
			bool worldPawnFactionDoesntMatter = false;
			float num = value5;
			Pawn value7 = this.extraPawnForExtraRelationChance.GetValue(slate);
			float value8 = this.relationWithExtraPawnChanceFactor.GetValue(slate);
			Gender? value9 = this.fixedGender.GetValue(slate);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(value, value2, context, tile, forceGenerateNewPawn, newborn, allowDead, allowDowned, canGeneratePawnRelations, value6, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, allowFood, flag2, inhabitant, certainlyBeenInCryptosleep, forceRedressWorldPawnIfFormerColonist, worldPawnFactionDoesntMatter, num, this.biocodeApparelChance.GetValue(slate), value7, value8, null, null, value3, value4, null, null, null, value9, null, null, null, null, null, false, false)
			{
				BiocodeApparelChance = this.biocodeApparelChance.GetValue(slate),
				ForbidAnyTitle = this.forbidAnyTitle.GetValue(slate)
			});
			if (this.ensureNonNumericName.GetValue(slate) && (pawn.Name == null || pawn.Name.Numerical))
			{
				pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(pawn, NameStyle.Full, null);
			}
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<Pawn>(this.storeAs.GetValue(slate), pawn, false);
			}
			if (this.addToList.GetValue(slate) != null)
			{
				QuestGenUtility.AddToOrMakeList(QuestGen.slate, this.addToList.GetValue(slate), pawn);
			}
			if (this.addToLists.GetValue(slate) != null)
			{
				foreach (string name in this.addToLists.GetValue(slate))
				{
					QuestGenUtility.AddToOrMakeList(QuestGen.slate, name, pawn);
				}
			}
			QuestGen.AddToGeneratedPawns(pawn);
			if (!pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			}
		}

		// Token: 0x04005316 RID: 21270
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005317 RID: 21271
		[NoTranslate]
		public SlateRef<string> addToList;

		// Token: 0x04005318 RID: 21272
		[NoTranslate]
		public SlateRef<IEnumerable<string>> addToLists;

		// Token: 0x04005319 RID: 21273
		public SlateRef<PawnKindDef> kindDef;

		// Token: 0x0400531A RID: 21274
		public SlateRef<Faction> faction;

		// Token: 0x0400531B RID: 21275
		public SlateRef<bool> forbidAnyTitle;

		// Token: 0x0400531C RID: 21276
		public SlateRef<bool> ensureNonNumericName;

		// Token: 0x0400531D RID: 21277
		public SlateRef<IEnumerable<TraitDef>> forcedTraits;

		// Token: 0x0400531E RID: 21278
		public SlateRef<IEnumerable<TraitDef>> prohibitedTraits;

		// Token: 0x0400531F RID: 21279
		public SlateRef<Pawn> extraPawnForExtraRelationChance;

		// Token: 0x04005320 RID: 21280
		public SlateRef<float> relationWithExtraPawnChanceFactor;

		// Token: 0x04005321 RID: 21281
		public SlateRef<bool?> allowAddictions;

		// Token: 0x04005322 RID: 21282
		public SlateRef<float> biocodeWeaponChance;

		// Token: 0x04005323 RID: 21283
		public SlateRef<float> biocodeApparelChance;

		// Token: 0x04005324 RID: 21284
		public SlateRef<bool> mustBeCapableOfViolence;

		// Token: 0x04005325 RID: 21285
		public SlateRef<Gender?> fixedGender;

		// Token: 0x04005326 RID: 21286
		private const int MinExpertSkill = 11;
	}
}
