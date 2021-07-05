using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001644 RID: 5700
	public class QuestNode_SetAndRestore : QuestNode
	{
		// Token: 0x06008533 RID: 34099 RVA: 0x002FD628 File Offset: 0x002FB828
		protected override bool TestRunInt(Slate slate)
		{
			Slate.VarRestoreInfo restoreInfo = slate.GetRestoreInfo(this.name.GetValue(slate));
			slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
			bool result;
			try
			{
				result = this.node.TestRun(slate);
			}
			finally
			{
				slate.Restore(restoreInfo);
			}
			return result;
		}

		// Token: 0x06008534 RID: 34100 RVA: 0x002FD690 File Offset: 0x002FB890
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Slate.VarRestoreInfo restoreInfo = QuestGen.slate.GetRestoreInfo(this.name.GetValue(slate));
			QuestGen.slate.Set<object>(this.name.GetValue(slate), this.value.GetValue(slate), false);
			try
			{
				this.node.Run();
			}
			finally
			{
				QuestGen.slate.Restore(restoreInfo);
			}
		}

		// Token: 0x040052FB RID: 21243
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x040052FC RID: 21244
		public SlateRef<object> value;

		// Token: 0x040052FD RID: 21245
		public QuestNode node;
	}
}
