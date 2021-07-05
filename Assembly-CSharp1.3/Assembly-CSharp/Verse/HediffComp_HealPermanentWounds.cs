using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020002A5 RID: 677
	public class HediffComp_HealPermanentWounds : HediffComp
	{
		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001294 RID: 4756 RVA: 0x0006AF07 File Offset: 0x00069107
		public HediffCompProperties_HealPermanentWounds Props
		{
			get
			{
				return (HediffCompProperties_HealPermanentWounds)this.props;
			}
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x0006AF14 File Offset: 0x00069114
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ResetTicksToHeal();
		}

		// Token: 0x06001296 RID: 4758 RVA: 0x0006AF22 File Offset: 0x00069122
		private void ResetTicksToHeal()
		{
			this.ticksToHeal = Rand.Range(15, 30) * 60000;
		}

		// Token: 0x06001297 RID: 4759 RVA: 0x0006AF39 File Offset: 0x00069139
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToHeal--;
			if (this.ticksToHeal <= 0)
			{
				this.TryHealRandomPermanentWound();
				this.ResetTicksToHeal();
			}
		}

		// Token: 0x06001298 RID: 4760 RVA: 0x0006AF60 File Offset: 0x00069160
		private void TryHealRandomPermanentWound()
		{
			Hediff hediff;
			if (!(from hd in base.Pawn.health.hediffSet.hediffs
			where hd.IsPermanent() || hd.def.chronic
			select hd).TryRandomElement(out hediff))
			{
				return;
			}
			HealthUtility.Cure(hediff);
			if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				Messages.Message("MessagePermanentWoundHealed".Translate(this.parent.LabelCap, base.Pawn.LabelShort, hediff.Label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x0006B024 File Offset: 0x00069224
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x0006B038 File Offset: 0x00069238
		public override string CompDebugString()
		{
			return "ticksToHeal: " + this.ticksToHeal;
		}

		// Token: 0x04000E11 RID: 3601
		private int ticksToHeal;
	}
}
