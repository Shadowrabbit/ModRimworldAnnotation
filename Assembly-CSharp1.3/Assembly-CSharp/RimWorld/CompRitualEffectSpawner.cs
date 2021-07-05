using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011FD RID: 4605
	public abstract class CompRitualEffectSpawner : ThingComp
	{
		// Token: 0x06006EC1 RID: 28353 RVA: 0x00250FBC File Offset: 0x0024F1BC
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(30))
			{
				this.ritual = this.parent.TargetOfRitual();
			}
			if (this.ritual != null)
			{
				this.Tick_InRitual(this.ritual);
				return;
			}
			this.Tick_OutOfRitual();
		}

		// Token: 0x06006EC2 RID: 28354
		protected abstract void Tick_InRitual(LordJob_Ritual ritual);

		// Token: 0x06006EC3 RID: 28355
		protected abstract void Tick_OutOfRitual();

		// Token: 0x04003D52 RID: 15698
		protected LordJob_Ritual ritual;

		// Token: 0x04003D53 RID: 15699
		private const int RitualCheckInterval = 30;
	}
}
