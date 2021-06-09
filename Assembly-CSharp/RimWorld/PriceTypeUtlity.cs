using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001BB1 RID: 7089
	public static class PriceTypeUtlity
	{
		// Token: 0x06009C2F RID: 39983 RVA: 0x002DD3D4 File Offset: 0x002DB5D4
		public static float PriceMultiplier(this PriceType pType)
		{
			switch (pType)
			{
			case PriceType.VeryCheap:
				return 0.4f;
			case PriceType.Cheap:
				return 0.7f;
			case PriceType.Normal:
				return 1f;
			case PriceType.Expensive:
				return 2f;
			case PriceType.Exorbitant:
				return 5f;
			default:
				return -1f;
			}
		}

		// Token: 0x06009C30 RID: 39984 RVA: 0x002DD424 File Offset: 0x002DB624
		public static PriceType ClosestPriceType(float priceFactor)
		{
			float num = 99999f;
			PriceType priceType = PriceType.Undefined;
			foreach (object obj in Enum.GetValues(typeof(PriceType)))
			{
				PriceType priceType2 = (PriceType)obj;
				float num2 = Mathf.Abs(priceFactor - priceType2.PriceMultiplier());
				if (num2 < num)
				{
					num = num2;
					priceType = priceType2;
				}
			}
			if (priceType == PriceType.Undefined)
			{
				priceType = PriceType.VeryCheap;
			}
			return priceType;
		}
	}
}
