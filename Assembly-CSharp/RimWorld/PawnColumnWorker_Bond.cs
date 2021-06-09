using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B6B RID: 7019
	[StaticConstructorOnStartup]
	public class PawnColumnWorker_Bond : PawnColumnWorker_Icon
	{
		// Token: 0x06009AB1 RID: 39601 RVA: 0x002D7CCC File Offset: 0x002D5ECC
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

		// Token: 0x06009AB2 RID: 39602 RVA: 0x00066FD4 File Offset: 0x000651D4
		protected override string GetIconTip(Pawn pawn)
		{
			return TrainableUtility.GetIconTooltipText(pawn);
		}

		// Token: 0x06009AB3 RID: 39603 RVA: 0x002D7D1C File Offset: 0x002D5F1C
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetCompareValueFor(a).CompareTo(this.GetCompareValueFor(b));
		}

		// Token: 0x06009AB4 RID: 39604 RVA: 0x002D7D40 File Offset: 0x002D5F40
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
			Log.ErrorOnce("Unknown bond type when trying to sort", 20536378, false);
			return 0;
		}

		// Token: 0x06009AB5 RID: 39605 RVA: 0x002D7D90 File Offset: 0x002D5F90
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

		// Token: 0x040062C9 RID: 25289
		private static readonly Texture2D BondIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/Bond", true);

		// Token: 0x040062CA RID: 25290
		private static readonly Texture2D BondBrokenIcon = ContentFinder<Texture2D>.Get("UI/Icons/Animal/BondBroken", true);
	}
}
