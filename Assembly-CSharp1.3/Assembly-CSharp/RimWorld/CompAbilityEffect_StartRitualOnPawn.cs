using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D6B RID: 3435
	public class CompAbilityEffect_StartRitualOnPawn : CompAbilityEffect_StartRitual
	{
		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06004FBC RID: 20412 RVA: 0x001AAF37 File Offset: 0x001A9137
		public new CompProperties_AbilityStartRitualOnPawn Props
		{
			get
			{
				return (CompProperties_AbilityStartRitualOnPawn)this.props;
			}
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x001AAF44 File Offset: 0x001A9144
		protected virtual Precept_Ritual RitualForTarget(Pawn pawn)
		{
			return base.Ritual;
		}

		// Token: 0x06004FBE RID: 20414 RVA: 0x001AAF4C File Offset: 0x001A914C
		public override Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
		{
			Pawn pawn = target.Pawn;
			Precept_Ritual precept_Ritual = this.RitualForTarget(pawn);
			TargetInfo targetInfo = TargetInfo.Invalid;
			if (precept_Ritual.targetFilter != null)
			{
				targetInfo = precept_Ritual.targetFilter.BestTarget(this.parent.pawn, target.ToTargetInfo(this.parent.pawn.MapHeld));
			}
			return precept_Ritual.GetRitualBeginWindow(targetInfo, null, confirmAction, this.parent.pawn, new Dictionary<string, Pawn>
			{
				{
					this.Props.targetRoleId,
					pawn
				}
			}, null);
		}
	}
}
