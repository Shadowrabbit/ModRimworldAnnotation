using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001116 RID: 4374
	public class CompCameraShaker : ThingComp
	{
		// Token: 0x170011F6 RID: 4598
		// (get) Token: 0x0600690B RID: 26891 RVA: 0x00237063 File Offset: 0x00235263
		public CompProperties_CameraShaker Props
		{
			get
			{
				return (CompProperties_CameraShaker)this.props;
			}
		}

		// Token: 0x0600690C RID: 26892 RVA: 0x00237070 File Offset: 0x00235270
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
