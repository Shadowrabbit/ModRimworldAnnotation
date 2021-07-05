using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001388 RID: 5000
	public class PawnColumnWorker_LifeStage : PawnColumnWorker_Icon
	{
		// Token: 0x060079A0 RID: 31136 RVA: 0x002AFF7A File Offset: 0x002AE17A
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.ageTracker.CurLifeStageRace.GetIcon(pawn);
		}

		// Token: 0x060079A1 RID: 31137 RVA: 0x002AFF90 File Offset: 0x002AE190
		protected override string GetIconTip(Pawn pawn)
		{
			int num = Mathf.FloorToInt(pawn.ageTracker.AgeBiologicalYearsFloat);
			int num2 = Mathf.FloorToInt(pawn.ageTracker.AgeBiologicalYearsFloat * 60f);
			num2 -= num * 60;
			string text = "";
			if (num > 0)
			{
				if (num == 1)
				{
					text += "Period1Year".Translate();
				}
				else
				{
					text += "PeriodYears".Translate(num);
				}
				text += ", ";
			}
			if (num2 > 0)
			{
				if (num2 == 1)
				{
					text += "Period1Day".Translate();
				}
				else
				{
					text += "PeriodDays".Translate(num2);
				}
			}
			else if (num <= 0)
			{
				text += "PeriodHours".Translate(((float)pawn.ageTracker.AgeBiologicalTicks / 2500f).ToString("0.0"));
			}
			return pawn.ageTracker.CurLifeStage.LabelCap + " (" + text.TrimEnd(new char[]
			{
				',',
				' '
			}) + ")";
		}

		// Token: 0x060079A2 RID: 31138 RVA: 0x002B00DC File Offset: 0x002AE2DC
		public override int Compare(Pawn a, Pawn b)
		{
			return a.ageTracker.AgeBiologicalTicks.CompareTo(b.ageTracker.AgeBiologicalTicks);
		}
	}
}
