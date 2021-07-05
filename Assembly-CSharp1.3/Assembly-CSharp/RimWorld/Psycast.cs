using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D84 RID: 3460
	public class Psycast : Ability
	{
		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x06005025 RID: 20517 RVA: 0x001AC944 File Offset: 0x001AAB44
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
					return (this.pawn.GetPsylinkLevel() >= this.def.level || this.def.level <= 0) && !this.pawn.psychicEntropy.WouldOverflowEntropy(this.def.EntropyGain);
				}
				return this.def.PsyfocusCost <= this.pawn.psychicEntropy.CurrentPsyfocus + 0.0005f;
			}
		}

		// Token: 0x06005026 RID: 20518 RVA: 0x001A6002 File Offset: 0x001A4202
		public Psycast(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x06005027 RID: 20519 RVA: 0x001A600B File Offset: 0x001A420B
		public Psycast(Pawn pawn, AbilityDef def) : base(pawn, def)
		{
		}

		// Token: 0x06005028 RID: 20520 RVA: 0x001AC9DA File Offset: 0x001AABDA
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

		// Token: 0x06005029 RID: 20521 RVA: 0x001AC9EC File Offset: 0x001AABEC
		public override bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.CheckRoyalty("Psycast"))
			{
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
						FleckMaker.Static(target.Cell, this.pawn.Map, FleckDefOf.PsycastAreaEffect, this.def.EffectRadius);
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

		// Token: 0x0600502A RID: 20522 RVA: 0x001ACB6C File Offset: 0x001AAD6C
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

		// Token: 0x0600502B RID: 20523 RVA: 0x001ACBDC File Offset: 0x001AADDC
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

		// Token: 0x0600502C RID: 20524 RVA: 0x001ACC58 File Offset: 0x001AAE58
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
				if (pawn.Faction != null && pawn.Faction == Faction.OfMechanoids)
				{
					if (base.EffectComps.Any((CompAbilityEffect e) => !e.Props.applicableToMechs))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x001ACCF8 File Offset: 0x001AAEF8
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

		// Token: 0x0600502E RID: 20526 RVA: 0x001ACEC8 File Offset: 0x001AB0C8
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

		// Token: 0x04002FDF RID: 12255
		private Mote moteCast;

		// Token: 0x04002FE0 RID: 12256
		private static float MoteCastFadeTime = 0.4f;

		// Token: 0x04002FE1 RID: 12257
		private static float MoteCastScale = 1f;

		// Token: 0x04002FE2 RID: 12258
		private static Vector3 MoteCastOffset = new Vector3(0f, 0f, 0.48f);
	}
}
