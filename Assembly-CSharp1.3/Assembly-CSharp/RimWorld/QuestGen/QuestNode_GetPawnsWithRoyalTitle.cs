using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001681 RID: 5761
	public class QuestNode_GetPawnsWithRoyalTitle : QuestNode
	{
		// Token: 0x06008613 RID: 34323 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06008614 RID: 34324 RVA: 0x00301AF4 File Offset: 0x002FFCF4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.pawns.GetValue(slate) != null)
			{
				IEnumerable<Pawn> filteredPawns = this.GetFilteredPawns(this.pawns.GetValue(slate));
				slate.Set<IEnumerable<Pawn>>(this.storeAs.GetValue(slate), filteredPawns, false);
				if (this.storeCountAs.GetValue(slate) != null)
				{
					slate.Set<int>(this.storeCountAs.GetValue(slate), filteredPawns.Count<Pawn>(), false);
				}
				if (this.storePawnsLabelAs.GetValue(slate) != null)
				{
					slate.Set<string>(this.storePawnsLabelAs.GetValue(slate), (from p in filteredPawns
					select p.LabelNoCountColored.Resolve()).ToCommaList(true, false), false);
				}
			}
		}

		// Token: 0x06008615 RID: 34325 RVA: 0x00301BB3 File Offset: 0x002FFDB3
		private IEnumerable<Pawn> GetFilteredPawns(List<Pawn> pawns)
		{
			Slate slate = QuestGen.slate;
			int num;
			for (int i = 0; i < pawns.Count; i = num + 1)
			{
				if (pawns[i].royalty != null && pawns[i].royalty.AllTitlesInEffectForReading.Any<RoyalTitle>())
				{
					yield return pawns[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x040053E7 RID: 21479
		public SlateRef<List<Pawn>> pawns;

		// Token: 0x040053E8 RID: 21480
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040053E9 RID: 21481
		[NoTranslate]
		public SlateRef<string> storeCountAs;

		// Token: 0x040053EA RID: 21482
		[NoTranslate]
		public SlateRef<string> storePawnsLabelAs;
	}
}
