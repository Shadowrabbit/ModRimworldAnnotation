using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200080A RID: 2058
	public static class ProfilerPairValidation
	{
		// Token: 0x060033D9 RID: 13273 RVA: 0x00028A93 File Offset: 0x00026C93
		public static void BeginSample(string token)
		{
			ProfilerPairValidation.profilerSignatures.Push(new StackTrace(1, true));
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x00150E50 File Offset: 0x0014F050
		public static void EndSample()
		{
			StackTrace stackTrace = ProfilerPairValidation.profilerSignatures.Pop();
			StackTrace stackTrace2 = new StackTrace(1, true);
			if (stackTrace2.FrameCount != stackTrace.FrameCount)
			{
				Log.Message(string.Format("Mismatch:\n{0}\n\n{1}", stackTrace.ToString(), stackTrace2.ToString()), false);
				return;
			}
			for (int i = 0; i < stackTrace2.FrameCount; i++)
			{
				if (stackTrace2.GetFrame(i).GetMethod() != stackTrace.GetFrame(i).GetMethod() && (!(stackTrace.GetFrame(i).GetMethod().DeclaringType == typeof(ProfilerThreadCheck)) || !(stackTrace2.GetFrame(i).GetMethod().DeclaringType == typeof(ProfilerThreadCheck))) && (!(stackTrace.GetFrame(i).GetMethod() == typeof(PathFinder).GetMethod("PfProfilerBeginSample", BindingFlags.Instance | BindingFlags.NonPublic)) || !(stackTrace2.GetFrame(i).GetMethod() == typeof(PathFinder).GetMethod("PfProfilerEndSample", BindingFlags.Instance | BindingFlags.NonPublic))))
				{
					Log.Message(string.Format("Mismatch:\n{0}\n\n{1}", stackTrace.ToString(), stackTrace2.ToString()), false);
					return;
				}
			}
		}

		// Token: 0x040023F4 RID: 9204
		public static Stack<StackTrace> profilerSignatures = new Stack<StackTrace>();
	}
}
