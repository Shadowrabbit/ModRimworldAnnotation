using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001362 RID: 4962
	public class Command_AbilitySpeech : Command_Ability
	{
		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x06006BF8 RID: 27640 RVA: 0x0021356C File Offset: 0x0021176C
		public override string Tooltip
		{
			get
			{
				TaggedString taggedString = this.ability.def.LabelCap + "\n\n" + "AbilitySpeechTooltip".Translate(this.ability.pawn.Named("ORGANIZER")) + "\n";
				if (this.ability.CooldownTicksRemaining > 0)
				{
					taggedString += "\n" + "AbilitySpeechCooldown".Translate() + ": " + this.ability.CooldownTicksRemaining.ToStringTicksToPeriod(true, false, true, true);
				}
				taggedString += "\n" + GatheringWorker_Speech.OutcomeBreakdownForPawn(this.ability.pawn);
				return taggedString;
			}
		}

		// Token: 0x06006BF9 RID: 27641 RVA: 0x00049819 File Offset: 0x00047A19
		public Command_AbilitySpeech(Ability ability) : base(ability)
		{
		}
	}
}
