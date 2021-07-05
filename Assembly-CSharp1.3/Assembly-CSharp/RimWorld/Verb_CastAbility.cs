using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D86 RID: 3462
	public class Verb_CastAbility : Verb
	{
		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x06005033 RID: 20531 RVA: 0x001AD0DC File Offset: 0x001AB2DC
		public static Color RadiusHighlightColor
		{
			get
			{
				return new Color(0.3f, 0.8f, 1f);
			}
		}

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x06005034 RID: 20532 RVA: 0x001AD0F2 File Offset: 0x001AB2F2
		public override string ReportLabel
		{
			get
			{
				return this.ability.def.label;
			}
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x06005035 RID: 20533 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06005036 RID: 20534 RVA: 0x001AD104 File Offset: 0x001AB304
		public override bool HidePawnTooltips
		{
			get
			{
				using (List<CompAbilityEffect>.Enumerator enumerator = this.ability.EffectComps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HideTargetPawnTooltip)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06005037 RID: 20535 RVA: 0x001AD164 File Offset: 0x001AB364
		public override ITargetingSource DestinationSelector
		{
			get
			{
				CompAbilityEffect_WithDest compAbilityEffect_WithDest = this.ability.CompOfType<CompAbilityEffect_WithDest>();
				if (compAbilityEffect_WithDest != null && compAbilityEffect_WithDest.Props.destination == AbilityEffectDestination.Selected)
				{
					return compAbilityEffect_WithDest;
				}
				return null;
			}
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x001AD191 File Offset: 0x001AB391
		protected override bool TryCastShot()
		{
			return this.ability.Activate(this.currentTarget, this.currentDestination);
		}

		// Token: 0x06005039 RID: 20537 RVA: 0x001AD1AC File Offset: 0x001AB3AC
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			CompAbilityEffect_WithDest compAbilityEffect_WithDest = this.ability.CompOfType<CompAbilityEffect_WithDest>();
			if (compAbilityEffect_WithDest != null && compAbilityEffect_WithDest.Props.destination == AbilityEffectDestination.Selected)
			{
				compAbilityEffect_WithDest.SetTarget(target);
				return;
			}
			this.ability.QueueCastingJob(target, null);
		}

		// Token: 0x0600503A RID: 20538 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool IsApplicableTo(LocalTargetInfo target, bool showMessages = false)
		{
			return true;
		}

		// Token: 0x0600503B RID: 20539 RVA: 0x001AD1F0 File Offset: 0x001AB3F0
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (this.verbProps.range > 0f)
			{
				if (!this.CanHitTarget(target))
				{
					if (target.IsValid)
					{
						Messages.Message(this.ability.def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), new LookTargets(new TargetInfo[]
						{
							this.ability.pawn,
							target.ToTargetInfo(this.ability.pawn.Map)
						}), MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
			}
			else if (!this.ability.pawn.CanReach(target, PathEndMode.Touch, this.ability.pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
			{
				if (target.IsValid)
				{
					Messages.Message(this.ability.def.LabelCap + ": " + "AbilityCannotReachTarget".Translate(), new LookTargets(new TargetInfo[]
					{
						this.ability.pawn,
						target.ToTargetInfo(this.ability.pawn.Map)
					}), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!this.IsApplicableTo(target, showMessages))
			{
				return false;
			}
			for (int i = 0; i < this.ability.EffectComps.Count; i++)
			{
				if (!this.ability.EffectComps[i].Valid(target, showMessages))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600503C RID: 20540 RVA: 0x001AD394 File Offset: 0x001AB594
		public override bool CanHitTarget(LocalTargetInfo targ)
		{
			return this.verbProps.range <= 0f || base.CanHitTarget(targ);
		}

		// Token: 0x0600503D RID: 20541 RVA: 0x001AD3B1 File Offset: 0x001AB5B1
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && this.IsApplicableTo(target, false) && this.ValidateTarget(target, false))
			{
				base.OnGUI(target);
			}
			else
			{
				GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
			}
			this.DrawAttachmentExtraLabel(target);
		}

		// Token: 0x0600503E RID: 20542 RVA: 0x001AD3EC File Offset: 0x001AB5EC
		protected void DrawAttachmentExtraLabel(LocalTargetInfo target)
		{
			foreach (CompAbilityEffect compAbilityEffect in this.ability.EffectComps)
			{
				string text = compAbilityEffect.ExtraLabelMouseAttachment(target);
				if (!text.NullOrEmpty())
				{
					Widgets.MouseAttachedLabel(text, 0f, 0f);
					break;
				}
			}
		}

		// Token: 0x0600503F RID: 20543 RVA: 0x001AD460 File Offset: 0x001AB660
		public void DrawRadius()
		{
			if (this.ability.pawn.Spawned)
			{
				GenDraw.DrawRadiusRing(this.ability.pawn.Position, this.verbProps.range);
			}
		}

		// Token: 0x06005040 RID: 20544 RVA: 0x001AD494 File Offset: 0x001AB694
		public override bool TryStartCastOn(LocalTargetInfo castTarg, LocalTargetInfo destTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false)
		{
			bool flag = base.TryStartCastOn(castTarg, destTarg, surpriseAttack, canHitNonTargetPawns, preventFriendlyFire);
			Pawn pawn;
			if (flag && this.ability.def.stunTargetWhileCasting && this.ability.def.verbProperties.warmupTime > 0f && (pawn = (castTarg.Thing as Pawn)) != null && pawn != this.ability.pawn)
			{
				pawn.stances.stunner.StunFor(this.ability.def.verbProperties.warmupTime.SecondsToTicks(), this.ability.pawn, false, false);
			}
			return flag;
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x001AD538 File Offset: 0x001AB738
		public override void DrawHighlight(LocalTargetInfo target)
		{
			AbilityDef def = this.ability.def;
			if (this.verbProps.range > 0f)
			{
				this.DrawRadius();
			}
			if (this.CanHitTarget(target) && this.IsApplicableTo(target, false))
			{
				if (def.HasAreaOfEffect)
				{
					if (target.IsValid)
					{
						GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
						GenDraw.DrawRadiusRing(target.Cell, def.EffectRadius, Verb_CastAbility.RadiusHighlightColor, null);
					}
				}
				else
				{
					GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
				}
			}
			if (target.IsValid)
			{
				this.ability.DrawEffectPreviews(target);
			}
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x06005042 RID: 20546 RVA: 0x001AD5D8 File Offset: 0x001AB7D8
		public override Texture2D UIIcon
		{
			get
			{
				return this.ability.def.uiIcon;
			}
		}

		// Token: 0x06005043 RID: 20547 RVA: 0x001AD5EA File Offset: 0x001AB7EA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Ability>(ref this.ability, "ability", false);
		}

		// Token: 0x04002FE3 RID: 12259
		public Ability ability;
	}
}
