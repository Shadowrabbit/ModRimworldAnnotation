using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Verse
{
	// Token: 0x02000496 RID: 1174
	public class ThreadLocalDeepProfiler
	{
		// Token: 0x060023CF RID: 9167 RVA: 0x000DEDC4 File Offset: 0x000DCFC4
		static ThreadLocalDeepProfiler()
		{
			for (int i = 0; i < 50; i++)
			{
				ThreadLocalDeepProfiler.Prefixes[i] = "";
				for (int j = 0; j < i; j++)
				{
					string[] prefixes = ThreadLocalDeepProfiler.Prefixes;
					int num = i;
					prefixes[num] += " -";
				}
			}
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x000DEE1A File Offset: 0x000DD01A
		public void Start(string label = null)
		{
			if (!Prefs.LogVerbose)
			{
				return;
			}
			this.watchers.Push(new ThreadLocalDeepProfiler.Watcher(label));
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x000DEE38 File Offset: 0x000DD038
		public void End()
		{
			if (!Prefs.LogVerbose)
			{
				return;
			}
			if (this.watchers.Count == 0)
			{
				Log.Error("Ended deep profiling while not profiling.");
				return;
			}
			ThreadLocalDeepProfiler.Watcher watcher = this.watchers.Pop();
			watcher.Watch.Stop();
			if (this.watchers.Count > 0)
			{
				this.watchers.Peek().AddChildResult(watcher);
				return;
			}
			this.Output(watcher);
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x000DEEA4 File Offset: 0x000DD0A4
		private void Output(ThreadLocalDeepProfiler.Watcher root)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (UnityData.IsInMainThread)
			{
				stringBuilder.AppendLine("--- Main thread ---");
			}
			else
			{
				stringBuilder.AppendLine("--- Thread " + Thread.CurrentThread.ManagedThreadId + " ---");
			}
			List<ThreadLocalDeepProfiler.Watcher> list = new List<ThreadLocalDeepProfiler.Watcher>();
			list.Add(root);
			this.AppendStringRecursive(stringBuilder, root.Label, root.Children, root.ElapsedMilliseconds, 0, list);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			this.HotspotAnalysis(stringBuilder, list);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x000DEF3C File Offset: 0x000DD13C
		private void HotspotAnalysis(StringBuilder sb, List<ThreadLocalDeepProfiler.Watcher> allWatchers)
		{
			List<ThreadLocalDeepProfiler.LabelTimeTuple> list = new List<ThreadLocalDeepProfiler.LabelTimeTuple>();
			foreach (IGrouping<string, ThreadLocalDeepProfiler.Watcher> grouping in from w in allWatchers
			group w by w.Label)
			{
				double num = 0.0;
				double num2 = 0.0;
				int num3 = 0;
				foreach (ThreadLocalDeepProfiler.Watcher watcher in grouping)
				{
					num3++;
					num += watcher.ElapsedMilliseconds;
					if (watcher.Children != null)
					{
						foreach (ThreadLocalDeepProfiler.Watcher watcher2 in watcher.Children)
						{
							num2 += watcher2.ElapsedMilliseconds;
						}
					}
				}
				list.Add(new ThreadLocalDeepProfiler.LabelTimeTuple
				{
					label = num3 + "x " + grouping.Key,
					totalTime = num,
					selfTime = num - num2
				});
			}
			sb.AppendLine("Hotspot analysis");
			sb.AppendLine("----------------------------------------");
			foreach (ThreadLocalDeepProfiler.LabelTimeTuple labelTimeTuple in from e in list
			orderby e.selfTime descending
			select e)
			{
				string[] array = new string[6];
				array[0] = labelTimeTuple.label;
				array[1] = " -> ";
				int num4 = 2;
				double num5 = labelTimeTuple.selfTime;
				array[num4] = num5.ToString("0.0000");
				array[3] = " ms (total (w/children): ";
				int num6 = 4;
				num5 = labelTimeTuple.totalTime;
				array[num6] = num5.ToString("0.0000");
				array[5] = " ms)";
				sb.AppendLine(string.Concat(array));
			}
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x000DF178 File Offset: 0x000DD378
		private void AppendStringRecursive(StringBuilder sb, string label, List<ThreadLocalDeepProfiler.Watcher> children, double elapsedTime, int depth, List<ThreadLocalDeepProfiler.Watcher> allWatchers)
		{
			if (children != null)
			{
				double num = elapsedTime;
				foreach (ThreadLocalDeepProfiler.Watcher watcher in children)
				{
					num -= watcher.ElapsedMilliseconds;
				}
				sb.AppendLine(string.Concat(new string[]
				{
					ThreadLocalDeepProfiler.Prefixes[depth],
					" ",
					elapsedTime.ToString("0.0000"),
					"ms (self: ",
					num.ToString("0.0000"),
					" ms) ",
					label
				}));
			}
			else
			{
				sb.AppendLine(string.Concat(new string[]
				{
					ThreadLocalDeepProfiler.Prefixes[depth],
					" ",
					elapsedTime.ToString("0.0000"),
					"ms ",
					label
				}));
			}
			if (children != null)
			{
				allWatchers.AddRange(children);
				foreach (IGrouping<string, ThreadLocalDeepProfiler.Watcher> grouping in from c in children
				group c by c.Label)
				{
					List<ThreadLocalDeepProfiler.Watcher> list = new List<ThreadLocalDeepProfiler.Watcher>();
					double num2 = 0.0;
					int num3 = 0;
					foreach (ThreadLocalDeepProfiler.Watcher watcher2 in grouping)
					{
						if (watcher2.Children != null)
						{
							foreach (ThreadLocalDeepProfiler.Watcher item in watcher2.Children)
							{
								list.Add(item);
							}
						}
						num2 += watcher2.ElapsedMilliseconds;
						num3++;
					}
					if (num3 <= 1)
					{
						this.AppendStringRecursive(sb, grouping.Key, list, num2, depth + 1, allWatchers);
					}
					else
					{
						this.AppendStringRecursive(sb, num3 + "x " + grouping.Key, list, num2, depth + 1, allWatchers);
					}
				}
			}
		}

		// Token: 0x0400169C RID: 5788
		private Stack<ThreadLocalDeepProfiler.Watcher> watchers = new Stack<ThreadLocalDeepProfiler.Watcher>();

		// Token: 0x0400169D RID: 5789
		private static readonly string[] Prefixes = new string[50];

		// Token: 0x0400169E RID: 5790
		private const int MaxDepth = 50;

		// Token: 0x02001C97 RID: 7319
		private class Watcher
		{
			// Token: 0x17001A02 RID: 6658
			// (get) Token: 0x0600A798 RID: 42904 RVA: 0x003840D0 File Offset: 0x003822D0
			public double ElapsedMilliseconds
			{
				get
				{
					return this.watch.Elapsed.TotalMilliseconds;
				}
			}

			// Token: 0x17001A03 RID: 6659
			// (get) Token: 0x0600A799 RID: 42905 RVA: 0x003840F0 File Offset: 0x003822F0
			public string Label
			{
				get
				{
					return this.label;
				}
			}

			// Token: 0x17001A04 RID: 6660
			// (get) Token: 0x0600A79A RID: 42906 RVA: 0x003840F8 File Offset: 0x003822F8
			public Stopwatch Watch
			{
				get
				{
					return this.watch;
				}
			}

			// Token: 0x17001A05 RID: 6661
			// (get) Token: 0x0600A79B RID: 42907 RVA: 0x00384100 File Offset: 0x00382300
			public List<ThreadLocalDeepProfiler.Watcher> Children
			{
				get
				{
					return this.children;
				}
			}

			// Token: 0x0600A79C RID: 42908 RVA: 0x00384108 File Offset: 0x00382308
			public Watcher(string label)
			{
				this.label = label;
				this.watch = Stopwatch.StartNew();
				this.children = null;
			}

			// Token: 0x0600A79D RID: 42909 RVA: 0x00384129 File Offset: 0x00382329
			public Watcher(string label, Stopwatch stopwatch)
			{
				this.label = label;
				this.watch = stopwatch;
				this.children = null;
			}

			// Token: 0x0600A79E RID: 42910 RVA: 0x00384146 File Offset: 0x00382346
			public void AddChildResult(ThreadLocalDeepProfiler.Watcher w)
			{
				if (this.children == null)
				{
					this.children = new List<ThreadLocalDeepProfiler.Watcher>();
				}
				this.children.Add(w);
			}

			// Token: 0x04006E43 RID: 28227
			private string label;

			// Token: 0x04006E44 RID: 28228
			private Stopwatch watch;

			// Token: 0x04006E45 RID: 28229
			private List<ThreadLocalDeepProfiler.Watcher> children;
		}

		// Token: 0x02001C98 RID: 7320
		private struct LabelTimeTuple
		{
			// Token: 0x04006E46 RID: 28230
			public string label;

			// Token: 0x04006E47 RID: 28231
			public double totalTime;

			// Token: 0x04006E48 RID: 28232
			public double selfTime;
		}
	}
}
