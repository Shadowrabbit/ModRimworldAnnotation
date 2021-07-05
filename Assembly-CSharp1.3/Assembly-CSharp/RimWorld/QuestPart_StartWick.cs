using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BAD RID: 2989
	public class QuestPart_StartWick : QuestPart
	{
		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x060045BF RID: 17855 RVA: 0x001716E9 File Offset: 0x0016F8E9
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				yield return this.explosiveThing;
				if (this.initiator != null)
				{
					yield return this.initiator;
				}
				yield break;
			}
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x001716FC File Offset: 0x0016F8FC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.explosiveThing != null)
			{
				CompExplosive compExplosive = this.explosiveThing.TryGetComp<CompExplosive>();
				if (compExplosive != null)
				{
					compExplosive.StartWick(this.initiator);
				}
			}
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x00171746 File Offset: 0x0016F946
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Thing>(ref this.explosiveThing, "explosiveThing", false);
			Scribe_References.Look<Thing>(ref this.initiator, "initiator", false);
		}

		// Token: 0x04002A7D RID: 10877
		public string inSignal;

		// Token: 0x04002A7E RID: 10878
		public Thing explosiveThing;

		// Token: 0x04002A7F RID: 10879
		public Thing initiator;
	}
}
