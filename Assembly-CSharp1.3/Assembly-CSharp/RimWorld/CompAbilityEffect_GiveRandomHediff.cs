using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D43 RID: 3395
	public class CompAbilityEffect_GiveRandomHediff : CompAbilityEffect
	{
		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06004F51 RID: 20305 RVA: 0x001A95EF File Offset: 0x001A77EF
		public new CompProperties_AbilityGiveRandomHediff Props
		{
			get
			{
				return (CompProperties_AbilityGiveRandomHediff)this.props;
			}
		}

		// Token: 0x06004F52 RID: 20306 RVA: 0x001A95FC File Offset: 0x001A77FC
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			HediffOption hediffOption = this.GetApplicableHediffs(target.Pawn).RandomElement<HediffOption>();
			BodyPartRecord partRecord = this.GetAcceptablePartsForHediff(target.Pawn, hediffOption).RandomElement<BodyPartRecord>();
			Hediff hediff = HediffMaker.MakeHediff(hediffOption.hediffDef, target.Pawn, partRecord);
			target.Pawn.health.AddHediff(hediff, null, null, null);
			if (base.SendLetter)
			{
				Find.LetterStack.ReceiveLetter(this.Props.customLetterLabel.Formatted(hediff.def.LabelCap), this.Props.customLetterText.Formatted(this.parent.pawn, target.Pawn, hediff.def.label), LetterDefOf.PositiveEvent, new LookTargets(target.Pawn), null, null, null, null);
			}
		}

		// Token: 0x06004F53 RID: 20307 RVA: 0x001A96EC File Offset: 0x001A78EC
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return target.Pawn != null && this.GetApplicableHediffs(target.Pawn).Any<HediffOption>() && base.CanApplyOn(target, dest);
		}

		// Token: 0x06004F54 RID: 20308 RVA: 0x001A9718 File Offset: 0x001A7918
		private List<HediffOption> GetApplicableHediffs(Pawn target)
		{
			List<HediffOption> list = new List<HediffOption>();
			foreach (HediffOption hediffOption in this.Props.options)
			{
				if (!this.GetAcceptablePartsForHediff(target, hediffOption).EnumerableNullOrEmpty<BodyPartRecord>())
				{
					list.Add(hediffOption);
				}
			}
			return list;
		}

		// Token: 0x06004F55 RID: 20309 RVA: 0x001A9788 File Offset: 0x001A7988
		private IEnumerable<BodyPartRecord> GetAcceptablePartsForHediff(Pawn target, HediffOption option)
		{
			if (!this.Props.allowDuplicates && (from x in target.health.hediffSet.hediffs
			where x.def == option.hediffDef
			select x).Any<Hediff>())
			{
				return null;
			}
			return from p in target.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where (option.bodyPart == null || p.def == option.bodyPart) && !target.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(p)
			select p;
		}
	}
}
