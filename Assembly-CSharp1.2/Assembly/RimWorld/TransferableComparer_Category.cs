using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB4 RID: 7092
	public class TransferableComparer_Category : TransferableComparer
	{
		// Token: 0x06009C37 RID: 39991 RVA: 0x00067D59 File Offset: 0x00065F59
		public override int Compare(Transferable lhs, Transferable rhs)
		{
			return TransferableComparer_Category.Compare(lhs.ThingDef, rhs.ThingDef);
		}

		// Token: 0x06009C38 RID: 39992 RVA: 0x002DDA04 File Offset: 0x002DBC04
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
