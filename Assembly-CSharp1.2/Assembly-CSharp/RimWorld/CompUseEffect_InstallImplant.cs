using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018DB RID: 6363
	public class CompUseEffect_InstallImplant : CompUseEffect
	{
		// Token: 0x17001626 RID: 5670
		// (get) Token: 0x06008CF7 RID: 36087 RVA: 0x0005E80D File Offset: 0x0005CA0D
		public CompProperties_UseEffectInstallImplant Props
		{
			get
			{
				return (CompProperties_UseEffectInstallImplant)this.props;
			}
		}

		// Token: 0x06008CF8 RID: 36088 RVA: 0x0028E4B0 File Offset: 0x0028C6B0
		public override void DoEffect(Pawn user)
		{
			BodyPartRecord bodyPartRecord = user.RaceProps.body.GetPartsWithDef(this.Props.bodyPart).FirstOrFallback(null);
			if (bodyPartRecord == null)
			{
				return;
			}
			Hediff firstHediffOfDef = user.health.hediffSet.GetFirstHediffOfDef(this.Props.hediffDef, false);
			if (firstHediffOfDef == null)
			{
				user.health.AddHediff(this.Props.hediffDef, bodyPartRecord, null, null);
				return;
			}
			if (this.Props.canUpgrade)
			{
				((Hediff_ImplantWithLevel)firstHediffOfDef).ChangeLevel(1);
			}
		}

		// Token: 0x06008CF9 RID: 36089 RVA: 0x0028E540 File Offset: 0x0028C740
		public override bool CanBeUsedBy(Pawn p, out string failReason)
		{
			if ((!p.IsFreeColonist || p.HasExtraHomeFaction(null)) && !this.Props.allowNonColonists)
			{
				failReason = "InstallImplantNotAllowedForNonColonists".Translate();
				return false;
			}
			if (p.RaceProps.body.GetPartsWithDef(this.Props.bodyPart).FirstOrFallback(null) == null)
			{
				failReason = "InstallImplantNoBodyPart".Translate() + ": " + this.Props.bodyPart.LabelShort;
				return false;
			}
			Hediff existingImplant = this.GetExistingImplant(p);
			if (existingImplant != null)
			{
				if (!this.Props.canUpgrade)
				{
					failReason = "InstallImplantAlreadyInstalled".Translate();
					return false;
				}
				Hediff_ImplantWithLevel hediff_ImplantWithLevel = (Hediff_ImplantWithLevel)existingImplant;
				if ((float)hediff_ImplantWithLevel.level >= hediff_ImplantWithLevel.def.maxSeverity)
				{
					failReason = "InstallImplantAlreadyMaxLevel".Translate();
					return false;
				}
			}
			failReason = null;
			return true;
		}

		// Token: 0x06008CFA RID: 36090 RVA: 0x0028E630 File Offset: 0x0028C830
		public Hediff GetExistingImplant(Pawn p)
		{
			for (int i = 0; i < p.health.hediffSet.hediffs.Count; i++)
			{
				Hediff hediff = p.health.hediffSet.hediffs[i];
				if (hediff.def == this.Props.hediffDef && hediff.Part == p.RaceProps.body.GetPartsWithDef(this.Props.bodyPart).FirstOrFallback(null))
				{
					return hediff;
				}
			}
			return null;
		}
	}
}
