using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F55 RID: 8021
	public class QuestNode_GetRelationsInfo : QuestNode
	{
		// Token: 0x0600AB27 RID: 43815 RVA: 0x0006FFE7 File Offset: 0x0006E1E7
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AB28 RID: 43816 RVA: 0x0006FFF1 File Offset: 0x0006E1F1
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AB29 RID: 43817 RVA: 0x0031E54C File Offset: 0x0031C74C
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
			slate.Set<string>(this.storeAs.GetValue(slate), QuestNode_GetRelationsInfo.tmpRelations.ToCommaList(true), false);
			QuestNode_GetRelationsInfo.tmpRelations.Clear();
		}

		// Token: 0x0400747B RID: 29819
		public SlateRef<Pawn> pawn;

		// Token: 0x0400747C RID: 29820
		public SlateRef<IEnumerable<Pawn>> otherPawns;

		// Token: 0x0400747D RID: 29821
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400747E RID: 29822
		public SlateRef<string> nonRelatedLabel;

		// Token: 0x0400747F RID: 29823
		public SlateRef<string> nonRelatedLabelPlural;

		// Token: 0x04007480 RID: 29824
		private static List<string> tmpRelations = new List<string>();
	}
}
