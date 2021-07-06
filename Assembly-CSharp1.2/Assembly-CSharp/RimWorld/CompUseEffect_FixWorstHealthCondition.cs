﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D7 RID: 6359
	public class CompUseEffect_FixWorstHealthCondition : CompUseEffect
	{
		// Token: 0x17001624 RID: 5668
		// (get) Token: 0x06008CDF RID: 36063 RVA: 0x0005E72A File Offset: 0x0005C92A
		private float HandCoverageAbsWithChildren
		{
			get
			{
				return ThingDefOf.Human.race.body.GetPartsWithDef(BodyPartDefOf.Hand).First<BodyPartRecord>().coverageAbsWithChildren;
			}
		}

		// Token: 0x06008CE0 RID: 36064 RVA: 0x0028DBC8 File Offset: 0x0028BDC8
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			Hediff hediff = this.FindLifeThreateningHediff(usedBy);
			if (hediff != null)
			{
				this.Cure(hediff);
				return;
			}
			if (HealthUtility.TicksUntilDeathDueToBloodLoss(usedBy) < 2500)
			{
				Hediff hediff2 = this.FindMostBleedingHediff(usedBy);
				if (hediff2 != null)
				{
					this.Cure(hediff2);
					return;
				}
			}
			if (usedBy.health.hediffSet.GetBrain() != null)
			{
				Hediff_Injury hediff_Injury = this.FindPermanentInjury(usedBy, Gen.YieldSingle<BodyPartRecord>(usedBy.health.hediffSet.GetBrain()));
				if (hediff_Injury != null)
				{
					this.Cure(hediff_Injury);
					return;
				}
			}
			BodyPartRecord bodyPartRecord = this.FindBiggestMissingBodyPart(usedBy, this.HandCoverageAbsWithChildren);
			if (bodyPartRecord != null)
			{
				this.Cure(bodyPartRecord, usedBy);
				return;
			}
			Hediff_Injury hediff_Injury2 = this.FindPermanentInjury(usedBy, from x in usedBy.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.def == BodyPartDefOf.Eye
			select x);
			if (hediff_Injury2 != null)
			{
				this.Cure(hediff_Injury2);
				return;
			}
			Hediff hediff3 = this.FindImmunizableHediffWhichCanKill(usedBy);
			if (hediff3 != null)
			{
				this.Cure(hediff3);
				return;
			}
			Hediff hediff4 = this.FindNonInjuryMiscBadHediff(usedBy, true);
			if (hediff4 != null)
			{
				this.Cure(hediff4);
				return;
			}
			Hediff hediff5 = this.FindNonInjuryMiscBadHediff(usedBy, false);
			if (hediff5 != null)
			{
				this.Cure(hediff5);
				return;
			}
			if (usedBy.health.hediffSet.GetBrain() != null)
			{
				Hediff_Injury hediff_Injury3 = this.FindInjury(usedBy, Gen.YieldSingle<BodyPartRecord>(usedBy.health.hediffSet.GetBrain()));
				if (hediff_Injury3 != null)
				{
					this.Cure(hediff_Injury3);
					return;
				}
			}
			BodyPartRecord bodyPartRecord2 = this.FindBiggestMissingBodyPart(usedBy, 0f);
			if (bodyPartRecord2 != null)
			{
				this.Cure(bodyPartRecord2, usedBy);
				return;
			}
			Hediff_Addiction hediff_Addiction = this.FindAddiction(usedBy);
			if (hediff_Addiction != null)
			{
				this.Cure(hediff_Addiction);
				return;
			}
			Hediff_Injury hediff_Injury4 = this.FindPermanentInjury(usedBy, null);
			if (hediff_Injury4 != null)
			{
				this.Cure(hediff_Injury4);
				return;
			}
			Hediff_Injury hediff_Injury5 = this.FindInjury(usedBy, null);
			if (hediff_Injury5 != null)
			{
				this.Cure(hediff_Injury5);
				return;
			}
		}

		// Token: 0x06008CE1 RID: 36065 RVA: 0x0028DD94 File Offset: 0x0028BF94
		private Hediff FindLifeThreateningHediff(Pawn pawn)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.everCurableByItem && !hediffs[i].FullyImmune())
				{
					HediffStage curStage = hediffs[i].CurStage;
					bool flag = curStage != null && curStage.lifeThreatening;
					bool flag2 = hediffs[i].def.lethalSeverity >= 0f && hediffs[i].Severity / hediffs[i].def.lethalSeverity >= 0.8f;
					if (flag || flag2)
					{
						float num2 = (hediffs[i].Part != null) ? hediffs[i].Part.coverageAbsWithChildren : 999f;
						if (hediff == null || num2 > num)
						{
							hediff = hediffs[i];
							num = num2;
						}
					}
				}
			}
			return hediff;
		}

		// Token: 0x06008CE2 RID: 36066 RVA: 0x0028DEAC File Offset: 0x0028C0AC
		private Hediff FindMostBleedingHediff(Pawn pawn)
		{
			float num = 0f;
			Hediff hediff = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.everCurableByItem)
				{
					float bleedRate = hediffs[i].BleedRate;
					if (bleedRate > 0f && (bleedRate > num || hediff == null))
					{
						num = bleedRate;
						hediff = hediffs[i];
					}
				}
			}
			return hediff;
		}

		// Token: 0x06008CE3 RID: 36067 RVA: 0x0028DF30 File Offset: 0x0028C130
		private Hediff FindImmunizableHediffWhichCanKill(Pawn pawn)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.everCurableByItem && hediffs[i].TryGetComp<HediffComp_Immunizable>() != null && !hediffs[i].FullyImmune() && this.CanEverKill(hediffs[i]))
				{
					float severity = hediffs[i].Severity;
					if (hediff == null || severity > num)
					{
						hediff = hediffs[i];
						num = severity;
					}
				}
			}
			return hediff;
		}

		// Token: 0x06008CE4 RID: 36068 RVA: 0x0028DFD8 File Offset: 0x0028C1D8
		private Hediff FindNonInjuryMiscBadHediff(Pawn pawn, bool onlyIfCanKill)
		{
			Hediff hediff = null;
			float num = -1f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Visible && hediffs[i].def.isBad && hediffs[i].def.everCurableByItem && !(hediffs[i] is Hediff_Injury) && !(hediffs[i] is Hediff_MissingPart) && !(hediffs[i] is Hediff_Addiction) && !(hediffs[i] is Hediff_AddedPart) && (!onlyIfCanKill || this.CanEverKill(hediffs[i])))
				{
					float num2 = (hediffs[i].Part != null) ? hediffs[i].Part.coverageAbsWithChildren : 999f;
					if (hediff == null || num2 > num)
					{
						hediff = hediffs[i];
						num = num2;
					}
				}
			}
			return hediff;
		}

		// Token: 0x06008CE5 RID: 36069 RVA: 0x0028E0D8 File Offset: 0x0028C2D8
		private BodyPartRecord FindBiggestMissingBodyPart(Pawn pawn, float minCoverage = 0f)
		{
			BodyPartRecord bodyPartRecord = null;
			foreach (Hediff_MissingPart hediff_MissingPart in pawn.health.hediffSet.GetMissingPartsCommonAncestors())
			{
				if (hediff_MissingPart.Part.coverageAbsWithChildren >= minCoverage && !pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(hediff_MissingPart.Part) && (bodyPartRecord == null || hediff_MissingPart.Part.coverageAbsWithChildren > bodyPartRecord.coverageAbsWithChildren))
				{
					bodyPartRecord = hediff_MissingPart.Part;
				}
			}
			return bodyPartRecord;
		}

		// Token: 0x06008CE6 RID: 36070 RVA: 0x0028E174 File Offset: 0x0028C374
		private Hediff_Addiction FindAddiction(Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
				if (hediff_Addiction != null && hediff_Addiction.Visible && hediff_Addiction.def.everCurableByItem)
				{
					return hediff_Addiction;
				}
			}
			return null;
		}

		// Token: 0x06008CE7 RID: 36071 RVA: 0x0028E1CC File Offset: 0x0028C3CC
		private Hediff_Injury FindPermanentInjury(Pawn pawn, IEnumerable<BodyPartRecord> allowedBodyParts = null)
		{
			Hediff_Injury hediff_Injury = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury2 = hediffs[i] as Hediff_Injury;
				if (hediff_Injury2 != null && hediff_Injury2.Visible && hediff_Injury2.IsPermanent() && hediff_Injury2.def.everCurableByItem && (allowedBodyParts == null || allowedBodyParts.Contains(hediff_Injury2.Part)) && (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity))
				{
					hediff_Injury = hediff_Injury2;
				}
			}
			return hediff_Injury;
		}

		// Token: 0x06008CE8 RID: 36072 RVA: 0x0028E250 File Offset: 0x0028C450
		private Hediff_Injury FindInjury(Pawn pawn, IEnumerable<BodyPartRecord> allowedBodyParts = null)
		{
			Hediff_Injury hediff_Injury = null;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Injury hediff_Injury2 = hediffs[i] as Hediff_Injury;
				if (hediff_Injury2 != null && hediff_Injury2.Visible && hediff_Injury2.def.everCurableByItem && (allowedBodyParts == null || allowedBodyParts.Contains(hediff_Injury2.Part)) && (hediff_Injury == null || hediff_Injury2.Severity > hediff_Injury.Severity))
				{
					hediff_Injury = hediff_Injury2;
				}
			}
			return hediff_Injury;
		}

		// Token: 0x06008CE9 RID: 36073 RVA: 0x0005E74F File Offset: 0x0005C94F
		private void Cure(Hediff hediff)
		{
			HealthUtility.CureHediff(hediff);
			Messages.Message("MessageHediffCuredByItem".Translate(hediff.LabelBase.CapitalizeFirst()), hediff.pawn, MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x06008CEA RID: 36074 RVA: 0x0005E78C File Offset: 0x0005C98C
		private void Cure(BodyPartRecord part, Pawn pawn)
		{
			pawn.health.RestorePart(part, null, true);
			Messages.Message("MessageBodyPartCuredByItem".Translate(part.LabelCap), pawn, MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x06008CEB RID: 36075 RVA: 0x0028E2CC File Offset: 0x0028C4CC
		private bool CanEverKill(Hediff hediff)
		{
			if (hediff.def.stages != null)
			{
				for (int i = 0; i < hediff.def.stages.Count; i++)
				{
					if (hediff.def.stages[i].lifeThreatening)
					{
						return true;
					}
				}
			}
			return hediff.def.lethalSeverity >= 0f;
		}
	}
}
