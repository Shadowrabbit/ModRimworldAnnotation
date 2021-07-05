using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200165C RID: 5724
	public class QuestPart_AddShipJob : QuestPart
	{
		// Token: 0x06008580 RID: 34176 RVA: 0x002FE9EA File Offset: 0x002FCBEA
		public virtual ShipJob GetShipJob()
		{
			return this.shipJob ?? ShipJobMaker.MakeShipJob(this.shipJobDef);
		}

		// Token: 0x06008581 RID: 34177 RVA: 0x002FEA04 File Offset: 0x002FCC04
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				switch (this.shipJobStartMode)
				{
				case ShipJobStartMode.Queue:
					this.transportShip.AddJob(this.GetShipJob());
					break;
				case ShipJobStartMode.Instant:
					this.transportShip.AddJob(this.GetShipJob());
					this.transportShip.TryGetNextJob();
					break;
				case ShipJobStartMode.Force:
					this.transportShip.ForceJob(this.GetShipJob());
					break;
				}
				this.shipJob = null;
			}
		}

		// Token: 0x06008582 RID: 34178 RVA: 0x002FEA88 File Offset: 0x002FCC88
		public override void Cleanup()
		{
			base.Cleanup();
			this.transportShip = null;
		}

		// Token: 0x06008583 RID: 34179 RVA: 0x002FEA98 File Offset: 0x002FCC98
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<ShipJobStartMode>(ref this.shipJobStartMode, "shipJobStartMode", ShipJobStartMode.Queue, false);
			Scribe_Deep.Look<ShipJob>(ref this.shipJob, "shipJob", Array.Empty<object>());
			Scribe_Defs.Look<ShipJobDef>(ref this.shipJobDef, "shipJobDef");
			Scribe_References.Look<TransportShip>(ref this.transportShip, "transportShip", false);
		}

		// Token: 0x0400535D RID: 21341
		public string inSignal;

		// Token: 0x0400535E RID: 21342
		public ShipJobDef shipJobDef;

		// Token: 0x0400535F RID: 21343
		public TransportShip transportShip;

		// Token: 0x04005360 RID: 21344
		public ShipJobStartMode shipJobStartMode;

		// Token: 0x04005361 RID: 21345
		public ShipJob shipJob;
	}
}
