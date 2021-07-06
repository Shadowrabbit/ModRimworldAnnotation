using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F23 RID: 7971
	public class QuestNode_GetHivesCountFromPoints : QuestNode
	{
		// Token: 0x0600AA74 RID: 43636 RVA: 0x0006FAAD File Offset: 0x0006DCAD
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AA75 RID: 43637 RVA: 0x0006FAB7 File Offset: 0x0006DCB7
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AA76 RID: 43638 RVA: 0x0031BAAC File Offset: 0x00319CAC
		private void SetVars(Slate slate)
		{
			int num = Mathf.RoundToInt(slate.Get<float>("points", 0f, false) / 220f);
			if (num < 1)
			{
				num = 1;
			}
			slate.Set<int>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x040073D6 RID: 29654
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
