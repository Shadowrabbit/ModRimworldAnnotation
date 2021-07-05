using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013B4 RID: 5044
	public class TransferableComparer_Category : TransferableComparer
	{
		// Token: 0x06007AC3 RID: 31427 RVA: 0x002B6611 File Offset: 0x002B4811
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return TransferableComparer_Category.Compare(lhs.ThingDef, rhs.ThingDef);
		}

		// Token: 0x06007AC4 RID: 31428 RVA: 0x002B6624 File Offset: 0x002B4824
		public static int Compare(ThingDef lhsTh, ThingDef rhsTh)
		{
			if (lhsTh.category != rhsTh.category)
			{
				return lhsTh.category.CompareTo(rhsTh.category);
			}
			float num = TransferableUIUtility.DefaultListOrderPriority(lhsTh);
			float num2 = TransferableUIUtility.DefaultListOrderPriority(rhsTh);
			if (num != num2)
			{
				return num.CompareTo(num2);
			}
			int num3 = 0;
			if (!lhsTh.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				num3 = (int)lhsTh.thingCategories[0].index;
			}
			int value = 0;
			if (!rhsTh.thingCategories.NullOrEmpty<ThingCategoryDef>())
			{
				value = (int)rhsTh.thingCategories[0].index;
			}
			return num3.CompareTo(value);
		}
	}
}
