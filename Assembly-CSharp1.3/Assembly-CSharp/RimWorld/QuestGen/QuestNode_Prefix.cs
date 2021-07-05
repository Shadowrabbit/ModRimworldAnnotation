using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200163F RID: 5695
	public class QuestNode_Prefix : QuestNode
	{
		// Token: 0x06008522 RID: 34082 RVA: 0x002FD1F0 File Offset: 0x002FB3F0
		protected override bool TestRunInt(Slate slate)
		{
			string value = this.prefix.GetValue(slate);
			List<Slate.VarRestoreInfo> varsRestoreInfo = QuestGenUtility.SetVarsForPrefix(this.parms, value, slate);
			if (!value.NullOrEmpty())
			{
				slate.PushPrefix(value, this.allowNonPrefixedLookup.GetValue(slate));
			}
			bool result;
			try
			{
				result = this.node.TestRun(slate);
			}
			finally
			{
				if (!value.NullOrEmpty())
				{
					slate.PopPrefix();
				}
				QuestGenUtility.GetReturnedVars(this.returnVarNames, value, slate);
				QuestGenUtility.RestoreVarsForPrefix(varsRestoreInfo, slate);
			}
			return result;
		}

		// Token: 0x06008523 RID: 34083 RVA: 0x002FD278 File Offset: 0x002FB478
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			string value = this.prefix.GetValue(slate);
			List<Slate.VarRestoreInfo> varsRestoreInfo = QuestGenUtility.SetVarsForPrefix(this.parms, value, QuestGen.slate);
			if (!value.NullOrEmpty())
			{
				QuestGen.slate.PushPrefix(value, this.allowNonPrefixedLookup.GetValue(slate));
			}
			try
			{
				this.node.Run();
			}
			finally
			{
				if (!value.NullOrEmpty())
				{
					QuestGen.slate.PopPrefix();
				}
				QuestGenUtility.GetReturnedVars(this.returnVarNames, value, QuestGen.slate);
				QuestGenUtility.RestoreVarsForPrefix(varsRestoreInfo, QuestGen.slate);
			}
		}

		// Token: 0x040052EE RID: 21230
		[NoTranslate]
		public SlateRef<string> prefix;

		// Token: 0x040052EF RID: 21231
		public SlateRef<bool> allowNonPrefixedLookup;

		// Token: 0x040052F0 RID: 21232
		public List<PrefixCapturedVar> parms = new List<PrefixCapturedVar>();

		// Token: 0x040052F1 RID: 21233
		[NoTranslate]
		public List<SlateRef<string>> returnVarNames = new List<SlateRef<string>>();

		// Token: 0x040052F2 RID: 21234
		public QuestNode node;
	}
}
