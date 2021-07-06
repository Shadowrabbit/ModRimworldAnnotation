using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000A6 RID: 166
	public class SubEffecter_ProgressBar : SubEffecter
	{
		// Token: 0x06000565 RID: 1381 RVA: 0x0000A876 File Offset: 0x00008A76
		public SubEffecter_ProgressBar(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0008BA78 File Offset: 0x00089C78
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.mote == null)
			{
				this.mote = (MoteProgressBar)MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
				this.mote.exactScale.x = 0.68f;
				this.mote.exactScale.z = 0.12f;
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0000A8C4 File Offset: 0x00008AC4
		public override void SubCleanup()
		{
			if (this.mote != null && !this.mote.Destroyed)
			{
				this.mote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x040002A1 RID: 673
		public MoteProgressBar mote;

		// Token: 0x040002A2 RID: 674
		private const float Width = 0.68f;

		// Token: 0x040002A3 RID: 675
		private const float Height = 0.12f;
	}
}
