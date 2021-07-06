using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B1 RID: 4273
	public class QuestPart_DescriptionPart : QuestPartActivable
	{
		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x06005D2F RID: 23855 RVA: 0x00040A17 File Offset: 0x0003EC17
		public override string DescriptionPart
		{
			get
			{
				return this.resolvedDescriptionPart;
			}
		}

		// Token: 0x06005D30 RID: 23856 RVA: 0x00040A1F File Offset: 0x0003EC1F
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.resolvedDescriptionPart = receivedArgs.GetFormattedText(this.descriptionPart);
		}

		// Token: 0x06005D31 RID: 23857 RVA: 0x00040A45 File Offset: 0x0003EC45
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.descriptionPart, "descriptionPart", null, false);
			Scribe_Values.Look<string>(ref this.resolvedDescriptionPart, "resolvedDescriptionPart", null, false);
		}

		// Token: 0x06005D32 RID: 23858 RVA: 0x00040A71 File Offset: 0x0003EC71
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.descriptionPart = "Debug description part.";
		}

		// Token: 0x04003E5C RID: 15964
		public string descriptionPart;

		// Token: 0x04003E5D RID: 15965
		private string resolvedDescriptionPart;
	}
}
