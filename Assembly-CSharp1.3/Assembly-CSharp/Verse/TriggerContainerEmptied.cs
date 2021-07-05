using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000342 RID: 834
	public class TriggerContainerEmptied : Thing
	{
		// Token: 0x060017C3 RID: 6083 RVA: 0x0008DECC File Offset: 0x0008C0CC
		public override void Tick()
		{
			if (base.Spawned && this.IsHashIntervalTick(60))
			{
				Thing thing = this.container;
				CompThingContainer compThingContainer = (thing != null) ? thing.TryGetComp<CompThingContainer>() : null;
				if (compThingContainer == null)
				{
					this.Destroy(DestroyMode.Vanish);
					return;
				}
				if (!compThingContainer.innerContainer.Any)
				{
					Find.SignalManager.SendSignal(new Signal(this.signalTag));
					if (!base.Destroyed)
					{
						this.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x0008DF3A File Offset: 0x0008C13A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
			Scribe_References.Look<Thing>(ref this.container, "container", false);
		}

		// Token: 0x0400105A RID: 4186
		public string signalTag;

		// Token: 0x0400105B RID: 4187
		public Thing container;
	}
}
