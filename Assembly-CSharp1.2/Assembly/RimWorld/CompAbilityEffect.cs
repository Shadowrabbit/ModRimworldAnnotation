using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001367 RID: 4967
	public abstract class CompAbilityEffect : AbilityComp
	{
		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x06006C02 RID: 27650 RVA: 0x0004987C File Offset: 0x00047A7C
		public CompProperties_AbilityEffect Props
		{
			get
			{
				return (CompProperties_AbilityEffect)this.props;
			}
		}

		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x06006C03 RID: 27651 RVA: 0x00049889 File Offset: 0x00047A89
		protected bool SendLetter
		{
			get
			{
				return this.Props.sendLetter && !this.Props.customLetterText.NullOrEmpty() && !this.Props.customLetterLabel.NullOrEmpty();
			}
		}

		// Token: 0x06006C04 RID: 27652 RVA: 0x00213960 File Offset: 0x00211B60
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
				Faction factionOrExtraMiniOrHomeFaction = pawn2.FactionOrExtraMiniOrHomeFaction;
				if (this.Props.goodwillImpact != 0 && pawn.Faction != null && factionOrExtraMiniOrHomeFaction != null && !factionOrExtraMiniOrHomeFaction.HostileTo(pawn.Faction) && (this.Props.applyGoodwillImpactToLodgers || !pawn2.IsQuestLodger()) && !pawn2.IsQuestHelper())
				{
					factionOrExtraMiniOrHomeFaction.TryAffectGoodwillWith(pawn.Faction, this.Props.goodwillImpact, true, true, "GoodwillChangedReason_UsedAbility".Translate(this.parent.def.LabelCap, pawn2.LabelShort), new GlobalTargetInfo?(pawn2));
				}
			}
			if (this.Props.clamorType != null)
			{
				GenClamor.DoClamor(this.parent.pawn, target.Cell, (float)this.Props.clamorRadius, this.Props.clamorType);
			}
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}
			if (!this.Props.message.NullOrEmpty())
			{
				Messages.Message(this.Props.message, this.parent.pawn, this.Props.messageType ?? MessageTypeDefOf.SilentInput, true);
			}
		}

		// Token: 0x06006C05 RID: 27653 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Apply(GlobalTargetInfo target)
		{
		}

		// Token: 0x06006C06 RID: 27654 RVA: 0x00213B18 File Offset: 0x00211D18
		public virtual bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Props.availableWhenTargetIsWounded || (target.Pawn.health.hediffSet.BleedRateTotal <= 0f && !target.Pawn.health.HasHediffsNeedingTend(false));
		}

		// Token: 0x06006C07 RID: 27655 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool CanApplyOn(GlobalTargetInfo target)
		{
			return true;
		}

		// Token: 0x06006C08 RID: 27656 RVA: 0x000498C1 File Offset: 0x00047AC1
		public virtual IEnumerable<PreCastAction> GetPreCastActions()
		{
			yield break;
		}

		// Token: 0x06006C09 RID: 27657 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void DrawEffectPreview(LocalTargetInfo target)
		{
		}

		// Token: 0x06006C0A RID: 27658 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			return true;
		}

		// Token: 0x06006C0B RID: 27659 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool Valid(GlobalTargetInfo target, bool throwMessages = false)
		{
			return true;
		}

		// Token: 0x06006C0C RID: 27660 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string ExtraLabel(LocalTargetInfo target)
		{
			return null;
		}

		// Token: 0x06006C0D RID: 27661 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string WorldMapExtraLabel(GlobalTargetInfo target)
		{
			return null;
		}

		// Token: 0x06006C0E RID: 27662 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string ConfirmationDialogText(LocalTargetInfo target)
		{
			return null;
		}

		// Token: 0x06006C0F RID: 27663 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string ConfirmationDialogText(GlobalTargetInfo target)
		{
			return null;
		}
	}
}
