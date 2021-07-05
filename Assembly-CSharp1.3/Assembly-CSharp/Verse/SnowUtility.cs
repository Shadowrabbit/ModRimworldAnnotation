using System;

namespace Verse
{
	// Token: 0x0200021A RID: 538
	public static class SnowUtility
	{
		// Token: 0x06000F4D RID: 3917 RVA: 0x00056CF7 File Offset: 0x00054EF7
		public static SnowCategory GetSnowCategory(float snowDepth)
		{
			if (snowDepth < 0.03f)
			{
				return SnowCategory.None;
			}
			if (snowDepth < 0.25f)
			{
				return SnowCategory.Dusting;
			}
			if (snowDepth < 0.5f)
			{
				return SnowCategory.Thin;
			}
			if (snowDepth < 0.75f)
			{
				return SnowCategory.Medium;
			}
			return SnowCategory.Thick;
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x00056D24 File Offset: 0x00054F24
		public static string GetDescription(SnowCategory category)
		{
			switch (category)
			{
			case SnowCategory.None:
				return "SnowNone".Translate();
			case SnowCategory.Dusting:
				return "SnowDusting".Translate();
			case SnowCategory.Thin:
				return "SnowThin".Translate();
			case SnowCategory.Medium:
				return "SnowMedium".Translate();
			case SnowCategory.Thick:
				return "SnowThick".Translate();
			default:
				return "Unknown snow";
			}
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x00056DA2 File Offset: 0x00054FA2
		public static int MovementTicksAddOn(SnowCategory category)
		{
			switch (category)
			{
			case SnowCategory.None:
				return 0;
			case SnowCategory.Dusting:
				return 0;
			case SnowCategory.Thin:
				return 4;
			case SnowCategory.Medium:
				return 8;
			case SnowCategory.Thick:
				return 12;
			default:
				return 0;
			}
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x00056DCC File Offset: 0x00054FCC
		public static void AddSnowRadial(IntVec3 center, Map map, float radius, float depth)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					float lengthHorizontal = (center - intVec).LengthHorizontal;
					float num2 = 1f - lengthHorizontal / radius;
					map.snowGrid.AddDepth(intVec, num2 * depth);
				}
			}
		}
	}
}
