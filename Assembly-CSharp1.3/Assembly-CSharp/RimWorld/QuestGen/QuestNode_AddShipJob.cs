using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001655 RID: 5717
	public class QuestNode_AddShipJob : QuestNode
	{
		// Token: 0x17001611 RID: 5649
		// (get) Token: 0x06008567 RID: 34151 RVA: 0x00002688 File Offset: 0x00000888
		protected virtual ShipJobDef DefaultShipJobDef
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06008568 RID: 34152 RVA: 0x002FE508 File Offset: 0x002FC708
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ShipJob shipJob = ShipJobMaker.MakeShipJob(this.jobDef.GetValue(slate) ?? this.DefaultShipJobDef);
			this.AddJobVars(shipJob, slate);
			QuestPart_AddShipJob part = new QuestPart_AddShipJob
			{
				inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false)),
				shipJob = shipJob,
				shipJobStartMode = (this.shipJobStartMode.GetValue(slate) ?? ShipJobStartMode.Queue),
				transportShip = this.transportShip.GetValue(slate)
			};
			QuestGen.quest.AddPart(part);
		}

		// Token: 0x06008569 RID: 34153 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void AddJobVars(ShipJob shipJob, Slate slate)
		{
		}

		// Token: 0x0600856A RID: 34154 RVA: 0x002FE5BB File Offset: 0x002FC7BB
		protected override bool TestRunInt(Slate slate)
		{
			return this.jobDef.GetValue(slate) != null || this.DefaultShipJobDef != null;
		}

		// Token: 0x0400534A RID: 21322
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400534B RID: 21323
		public SlateRef<TransportShip> transportShip;

		// Token: 0x0400534C RID: 21324
		public SlateRef<ShipJobDef> jobDef;

		// Token: 0x0400534D RID: 21325
		public SlateRef<ShipJobStartMode?> shipJobStartMode;
	}
}
