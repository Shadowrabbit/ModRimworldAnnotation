using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000811 RID: 2065
	public class SubEffecter_InteractSymbol : SubEffecter
	{
		// Token: 0x060033F5 RID: 13301 RVA: 0x0000A876 File Offset: 0x00008A76
		public SubEffecter_InteractSymbol(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x00028B71 File Offset: 0x00026D71
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.interactMote == null)
			{
				this.interactMote = MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
			}
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x00028B93 File Offset: 0x00026D93
		public override void SubCleanup()
		{
			if (this.interactMote != null && !this.interactMote.Destroyed)
			{
				this.interactMote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04002403 RID: 9219
		private Mote interactMote;
	}
}
