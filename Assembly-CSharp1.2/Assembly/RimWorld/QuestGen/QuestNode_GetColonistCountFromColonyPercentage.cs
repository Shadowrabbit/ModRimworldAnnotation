using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F16 RID: 7958
	public class QuestNode_GetColonistCountFromColonyPercentage : QuestNode
	{
		// Token: 0x0600AA46 RID: 43590 RVA: 0x0006F9B1 File Offset: 0x0006DBB1
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AA47 RID: 43591 RVA: 0x0031B378 File Offset: 0x00319578
		private void SetVars(Slate slate)
		{
			string value = this.storeAs.GetValue(slate);
			int num = PawnsFinder.AllMaps_FreeColonistsSpawned.Count((Pawn c) => !c.IsQuestLodger());
			int var = Mathf.Clamp((int)((float)num * this.colonyPercentage.GetValue(slate)), 1, num - 1);
			slate.Set<int>(value, var, false);
		}

		// Token: 0x0600AA48 RID: 43592 RVA: 0x0031B3E0 File Offset: 0x003195E0
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

		// Token: 0x040073A6 RID: 29606
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040073A7 RID: 29607
		public SlateRef<float> colonyPercentage;

		// Token: 0x040073A8 RID: 29608
		public SlateRef<int> mustHaveFreeColonistsAvailableCount;
	}
}
