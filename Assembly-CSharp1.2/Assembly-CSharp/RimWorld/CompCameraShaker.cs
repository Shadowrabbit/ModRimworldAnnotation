using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017A8 RID: 6056
	public class CompCameraShaker : ThingComp
	{
		// Token: 0x170014B3 RID: 5299
		// (get) Token: 0x060085D4 RID: 34260 RVA: 0x00059BD9 File Offset: 0x00057DD9
		public CompProperties_CameraShaker Props
		{
			get
			{
				return (CompProperties_CameraShaker)this.props;
			}
		}

		// Token: 0x060085D5 RID: 34261 RVA: 0x00276F78 File Offset: 0x00275178
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Spawned && this.parent.Map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.SetMinShake(this.Props.mag);
			}
		}
	}
}
