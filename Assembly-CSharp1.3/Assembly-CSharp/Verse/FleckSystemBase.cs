using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200019A RID: 410
	public class FleckSystemBase<TFleck> : FleckSystem where TFleck : struct, IFleck
	{
		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x0001276E File Offset: 0x0001096E
		protected virtual bool ParallelizedDrawing
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0003E664 File Offset: 0x0003C864
		public override void Update()
		{
			try
			{
				for (int i = this.dataRealtime.Count - 1; i >= 0; i--)
				{
					TFleck value = this.dataRealtime[i];
					if (value.TimeInterval(Time.deltaTime, this.parent.parent))
					{
						this.tmpRemoveIndices.Add(i);
					}
					else
					{
						this.dataRealtime[i] = value;
					}
				}
				this.dataRealtime.RemoveBatchUnordered(this.tmpRemoveIndices);
			}
			finally
			{
				this.tmpRemoveIndices.Clear();
			}
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0003E700 File Offset: 0x0003C900
		public override void Tick()
		{
			try
			{
				for (int i = this.dataGametime.Count - 1; i >= 0; i--)
				{
					TFleck value = this.dataGametime[i];
					if (value.TimeInterval(0.016666668f, this.parent.parent))
					{
						this.tmpRemoveIndices.Add(i);
					}
					else
					{
						this.dataGametime[i] = value;
					}
				}
				this.dataGametime.RemoveBatchUnordered(this.tmpRemoveIndices);
			}
			finally
			{
				this.tmpRemoveIndices.Clear();
			}
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0003E79C File Offset: 0x0003C99C
		private static void DrawParallel(object state)
		{
			FleckParallelizationInfo fleckParallelizationInfo = (FleckParallelizationInfo)state;
			try
			{
				List<FleckThrown> list = (List<FleckThrown>)fleckParallelizationInfo.data;
				for (int i = fleckParallelizationInfo.startIndex; i < fleckParallelizationInfo.endIndex; i++)
				{
					list[i].Draw(fleckParallelizationInfo.drawBatch);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			finally
			{
				fleckParallelizationInfo.doneEvent.Set();
			}
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0003E820 File Offset: 0x0003CA20
		public override void Draw(DrawBatch drawBatch)
		{
			FleckSystemBase<TFleck>.<>c__DisplayClass11_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.drawBatch = drawBatch;
			foreach (FleckDef fleckDef in this.handledDefs)
			{
				if (fleckDef.graphicData != null)
				{
					fleckDef.graphicData.ExplicitlyInitCachedGraphic();
				}
				if (fleckDef.randomGraphics != null)
				{
					foreach (GraphicData graphicData in fleckDef.randomGraphics)
					{
						graphicData.ExplicitlyInitCachedGraphic();
					}
				}
			}
			if (this.ParallelizedDrawing)
			{
				if (this.CachedDrawParallelWaitCallback == null)
				{
					this.CachedDrawParallelWaitCallback = new WaitCallback(FleckSystemBase<TFleck>.DrawParallel);
				}
				FleckSystemBase<TFleck>.<>c__DisplayClass11_1 CS$<>8__locals2;
				CS$<>8__locals2.parallelizationDegree = Environment.ProcessorCount;
				this.<Draw>g__Process|11_1(this.dataRealtime, ref CS$<>8__locals1, ref CS$<>8__locals2);
				this.<Draw>g__Process|11_1(this.dataGametime, ref CS$<>8__locals1, ref CS$<>8__locals2);
				return;
			}
			this.<Draw>g__Process|11_0(this.dataRealtime, ref CS$<>8__locals1);
			this.<Draw>g__Process|11_0(this.dataGametime, ref CS$<>8__locals1);
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0003E944 File Offset: 0x0003CB44
		public override void CreateFleck(FleckCreationData creationData)
		{
			TFleck item = Activator.CreateInstance<TFleck>();
			item.Setup(creationData);
			if (creationData.def.realTime)
			{
				this.dataRealtime.Add(item);
				return;
			}
			this.dataGametime.Add(item);
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0000313F File Offset: 0x0000133F
		public override void ExposeData()
		{
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0003E9CC File Offset: 0x0003CBCC
		[CompilerGenerated]
		private void <Draw>g__Process|11_1(List<TFleck> data, ref FleckSystemBase<TFleck>.<>c__DisplayClass11_0 A_2, ref FleckSystemBase<TFleck>.<>c__DisplayClass11_1 A_3)
		{
			if (data.Count > 0)
			{
				try
				{
					this.tmpParallelizationSlices.Clear();
					GenThreading.SliceWorkNoAlloc(0, data.Count, A_3.parallelizationDegree, this.tmpParallelizationSlices);
					foreach (GenThreading.Slice slice in this.tmpParallelizationSlices)
					{
						FleckParallelizationInfo parallelizationInfo = FleckUtility.GetParallelizationInfo();
						parallelizationInfo.startIndex = slice.fromInclusive;
						parallelizationInfo.endIndex = slice.toExclusive;
						parallelizationInfo.data = data;
						ThreadPool.QueueUserWorkItem(this.CachedDrawParallelWaitCallback, parallelizationInfo);
						this.tmpParallelizationInfo.Add(parallelizationInfo);
					}
					foreach (FleckParallelizationInfo fleckParallelizationInfo in this.tmpParallelizationInfo)
					{
						fleckParallelizationInfo.doneEvent.WaitOne();
						A_2.drawBatch.MergeWith(fleckParallelizationInfo.drawBatch);
					}
				}
				finally
				{
					foreach (FleckParallelizationInfo info in this.tmpParallelizationInfo)
					{
						FleckUtility.ReturnParallelizationInfo(info);
					}
					this.tmpParallelizationInfo.Clear();
				}
			}
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0003EB38 File Offset: 0x0003CD38
		[CompilerGenerated]
		private void <Draw>g__Process|11_0(List<TFleck> data, ref FleckSystemBase<TFleck>.<>c__DisplayClass11_0 A_2)
		{
			for (int i = data.Count - 1; i >= 0; i--)
			{
				TFleck tfleck = data[i];
				tfleck.Draw(A_2.drawBatch);
			}
		}

		// Token: 0x04000997 RID: 2455
		private List<TFleck> dataRealtime = new List<TFleck>();

		// Token: 0x04000998 RID: 2456
		private List<TFleck> dataGametime = new List<TFleck>();

		// Token: 0x04000999 RID: 2457
		private List<GenThreading.Slice> tmpParallelizationSlices = new List<GenThreading.Slice>();

		// Token: 0x0400099A RID: 2458
		private List<FleckParallelizationInfo> tmpParallelizationInfo = new List<FleckParallelizationInfo>();

		// Token: 0x0400099B RID: 2459
		private List<int> tmpRemoveIndices = new List<int>();

		// Token: 0x0400099C RID: 2460
		private WaitCallback CachedDrawParallelWaitCallback;
	}
}
