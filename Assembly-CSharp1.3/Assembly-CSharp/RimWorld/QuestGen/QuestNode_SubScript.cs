using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001647 RID: 5703
	public class QuestNode_SubScript : QuestNode
	{
		// Token: 0x0600853C RID: 34108 RVA: 0x002FDA3C File Offset: 0x002FBC3C
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

		// Token: 0x0600853D RID: 34109 RVA: 0x002FDAD0 File Offset: 0x002FBCD0
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

		// Token: 0x0600853E RID: 34110 RVA: 0x002FDB74 File Offset: 0x002FBD74
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

		// Token: 0x04005309 RID: 21257
		[TranslationHandle(Priority = 100)]
		public SlateRef<QuestScriptDef> def;

		// Token: 0x0400530A RID: 21258
		[NoTranslate]
		public SlateRef<string> prefix;

		// Token: 0x0400530B RID: 21259
		public SlateRef<bool> allowNonPrefixedLookup;

		// Token: 0x0400530C RID: 21260
		public List<PrefixCapturedVar> parms = new List<PrefixCapturedVar>();

		// Token: 0x0400530D RID: 21261
		[NoTranslate]
		public List<SlateRef<string>> returnVarNames = new List<SlateRef<string>>();
	}
}
