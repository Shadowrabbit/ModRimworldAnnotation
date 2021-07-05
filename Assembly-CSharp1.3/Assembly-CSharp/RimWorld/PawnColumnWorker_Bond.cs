using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001381 RID: 4993
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Bond : PawnColumnWorker_Icon
	{
		// Token: 0x06007976 RID: 31094 RVA: 0x002AFA84 File Offset: 0x002ADC84
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			IEnumerable<Pawn> allColonistBondsFor = TrainableUtility.GetAllColonistBondsFor(pawn);
			if (!allColonistBondsFor.Any<Pawn>())
			{
				return null;
			}
			if (allColonistBondsFor.Any((Pawn bond) => bond == pawn.playerSettings.Master))
			{
				return PawnColumnWorker_Bond.BondIcon;
			}
			return PawnColumnWorker_Bond.BondBrokenIcon;
		}

		// Token: 0x06007977 RID: 31095 RVA: 0x002AFAD3 File Offset: 0x002ADCD3
		protected override string GetIconTip(Pawn pawn)
		{
			return TrainableUtility.GetIconTooltipText(pawn);
		}

		// Token: 0x06007978 RID: 31096 RVA: 0x002AFADC File Offset: 0x002ADCDC
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetCompareValueFor(a).CompareTo(this.GetCompareValueFor(b));
		}

		// Token: 0x06007979 RID: 31097 RVA: 0x002AFB00 File Offset: 0x002ADD00
		public int GetCompareValueFor(Pawn a)
		{
			Texture2D iconFor = this.GetIconFor(a);
			if (iconFor == null)
			{
				return 0;
			}
			if (iconFor == PawnColumnWorker_Bond.BondBrokenIcon)
			{
				return 1;
			}
			if (iconFor == PawnColumnWorker_Bond.BondIcon)
			{
				return 2;
			}
			Log.ErrorOnce("Unknown bond type when trying to sort", 20536378);
			return 0;
		}

		// Token: 0x0600797A RID: 31098 RVA: 0x002AFB50 File Offset: 0x002ADD50
		protected override void PaintedIcon(Pawn pawn)
		{
			if (this.GetIconFor(pawn) != PawnColumnWorker_Bond.BondBrokenIcon)
			{
				return;
			}
			if (!pawn.training.HasLearned(TrainableDefOf.Obedience))
			{
				return;
			}
			pawn.playerSettings.Master = (from master in TrainableUtility.GetAllColonistBondsFor(pawn)
			where TrainableUtility.CanBeMaster(master, pawn, true)
			select master).FirstOrDefault<Pawn>();
		}

		// Token: 0x04004380 RID: 17280
		private static readonly Texture2D BondIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Bond", true);

		// Token: 0x04004381 RID: 17281
		private static readonly Texture2D BondBrokenIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/BondBroken", true);
	}
}
