using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200167A RID: 5754
	public class QuestNode_GetMonumentSize : QuestNode
	{
		// Token: 0x060085F2 RID: 34290 RVA: 0x0030092C File Offset: 0x002FEB2C
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x060085F3 RID: 34291 RVA: 0x00300936 File Offset: 0x002FEB36
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060085F4 RID: 34292 RVA: 0x00300943 File Offset: 0x002FEB43
		private void DoWork(Slate slate)
		{
			if (this.monumentMarker.GetValue(slate) == null)
			{
				return;
			}
			slate.Set<IntVec2>(this.storeAs.GetValue(slate), this.monumentMarker.GetValue(slate).Size, false);
		}

		// Token: 0x040053B7 RID: 21431
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040053B8 RID: 21432
		public SlateRef<MonumentMarker> monumentMarker;
	}
}
