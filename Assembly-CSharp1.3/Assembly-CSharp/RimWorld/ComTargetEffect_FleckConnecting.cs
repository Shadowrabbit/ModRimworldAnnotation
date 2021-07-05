using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E5 RID: 4581
	public class ComTargetEffect_FleckConnecting : CompTargetEffect
	{
		// Token: 0x1700132A RID: 4906
		// (get) Token: 0x06006E74 RID: 28276 RVA: 0x0025038E File Offset: 0x0024E58E
		private CompProperties_TargetEffect_FleckConnecting Props
		{
			get
			{
				return (CompProperties_TargetEffect_FleckConnecting)this.props;
			}
		}

		// Token: 0x06006E75 RID: 28277 RVA: 0x0025039B File Offset: 0x0024E59B
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (this.Props.fleckDef != null)
			{
				FleckMaker.ConnectingLine(user.DrawPos, target.DrawPos, this.Props.fleckDef, user.Map, 1f);
			}
		}
	}
}
