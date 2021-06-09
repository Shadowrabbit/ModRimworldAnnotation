using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003BD RID: 957
	public class HediffComp_CauseMentalState : HediffComp
	{
		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x060017DA RID: 6106 RVA: 0x00016B82 File Offset: 0x00014D82
		public HediffCompProperties_CauseMentalState Props
		{
			get
			{
				return (HediffCompProperties_CauseMentalState)this.props;
			}
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x000DD90C File Offset: 0x000DBB0C
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (base.Pawn.IsHashIntervalTick(60))
			{
				if (base.Pawn.RaceProps.Humanlike)
				{
					if (base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.humanMentalState && Rand.MTBEventOccurs(this.Props.mtbDaysToCauseMentalState, 60000f, 60f) && base.Pawn.Awake() && base.Pawn.mindState.mentalStateHandler.TryStartMentalState(this.Props.humanMentalState, this.parent.def.LabelCap, false, false, null, true) && base.Pawn.Spawned)
					{
						this.SendLetter(this.Props.humanMentalState);
						return;
					}
				}
				else if (base.Pawn.RaceProps.Animal && base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.animalMentalState && (this.Props.animalMentalStateAlias == null || base.Pawn.mindState.mentalStateHandler.CurStateDef != this.Props.animalMentalStateAlias) && Rand.MTBEventOccurs(this.Props.mtbDaysToCauseMentalState, 60000f, 60f) && base.Pawn.Awake() && base.Pawn.mindState.mentalStateHandler.TryStartMentalState(this.Props.animalMentalState, this.parent.def.LabelCap, false, false, null, true) && base.Pawn.Spawned)
				{
					this.SendLetter(this.Props.animalMentalState);
				}
			}
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x000DDAE4 File Offset: 0x000DBCE4
		public override void CompPostPostRemoved()
		{
			if (this.Props.endMentalStateOnCure && ((base.Pawn.RaceProps.Humanlike && base.Pawn.mindState.mentalStateHandler.CurStateDef == this.Props.humanMentalState) || (base.Pawn.RaceProps.Animal && (base.Pawn.mindState.mentalStateHandler.CurStateDef == this.Props.animalMentalState || base.Pawn.mindState.mentalStateHandler.CurStateDef == this.Props.animalMentalStateAlias))) && !base.Pawn.mindState.mentalStateHandler.CurState.causedByMood)
			{
				base.Pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
			}
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x000DDBC4 File Offset: 0x000DBDC4
		private void SendLetter(MentalStateDef mentalStateDef)
		{
			Find.LetterStack.ReceiveLetter((mentalStateDef.beginLetterLabel ?? mentalStateDef.LabelCap).CapitalizeFirst() + ": " + base.Pawn.LabelShortCap, base.Pawn.mindState.mentalStateHandler.CurState.GetBeginLetterText() + "\n\n" + "CausedByHediff".Translate(this.parent.LabelCap), this.Props.letterDef, base.Pawn, null, null, null, null);
		}
	}
}
