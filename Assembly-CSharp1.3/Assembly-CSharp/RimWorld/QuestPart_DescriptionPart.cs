using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B6C RID: 2924
	public class QuestPart_DescriptionPart : QuestPartActivable
	{
		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06004467 RID: 17511 RVA: 0x0016B264 File Offset: 0x00169464
		public override string DescriptionPart
		{
			get
			{
				return this.resolvedDescriptionPart;
			}
		}

		// Token: 0x06004468 RID: 17512 RVA: 0x0016B26C File Offset: 0x0016946C
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.resolvedDescriptionPart = receivedArgs.GetFormattedText(this.descriptionPart);
		}

		// Token: 0x06004469 RID: 17513 RVA: 0x0016B292 File Offset: 0x00169492
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.descriptionPart, "descriptionPart", null, false);
			Scribe_Values.Look<string>(ref this.resolvedDescriptionPart, "resolvedDescriptionPart", null, false);
		}

		// Token: 0x0600446A RID: 17514 RVA: 0x0016B2BE File Offset: 0x001694BE
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.descriptionPart = "Debug description part.";
		}

		// Token: 0x04002984 RID: 10628
		public string descriptionPart;

		// Token: 0x04002985 RID: 10629
		private string resolvedDescriptionPart;
	}
}
