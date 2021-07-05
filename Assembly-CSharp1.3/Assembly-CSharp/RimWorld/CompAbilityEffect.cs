using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D1F RID: 3359
	public abstract class CompAbilityEffect : AbilityComp
	{
		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x06004ED1 RID: 20177 RVA: 0x001A6A45 File Offset: 0x001A4C45
		public CompProperties_AbilityEffect Props
		{
			get
			{
				return (CompProperties_AbilityEffect)this.props;
			}
		}

		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06004ED2 RID: 20178 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool HideTargetPawnTooltip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x06004ED3 RID: 20179 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ShouldHideGizmo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x06004ED4 RID: 20180 RVA: 0x001A6A52 File Offset: 0x001A4C52
		protected bool SendLetter
		{
			get
			{
				return this.Props.sendLetter && !this.Props.customLetterText.NullOrEmpty() && !this.Props.customLetterLabel.NullOrEmpty();
			}
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x001A6A8C File Offset: 0x001A4C8C
		public virtual void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.Props.screenShakeIntensity > 1E-45f)
			{
				Find.CameraDriver.shaker.DoShake(this.Props.screenShakeIntensity);
			}
			Pawn pawn = this.parent.pawn;
			Pawn pawn2 = target.Pawn;
			if (pawn2 != null)
			{
				Faction homeFaction = pawn2.HomeFaction;
				if (this.Props.goodwillImpact != 0 && pawn.Faction == Faction.OfPlayer && homeFaction != null && !homeFaction.HostileTo(pawn.Faction) && (this.Props.applyGoodwillImpactToLodgers || !pawn2.IsQuestLodger()) && !pawn2.IsQuestHelper())
				{
					Faction.OfPlayer.TryAffectGoodwillWith(homeFaction, this.Props.goodwillImpact, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
				}
			}
			if (this.Props.clamorType != null)
			{
				GenClamor.DoClamor(this.parent.pawn, target.Cell, (float)this.Props.clamorRadius, this.Props.clamorType);
			}
			Gender gender = pawn.gender;
			SoundDef soundDef;
			if (gender != Gender.Male)
			{
				if (gender != Gender.Female)
				{
					soundDef = this.Props.sound;
				}
				else
				{
					soundDef = (this.Props.soundFemale ?? this.Props.sound);
				}
			}
			else
			{
				soundDef = (this.Props.soundMale ?? this.Props.sound);
			}
			if (soundDef != null)
			{
				soundDef.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}
			if (!this.Props.message.NullOrEmpty())
			{
				Messages.Message(this.Props.message, this.parent.pawn, this.Props.messageType ?? MessageTypeDefOf.SilentInput, true);
			}
		}

		// Token: 0x06004ED6 RID: 20182 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Apply(GlobalTargetInfo target)
		{
		}

		// Token: 0x06004ED7 RID: 20183 RVA: 0x001A6C5C File Offset: 0x001A4E5C
		public virtual bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Props.availableWhenTargetIsWounded || (target.Pawn.health.hediffSet.BleedRateTotal <= 0f && !target.Pawn.health.HasHediffsNeedingTend(false));
		}

		// Token: 0x06004ED8 RID: 20184 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanApplyOn(GlobalTargetInfo target)
		{
			return true;
		}

		// Token: 0x06004ED9 RID: 20185 RVA: 0x001A6CAA File Offset: 0x001A4EAA
		public virtual IEnumerable<PreCastAction> GetPreCastActions()
		{
			yield break;
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DrawEffectPreview(LocalTargetInfo target)
		{
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			return true;
		}

		// Token: 0x06004EDC RID: 20188 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool Valid(GlobalTargetInfo target, bool throwMessages = false)
		{
			return true;
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			return null;
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string WorldMapExtraLabel(GlobalTargetInfo target)
		{
			return null;
		}

		// Token: 0x06004EDF RID: 20191 RVA: 0x00002688 File Offset: 0x00000888
		public virtual Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
		{
			return null;
		}

		// Token: 0x06004EE0 RID: 20192 RVA: 0x00002688 File Offset: 0x00000888
		public virtual Window ConfirmationDialog(GlobalTargetInfo target, Action confirmAction)
		{
			return null;
		}

		// Token: 0x06004EE1 RID: 20193 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string ExtraTooltipPart()
		{
			return null;
		}

		// Token: 0x06004EE2 RID: 20194 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void OnGizmoUpdate()
		{
		}
	}
}
