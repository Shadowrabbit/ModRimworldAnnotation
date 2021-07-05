using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200166A RID: 5738
	public class QuestNode_GetColonistCountFromColonyPercentage : QuestNode
	{
		// Token: 0x060085AC RID: 34220 RVA: 0x002FF320 File Offset: 0x002FD520
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060085AD RID: 34221 RVA: 0x002FF330 File Offset: 0x002FD530
		private void SetVars(Slate slate)
		{
			string value = this.storeAs.GetValue(slate);
			int num = PawnsFinder.AllMaps_FreeColonistsSpawned.Count((Pawn c) => !c.IsQuestLodger());
			int var = Mathf.Clamp((int)((float)num * this.colonyPercentage.GetValue(slate)), 1, num - 1);
			slate.Set<int>(value, var, false);
		}

		// Token: 0x060085AE RID: 34222 RVA: 0x002FF398 File Offset: 0x002FD598
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			float num = (float)this.mustHaveFreeColonistsAvailableCount.GetValue(slate);
			if (num > 0f)
			{
				return (float)PawnsFinder.AllMaps_FreeColonistsSpawned.Count((Pawn c) => !c.IsQuestLodger()) >= num;
			}
			return true;
		}

		// Token: 0x0400537F RID: 21375
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005380 RID: 21376
		public SlateRef<float> colonyPercentage;

		// Token: 0x04005381 RID: 21377
		public SlateRef<int> mustHaveFreeColonistsAvailableCount;
	}
}
