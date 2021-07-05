using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200120C RID: 4620
	public class CompUseEffect_InstallImplant : CompUseEffect
	{
		// Token: 0x17001347 RID: 4935
		// (get) Token: 0x06006EF6 RID: 28406 RVA: 0x002517C3 File Offset: 0x0024F9C3
		public CompProperties_UseEffectInstallImplant Props
		{
			get
			{
				return (CompProperties_UseEffectInstallImplant)this.props;
			}
		}

		// Token: 0x06006EF7 RID: 28407 RVA: 0x002517D0 File Offset: 0x0024F9D0
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
				((Hediff_Level)firstHediffOfDef).ChangeLevel(1);
			}
		}

		// Token: 0x06006EF8 RID: 28408 RVA: 0x00251860 File Offset: 0x0024FA60
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
				Hediff_Level hediff_Level = (Hediff_Level)existingImplant;
				if ((float)hediff_Level.level >= hediff_Level.def.maxSeverity)
				{
					failReason = "InstallImplantAlreadyMaxLevel".Translate();
					return false;
				}
			}
			failReason = null;
			return true;
		}

		// Token: 0x06006EF9 RID: 28409 RVA: 0x00251950 File Offset: 0x0024FB50
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
