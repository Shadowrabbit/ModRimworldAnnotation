using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200049E RID: 1182
	public class SubEffecter_InteractSymbol : SubEffecter
	{
		// Token: 0x060023F8 RID: 9208 RVA: 0x000158D1 File Offset: 0x00013AD1
		public SubEffecter_InteractSymbol(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x000DFE16 File Offset: 0x000DE016
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.interactMote == null)
			{
				this.interactMote = MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
			}
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x000DFE38 File Offset: 0x000DE038
		public override void SubCleanup()
		{
			if (this.interactMote != null && !this.interactMote.Destroyed)
			{
				this.interactMote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x040016B1 RID: 5809
		private Mote interactMote;
	}
}
