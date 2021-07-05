using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000498 RID: 1176
	public static class ProfilerPairValidation
	{
		// Token: 0x060023DD RID: 9181 RVA: 0x000DF56A File Offset: 0x000DD76A
		public static void BeginSample(string token)
		{
			ProfilerPairValidation.profilerSignatures.Push(new StackTrace(1, true));
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x000DF580 File Offset: 0x000DD780
		public static void EndSample()
		{
			StackTrace stackTrace = ProfilerPairValidation.profilerSignatures.Pop();
			StackTrace stackTrace2 = new StackTrace(1, true);
			if (stackTrace2.FrameCount != stackTrace.FrameCount)
			{
				Log.Message(string.Format("Mismatch:\n{0}\n\n{1}", stackTrace.ToString(), stackTrace2.ToString()));
				return;
			}
			for (int i = 0; i < stackTrace2.FrameCount; i++)
			{
				if (stackTrace2.GetFrame(i).GetMethod() != stackTrace.GetFrame(i).GetMethod() && (!(stackTrace.GetFrame(i).GetMethod().DeclaringType == typeof(ProfilerThreadCheck)) || !(stackTrace2.GetFrame(i).GetMethod().DeclaringType == typeof(ProfilerThreadCheck))) && (!(stackTrace.GetFrame(i).GetMethod() == typeof(PathFinder).GetMethod("PfProfilerBeginSample", BindingFlags.Instance | BindingFlags.NonPublic)) || !(stackTrace2.GetFrame(i).GetMethod() == typeof(PathFinder).GetMethod("PfProfilerEndSample", BindingFlags.Instance | BindingFlags.NonPublic))))
				{
					Log.Message(string.Format("Mismatch:\n{0}\n\n{1}", stackTrace.ToString(), stackTrace2.ToString()));
					return;
				}
			}
		}

		// Token: 0x040016A1 RID: 5793
		public static Stack<StackTrace> profilerSignatures = new Stack<StackTrace>();
	}
}
