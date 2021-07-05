using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F8 RID: 5112
	public static class QuadrumUtility
	{
		// Token: 0x170015BA RID: 5562
		// (get) Token: 0x06007CC9 RID: 31945 RVA: 0x0001276E File Offset: 0x0001096E
		public static Quadrum FirstQuadrum
		{
			get
			{
				return Quadrum.Aprimay;
			}
		}

		// Token: 0x170015BB RID: 5563
		// (get) Token: 0x06007CCA RID: 31946 RVA: 0x002C3889 File Offset: 0x002C1A89
		public static List<Quadrum> Quadrums
		{
			get
			{
				return QuadrumUtility.quadrumList;
			}
		}

		// Token: 0x170015BC RID: 5564
		// (get) Token: 0x06007CCB RID: 31947 RVA: 0x002C3890 File Offset: 0x002C1A90
		public static List<Quadrum> QuadrumsInChronologicalOrder
		{
			get
			{
				return QuadrumUtility.quadrumListInChronologicanOrder;
			}
		}

		// Token: 0x06007CCC RID: 31948 RVA: 0x002C3897 File Offset: 0x002C1A97
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

		// Token: 0x06007CCD RID: 31949 RVA: 0x002C38BC File Offset: 0x002C1ABC
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

		// Token: 0x06007CCE RID: 31950 RVA: 0x002C38E1 File Offset: 0x002C1AE1
		public static float GetMiddleYearPct(this Quadrum quadrum)
		{
			return quadrum.GetMiddleTwelfth().GetMiddleYearPct();
		}

		// Token: 0x06007CCF RID: 31951 RVA: 0x002C38F0 File Offset: 0x002C1AF0
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

		// Token: 0x06007CD0 RID: 31952 RVA: 0x002C395C File Offset: 0x002C1B5C
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

		// Token: 0x06007CD1 RID: 31953 RVA: 0x002C39C6 File Offset: 0x002C1BC6
		public static Season GetSeason(this Quadrum q, Vector2 location)
		{
			return q.GetSeason(location.y);
		}

		// Token: 0x06007CD2 RID: 31954 RVA: 0x002C39D4 File Offset: 0x002C1BD4
		public static Season GetSeason(this Quadrum q, float latitude)
		{
			return SeasonUtility.GetReportedSeason(q.GetMiddleYearPct(), latitude);
		}

		// Token: 0x06007CD3 RID: 31955 RVA: 0x002C39E4 File Offset: 0x002C1BE4
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

		// Token: 0x06007CD4 RID: 31956 RVA: 0x002C3A5C File Offset: 0x002C1C5C
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
					}));
					break;
				}
				twelfths.Remove(twelfth);
			}
			twelfths.Remove(rightMostTwelfth);
			return GenDate.QuadrumDateStringAt(leftMostTwelfth) + " - " + GenDate.QuadrumDateStringAt(rightMostTwelfth);
		}

		// Token: 0x040044F9 RID: 17657
		private static readonly List<Quadrum> quadrumList = new List<Quadrum>
		{
			Quadrum.Aprimay,
			Quadrum.Decembary,
			Quadrum.Jugust,
			Quadrum.Septober
		};

		// Token: 0x040044FA RID: 17658
		private static readonly List<Quadrum> quadrumListInChronologicanOrder = new List<Quadrum>
		{
			Quadrum.Aprimay,
			Quadrum.Jugust,
			Quadrum.Septober,
			Quadrum.Decembary
		};
	}
}
