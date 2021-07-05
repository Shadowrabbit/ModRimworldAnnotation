using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D1A RID: 3354
	public class Command_AbilitySpeech : Command_Ability
	{
		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x06004EC7 RID: 20167 RVA: 0x001A6480 File Offset: 0x001A4680
		protected Precept_Ritual Ritual
		{
			get
			{
				if (this.ritualCached == null)
				{
					CompAbilityEffect_StartRitual compAbilityEffect_StartRitual = this.ability.CompOfType<CompAbilityEffect_StartRitual>();
					if (compAbilityEffect_StartRitual != null)
					{
						this.ritualCached = compAbilityEffect_StartRitual.Ritual;
					}
				}
				return this.ritualCached;
			}
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x06004EC8 RID: 20168 RVA: 0x001A64B8 File Offset: 0x001A46B8
		public override string Tooltip
		{
			get
			{
				string text = this.Ritual.Label.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + this.Ritual.def.description.Formatted(this.ability.pawn.Named("ORGANIZER")).Resolve() + "\n";
				if (this.ability.CooldownTicksRemaining > 0)
				{
					text = string.Concat(new string[]
					{
						text,
						"\n",
						"AbilitySpeechCooldown".Translate().Resolve(),
						": ",
						this.ability.CooldownTicksRemaining.ToStringTicksToPeriod(true, false, true, true)
					});
				}
				if (this.Ritual.outcomeEffect != null)
				{
					text = text + "\n" + this.Ritual.outcomeEffect.ExtraAlertParagraph(this.Ritual);
				}
				text = string.Concat(new string[]
				{
					text,
					"\n\n",
					("AbilitySpeechTargetsLabel".Translate().Resolve() + ":").Colorize(ColoredText.TipSectionTitleColor),
					"\n",
					this.Ritual.targetFilter.GetTargetInfos(this.ability.pawn).ToLineList(" -  ", false)
				});
				return text.CapitalizeFirst();
			}
		}

		// Token: 0x06004EC9 RID: 20169 RVA: 0x001A6624 File Offset: 0x001A4824
		public Command_AbilitySpeech(Ability ability) : base(ability)
		{
			this.defaultLabel = "BeginRitual".Translate(this.Ritual.Label);
			this.icon = ((ability.def.iconPath == null) ? this.Ritual.Icon : ContentFinder<Texture2D>.Get(ability.def.iconPath, true));
		}

		// Token: 0x06004ECA RID: 20170 RVA: 0x001A6690 File Offset: 0x001A4890
		protected override void DisabledCheck()
		{
			base.DisabledCheck();
			if (!this.ability.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking))
			{
				this.disabled = true;
				this.disabledReason = "AbilitySpeechDisabledCantSpeak".Translate(this.ability.pawn);
			}
		}

		// Token: 0x04002F5D RID: 12125
		private Precept_Ritual ritualCached;
	}
}
