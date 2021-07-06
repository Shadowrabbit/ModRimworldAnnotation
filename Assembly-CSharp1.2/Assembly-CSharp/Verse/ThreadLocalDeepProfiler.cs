using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Verse
{
	// Token: 0x02000804 RID: 2052
	public class ThreadLocalDeepProfiler
	{
		// Token: 0x060033BD RID: 13245 RVA: 0x00150710 File Offset: 0x0014E910
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

		// Token: 0x060033BE RID: 13246 RVA: 0x0002896D File Offset: 0x00026B6D
		public void Start(string label = null)
		{
			if (!Prefs.LogVerbose)
			{
				return;
			}
			this.watchers.Push(new ThreadLocalDeepProfiler.Watcher(label));
		}

		// Token: 0x060033BF RID: 13247 RVA: 0x00150768 File Offset: 0x0014E968
		public void End()
		{
			if (!Prefs.LogVerbose)
			{
				return;
			}
			if (this.watchers.Count == 0)
			{
				Log.Error("Ended deep profiling while not profiling.", false);
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

		// Token: 0x060033C0 RID: 13248 RVA: 0x001507D4 File Offset: 0x0014E9D4
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
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x060033C1 RID: 13249 RVA: 0x0015086C File Offset: 0x0014EA6C
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

		// Token: 0x060033C2 RID: 13250 RVA: 0x00150AA8 File Offset: 0x0014ECA8
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

		// Token: 0x040023E4 RID: 9188
		private Stack<ThreadLocalDeepProfiler.Watcher> watchers = new Stack<ThreadLocalDeepProfiler.Watcher>();

		// Token: 0x040023E5 RID: 9189
		private static readonly string[] Prefixes = new string[50];

		// Token: 0x040023E6 RID: 9190
		private const int MaxDepth = 50;

		// Token: 0x02000805 RID: 2053
		private class Watcher
		{
			// Token: 0x170007D8 RID: 2008
			// (get) Token: 0x060033C4 RID: 13252 RVA: 0x00150CF8 File Offset: 0x0014EEF8
			public double ElapsedMilliseconds
			{
				get
				{
					return this.watch.Elapsed.TotalMilliseconds;
				}
			}

			// Token: 0x170007D9 RID: 2009
			// (get) Token: 0x060033C5 RID: 13253 RVA: 0x0002899B File Offset: 0x00026B9B
			public string Label
			{
				get
				{
					return this.label;
				}
			}

			// Token: 0x170007DA RID: 2010
			// (get) Token: 0x060033C6 RID: 13254 RVA: 0x000289A3 File Offset: 0x00026BA3
			public Stopwatch Watch
			{
				get
				{
					return this.watch;
				}
			}

			// Token: 0x170007DB RID: 2011
			// (get) Token: 0x060033C7 RID: 13255 RVA: 0x000289AB File Offset: 0x00026BAB
			public List<ThreadLocalDeepProfiler.Watcher> Children
			{
				get
				{
					return this.children;
				}
			}

			// Token: 0x060033C8 RID: 13256 RVA: 0x000289B3 File Offset: 0x00026BB3
			public Watcher(string label)
			{
				this.label = label;
				this.watch = Stopwatch.StartNew();
				this.children = null;
			}

			// Token: 0x060033C9 RID: 13257 RVA: 0x000289D4 File Offset: 0x00026BD4
			public Watcher(string label, Stopwatch stopwatch)
			{
				this.label = label;
				this.watch = stopwatch;
				this.children = null;
			}

			// Token: 0x060033CA RID: 13258 RVA: 0x000289F1 File Offset: 0x00026BF1
			public void AddChildResult(ThreadLocalDeepProfiler.Watcher w)
			{
				if (this.children == null)
				{
					this.children = new List<ThreadLocalDeepProfiler.Watcher>();
				}
				this.children.Add(w);
			}

			// Token: 0x040023E7 RID: 9191
			private string label;

			// Token: 0x040023E8 RID: 9192
			private Stopwatch watch;

			// Token: 0x040023E9 RID: 9193
			private List<ThreadLocalDeepProfiler.Watcher> children;
		}

		// Token: 0x02000806 RID: 2054
		private struct LabelTimeTuple
		{
			// Token: 0x040023EA RID: 9194
			public string label;

			// Token: 0x040023EB RID: 9195
			public double totalTime;

			// Token: 0x040023EC RID: 9196
			public double selfTime;
		}
	}
}
