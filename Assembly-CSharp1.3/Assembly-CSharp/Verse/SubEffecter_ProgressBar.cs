using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200005E RID: 94
	public class SubEffecter_ProgressBar : SubEffecter
	{
		// Token: 0x0600040C RID: 1036 RVA: 0x000158D1 File Offset: 0x00013AD1
		public SubEffecter_ProgressBar(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x00015AB0 File Offset: 0x00013CB0
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.mote == null)
			{
				this.mote = (MoteProgressBar)MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
				this.mote.exactScale.x = 0.68f;
				this.mote.exactScale.z = 0.12f;
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00015B0C File Offset: 0x00013D0C
		public override void SubCleanup()
		{
			if (this.mote != null && !this.mote.Destroyed)
			{
				this.mote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04000134 RID: 308
		public MoteProgressBar mote;

		// Token: 0x04000135 RID: 309
		private const float Width = 0.68f;

		// Token: 0x04000136 RID: 310
		private const float Height = 0.12f;
	}
}
