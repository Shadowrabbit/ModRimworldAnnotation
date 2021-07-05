using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000343 RID: 835
	public class TriggerUnfogged : Thing
	{
		// Token: 0x060017C6 RID: 6086 RVA: 0x0008DF65 File Offset: 0x0008C165
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

		// Token: 0x060017C7 RID: 6087 RVA: 0x0008DFA0 File Offset: 0x0008C1A0
		public void Activated()
		{
			Find.SignalManager.SendSignal(new Signal(this.signalTag));
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x0008DFC6 File Offset: 0x0008C1C6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
			Scribe_Values.Look<bool>(ref this.everFogged, "everFogged", false, false);
		}

		// Token: 0x0400105C RID: 4188
		public string signalTag;

		// Token: 0x0400105D RID: 4189
		private bool everFogged;
	}
}
