using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013AE RID: 5038
	public class Verb_CastPsycast : Verb_CastAbility
	{
		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x06006D52 RID: 27986 RVA: 0x0004A559 File Offset: 0x00048759
		public Psycast Psycast
		{
			get
			{
				return this.ability as Psycast;
			}
		}

		// Token: 0x06006D53 RID: 27987 RVA: 0x00218200 File Offset: 0x00216400
		public override bool IsApplicableTo(LocalTargetInfo target, bool showMessages = false)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Psycasts are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 324345647, false);
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

		// Token: 0x06006D54 RID: 27988 RVA: 0x0004A566 File Offset: 0x00048766
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			if (!this.IsApplicableTo(target, false))
			{
				return;
			}
			base.OrderForceTarget(target);
		}

		// Token: 0x06006D55 RID: 27989 RVA: 0x002182B0 File Offset: 0x002164B0
		public override bool ValidateTarget(LocalTargetInfo target)
		{
			if (!base.ValidateTarget(target))
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

		// Token: 0x06006D56 RID: 27990 RVA: 0x00218438 File Offset: 0x00216638
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
			if (ThingRequiringRoyalPermissionUtility.IsViolatingRulesOfAnyFaction_NewTemp(HediffDefOf.PsychicAmplifier, this.CasterPawn, this.Psycast.def.level, true) && this.Psycast.def.DetectionChance > 0f)
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

		// Token: 0x06006D57 RID: 27991 RVA: 0x00218654 File Offset: 0x00216854
		private void DrawIneffectiveWarning(LocalTargetInfo target)
		{
			if (target.Pawn != null)
			{
				Vector3 drawPos = target.Pawn.DrawPos;
				drawPos.z += 1f;
				GenMapUI.DrawText(new Vector2(drawPos.x, drawPos.z), "Ineffective".Translate(), Color.red);
			}
		}

		// Token: 0x06006D58 RID: 27992 RVA: 0x002186B4 File Offset: 0x002168B4
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

		// Token: 0x04004848 RID: 18504
		private const float StatLabelOffsetY = 1f;
	}
}
