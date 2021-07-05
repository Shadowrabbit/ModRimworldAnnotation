using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011AF RID: 4527
	public static class ThingStyleHelper
	{
		// Token: 0x06006D0C RID: 27916 RVA: 0x002495FE File Offset: 0x002477FE
		public static bool CanBeStyled(this ThingDef thingDef)
		{
			return thingDef.CompDefFor<CompStyleable>() != null;
		}

		// Token: 0x06006D0D RID: 27917 RVA: 0x0024960C File Offset: 0x0024780C
		public static bool GetEverSeenByPlayer(this Thing thing)
		{
			CompStyleable compStyleable = thing.TryGetComp<CompStyleable>();
			return compStyleable != null && compStyleable.everSeenByPlayer;
		}

		// Token: 0x06006D0E RID: 27918 RVA: 0x0024962C File Offset: 0x0024782C
		public static ThingStyleDef GetStyleDef(this Thing thing)
		{
			CompStyleable compStyleable = thing.TryGetComp<CompStyleable>();
			if (compStyleable == null)
			{
				return null;
			}
			return compStyleable.styleDef;
		}

		// Token: 0x06006D0F RID: 27919 RVA: 0x0024964C File Offset: 0x0024784C
		public static Precept_ThingStyle GetStyleSourcePrecept(this Thing thing)
		{
			CompStyleable compStyleable = thing.TryGetComp<CompStyleable>();
			if (compStyleable == null)
			{
				return null;
			}
			return compStyleable.SourcePrecept;
		}

		// Token: 0x06006D10 RID: 27920 RVA: 0x0024966C File Offset: 0x0024786C
		public static void SetEverSeenByPlayer(this Thing thing, bool everSeenByPlayer)
		{
			CompStyleable compStyleable = thing.TryGetComp<CompStyleable>();
			if (compStyleable != null)
			{
				compStyleable.everSeenByPlayer = everSeenByPlayer;
			}
			if (everSeenByPlayer && thing.StyleSourcePrecept is Precept_Relic)
			{
				thing.StyleSourcePrecept.ideo.Notify_RelicSeenByPlayer(thing);
			}
		}

		// Token: 0x06006D11 RID: 27921 RVA: 0x002496AC File Offset: 0x002478AC
		public static void SetStyleDef(this Thing thing, ThingStyleDef styleDef)
		{
			CompStyleable compStyleable = thing.TryGetComp<CompStyleable>();
			if (compStyleable == null)
			{
				if (styleDef != null)
				{
					Log.Warning("Tried setting ThingStyleDef to a thing without CompStyleable (" + thing.def.defName + ")!");
				}
				return;
			}
			compStyleable.styleDef = styleDef;
		}

		// Token: 0x06006D12 RID: 27922 RVA: 0x002496F0 File Offset: 0x002478F0
		public static void SetStyleSourcePrecept(this Thing thing, Precept_ThingStyle precept)
		{
			CompStyleable compStyleable = thing.TryGetComp<CompStyleable>();
			if (compStyleable == null)
			{
				if (precept != null)
				{
					Log.Warning("Tried setting StyleSourcePrecept to a thing without CompStyleable (" + thing.def.defName + ")!");
				}
				return;
			}
			compStyleable.SourcePrecept = precept;
		}
	}
}
