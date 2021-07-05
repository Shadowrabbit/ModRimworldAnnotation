using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D69 RID: 3433
	public class CompAbilityEffect_StartRitual : CompAbilityEffect
	{
		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06004FB5 RID: 20405 RVA: 0x001AAD87 File Offset: 0x001A8F87
		public new CompProperties_AbilityStartRitual Props
		{
			get
			{
				return (CompProperties_AbilityStartRitual)this.props;
			}
		}

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06004FB6 RID: 20406 RVA: 0x001AAD94 File Offset: 0x001A8F94
		public Precept_Ritual Ritual
		{
			get
			{
				if (this.ritualCached == null)
				{
					for (int i = 0; i < this.parent.pawn.Ideo.PreceptsListForReading.Count; i++)
					{
						if (this.parent.pawn.Ideo.PreceptsListForReading[i].def == this.Props.ritualDef)
						{
							this.ritualCached = (Precept_Ritual)this.parent.pawn.Ideo.PreceptsListForReading[i];
							break;
						}
					}
				}
				return this.ritualCached;
			}
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x0000313F File Offset: 0x0000133F
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
		}

		// Token: 0x06004FB8 RID: 20408 RVA: 0x001AAE2C File Offset: 0x001A902C
		public override bool GizmoDisabled(out string reason)
		{
			if (GatheringsUtility.AnyLordJobPreventsNewGatherings(this.parent.pawn.Map))
			{
				reason = "AbilitySpeechDisabledAnotherGatheringInProgress".Translate();
				return true;
			}
			string text;
			if (this.Ritual != null && this.Ritual.targetFilter != null && !this.Ritual.targetFilter.CanStart(this.parent.pawn, TargetInfo.Invalid, out text))
			{
				reason = text;
				return true;
			}
			reason = null;
			return false;
		}

		// Token: 0x06004FB9 RID: 20409 RVA: 0x001AAEAC File Offset: 0x001A90AC
		public override Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
		{
			TargetInfo targetInfo = TargetInfo.Invalid;
			if (this.Ritual.targetFilter != null)
			{
				targetInfo = this.Ritual.targetFilter.BestTarget(this.parent.pawn, target.ToTargetInfo(this.parent.pawn.MapHeld));
			}
			return this.Ritual.GetRitualBeginWindow(targetInfo, null, confirmAction, this.parent.pawn, null, null);
		}

		// Token: 0x04002FC4 RID: 12228
		private Precept_Ritual ritualCached;
	}
}
