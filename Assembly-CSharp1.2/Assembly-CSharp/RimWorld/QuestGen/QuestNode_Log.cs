using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F05 RID: 7941
	public class QuestNode_Log : QuestNode
	{
		// Token: 0x0600AA10 RID: 43536 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AA11 RID: 43537 RVA: 0x0006F8C2 File Offset: 0x0006DAC2
		protected override void RunInt()
		{
			Log.Message("QuestNode_Log: " + this.message.ToString(QuestGen.slate), false);
		}

		// Token: 0x04007361 RID: 29537
		[NoTranslate]
		public SlateRef<object> message;
	}
}
