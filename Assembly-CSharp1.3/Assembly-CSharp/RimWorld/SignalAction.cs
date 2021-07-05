using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001097 RID: 4247
	public abstract class SignalAction : Thing
	{
		// Token: 0x0600654B RID: 25931 RVA: 0x00223845 File Offset: 0x00221A45
		public override void Notify_SignalReceived(Signal signal)
		{
			base.Notify_SignalReceived(signal);
			if (signal.tag == this.signalTag)
			{
				this.DoAction(signal.args);
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600654C RID: 25932
		protected abstract void DoAction(SignalArgs args);

		// Token: 0x0600654D RID: 25933 RVA: 0x0022387C File Offset: 0x00221A7C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04003906 RID: 14598
		public string signalTag;
	}
}
