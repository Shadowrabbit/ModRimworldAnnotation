using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C39 RID: 7225
	public static class QuadrumUtility
	{
		// Token: 0x170018C8 RID: 6344
		// (get) Token: 0x06009F1E RID: 40734 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public static Quadrum FirstQuadrum
		{
			get
			{
				return Quadrum.Aprimay;
			}
		}

		// Token: 0x06009F1F RID: 40735 RVA: 0x00069FEF File Offset: 0x000681EF
		public static Twelfth GetFirstTwelfth(this Quadrum quadrum)
		{
			switch (quadrum)
			{
			case Quadrum.Aprimay:
				return Twelfth.First;
			case Quadrum.Jugust:
				return Twelfth.Fourth;
			case Quadrum.Septober:
				return Twelfth.Seventh;
			case Quadrum.Decembary:
				return Twelfth.Tenth;
			default:
				return Twelfth.Undefined;
			}
		}

		// Token: 0x06009F20 RID: 40736 RVA: 0x0006A014 File Offset: 0x00068214
		public static Twelfth GetMiddleTwelfth(this Quadrum quadrum)
		{
			switch (quadrum)
			{
			case Quadrum.Aprimay:
				return Twelfth.Second;
			case Quadrum.Jugust:
				return Twelfth.Fifth;
			case Quadrum.Septober:
				return Twelfth.Eighth;
			case Quadrum.Decembary:
				return Twelfth.Eleventh;
			default:
				return Twelfth.Undefined;
			}
		}

		// Token: 0x06009F21 RID: 40737 RVA: 0x0006A039 File Offset: 0x00068239
		public static float GetMiddleYearPct(this Quadrum quadrum)
		{
			return quadrum.GetMiddleTwelfth().GetMiddleYearPct();
		}

		// Token: 0x06009F22 RID: 40738 RVA: 0x002E9D74 File Offset: 0x002E7F74
		public static string Label(this Quadrum quadrum)
		{
			switch (quadrum)
			{
			case Quadrum.Aprimay:
				return "QuadrumAprimay".Translate();
			case Quadrum.Jugust:
				return "QuadrumJugust".Translate();
			case Quadrum.Septober:
				return "QuadrumSeptober".Translate();
			case Quadrum.Decembary:
				return "QuadrumDecembary".Translate();
			default:
				return "Unknown quadrum";
			}
		}

		// Token: 0x06009F23 RID: 40739 RVA: 0x002E9DE0 File Offset: 0x002E7FE0
		public static string LabelShort(this Quadrum quadrum)
		{
			switch (quadrum)
			{
			case Quadrum.Aprimay:
				return "QuadrumAprimay_Short".Translate();
			case Quadrum.Jugust:
				return "QuadrumJugust_Short".Translate();
			case Quadrum.Septober:
				return "QuadrumSeptober_Short".Translate();
			case Quadrum.Decembary:
				return "QuadrumDecembary_Short".Translate();
			default:
				return "Unknown quadrum";
			}
		}

		// Token: 0x06009F24 RID: 40740 RVA: 0x0006A046 File Offset: 0x00068246
		public static Season GetSeason(this Quadrum q, float latitude)
		{
			return SeasonUtility.GetReportedSeason(q.GetMiddleYearPct(), latitude);
		}

		// Token: 0x06009F25 RID: 40741 RVA: 0x002E9E4C File Offset: 0x002E804C
		public static string QuadrumsRangeLabel(List<Twelfth> twelfths)
		{
			if (twelfths.Count == 0)
			{
				return "";
			}
			if (twelfths.Count == 12)
			{
				return "WholeYear".Translate();
			}
			string text = "";
			for (int i = 0; i < 12; i++)
			{
				Twelfth twelfth = (Twelfth)i;
				if (twelfths.Contains(twelfth))
				{
					if (!text.NullOrEmpty())
					{
						text += ", ";
					}
					text += QuadrumUtility.QuadrumsContinuousRangeLabel(twelfths, twelfth);
				}
			}
			return text;
		}

		// Token: 0x06009F26 RID: 40742 RVA: 0x002E9EC4 File Offset: 0x002E80C4
		private static string QuadrumsContinuousRangeLabel(List<Twelfth> twelfths, Twelfth rootTwelfth)
		{
			Twelfth leftMostTwelfth = TwelfthUtility.GetLeftMostTwelfth(twelfths, rootTwelfth);
			Twelfth rightMostTwelfth = TwelfthUtility.GetRightMostTwelfth(twelfths, rootTwelfth);
			for (Twelfth twelfth = leftMostTwelfth; twelfth != rightMostTwelfth; twelfth = TwelfthUtility.TwelfthAfter(twelfth))
			{
				if (!twelfths.Contains(twelfth))
				{
					Log.Error(string.Concat(new object[]
					{
						"Twelfths doesn't contain ",
						twelfth,
						" (",
						leftMostTwelfth,
						"..",
						rightMostTwelfth,
						")"
					}), false);
					break;
				}
				twelfths.Remove(twelfth);
			}
			twelfths.Remove(rightMostTwelfth);
			return GenDate.QuadrumDateStringAt(leftMostTwelfth) + " - " + GenDate.QuadrumDateStringAt(rightMostTwelfth);
		}
	}
}
