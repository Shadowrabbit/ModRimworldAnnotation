using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001791 RID: 6033
	public static class CaravanArrivalActionUtility
	{
		// Token: 0x06008B68 RID: 35688 RVA: 0x003211EF File Offset: 0x0031F3EF
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions<T>(Func<FloatMenuAcceptanceReport> acceptanceReportGetter, Func<T> arrivalActionGetter, string label, Caravan caravan, int pathDestination, WorldObject revalidateWorldClickTarget, Action<Action> confirmActionProxy = null) where T : CaravanArrivalAction
		{
			CaravanArrivalActionUtility.<>c__DisplayClass0_0<T> CS$<>8__locals1 = new CaravanArrivalActionUtility.<>c__DisplayClass0_0<T>();
			CS$<>8__locals1.acceptanceReportGetter = acceptanceReportGetter;
			CS$<>8__locals1.caravan = caravan;
			CS$<>8__locals1.pathDestination = pathDestination;
			CS$<>8__locals1.arrivalActionGetter = arrivalActionGetter;
			CS$<>8__locals1.confirmActionProxy = confirmActionProxy;
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = CS$<>8__locals1.acceptanceReportGetter();
			if (floatMenuAcceptanceReport.Accepted || !floatMenuAcceptanceReport.FailReason.NullOrEmpty() || !floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
			{
				if (!floatMenuAcceptanceReport.FailReason.NullOrEmpty())
				{
					yield return new FloatMenuOption(label + " (" + floatMenuAcceptanceReport.FailReason + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else
				{
					CaravanArrivalActionUtility.<>c__DisplayClass0_1<T> CS$<>8__locals2 = new CaravanArrivalActionUtility.<>c__DisplayClass0_1<T>();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.action = delegate()
					{
						FloatMenuAcceptanceReport floatMenuAcceptanceReport2 = CS$<>8__locals2.CS$<>8__locals1.acceptanceReportGetter();
						if (floatMenuAcceptanceReport2.Accepted)
						{
							CS$<>8__locals2.CS$<>8__locals1.caravan.pather.StartPath(CS$<>8__locals2.CS$<>8__locals1.pathDestination, CS$<>8__locals2.CS$<>8__locals1.arrivalActionGetter(), true, true);
							return;
						}
						if (!floatMenuAcceptanceReport2.FailMessage.NullOrEmpty())
						{
							Messages.Message(floatMenuAcceptanceReport2.FailMessage, new GlobalTargetInfo(CS$<>8__locals2.CS$<>8__locals1.pathDestination), MessageTypeDefOf.RejectInput, false);
						}
					};
					yield return new FloatMenuOption(label, (CS$<>8__locals2.CS$<>8__locals1.confirmActionProxy == null) ? CS$<>8__locals2.action : delegate()
					{
						CS$<>8__locals2.CS$<>8__locals1.confirmActionProxy(CS$<>8__locals2.action);
					}, MenuOptionPriority.Default, null, null, 0f, null, revalidateWorldClickTarget, true, 0);
					if (Prefs.DevMode)
					{
						yield return new FloatMenuOption(label + " (Dev: instantly)", delegate()
						{
							FloatMenuAcceptanceReport floatMenuAcceptanceReport2 = CS$<>8__locals2.CS$<>8__locals1.acceptanceReportGetter();
							if (floatMenuAcceptanceReport2.Accepted)
							{
								CS$<>8__locals2.CS$<>8__locals1.caravan.Tile = CS$<>8__locals2.CS$<>8__locals1.pathDestination;
								CS$<>8__locals2.CS$<>8__locals1.caravan.pather.StopDead();
								CS$<>8__locals2.CS$<>8__locals1.arrivalActionGetter().Arrived(CS$<>8__locals2.CS$<>8__locals1.caravan);
								return;
							}
							if (!floatMenuAcceptanceReport2.FailMessage.NullOrEmpty())
							{
								Messages.Message(floatMenuAcceptanceReport2.FailMessage, new GlobalTargetInfo(CS$<>8__locals2.CS$<>8__locals1.pathDestination), MessageTypeDefOf.RejectInput, false);
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, revalidateWorldClickTarget, true, 0);
					}
					CS$<>8__locals2 = null;
				}
			}
			yield break;
		}
	}
}
