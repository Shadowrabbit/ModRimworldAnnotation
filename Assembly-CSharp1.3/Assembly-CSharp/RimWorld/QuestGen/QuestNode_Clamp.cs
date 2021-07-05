using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200163B RID: 5691
	public class QuestNode_Clamp : QuestNode
	{
		// Token: 0x06008515 RID: 34069 RVA: 0x002FCDA7 File Offset: 0x002FAFA7
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x06008516 RID: 34070 RVA: 0x002FCDB1 File Offset: 0x002FAFB1
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x06008517 RID: 34071 RVA: 0x002FCDC0 File Offset: 0x002FAFC0
		private void DoWork(Slate slate)
		{
			double num = this.value.GetValue(slate);
			if (this.min.GetValue(slate) != null)
			{
				num = Math.Max(num, this.min.GetValue(slate).Value);
			}
			if (this.max.GetValue(slate) != null)
			{
				num = Math.Min(num, this.max.GetValue(slate).Value);
			}
			slate.Set<double>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x040052D5 RID: 21205
		public SlateRef<double> value;

		// Token: 0x040052D6 RID: 21206
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040052D7 RID: 21207
		public SlateRef<double?> min;

		// Token: 0x040052D8 RID: 21208
		public SlateRef<double?> max;
	}
}
