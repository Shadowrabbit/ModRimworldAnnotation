using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011FF RID: 4607
	public class CompRitualTargetMoteSpawner : CompRitualEffectSpawner
	{
		// Token: 0x1700133D RID: 4925
		// (get) Token: 0x06006EC6 RID: 28358 RVA: 0x00251022 File Offset: 0x0024F222
		private CompProperties_RitualTargetMoteSpawner Props
		{
			get
			{
				return (CompProperties_RitualTargetMoteSpawner)this.props;
			}
		}

		// Token: 0x06006EC7 RID: 28359 RVA: 0x00251030 File Offset: 0x0024F230
		protected override void Tick_InRitual(LordJob_Ritual ritual)
		{
			if (this.mote == null || this.mote.Destroyed)
			{
				this.mote = MoteMaker.MakeStaticMote(this.parent.Position.ToVector3Shifted(), this.parent.Map, this.Props.mote, 1f);
			}
			this.mote.Maintain();
		}

		// Token: 0x06006EC8 RID: 28360 RVA: 0x00251096 File Offset: 0x0024F296
		protected override void Tick_OutOfRitual()
		{
			if (this.mote != null && !this.mote.Destroyed)
			{
				Mote mote = this.mote;
				if (mote != null)
				{
					mote.Destroy(DestroyMode.Vanish);
				}
			}
			this.mote = null;
		}

		// Token: 0x04003D55 RID: 15701
		private Mote mote;
	}
}
