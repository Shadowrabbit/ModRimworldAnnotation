using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200168D RID: 5773
	public class QuestNode_GetRelationsInfo : QuestNode
	{
		// Token: 0x06008642 RID: 34370 RVA: 0x003025D9 File Offset: 0x003007D9
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06008643 RID: 34371 RVA: 0x003025E3 File Offset: 0x003007E3
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06008644 RID: 34372 RVA: 0x003025F0 File Offset: 0x003007F0
		private void SetVars(Slate slate)
		{
			if (this.pawn.GetValue(slate) == null)
			{
				return;
			}
			if (this.otherPawns.GetValue(slate) == null)
			{
				return;
			}
			QuestNode_GetRelationsInfo.tmpRelations.Clear();
			int num = 0;
			foreach (Pawn other in this.otherPawns.GetValue(slate))
			{
				PawnRelationDef mostImportantRelation = this.pawn.GetValue(slate).GetMostImportantRelation(other);
				if (mostImportantRelation != null)
				{
					QuestNode_GetRelationsInfo.tmpRelations.Add(mostImportantRelation.GetGenderSpecificLabel(other));
				}
				else
				{
					num++;
				}
			}
			if (num == 1)
			{
				QuestNode_GetRelationsInfo.tmpRelations.Add(this.nonRelatedLabel.GetValue(slate));
			}
			else if (num >= 2)
			{
				QuestNode_GetRelationsInfo.tmpRelations.Add(num + " " + this.nonRelatedLabelPlural.GetValue(slate));
			}
			if (!QuestNode_GetRelationsInfo.tmpRelations.Any<string>())
			{
				return;
			}
			slate.Set<string>(this.storeAs.GetValue(slate), QuestNode_GetRelationsInfo.tmpRelations.ToCommaList(true, false), false);
			QuestNode_GetRelationsInfo.tmpRelations.Clear();
		}

		// Token: 0x0400540E RID: 21518
		public SlateRef<Pawn> pawn;

		// Token: 0x0400540F RID: 21519
		public SlateRef<IEnumerable<Pawn>> otherPawns;

		// Token: 0x04005410 RID: 21520
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005411 RID: 21521
		public SlateRef<string> nonRelatedLabel;

		// Token: 0x04005412 RID: 21522
		public SlateRef<string> nonRelatedLabelPlural;

		// Token: 0x04005413 RID: 21523
		private static List<string> tmpRelations = new List<string>();
	}
}
