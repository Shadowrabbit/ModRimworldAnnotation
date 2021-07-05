using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EF9 RID: 7929
	public class QuestNode_Prefix : QuestNode
	{
		// Token: 0x0600A9E5 RID: 43493 RVA: 0x00319F48 File Offset: 0x00318148
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

		// Token: 0x0600A9E6 RID: 43494 RVA: 0x00319FD0 File Offset: 0x003181D0
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

		// Token: 0x04007338 RID: 29496
		[NoTranslate]
		public SlateRef<string> prefix;

		// Token: 0x04007339 RID: 29497
		public SlateRef<bool> allowNonPrefixedLookup;

		// Token: 0x0400733A RID: 29498
		public List<PrefixCapturedVar> parms = new List<PrefixCapturedVar>();

		// Token: 0x0400733B RID: 29499
		[NoTranslate]
		public List<SlateRef<string>> returnVarNames = new List<SlateRef<string>>();

		// Token: 0x0400733C RID: 29500
		public QuestNode node;
	}
}
