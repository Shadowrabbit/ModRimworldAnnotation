using System;
using System.Threading;
using Verse;

// Token: 0x0200000A RID: 10
public class FleckParallelizationInfo
{
	// Token: 0x04000012 RID: 18
	public int startIndex;

	// Token: 0x04000013 RID: 19
	public int endIndex;

	// Token: 0x04000014 RID: 20
	public object data;

	// Token: 0x04000015 RID: 21
	public DrawBatch drawBatch = new DrawBatch();

	// Token: 0x04000016 RID: 22
	public ManualResetEvent doneEvent = new ManualResetEvent(false);
}
