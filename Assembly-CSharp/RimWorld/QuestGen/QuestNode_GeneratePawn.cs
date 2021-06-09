using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F0A RID: 7946
	public class QuestNode_GeneratePawn : QuestNode
	{
		// Token: 0x0600AA1F RID: 43551 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AA20 RID: 43552 RVA: 0x0031AAA0 File Offset: 0x00318CA0
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
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(value, value2, context, tile, forceGenerateNewPawn, newborn, allowDead, allowDowned, canGeneratePawnRelations, this.mustBeCapableOfViolence.GetValue(slate), 1f, false, true, true, flag, false, false, false, false, value5, this.extraPawnForExtraRelationChance.GetValue(slate), this.relationWithExtraPawnChanceFactor.GetValue(slate), null, null, value3, value4, null, null, null, null, null, null, null, null)
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

		// Token: 0x04007368 RID: 29544
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007369 RID: 29545
		[NoTranslate]
		public SlateRef<string> addToList;

		// Token: 0x0400736A RID: 29546
		[NoTranslate]
		public SlateRef<IEnumerable<string>> addToLists;

		// Token: 0x0400736B RID: 29547
		public SlateRef<PawnKindDef> kindDef;

		// Token: 0x0400736C RID: 29548
		public SlateRef<Faction> faction;

		// Token: 0x0400736D RID: 29549
		public SlateRef<bool> forbidAnyTitle;

		// Token: 0x0400736E RID: 29550
		public SlateRef<bool> ensureNonNumericName;

		// Token: 0x0400736F RID: 29551
		public SlateRef<IEnumerable<TraitDef>> forcedTraits;

		// Token: 0x04007370 RID: 29552
		public SlateRef<IEnumerable<TraitDef>> prohibitedTraits;

		// Token: 0x04007371 RID: 29553
		public SlateRef<Pawn> extraPawnForExtraRelationChance;

		// Token: 0x04007372 RID: 29554
		public SlateRef<float> relationWithExtraPawnChanceFactor;

		// Token: 0x04007373 RID: 29555
		public SlateRef<bool?> allowAddictions;

		// Token: 0x04007374 RID: 29556
		public SlateRef<float> biocodeWeaponChance;

		// Token: 0x04007375 RID: 29557
		public SlateRef<float> biocodeApparelChance;

		// Token: 0x04007376 RID: 29558
		public SlateRef<bool> mustBeCapableOfViolence;

		// Token: 0x04007377 RID: 29559
		private const int MinExpertSkill = 11;
	}
}
