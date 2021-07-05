using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001654 RID: 5716
	public class QuestNode_GenerateTransportShip : QuestNode
	{
		// Token: 0x06008564 RID: 34148 RVA: 0x002FE3E8 File Offset: 0x002FC5E8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			TransportShip transportShip = TransportShipMaker.MakeTransportShip(this.def.GetValue(slate), null, this.shipThing.GetValue(slate));
			if (!this.storeAs.GetValue(slate).NullOrEmpty())
			{
				slate.Set<TransportShip>(this.storeAs.GetValue(slate), transportShip, false);
			}
			QuestPart_SetupTransportShip questPart_SetupTransportShip = new QuestPart_SetupTransportShip();
			questPart_SetupTransportShip.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SetupTransportShip.transportShip = transportShip;
			List<Thing> items;
			if (!(this.contents != null))
			{
				items = null;
			}
			else
			{
				items = (from c in this.contents.GetValue(slate)
				where !(c is Pawn)
				select c).ToList<Thing>();
			}
			questPart_SetupTransportShip.items = items;
			questPart_SetupTransportShip.pawns = ((this.contents != null) ? this.contents.GetValue(slate).OfType<Pawn>().ToList<Pawn>() : null);
			QuestPart_SetupTransportShip part = questPart_SetupTransportShip;
			QuestGen.quest.AddPart(part);
		}

		// Token: 0x06008565 RID: 34149 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04005345 RID: 21317
		public SlateRef<TransportShipDef> def;

		// Token: 0x04005346 RID: 21318
		public SlateRef<Thing> shipThing;

		// Token: 0x04005347 RID: 21319
		public SlateRef<IEnumerable<Thing>> contents;

		// Token: 0x04005348 RID: 21320
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04005349 RID: 21321
		[NoTranslate]
		public SlateRef<string> inSignal;
	}
}
