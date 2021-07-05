using System;
using System.Collections.Generic;
using Verse;

// Token: 0x0200000B RID: 11
public static class FleckUtility
{
	// Token: 0x06000021 RID: 33 RVA: 0x00003867 File Offset: 0x00001A67
	public static FleckParallelizationInfo GetParallelizationInfo()
	{
		if (FleckUtility.parallelizationInfosPool.Count == 0)
		{
			return new FleckParallelizationInfo();
		}
		return FleckUtility.parallelizationInfosPool.Pop<FleckParallelizationInfo>();
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00003885 File Offset: 0x00001A85
	public static void ReturnParallelizationInfo(FleckParallelizationInfo info)
	{
		info.doneEvent.Reset();
		info.drawBatch.Flush(false);
		FleckUtility.parallelizationInfosPool.Add(info);
	}

	// Token: 0x04000017 RID: 23
	private static List<FleckParallelizationInfo> parallelizationInfosPool = new List<FleckParallelizationInfo>();
}
