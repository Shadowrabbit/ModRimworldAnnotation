using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D67 RID: 3431
	public class CompAbilityEffect_StartConversion : CompAbilityEffect_StartRitualOnPawn
	{
		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06004FB1 RID: 20401 RVA: 0x001AAD34 File Offset: 0x001A8F34
		public new CompProperties_AbilityStartConversion Props
		{
			get
			{
				return (CompProperties_AbilityStartConversion)this.props;
			}
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x001AAD41 File Offset: 0x001A8F41
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			return RitualBehaviorWorker_Conversion.ValidateConvertee(target.Pawn, this.parent.pawn, throwMessages) && base.Valid(target, throwMessages);
		}
	}
}
