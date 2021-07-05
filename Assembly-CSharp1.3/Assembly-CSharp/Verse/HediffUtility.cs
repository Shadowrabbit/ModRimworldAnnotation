using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002C2 RID: 706
	public static class HediffUtility
	{
		// Token: 0x06001310 RID: 4880 RVA: 0x0006C944 File Offset: 0x0006AB44
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

		// Token: 0x06001311 RID: 4881 RVA: 0x0006C9B0 File Offset: 0x0006ABB0
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

		// Token: 0x06001312 RID: 4882 RVA: 0x0006C9DC File Offset: 0x0006ABDC
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

		// Token: 0x06001313 RID: 4883 RVA: 0x0006CA08 File Offset: 0x0006AC08
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

		// Token: 0x06001314 RID: 4884 RVA: 0x0006CA33 File Offset: 0x0006AC33
		public static bool CanHealFromTending(this Hediff_Injury hd)
		{
			return hd.IsTended() && !hd.IsPermanent();
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x0006CA48 File Offset: 0x0006AC48
		public static bool CanHealNaturally(this Hediff_Injury hd)
		{
			return !hd.IsPermanent();
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x0006CA54 File Offset: 0x0006AC54
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
