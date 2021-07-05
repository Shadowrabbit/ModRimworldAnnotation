using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D49 RID: 3401
	public class CompAbilityEffect_MustBeCapableOf : CompAbilityEffect
	{
		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x06004F64 RID: 20324 RVA: 0x001A9A46 File Offset: 0x001A7C46
		public new CompProperties_AbilityMustBeCapableOf Props
		{
			get
			{
				return (CompProperties_AbilityMustBeCapableOf)this.props;
			}
		}

		// Token: 0x06004F65 RID: 20325 RVA: 0x001A9A54 File Offset: 0x001A7C54
		public override bool GizmoDisabled(out string reason)
		{
			foreach (WorkTags workTags in this.Props.workTags.GetAllSelectedItems<WorkTags>())
			{
				if (this.parent.pawn.WorkTagIsDisabled(workTags))
				{
					reason = "AbilityDisabled_IncapableOfWorkTag".Translate(this.parent.pawn.Named("PAWN"), workTags.LabelTranslated());
					return true;
				}
			}
			reason = null;
			return false;
		}
	}
}
