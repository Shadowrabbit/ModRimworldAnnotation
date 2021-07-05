using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005BD RID: 1469
	public static class JobUtility
	{
		// Token: 0x06002ADF RID: 10975 RVA: 0x00100F50 File Offset: 0x000FF150
		public static void TryStartErrorRecoverJob(Pawn pawn, string message, Exception exception = null, JobDriver concreteDriver = null)
		{
			string text = message;
			JobUtility.AppendVarsInfoToDebugMessage(pawn, ref text, concreteDriver);
			if (exception != null)
			{
				text = text + "\n" + exception;
			}
			Log.Error(text);
			if (pawn.jobs != null)
			{
				if (pawn.jobs.curJob != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Errored, false, true);
				}
				if (JobUtility.startingErrorRecoverJob)
				{
					Log.Error("An error occurred while starting an error recover job. We have to stop now to avoid infinite loops. This means that the pawn is now jobless which can cause further bugs. pawn=" + pawn.ToStringSafe<Pawn>());
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

		// Token: 0x06002AE0 RID: 10976 RVA: 0x00101008 File Offset: 0x000FF208
		public static string GetResolvedJobReport(string baseText, LocalTargetInfo a)
		{
			return JobUtility.GetResolvedJobReport(baseText, a, LocalTargetInfo.Invalid, LocalTargetInfo.Invalid);
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x0010101B File Offset: 0x000FF21B
		public static string GetResolvedJobReport(string baseText, LocalTargetInfo a, LocalTargetInfo b)
		{
			return JobUtility.GetResolvedJobReport(baseText, a, b, LocalTargetInfo.Invalid);
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x0010102C File Offset: 0x000FF22C
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

		// Token: 0x06002AE3 RID: 10979 RVA: 0x00101068 File Offset: 0x000FF268
		public static string GetResolvedJobReportRaw(string baseText, string aText, object aObj, string bText, object bObj, string cText, object cObj)
		{
			baseText = baseText.Formatted(aObj.Named("TargetA"), bObj.Named("TargetB"), cObj.Named("TargetC"));
			baseText = baseText.Replace("TargetA", aText);
			baseText = baseText.Replace("TargetB", bText);
			baseText = baseText.Replace("TargetC", cText);
			return baseText;
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x001010D4 File Offset: 0x000FF2D4
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

		// Token: 0x06002AE6 RID: 10982 RVA: 0x001011F8 File Offset: 0x000FF3F8
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

		// Token: 0x04001A55 RID: 6741
		private static bool startingErrorRecoverJob;
	}
}
