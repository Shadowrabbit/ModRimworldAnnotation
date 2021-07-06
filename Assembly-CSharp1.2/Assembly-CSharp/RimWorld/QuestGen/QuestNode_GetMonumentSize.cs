using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F30 RID: 7984
	public class QuestNode_GetMonumentSize : QuestNode
	{
		// Token: 0x0600AAA5 RID: 43685 RVA: 0x0006FC40 File Offset: 0x0006DE40
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600AAA6 RID: 43686 RVA: 0x0006FC4A File Offset: 0x0006DE4A
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600AAA7 RID: 43687 RVA: 0x0006FC57 File Offset: 0x0006DE57
		private void DoWork(Slate slate)
		{
			if (this.monumentMarker.GetValue(slate) == null)
			{
				return;
			}
			slate.Set<IntVec2>(this.storeAs.GetValue(slate), this.monumentMarker.GetValue(slate).Size, false);
		}

		// Token: 0x040073F4 RID: 29684
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040073F5 RID: 29685
		public SlateRef<MonumentMarker> monumentMarker;
	}
}
