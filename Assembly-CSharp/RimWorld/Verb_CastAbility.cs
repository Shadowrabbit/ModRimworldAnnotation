using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013AD RID: 5037
	public class Verb_CastAbility : Verb
	{
		// Token: 0x170010E7 RID: 4327
		// (get) Token: 0x06006D41 RID: 27969 RVA: 0x0004A464 File Offset: 0x00048664
		public static Color RadiusHighlightColor
		{
			get
			{
				return new Color(0.3f, 0.8f, 1f);
			}
		}

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x06006D42 RID: 27970 RVA: 0x0004A47A File Offset: 0x0004867A
		public override string ReportLabel
		{
			get
			{
				return this.ability.def.label;
			}
		}

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x06006D43 RID: 27971 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x06006D44 RID: 27972 RVA: 0x00217E3C File Offset: 0x0021603C
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

		// Token: 0x06006D45 RID: 27973 RVA: 0x0004A48C File Offset: 0x0004868C
		protected override bool TryCastShot()
		{
			return this.ability.Activate(this.currentTarget, this.currentDestination);
		}

		// Token: 0x06006D46 RID: 27974 RVA: 0x00217E6C File Offset: 0x0021606C
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

		// Token: 0x06006D47 RID: 27975 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool IsApplicableTo(LocalTargetInfo target, bool showMessages = false)
		{
			return true;
		}

		// Token: 0x06006D48 RID: 27976 RVA: 0x00217EB0 File Offset: 0x002160B0
		public override bool ValidateTarget(LocalTargetInfo target)
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
			else if (!this.ability.pawn.CanReach(target, PathEndMode.Touch, this.ability.pawn.NormalMaxDanger(), false, TraverseMode.ByPawn))
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
			if (!this.IsApplicableTo(target, true))
			{
				return false;
			}
			for (int i = 0; i < this.ability.EffectComps.Count; i++)
			{
				if (!this.ability.EffectComps[i].Valid(target, true))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006D49 RID: 27977 RVA: 0x0004A4A5 File Offset: 0x000486A5
		public override bool CanHitTarget(LocalTargetInfo targ)
		{
			return this.verbProps.range <= 0f || base.CanHitTarget(targ);
		}

		// Token: 0x06006D4A RID: 27978 RVA: 0x0004A4C2 File Offset: 0x000486C2
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && this.IsApplicableTo(target, false) && this.ValidateTarget(target))
			{
				base.OnGUI(target);
			}
			else
			{
				GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
			}
			this.DrawAttachmentExtraLabel(target);
		}

		// Token: 0x06006D4B RID: 27979 RVA: 0x00218054 File Offset: 0x00216254
		protected void DrawAttachmentExtraLabel(LocalTargetInfo target)
		{
			foreach (CompAbilityEffect compAbilityEffect in this.ability.EffectComps)
			{
				string text = compAbilityEffect.ExtraLabel(target);
				if (!text.NullOrEmpty())
				{
					Widgets.MouseAttachedLabel(text);
					break;
				}
			}
		}

		// Token: 0x06006D4C RID: 27980 RVA: 0x0004A4FA File Offset: 0x000486FA
		public void DrawRadius()
		{
			if (this.ability.pawn.Spawned)
			{
				GenDraw.DrawRadiusRing(this.ability.pawn.Position, this.verbProps.range);
			}
		}

		// Token: 0x06006D4D RID: 27981 RVA: 0x002180BC File Offset: 0x002162BC
		public override bool TryStartCastOn(LocalTargetInfo castTarg, LocalTargetInfo destTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true)
		{
			bool flag = base.TryStartCastOn(castTarg, destTarg, surpriseAttack, canHitNonTargetPawns);
			Pawn pawn;
			if (flag && this.ability.def.stunTargetWhileCasting && this.ability.def.verbProperties.warmupTime > 0f && (pawn = (castTarg.Thing as Pawn)) != null && pawn != this.ability.pawn)
			{
				pawn.stances.stunner.StunFor_NewTmp(this.ability.def.verbProperties.warmupTime.SecondsToTicks(), this.ability.pawn, false, false);
			}
			return flag;
		}

		// Token: 0x06006D4E RID: 27982 RVA: 0x00218160 File Offset: 0x00216360
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

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x06006D4F RID: 27983 RVA: 0x0004A52E File Offset: 0x0004872E
		public override Texture2D UIIcon
		{
			get
			{
				return this.ability.def.uiIcon;
			}
		}

		// Token: 0x06006D50 RID: 27984 RVA: 0x0004A540 File Offset: 0x00048740
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Ability>(ref this.ability, "ability", false);
		}

		// Token: 0x04004847 RID: 18503
		public Ability ability;
	}
}
