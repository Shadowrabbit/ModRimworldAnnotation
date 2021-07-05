using System;

namespace RimWorld.QuestGen
{
	// Token: 0x0200165A RID: 5722
	public class QuestNode_SendTransportShipAwayOnCleanup : QuestNode
	{
		// Token: 0x06008577 RID: 34167 RVA: 0x002FE75C File Offset: 0x002FC95C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			QuestPart_SendTransportShipAwayOnCleanup part = new QuestPart_SendTransportShipAwayOnCleanup
			{
				transportShip = this.transportShip.GetValue(slate),
				unsatisfiedDropMode = (this.unsatisfiedDropMode.GetValue(slate) ?? TransportShipDropMode.NonRequired),
				unloadContents = this.unloadContents.GetValue(slate)
			};
			QuestGen.quest.AddPart(part);
		}

		// Token: 0x06008578 RID: 34168 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04005356 RID: 21334
		public SlateRef<TransportShip> transportShip;

		// Token: 0x04005357 RID: 21335
		public SlateRef<bool> unloadContents;

		// Token: 0x04005358 RID: 21336
		public SlateRef<TransportShipDropMode?> unsatisfiedDropMode;
	}
}
