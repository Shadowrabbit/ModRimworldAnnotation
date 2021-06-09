using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001730 RID: 5936
	[StaticConstructorOnStartup]
	public class DropPodIncoming : Skyfaller, IActiveDropPod, IThingHolder
	{
		// Token: 0x1700145D RID: 5213
		// (get) Token: 0x060082EC RID: 33516 RVA: 0x00057E59 File Offset: 0x00056059
		// (set) Token: 0x060082ED RID: 33517 RVA: 0x00057E71 File Offset: 0x00056071
		public ActiveDropPodInfo Contents
		{
			get
			{
				return ((ActiveDropPod)this.innerContainer[0]).Contents;
			}
			set
			{
				((ActiveDropPod)this.innerContainer[0]).Contents = value;
			}
		}

		// Token: 0x060082EE RID: 33518 RVA: 0x0026D080 File Offset: 0x0026B280
		protected override void SpawnThings()
		{
			if (this.Contents.spawnWipeMode == null)
			{
				base.SpawnThings();
				return;
			}
			for (int i = this.innerContainer.Count - 1; i >= 0; i--)
			{
				GenSpawn.Spawn(this.innerContainer[i], base.Position, base.Map, this.Contents.spawnWipeMode.Value);
			}
		}

		// Token: 0x060082EF RID: 33519 RVA: 0x0026D0EC File Offset: 0x0026B2EC
		protected override void Impact()
		{
			for (int i = 0; i < 6; i++)
			{
				MoteMaker.ThrowDustPuff(base.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(1f), base.Map, 1.2f);
			}
			MoteMaker.ThrowLightningGlow(base.Position.ToVector3Shifted(), base.Map, 2f);
			GenClamor.DoClamor(this, 15f, ClamorDefOf.Impact);
			base.Impact();
		}
	}
}
