using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A10 RID: 2576
	public static class JobUtility
	{
		// Token: 0x06003D93 RID: 15763 RVA: 0x00175B1C File Offset: 0x00173D1C
		public static void TryStartErrorRecoverJob(Pawn pawn, string message, Exception exception = null, JobDriver concreteDriver = null)
		{
			string text = message;
			JobUtility.AppendVarsInfoToDebugMessage(pawn, ref text, concreteDriver);
			if (exception != null)
			{
				text = text + "\n" + exception;
			}
			Log.Error(text, false);
			if (pawn.jobs != null)
			{
				if (pawn.jobs.curJob != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Errored, false, true);
				}
				if (JobUtility.startingErrorRecoverJob)
				{
					Log.Error("An error occurred while starting an error recover job. We have to stop now to avoid infinite loops. This means that the pawn is now jobless which can cause further bugs. pawn=" + pawn.ToStringSafe<Pawn>(), false);
					return;
				}
				JobUtility.startingErrorRecoverJob = true;
				try
				{
					pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Wait, 150, false), JobCondition.None, null, false, true, null, null, false, false);
				}
				finally
				{
					JobUtility.startingErrorRecoverJob = false;
				}
			}
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x0002E61A File Offset: 0x0002C81A
		public static string GetResolvedJobReport(string baseText, LocalTargetInfo a)
		{
			return JobUtility.GetResolvedJobReport(baseText, a, LocalTargetInfo.Invalid, LocalTargetInfo.Invalid);
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x0002E62D File Offset: 0x0002C82D
		public static string GetResolvedJobReport(string baseText, LocalTargetInfo a, LocalTargetInfo b)
		{
			return JobUtility.GetResolvedJobReport(baseText, a, b, LocalTargetInfo.Invalid);
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x00175BD8 File Offset: 0x00173DD8
		public static string GetResolvedJobReport(string baseText, LocalTargetInfo a, LocalTargetInfo b, LocalTargetInfo c)
		{
			string aText;
			object aObj;
			JobUtility.<GetResolvedJobReport>g__GetText|4_0(a, out aText, out aObj);
			string bText;
			object bObj;
			JobUtility.<GetResolvedJobReport>g__GetText|4_0(b, out bText, out bObj);
			string cText;
			object cObj;
			JobUtility.<GetResolvedJobReport>g__GetText|4_0(c, out cText, out cObj);
			return JobUtility.GetResolvedJobReportRaw(baseText, aText, aObj, bText, bObj, cText, cObj);
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x00175C14 File Offset: 0x00173E14
		public static string GetResolvedJobReportRaw(string baseText, string aText, object aObj, string bText, object bObj, string cText, object cObj)
		{
			baseText = baseText.Formatted(aObj.Named("TargetA"), bObj.Named("TargetB"), cObj.Named("TargetC"));
			baseText = baseText.Replace("TargetA", aText);
			baseText = baseText.Replace("TargetB", bText);
			baseText = baseText.Replace("TargetC", cText);
			return baseText;
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x00175C80 File Offset: 0x00173E80
		private static void AppendVarsInfoToDebugMessage(Pawn pawn, ref string msg, JobDriver concreteDriver)
		{
			if (concreteDriver != null)
			{
				msg = string.Concat(new object[]
				{
					msg,
					" driver=",
					concreteDriver.GetType().Name,
					" (toilIndex=",
					concreteDriver.CurToilIndex,
					")"
				});
				if (concreteDriver.job != null)
				{
					msg = msg + " driver.job=(" + concreteDriver.job.ToStringSafe<Job>() + ")";
					return;
				}
			}
			else if (pawn.jobs != null)
			{
				if (pawn.jobs.curDriver != null)
				{
					msg = string.Concat(new object[]
					{
						msg,
						" curDriver=",
						pawn.jobs.curDriver.GetType().Name,
						" (toilIndex=",
						pawn.jobs.curDriver.CurToilIndex,
						")"
					});
				}
				if (pawn.jobs.curJob != null)
				{
					msg = msg + " curJob=(" + pawn.jobs.curJob.ToStringSafe<Job>() + ")";
				}
			}
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x00175DA4 File Offset: 0x00173FA4
		[CompilerGenerated]
		internal static void <GetResolvedJobReport>g__GetText|4_0(LocalTargetInfo x, out string backCompatibleText, out object obj)
		{
			if (!x.IsValid)
			{
				backCompatibleText = "UnknownLower".Translate();
				obj = backCompatibleText;
				return;
			}
			if (x.HasThing)
			{
				backCompatibleText = x.Thing.LabelShort;
				obj = x.Thing;
				return;
			}
			backCompatibleText = "AreaLower".Translate();
			obj = backCompatibleText;
		}

		// Token: 0x04002AB2 RID: 10930
		private static bool startingErrorRecoverJob;
	}
}
