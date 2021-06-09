using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F29 RID: 7977
	public class QuestNode_GetMapOf : QuestNode
	{
		// Token: 0x0600AA8A RID: 43658 RVA: 0x0006FB37 File Offset: 0x0006DD37
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600AA8B RID: 43659 RVA: 0x0006FB41 File Offset: 0x0006DD41
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600AA8C RID: 43660 RVA: 0x0006FB4E File Offset: 0x0006DD4E
		private void DoWork(Slate slate)
		{
			if (this.mapOf.GetValue(slate) != null)
			{
				slate.Set<Map>(this.storeAs.GetValue(slate), this.mapOf.GetValue(slate).MapHeld, false);
			}
		}

		// Token: 0x040073E5 RID: 29669
		[NoTranslate]
		public SlateRef<string> storeAs = "map";

		// Token: 0x040073E6 RID: 29670
		public SlateRef<Thing> mapOf;
	}
}
