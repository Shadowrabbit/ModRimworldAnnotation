using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001673 RID: 5747
	public class QuestNode_GetHivesCountFromPoints : QuestNode
	{
		// Token: 0x060085D2 RID: 34258 RVA: 0x002FFC32 File Offset: 0x002FDE32
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x060085D3 RID: 34259 RVA: 0x002FFC3C File Offset: 0x002FDE3C
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060085D4 RID: 34260 RVA: 0x002FFC4C File Offset: 0x002FDE4C
		private void SetVars(Slate slate)
		{
			int num = Mathf.RoundToInt(slate.Get<float>("points", 0f, false) / 220f);
			if (num < 1)
			{
				num = 1;
			}
			slate.Set<int>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x040053A6 RID: 21414
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
