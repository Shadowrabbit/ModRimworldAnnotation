using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000286 RID: 646
	public class HediffComp_DamageBrain : HediffComp
	{
		// Token: 0x1700038C RID: 908
		// (get) Token: 0x0600123B RID: 4667 RVA: 0x00069870 File Offset: 0x00067A70
		public HediffCompProperties_DamageBrain Props
		{
			get
			{
				return (HediffCompProperties_DamageBrain)this.props;
			}
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00069880 File Offset: 0x00067A80
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.Props.mtbDaysPerStage[this.parent.CurStageIndex] > 0f && base.Pawn.IsHashIntervalTick(60) && Rand.MTBEventOccurs(this.Props.mtbDaysPerStage[this.parent.CurStageIndex], 60000f, 60f))
			{
				BodyPartRecord brain = base.Pawn.health.hediffSet.GetBrain();
				if (brain == null)
				{
					return;
				}
				int randomInRange = this.Props.damageAmount.RandomInRange;
				base.Pawn.TakeDamage(new DamageInfo(DamageDefOf.Burn, (float)randomInRange, 0f, -1f, null, brain, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
				Messages.Message("MessageReceivedBrainDamageFromHediff".Translate(base.Pawn.Named("PAWN"), randomInRange, this.parent.Label), base.Pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}
	}
}
