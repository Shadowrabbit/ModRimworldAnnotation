using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D88 RID: 3464
	public class Verb_CastPsycast : Verb_CastAbility
	{
		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x06005049 RID: 20553 RVA: 0x001AD6AD File Offset: 0x001AB8AD
		public Psycast Psycast
		{
			get
			{
				return this.ability as Psycast;
			}
		}

		// Token: 0x0600504A RID: 20554 RVA: 0x001AD6BC File Offset: 0x001AB8BC
		public override bool IsApplicableTo(LocalTargetInfo target, bool showMessages = false)
		{
			if (!ModLister.CheckRoyalty("Psycast"))
			{
				return false;
			}
			if (!base.IsApplicableTo(target, showMessages))
			{
				return false;
			}
			if (!this.Psycast.def.HasAreaOfEffect && !this.Psycast.CanApplyPsycastTo(target))
			{
				if (showMessages)
				{
					Messages.Message(this.ability.def.LabelCap + ": " + "AbilityTargetPsychicallyDeaf".Translate(), target.ToTargetInfo(this.ability.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x001AD75E File Offset: 0x001AB95E
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			if (!this.IsApplicableTo(target, false))
			{
				return;
			}
			base.OrderForceTarget(target);
		}

		// Token: 0x0600504C RID: 20556 RVA: 0x001AD774 File Offset: 0x001AB974
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!base.ValidateTarget(target, showMessages))
			{
				return false;
			}
			if (this.CasterPawn.psychicEntropy.PsychicSensitivity < 1E-45f)
			{
				Messages.Message("CommandPsycastZeroPsychicSensitivity".Translate(), this.caster, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (this.Psycast.def.EntropyGain > 1E-45f && this.CasterPawn.psychicEntropy.WouldOverflowEntropy(this.Psycast.def.EntropyGain + PsycastUtility.TotalEntropyFromQueuedPsycasts(this.CasterPawn)))
			{
				Messages.Message("CommandPsycastWouldExceedEntropy".Translate(), this.caster, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			float num = this.Psycast.FinalPsyfocusCost(target);
			float num2 = PsycastUtility.TotalPsyfocusCostOfQueuedPsycasts(this.CasterPawn);
			float num3 = num + num2;
			if (num > 1E-45f && num3 > this.CasterPawn.psychicEntropy.CurrentPsyfocus + 0.0005f)
			{
				Messages.Message("CommandPsycastNotEnoughPsyfocus".Translate(num3.ToStringPercent("0.#"), (this.CasterPawn.psychicEntropy.CurrentPsyfocus - num2).ToStringPercent("0.#"), this.Psycast.def.label.Named("PSYCASTNAME"), this.caster.Named("CASTERNAME")), this.caster, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			return true;
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x001AD8FC File Offset: 0x001ABAFC
		public override void OnGUI(LocalTargetInfo target)
		{
			bool flag = this.ability.EffectComps.Any((CompAbilityEffect e) => e.Props.psychic);
			Texture2D texture2D = this.UIIcon;
			if (!this.Psycast.CanApplyPsycastTo(target))
			{
				texture2D = TexCommand.CannotShoot;
				this.DrawIneffectiveWarning(target);
			}
			if (target.IsValid && this.CanHitTarget(target) && this.IsApplicableTo(target, false))
			{
				if (flag)
				{
					foreach (LocalTargetInfo target2 in this.ability.GetAffectedTargets(target))
					{
						if (this.Psycast.CanApplyPsycastTo(target2))
						{
							this.DrawSensitivityStat(target2);
						}
						else
						{
							this.DrawIneffectiveWarning(target2);
						}
					}
				}
				if (this.ability.EffectComps.Any((CompAbilityEffect e) => !e.Valid(target, false)))
				{
					texture2D = TexCommand.CannotShoot;
				}
			}
			else
			{
				texture2D = TexCommand.CannotShoot;
			}
			if (ThingRequiringRoyalPermissionUtility.IsViolatingRulesOfAnyFaction(HediffDefOf.PsychicAmplifier, this.CasterPawn, this.Psycast.def.level) && this.Psycast.def.DetectionChance > 0f)
			{
				TaggedString taggedString = "Illegal".Translate().ToUpper() + "\n" + this.Psycast.def.DetectionChance.ToStringPercent() + " " + "DetectionChance".Translate();
				Text.Font = GameFont.Small;
				GenUI.DrawMouseAttachment(texture2D, taggedString, 0f, default(Vector2), null, true, new Color(0.25f, 0f, 0f));
			}
			else
			{
				GenUI.DrawMouseAttachment(texture2D);
			}
			base.DrawAttachmentExtraLabel(target);
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x001ADB14 File Offset: 0x001ABD14
		private void DrawIneffectiveWarning(LocalTargetInfo target)
		{
			if (target.Pawn != null)
			{
				Vector3 drawPos = target.Pawn.DrawPos;
				drawPos.z += 1f;
				GenMapUI.DrawText(new Vector2(drawPos.x, drawPos.z), "Ineffective".Translate(), Color.red);
			}
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x001ADB74 File Offset: 0x001ABD74
		private void DrawSensitivityStat(LocalTargetInfo target)
		{
			if (target.Pawn != null)
			{
				Pawn pawn = target.Pawn;
				float statValue = pawn.GetStatValue(StatDefOf.PsychicSensitivity, true);
				Vector3 drawPos = pawn.DrawPos;
				drawPos.z += 1f;
				GenMapUI.DrawText(new Vector2(drawPos.x, drawPos.z), StatDefOf.PsychicSensitivity.LabelCap + ": " + statValue, (statValue > float.Epsilon) ? Color.white : Color.red);
			}
		}

		// Token: 0x04002FE4 RID: 12260
		private const float StatLabelOffsetY = 1f;
	}
}
