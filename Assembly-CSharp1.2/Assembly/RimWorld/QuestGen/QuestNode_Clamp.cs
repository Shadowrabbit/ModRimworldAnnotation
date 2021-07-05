using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EF5 RID: 7925
	public class QuestNode_Clamp : QuestNode
	{
		// Token: 0x0600A9D8 RID: 43480 RVA: 0x0006F733 File Offset: 0x0006D933
		protected override bool TestRunInt(Slate slate)
		{
			this.DoWork(slate);
			return true;
		}

		// Token: 0x0600A9D9 RID: 43481 RVA: 0x0006F73D File Offset: 0x0006D93D
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600A9DA RID: 43482 RVA: 0x00319B54 File Offset: 0x00317D54
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

		// Token: 0x04007320 RID: 29472
		public SlateRef<double> value;

		// Token: 0x04007321 RID: 29473
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007322 RID: 29474
		public SlateRef<double?> min;

		// Token: 0x04007323 RID: 29475
		public SlateRef<double?> max;
	}
}
