using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011E7 RID: 4583
	public class ComTargetEffect_FleckOnTarget : CompTargetEffect
	{
		// Token: 0x1700132B RID: 4907
		// (get) Token: 0x06006E78 RID: 28280 RVA: 0x002503E9 File Offset: 0x0024E5E9
		private CompProperties_TargetEffect_FleckOnTarget Props
		{
			get
			{
				return (CompProperties_TargetEffect_FleckOnTarget)this.props;
			}
		}

		// Token: 0x06006E79 RID: 28281 RVA: 0x002503F6 File Offset: 0x0024E5F6
		public override void DoEffectOn(Pawn user, Thing target)
		{
			if (this.Props.fleckDef != null)
			{
				FleckMaker.AttachedOverlay(target, this.Props.fleckDef, Vector3.zero, 1f, -1f);
			}
		}
	}
}
