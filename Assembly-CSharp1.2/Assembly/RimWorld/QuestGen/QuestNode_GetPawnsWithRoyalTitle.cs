using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F42 RID: 8002
	public class QuestNode_GetPawnsWithRoyalTitle : QuestNode
	{
		// Token: 0x0600AAE2 RID: 43746 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AAE3 RID: 43747 RVA: 0x0031D9B4 File Offset: 0x0031BBB4
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
					select p.LabelNoCountColored.Resolve()).ToCommaList(true), false);
				}
			}
		}

		// Token: 0x0600AAE4 RID: 43748 RVA: 0x0006FE1D File Offset: 0x0006E01D
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

		// Token: 0x0400743E RID: 29758
		public SlateRef<List<Pawn>> pawns;

		// Token: 0x0400743F RID: 29759
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007440 RID: 29760
		[NoTranslate]
		public SlateRef<string> storeCountAs;

		// Token: 0x04007441 RID: 29761
		[NoTranslate]
		public SlateRef<string> storePawnsLabelAs;
	}
}
