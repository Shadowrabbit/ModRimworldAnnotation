using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011ED RID: 4589
	public class ComTargetEffect_MoteConnecting : CompTargetEffect
	{
		// Token: 0x1700132D RID: 4909
		// (get) Token: 0x06006E84 RID: 28292 RVA: 0x00250580 File Offset: 0x0024E780
		private CompProperties_TargetEffect_MoteConnecting Props
		{
			get
			{
				return (CompProperties_TargetEffect_MoteConnecting)this.props;
			}
		}

		// Token: 0x06006E85 RID: 28293 RVA: 0x0025058D File Offset: 0x0024E78D
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (this.Props.moteDef != null)
			{
				MoteMaker.MakeConnectingLine(user.DrawPos, target.DrawPos, this.Props.moteDef, user.Map, 1f);
			}
		}
	}
}
