using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000282 RID: 642
	public class HediffComp_ChangeImplantLevel : HediffComp
	{
		// Token: 0x17000389 RID: 905
		// (get) Token: 0x0600122F RID: 4655 RVA: 0x00069628 File Offset: 0x00067828
		public HediffCompProperties_ChangeImplantLevel Props
		{
			get
			{
				return (HediffCompProperties_ChangeImplantLevel)this.props;
			}
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00069638 File Offset: 0x00067838
		public override void CompPostTick(ref float severityAdjustment)
		{
			float mtbDays = this.Props.probabilityPerStage[this.parent.CurStageIndex].mtbDays;
			if (mtbDays > 0f && base.Pawn.IsHashIntervalTick(60))
			{
				ChangeImplantLevel_Probability changeImplantLevel_Probability = this.Props.probabilityPerStage[this.parent.CurStageIndex];
				if ((this.lastChangeLevelTick < 0 || (float)(Find.TickManager.TicksGame - this.lastChangeLevelTick) >= changeImplantLevel_Probability.minIntervalDays * 60000f) && Rand.MTBEventOccurs(mtbDays, 60000f, 60f))
				{
					Hediff_Level hediff_Level = this.parent.pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.implant, false) as Hediff_Level;
					if (hediff_Level != null)
					{
						hediff_Level.ChangeLevel(this.Props.levelOffset);
						this.lastChangeLevelTick = Find.TickManager.TicksGame;
						Messages.Message("MessageLostImplantLevelFromHediff".Translate(this.parent.pawn.Named("PAWN"), hediff_Level.LabelBase, this.parent.Label), this.parent.pawn, MessageTypeDefOf.NegativeEvent, true);
					}
				}
			}
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x0006978A File Offset: 0x0006798A
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.lastChangeLevelTick, "lastChangeLevelTick", 0, false);
		}

		// Token: 0x04000DD4 RID: 3540
		public int lastChangeLevelTick = -1;
	}
}
