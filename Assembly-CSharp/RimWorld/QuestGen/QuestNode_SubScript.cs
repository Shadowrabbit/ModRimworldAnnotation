using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F03 RID: 7939
	public class QuestNode_SubScript : QuestNode
	{
		// Token: 0x0600AA09 RID: 43529 RVA: 0x0031A7E8 File Offset: 0x003189E8
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
				result = this.def.GetValue(slate).root.TestRun(slate);
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

		// Token: 0x0600AA0A RID: 43530 RVA: 0x0031A87C File Offset: 0x00318A7C
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
				this.def.GetValue(slate).Run();
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

		// Token: 0x0600AA0B RID: 43531 RVA: 0x0006F859 File Offset: 0x0006DA59
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.ToString(),
				" (",
				this.def,
				")"
			});
		}

		// Token: 0x0400735B RID: 29531
		[TranslationHandle(Priority = 100)]
		public SlateRef<QuestScriptDef> def;

		// Token: 0x0400735C RID: 29532
		[NoTranslate]
		public SlateRef<string> prefix;

		// Token: 0x0400735D RID: 29533
		public SlateRef<bool> allowNonPrefixedLookup;

		// Token: 0x0400735E RID: 29534
		public List<PrefixCapturedVar> parms = new List<PrefixCapturedVar>();

		// Token: 0x0400735F RID: 29535
		[NoTranslate]
		public List<SlateRef<string>> returnVarNames = new List<SlateRef<string>>();
	}
}
