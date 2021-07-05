using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020004D2 RID: 1234
	public class TriggerUnfogged : Thing
	{
		// Token: 0x06001EC4 RID: 7876 RVA: 0x0001B318 File Offset: 0x00019518
		public override void Tick()
		{
			if (base.Spawned)
			{
				if (base.Position.Fogged(base.Map))
				{
					this.everFogged = true;
					return;
				}
				if (this.everFogged)
				{
					this.Activated();
					return;
				}
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x0001B353 File Offset: 0x00019553
		public void Activated()
		{
			Find.SignalManager.SendSignal(new Signal(this.signalTag));
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x0001B379 File Offset: 0x00019579
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
			Scribe_Values.Look<bool>(ref this.everFogged, "everFogged", false, false);
		}

		// Token: 0x040015D5 RID: 5589
		public string signalTag;

		// Token: 0x040015D6 RID: 5590
		private bool everFogged;
	}
}
