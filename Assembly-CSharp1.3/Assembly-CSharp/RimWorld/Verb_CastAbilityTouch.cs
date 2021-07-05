using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D87 RID: 3463
	public class Verb_CastAbilityTouch : Verb_CastAbility
	{
		// Token: 0x06005045 RID: 20549 RVA: 0x001AD603 File Offset: 0x001AB803
		public override void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid && this.IsApplicableTo(target, false))
			{
				GenDraw.DrawTargetHighlight(target);
				this.ability.DrawEffectPreviews(target);
			}
		}

		// Token: 0x06005046 RID: 20550 RVA: 0x001AD62A File Offset: 0x001AB82A
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.ValidateTarget(target, false))
			{
				GenUI.DrawMouseAttachment(this.UIIcon);
			}
			else
			{
				GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
			}
			base.DrawAttachmentExtraLabel(target);
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x001AD654 File Offset: 0x001AB854
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
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
	}
}
