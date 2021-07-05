using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011CB RID: 4555
	public class CompUsableImplant : CompUsable
	{
		// Token: 0x06006DF6 RID: 28150 RVA: 0x0024DD6C File Offset: 0x0024BF6C
		protected override string FloatMenuOptionLabel(Pawn pawn)
		{
			CompUseEffect_InstallImplant compUseEffect_InstallImplant = this.parent.TryGetComp<CompUseEffect_InstallImplant>();
			if (compUseEffect_InstallImplant != null)
			{
				Hediff_Level hediff_Level = compUseEffect_InstallImplant.GetExistingImplant(pawn) as Hediff_Level;
				if (hediff_Level != null && compUseEffect_InstallImplant.Props.canUpgrade && (float)hediff_Level.level < hediff_Level.def.maxSeverity)
				{
					return "UpgradeImplant".Translate(hediff_Level.def.label, hediff_Level.level + 1);
				}
			}
			return base.FloatMenuOptionLabel(pawn);
		}

		// Token: 0x06006DF7 RID: 28151 RVA: 0x0024DDF0 File Offset: 0x0024BFF0
		public override void TryStartUseJob(Pawn pawn, LocalTargetInfo extraTarget)
		{
			CompUseEffect_InstallImplant useEffectImplant = this.parent.TryGetComp<CompUseEffect_InstallImplant>();
			Hediff_Level hediff_Level = useEffectImplant.GetExistingImplant(pawn) as Hediff_Level;
			TaggedString text = CompRoyalImplant.CheckForViolations(pawn, useEffectImplant.Props.hediffDef, (hediff_Level != null && useEffectImplant.Props.canUpgrade) ? 1 : 0);
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

		// Token: 0x06006DF8 RID: 28152 RVA: 0x0024DEE0 File Offset: 0x0024C0E0
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
