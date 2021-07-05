using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200109F RID: 4255
	public class SignalAction_Message : SignalAction
	{
		// Token: 0x0600656D RID: 25965 RVA: 0x0022414B File Offset: 0x0022234B
		protected override void DoAction(SignalArgs args)
		{
			Messages.Message(this.message, this.lookTargets, this.messageType ?? MessageTypeDefOf.NeutralEvent, this.historical);
		}

		// Token: 0x0600656E RID: 25966 RVA: 0x00224174 File Offset: 0x00222374
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.message, "message", null, false);
			Scribe_Values.Look<bool>(ref this.historical, "historical", false, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Defs.Look<MessageTypeDef>(ref this.messageType, "messageType");
		}

		// Token: 0x04003921 RID: 14625
		public string message;

		// Token: 0x04003922 RID: 14626
		public bool historical = true;

		// Token: 0x04003923 RID: 14627
		public MessageTypeDef messageType;

		// Token: 0x04003924 RID: 14628
		public LookTargets lookTargets;
	}
}
