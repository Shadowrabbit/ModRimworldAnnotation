using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020003E1 RID: 993
	public class HediffComp_HealPermanentWounds : HediffComp
	{
		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x0600185C RID: 6236 RVA: 0x000171F0 File Offset: 0x000153F0
		public HediffCompProperties_HealPermanentWounds Props
		{
			get
			{
				return (HediffCompProperties_HealPermanentWounds)this.props;
			}
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x000171FD File Offset: 0x000153FD
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.ResetTicksToHeal();
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x0001720B File Offset: 0x0001540B
		private void ResetTicksToHeal()
		{
			this.ticksToHeal = Rand.Range(15, 30) * 60000;
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x00017222 File Offset: 0x00015422
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToHeal--;
			if (this.ticksToHeal <= 0)
			{
				this.TryHealRandomPermanentWound();
				this.ResetTicksToHeal();
			}
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x000DF2A4 File Offset: 0x000DD4A4
		private void TryHealRandomPermanentWound()
		{
			Hediff hediff;
			if (!(from hd in base.Pawn.health.hediffSet.hediffs
			where hd.IsPermanent() || hd.def.chronic
			select hd).TryRandomElement(out hediff))
			{
				return;
			}
			HealthUtility.CureHediff(hediff);
			if (PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				Messages.Message("MessagePermanentWoundHealed".Translate(this.parent.LabelCap, base.Pawn.LabelShort, hediff.Label, base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x00017247 File Offset: 0x00015447
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x0001725B File Offset: 0x0001545B
		public override string CompDebugString()
		{
			return "ticksToHeal: " + this.ticksToHeal;
		}

		// Token: 0x04001279 RID: 4729
		private int ticksToHeal;
	}
}
