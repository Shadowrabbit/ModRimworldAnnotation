using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200027D RID: 637
	public class HediffComp_CauseMentalState : HediffComp
	{
		// Token: 0x17000386 RID: 902
		// (get) Token: 0x0600121F RID: 4639 RVA: 0x00069176 File Offset: 0x00067376
		public HediffCompProperties_CauseMentalState Props
		{
			get
			{
				return (HediffCompProperties_CauseMentalState)this.props;
			}
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00069184 File Offset: 0x00067384
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (base.Pawn.IsHashIntervalTick(60))
			{
				if (base.Pawn.RaceProps.Humanlike)
				{
					if (base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.humanMentalState && Rand.MTBEventOccurs(this.Props.mtbDaysToCauseMentalState, 60000f, 60f) && base.Pawn.Awake() && base.Pawn.mindState.mentalStateHandler.TryStartMentalState(this.Props.humanMentalState, this.parent.def.LabelCap, false, false, null, true, false, false) && base.Pawn.Spawned)
					{
						this.SendLetter(this.Props.humanMentalState);
						return;
					}
				}
				else if (base.Pawn.RaceProps.Animal && base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.animalMentalState && (this.Props.animalMentalStateAlias == null || base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.animalMentalStateAlias) && Rand.MTBEventOccurs(this.Props.mtbDaysToCauseMentalState, 60000f, 60f) && base.Pawn.Awake() && base.Pawn.mindState.mentalStateHandler.TryStartMentalState(this.Props.animalMentalState, this.parent.def.LabelCap, false, false, null, true, false, false) && base.Pawn.Spawned)
				{
					this.SendLetter(this.Props.animalMentalState);
				}
			}
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x00069360 File Offset: 0x00067560
		public override void CompPostPostRemoved()
		{
			if (this.Props.endMentalStateOnCure && ((base.Pawn.RaceProps.Humanlike && base.Pawn.mindState.mentalStateHandler.CurStateDef == this.Props.humanMentalState) || (base.Pawn.RaceProps.Animal && (base.Pawn.mindState.mentalStateHandler.CurStateDef == this.Props.animalMentalState || base.Pawn.mindState.mentalStateHandler.CurStateDef == this.Props.animalMentalStateAlias))) && !base.Pawn.mindState.mentalStateHandler.CurState.causedByMood)
			{
				base.Pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
			}
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00069440 File Offset: 0x00067640
		private void SendLetter(MentalStateDef mentalStateDef)
		{
			Find.LetterStack.ReceiveLetter((mentalStateDef.beginLetterLabel ?? mentalStateDef.LabelCap).CapitalizeFirst() + ": " + base.Pawn.LabelShortCap, base.Pawn.mindState.mentalStateHandler.CurState.GetBeginLetterText() + "\n\n" + "CausedByHediff".Translate(this.parent.LabelCap), this.Props.letterDef, base.Pawn, null, null, null, null);
		}
	}
}
