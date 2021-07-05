using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011EF RID: 4591
	public class ComTargetEffect_MoteOnTarget : CompTargetEffect
	{
		// Token: 0x1700132E RID: 4910
		// (get) Token: 0x06006E88 RID: 28296 RVA: 0x002505DC File Offset: 0x0024E7DC
		private CompProperties_TargetEffect_MoteOnTarget Props
		{
			get
			{
				return (CompProperties_TargetEffect_MoteOnTarget)this.props;
			}
		}

		// Token: 0x06006E89 RID: 28297 RVA: 0x002505E9 File Offset: 0x0024E7E9
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (this.Props.moteDef != null)
			{
				MoteMaker.MakeAttachedOverlay(target, this.Props.moteDef, Vector3.zero, 1f, -1f);
			}
		}
	}
}
