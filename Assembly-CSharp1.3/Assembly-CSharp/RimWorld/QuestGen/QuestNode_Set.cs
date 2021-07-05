using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001643 RID: 5699
	public class QuestNode_Set : QuestNode
	{
		// Token: 0x0600852F RID: 34095 RVA: 0x002FD5B9 File Offset: 0x002FB7B9
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06008530 RID: 34096 RVA: 0x002FD5C3 File Offset: 0x002FB7C3
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06008531 RID: 34097 RVA: 0x002FD5D0 File Offset: 0x002FB7D0
		private void SetVars(Slate slate)
		{
			object obj = this.value.GetValue(slate);
			if (this.convertTo.GetValue(slate) != null)
			{
				obj = ConvertHelper.Convert(obj, this.convertTo.GetValue(slate));
			}
			slate.Set<object>(this.name.GetValue(slate), obj, false);
		}

		// Token: 0x040052F8 RID: 21240
		[NoTranslate]
		public SlateRef<string> name;

		// Token: 0x040052F9 RID: 21241
		public SlateRef<object> value;

		// Token: 0x040052FA RID: 21242
		public SlateRef<Type> convertTo;
	}
}
