using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F04 RID: 7940
	public class QuestNode_Unset : QuestNode
	{
		// Token: 0x0600AA0D RID: 43533 RVA: 0x0006F8AB File Offset: 0x0006DAAB
		protected override bool TestRunInt(Slate slate)
		{
			slate.Remove(this.name.GetValue(slate), false);
			return true;
		}

		// Token: 0x0600AA0E RID: 43534 RVA: 0x0031A920 File Offset: 0x00318B20
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestGen.slate.Remove(this.name.GetValue(slate), false);
		}

		// Token: 0x04007360 RID: 29536
		[NoTranslate]
		public SlateRef<string> name;
	}
}
