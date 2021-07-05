using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003FF RID: 1023
	public static class HediffUtility
	{
		// Token: 0x060018E1 RID: 6369 RVA: 0x000E0660 File Offset: 0x000DE860
		public static T TryGetComp<T>(this Hediff hd) where T : HediffComp
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return default(T);
			}
			if (hediffWithComps.comps != null)
			{
				for (int i = 0; i < hediffWithComps.comps.Count; i++)
				{
					T t = hediffWithComps.comps[i] as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x000E06CC File Offset: 0x000DE8CC
		public static bool IsTended(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return false;
			}
			HediffComp_TendDuration hediffComp_TendDuration = hediffWithComps.TryGetComp<HediffComp_TendDuration>();
			return hediffComp_TendDuration != null && hediffComp_TendDuration.IsTended;
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x000E06F8 File Offset: 0x000DE8F8
		public static bool IsPermanent(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return false;
			}
			HediffComp_GetsPermanent hediffComp_GetsPermanent = hediffWithComps.TryGetComp<HediffComp_GetsPermanent>();
			return hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent;
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x000E0724 File Offset: 0x000DE924
		public static bool FullyImmune(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return false;
			}
			HediffComp_Immunizable hediffComp_Immunizable = hediffWithComps.TryGetComp<HediffComp_Immunizable>();
			return hediffComp_Immunizable != null && hediffComp_Immunizable.FullyImmune;
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x00017ACA File Offset: 0x00015CCA
		public static bool CanHealFromTending(this Hediff_Injury hd)
		{
			return hd.IsTended() && !hd.IsPermanent();
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x00017ADF File Offset: 0x00015CDF
		public static bool CanHealNaturally(this Hediff_Injury hd)
		{
			return !hd.IsPermanent();
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x000E0750 File Offset: 0x000DE950
		public static int CountAddedAndImplantedParts(this HediffSet hs)
		{
			int num = 0;
			List<Hediff> hediffs = hs.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.countsAsAddedPartOrImplant)
				{
					num++;
				}
			}
			return num;
		}
	}
}
