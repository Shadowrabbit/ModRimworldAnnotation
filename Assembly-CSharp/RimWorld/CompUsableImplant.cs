using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001892 RID: 6290
	public class CompUsableImplant : CompUsable
	{
		// Token: 0x06008BA7 RID: 35751 RVA: 0x0028AAD4 File Offset: 0x00288CD4
		protected override string FloatMenuOptionLabel(Pawn pawn)
		{
			CompUseEffect_InstallImplant compUseEffect_InstallImplant = this.parent.TryGetComp<CompUseEffect_InstallImplant>();
			if (compUseEffect_InstallImplant != null)
			{
				Hediff_ImplantWithLevel hediff_ImplantWithLevel = compUseEffect_InstallImplant.GetExistingImplant(pawn) as Hediff_ImplantWithLevel;
				if (hediff_ImplantWithLevel != null && compUseEffect_InstallImplant.Props.canUpgrade && (float)hediff_ImplantWithLevel.level < hediff_ImplantWithLevel.def.maxSeverity)
				{
					return "UpgradeImplant".Translate(hediff_ImplantWithLevel.def.label, hediff_ImplantWithLevel.level + 1);
				}
			}
			return base.FloatMenuOptionLabel(pawn);
		}

		// Token: 0x06008BA8 RID: 35752 RVA: 0x0028AB58 File Offset: 0x00288D58
		public override void TryStartUseJob(Pawn pawn, LocalTargetInfo extraTarget)
		{
			CompUseEffect_InstallImplant useEffectImplant = this.parent.TryGetComp<CompUseEffect_InstallImplant>();
			Hediff_ImplantWithLevel hediff_ImplantWithLevel = useEffectImplant.GetExistingImplant(pawn) as Hediff_ImplantWithLevel;
			TaggedString text = CompRoyalImplant.CheckForViolations(pawn, useEffectImplant.Props.hediffDef, (hediff_ImplantWithLevel != null && useEffectImplant.Props.canUpgrade) ? 1 : 0);
			if (!text.NullOrEmpty())
			{
				Find.WindowStack.Add(new Dialog_MessageBox(text, "Yes".Translate(), delegate()
				{
					this.UseJobInternal(pawn, extraTarget, useEffectImplant.Props.hediffDef);
				}, "No".Translate(), null, null, false, null, null));
				return;
			}
			this.UseJobInternal(pawn, extraTarget, useEffectImplant.Props.hediffDef);
		}

		// Token: 0x06008BA9 RID: 35753 RVA: 0x0028AC48 File Offset: 0x00288E48
		private void UseJobInternal(Pawn pawn, LocalTargetInfo extraTarget, HediffDef hediff)
		{
			base.TryStartUseJob(pawn, extraTarget);
			if (hediff == HediffDefOf.PsychicAmplifier && pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) < 1E-45f)
			{
				Messages.Message("MessagePsylinkNoSensitivity".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}
