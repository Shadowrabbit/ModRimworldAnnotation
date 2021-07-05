using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010CC RID: 4300
	[StaticConstructorOnStartup]
	public class DropPodIncoming : Skyfaller, IActiveDropPod, IThingHolder
	{
		// Token: 0x170011A8 RID: 4520
		// (get) Token: 0x060066E1 RID: 26337 RVA: 0x0022C0AD File Offset: 0x0022A2AD
		// (set) Token: 0x060066E2 RID: 26338 RVA: 0x0022C0C5 File Offset: 0x0022A2C5
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

		// Token: 0x060066E3 RID: 26339 RVA: 0x0022C0E0 File Offset: 0x0022A2E0
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

		// Token: 0x060066E4 RID: 26340 RVA: 0x0022C14C File Offset: 0x0022A34C
		protected override void Impact()
		{
			for (int i = 0; i < 6; i++)
			{
				FleckMaker.ThrowDustPuff(base.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(1f), base.Map, 1.2f);
			}
			FleckMaker.ThrowLightningGlow(base.Position.ToVector3Shifted(), base.Map, 2f);
			GenClamor.DoClamor(this, 15f, ClamorDefOf.Impact);
			base.Impact();
		}
	}
}
