using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA2 RID: 4002
	public class RitualStageAction_Message : RitualStageAction
	{
		// Token: 0x06005E9B RID: 24219 RVA: 0x00206C4C File Offset: 0x00204E4C
		public override void Apply(LordJob_Ritual ritual)
		{
			Messages.Message(this.text.Formatted(ritual.Ritual.Label).CapitalizeFirst(), ritual.selectedTarget, this.messageTypeDef, false);
		}

		// Token: 0x06005E9C RID: 24220 RVA: 0x00206C98 File Offset: 0x00204E98
		public override void ExposeData()
		{
			Scribe_Defs.Look<MessageTypeDef>(ref this.messageTypeDef, "messageTypeDef");
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
		}

		// Token: 0x0400368C RID: 13964
		[MustTranslate]
		public string text;

		// Token: 0x0400368D RID: 13965
		public MessageTypeDef messageTypeDef;
	}
}
