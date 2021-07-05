using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018CD RID: 6349
	public class ComTargetEffect_MoteOnTarget : CompTargetEffect
	{
		// Token: 0x1700161C RID: 5660
		// (get) Token: 0x06008CBE RID: 36030 RVA: 0x0005E55F File Offset: 0x0005C75F
		private CompProperties_TargetEffect_MoteOnTarget Props
		{
			get
			{
				return (CompProperties_TargetEffect_MoteOnTarget)this.props;
			}
		}

		// Token: 0x06008CBF RID: 36031 RVA: 0x0005E56C File Offset: 0x0005C76C
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (this.Props.moteDef != null)
			{
				MoteMaker.MakeAttachedOverlay(target, this.Props.moteDef, Vector3.zero, 1f, -1f);
			}
		}
	}
}
