using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003C3 RID: 963
	public class HediffComp_ChangeImplantLevel : HediffComp
	{
		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x060017F3 RID: 6131 RVA: 0x00016CA4 File Offset: 0x00014EA4
		public HediffCompProperties_ChangeImplantLevel Props
		{
			get
			{
				return (HediffCompProperties_ChangeImplantLevel)this.props;
			}
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x000DDF28 File Offset: 0x000DC128
		public override void CompPostTick(ref float severityAdjustment)
		{
			float mtbDays = this.Props.probabilityPerStage[this.parent.CurStageIndex].mtbDays;
			if (mtbDays > 0f && base.Pawn.IsHashIntervalTick(60))
			{
				ChangeImplantLevel_Probability changeImplantLevel_Probability = this.Props.probabilityPerStage[this.parent.CurStageIndex];
				if ((this.lastChangeLevelTick < 0 || (float)(Find.TickManager.TicksGame - this.lastChangeLevelTick) >= changeImplantLevel_Probability.minIntervalDays * 60000f) && Rand.MTBEventOccurs(mtbDays, 60000f, 60f))
				{
					Hediff_ImplantWithLevel hediff_ImplantWithLevel = this.parent.pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.implant, false) as Hediff_ImplantWithLevel;
					if (hediff_ImplantWithLevel != null)
					{
						hediff_ImplantWithLevel.ChangeLevel(this.Props.levelOffset);
						this.lastChangeLevelTick = Find.TickManager.TicksGame;
						Messages.Message("MessageLostImplantLevelFromHediff".Translate(this.parent.pawn.Named("PAWN"), hediff_ImplantWithLevel.LabelBase, this.parent.Label), this.parent.pawn, MessageTypeDefOf.NegativeEvent, true);
					}
				}
			}
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x00016CB1 File Offset: 0x00014EB1
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.lastChangeLevelTick, "lastChangeLevelTick", 0, false);
		}

		// Token: 0x04001238 RID: 4664
		public int lastChangeLevelTick = -1;
	}
}
