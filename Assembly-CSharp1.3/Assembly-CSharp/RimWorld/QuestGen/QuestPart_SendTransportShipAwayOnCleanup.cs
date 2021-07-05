using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001663 RID: 5731
	public class QuestPart_SendTransportShipAwayOnCleanup : QuestPart
	{
		// Token: 0x06008595 RID: 34197 RVA: 0x002FEE46 File Offset: 0x002FD046
		public override void Cleanup()
		{
			SendTransportShipAwayUtility.SendTransportShipAway(this.transportShip, this.unloadContents, this.unsatisfiedDropMode);
			this.transportShip = null;
		}

		// Token: 0x06008596 RID: 34198 RVA: 0x002FEE66 File Offset: 0x002FD066
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.unloadContents, "unloadContents", false, false);
			Scribe_Values.Look<TransportShipDropMode>(ref this.unsatisfiedDropMode, "unsatisfiedDropMode", TransportShipDropMode.None, false);
			Scribe_References.Look<TransportShip>(ref this.transportShip, "transportShip", false);
		}

		// Token: 0x0400536E RID: 21358
		public TransportShip transportShip;

		// Token: 0x0400536F RID: 21359
		public TransportShipDropMode unsatisfiedDropMode;

		// Token: 0x04005370 RID: 21360
		public bool unloadContents;
	}
}
