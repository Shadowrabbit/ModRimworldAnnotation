using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F00 RID: 7936
	public class QuestNode_SetAndRestore : QuestNode
	{
		// Token: 0x0600AA00 RID: 43520 RVA: 0x0031A404 File Offset: 0x00318604
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

		// Token: 0x0600AA01 RID: 43521 RVA: 0x0031A46C File Offset: 0x0031866C
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

		// Token: 0x0400734D RID: 29517
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x0400734E RID: 29518
		public SlateRef<object> value;

		// Token: 0x0400734F RID: 29519
		public QuestNode node;
	}
}
