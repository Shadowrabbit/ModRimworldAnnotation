using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F1E RID: 7966
	public class QuestNode_GetFactionOf : QuestNode
	{
		// Token: 0x0600AA64 RID: 43620 RVA: 0x0006FA41 File Offset: 0x0006DC41
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600AA65 RID: 43621 RVA: 0x0006FA4B File Offset: 0x0006DC4B
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600AA66 RID: 43622 RVA: 0x0031B938 File Offset: 0x00319B38
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

		// Token: 0x040073C8 RID: 29640
		public SlateRef<Thing> thing;

		// Token: 0x040073C9 RID: 29641
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
