using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020013A9 RID: 5033
	public class Psycast : Ability
	{
		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x06006D25 RID: 27941 RVA: 0x002175F8 File Offset: 0x002157F8
		public override bool CanCast
		{
			get
			{
				if (!base.CanCast)
				{
					return false;
				}
				if (this.def.EntropyGain > 1E-45f)
				{
					Hediff hediff = this.pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff h) => h.def == HediffDefOf.PsychicAmplifier);
					return ((hediff != null && hediff.Severity >= (float)this.def.level) || this.def.level <= 0) && !this.pawn.psychicEntropy.WouldOverflowEntropy(this.def.EntropyGain);
				}
				return this.def.PsyfocusCost <= this.pawn.psychicEntropy.CurrentPsyfocus + 0.0005f;
			}
		}

		// Token: 0x06006D26 RID: 27942 RVA: 0x0004A3BF File Offset: 0x000485BF
		public Psycast(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x06006D27 RID: 27943 RVA: 0x0004A3C8 File Offset: 0x000485C8
		public Psycast(Pawn pawn, AbilityDef def) : base(pawn, def)
		{
		}

		// Token: 0x06006D28 RID: 27944 RVA: 0x0004A3D2 File Offset: 0x000485D2
		public override IEnumerable<Command> GetGizmos()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				yield break;
			}
			if (this.gizmo == null)
			{
				this.gizmo = new Command_Psycast(this);
			}
			yield return this.gizmo;
			yield break;
		}

		// Token: 0x06006D29 RID: 27945 RVA: 0x002176CC File Offset: 0x002158CC
		public override bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Psycasts are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 324345643, false);
				return false;
			}
			if (this.def.EntropyGain > 1E-45f && !this.pawn.psychicEntropy.TryAddEntropy(this.def.EntropyGain, null, true, false))
			{
				return false;
			}
			float num = base.FinalPsyfocusCost(target);
			if (num > 1E-45f)
			{
				this.pawn.psychicEntropy.OffsetPsyfocusDirectly(-num);
			}
			if (this.def.showPsycastEffects)
			{
				if (base.EffectComps.Any((CompAbilityEffect c) => c.Props.psychic))
				{
					if (this.def.HasAreaOfEffect)
					{
						MoteMaker.MakeStaticMote(target.Cell, this.pawn.Map, ThingDefOf.Mote_PsycastAreaEffect, this.def.EffectRadius);
						SoundDefOf.PsycastPsychicPulse.PlayOneShot(new TargetInfo(target.Cell, this.pawn.Map, false));
					}
					else
					{
						SoundDefOf.PsycastPsychicEffect.PlayOneShot(new TargetInfo(target.Cell, this.pawn.Map, false));
					}
				}
				else if (this.def.HasAreaOfEffect && this.def.canUseAoeToGetTargets)
				{
					SoundDefOf.Psycast_Skip_Pulse.PlayOneShot(new TargetInfo(target.Cell, this.pawn.Map, false));
				}
			}
			return base.Activate(target, dest);
		}

		// Token: 0x06006D2A RID: 27946 RVA: 0x00217858 File Offset: 0x00215A58
		public override bool Activate(GlobalTargetInfo target)
		{
			if (this.def.EntropyGain > 1E-45f && !this.pawn.psychicEntropy.TryAddEntropy(this.def.EntropyGain, null, true, false))
			{
				return false;
			}
			float psyfocusCost = this.def.PsyfocusCost;
			if (psyfocusCost > 1E-45f)
			{
				this.pawn.psychicEntropy.OffsetPsyfocusDirectly(-psyfocusCost);
			}
			return base.Activate(target);
		}

		// Token: 0x06006D2B RID: 27947 RVA: 0x002178C8 File Offset: 0x00215AC8
		protected override void ApplyEffects(IEnumerable<CompAbilityEffect> effects, LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.CanApplyPsycastTo(target))
			{
				using (IEnumerator<CompAbilityEffect> enumerator = effects.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CompAbilityEffect compAbilityEffect = enumerator.Current;
						compAbilityEffect.Apply(target, dest);
					}
					return;
				}
			}
			MoteMaker.ThrowText(target.CenterVector3, this.pawn.Map, "TextMote_Immune".Translate(), -1f);
		}

		// Token: 0x06006D2C RID: 27948 RVA: 0x00217944 File Offset: 0x00215B44
		public bool CanApplyPsycastTo(LocalTargetInfo target)
		{
			if (!base.EffectComps.Any((CompAbilityEffect e) => e.Props.psychic))
			{
				return true;
			}
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) < 1E-45f)
				{
					return false;
				}
				if (pawn.Faction == Faction.OfMechanoids)
				{
					if (base.EffectComps.Any((CompAbilityEffect e) => !e.Props.applicableToMechs))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06006D2D RID: 27949 RVA: 0x002179DC File Offset: 0x00215BDC
		public override bool GizmoDisabled(out string reason)
		{
			if (this.pawn.psychicEntropy.PsychicSensitivity < 1E-45f)
			{
				reason = "CommandPsycastZeroPsychicSensitivity".Translate();
				return true;
			}
			float num = PsycastUtility.TotalPsyfocusCostOfQueuedPsycasts(this.pawn);
			if (this.def.level > 0 && this.pawn.GetPsylinkLevel() < this.def.level)
			{
				reason = "CommandPsycastHigherLevelPsylinkRequired".Translate(this.def.level);
				return true;
			}
			if (this.def.PsyfocusCost + num > this.pawn.psychicEntropy.CurrentPsyfocus + 0.0005f)
			{
				reason = "CommandPsycastNotEnoughPsyfocus".Translate(this.def.PsyfocusCostPercent, (this.pawn.psychicEntropy.CurrentPsyfocus - num).ToStringPercent("0.#"), this.def.label.Named("PSYCASTNAME"), this.pawn.Named("CASTERNAME"));
				return true;
			}
			if (this.def.level > this.pawn.psychicEntropy.MaxAbilityLevel)
			{
				reason = "CommandPsycastLowPsyfocus".Translate(Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[this.def.RequiredPsyfocusBand].ToStringPercent());
				return true;
			}
			if (this.def.EntropyGain > 1E-45f && this.pawn.psychicEntropy.WouldOverflowEntropy(this.def.EntropyGain + PsycastUtility.TotalEntropyFromQueuedPsycasts(this.pawn)))
			{
				reason = "CommandPsycastWouldExceedEntropy".Translate(this.def.label);
				return true;
			}
			return base.GizmoDisabled(out reason);
		}

		// Token: 0x06006D2E RID: 27950 RVA: 0x00217BAC File Offset: 0x00215DAC
		public override void AbilityTick()
		{
			base.AbilityTick();
			if (this.pawn.Spawned && base.Casting)
			{
				if (this.moteCast == null || this.moteCast.Destroyed)
				{
					this.moteCast = MoteMaker.MakeAttachedOverlay(this.pawn, ThingDefOf.Mote_CastPsycast, Psycast.MoteCastOffset, Psycast.MoteCastScale, base.verb.verbProps.warmupTime - Psycast.MoteCastFadeTime);
					return;
				}
				this.moteCast.Maintain();
			}
		}

		// Token: 0x0400483A RID: 18490
		private Mote moteCast;

		// Token: 0x0400483B RID: 18491
		private static float MoteCastFadeTime = 0.4f;

		// Token: 0x0400483C RID: 18492
		private static float MoteCastScale = 1f;

		// Token: 0x0400483D RID: 18493
		private static Vector3 MoteCastOffset = new Vector3(0f, 0f, 0.48f);
	}
}
