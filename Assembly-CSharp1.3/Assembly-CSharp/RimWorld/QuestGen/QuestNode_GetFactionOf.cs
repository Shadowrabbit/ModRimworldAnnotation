using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001670 RID: 5744
	public class QuestNode_GetFactionOf : QuestNode
	{
		// Token: 0x060085C6 RID: 34246 RVA: 0x002FFA77 File Offset: 0x002FDC77
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x060085C7 RID: 34247 RVA: 0x002FFA81 File Offset: 0x002FDC81
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060085C8 RID: 34248 RVA: 0x002FFA90 File Offset: 0x002FDC90
		private void DoWork(Slate slate)
		{
			Faction var = null;
			Thing value = this.thing.GetValue(slate);
			if (value != null)
			{
				var = value.Faction;
			}
			slate.Set<Faction>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x0400539D RID: 21405
		public SlateRef<Thing> thing;

		// Token: 0x0400539E RID: 21406
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
