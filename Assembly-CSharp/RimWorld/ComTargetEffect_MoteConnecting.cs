using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018CB RID: 6347
	public class ComTargetEffect_MoteConnecting : CompTargetEffect
	{
		// Token: 0x1700161B RID: 5659
		// (get) Token: 0x06008CBA RID: 36026 RVA: 0x0005E503 File Offset: 0x0005C703
		private CompProperties_TargetEffect_MoteConnecting Props
		{
			get
			{
				return (CompProperties_TargetEffect_MoteConnecting)this.props;
			}
		}

		// Token: 0x06008CBB RID: 36027 RVA: 0x0005E510 File Offset: 0x0005C710
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (this.Props.moteDef != null)
			{
				MoteMaker.MakeConnectingLine(user.DrawPos, target.DrawPos, this.Props.moteDef, user.Map, 1f);
			}
		}
	}
}
