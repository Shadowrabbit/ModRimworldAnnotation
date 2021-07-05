using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001201 RID: 4609
	public class CompRitualTargetEffecterSpawner : CompRitualEffectSpawner
	{
		// Token: 0x1700133E RID: 4926
		// (get) Token: 0x06006ECB RID: 28363 RVA: 0x002510E6 File Offset: 0x0024F2E6
		private CompProperties_RitualTargetEffecterSpawner Props
		{
			get
			{
				return (CompProperties_RitualTargetEffecterSpawner)this.props;
			}
		}

		// Token: 0x06006ECC RID: 28364 RVA: 0x002510F3 File Offset: 0x0024F2F3
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecter = null;
		}

		// Token: 0x06006ECD RID: 28365 RVA: 0x00251114 File Offset: 0x0024F314
		protected override void Tick_InRitual(LordJob_Ritual ritual)
		{
			if (this.Props.minRitualProgress > 0f && ritual.Progress < this.Props.minRitualProgress)
			{
				return;
			}
			if (this.effecter == null)
			{
				this.effecter = this.Props.effecter.Spawn();
				this.effecter.Trigger(this.parent, this.parent);
			}
			this.effecter.EffectTick(this.parent, this.parent);
		}

		// Token: 0x06006ECE RID: 28366 RVA: 0x002511A7 File Offset: 0x0024F3A7
		protected override void Tick_OutOfRitual()
		{
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecter = null;
		}

		// Token: 0x04003D58 RID: 15704
		private Effecter effecter;
	}
}
