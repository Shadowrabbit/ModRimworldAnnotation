using System;

namespace Verse.AI
{
	// Token: 0x02000A98 RID: 2712
	public struct ThinkResult : IEquatable<ThinkResult>
	{
		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x0600404C RID: 16460 RVA: 0x0003020E File Offset: 0x0002E40E
		public Job Job
		{
			get
			{
				return this.jobInt;
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x0600404D RID: 16461 RVA: 0x00030216 File Offset: 0x0002E416
		public ThinkNode SourceNode
		{
			get
			{
				return this.sourceNodeInt;
			}
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x0600404E RID: 16462 RVA: 0x0003021E File Offset: 0x0002E41E
		public JobTag? Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x0600404F RID: 16463 RVA: 0x00030226 File Offset: 0x0002E426
		public bool FromQueue
		{
			get
			{
				return this.fromQueue;
			}
		}

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x06004050 RID: 16464 RVA: 0x0003022E File Offset: 0x0002E42E
		public bool IsValid
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x06004051 RID: 16465 RVA: 0x0018266C File Offset: 0x0018086C
		public static ThinkResult NoJob
		{
			get
			{
				return new ThinkResult(null, null, null, false);
			}
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x00030239 File Offset: 0x0002E439
		public ThinkResult(Job job, ThinkNode sourceNode, JobTag? tag = null, bool fromQueue = false)
		{
			this.jobInt = job;
			this.sourceNodeInt = sourceNode;
			this.tag = tag;
			this.fromQueue = fromQueue;
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x0018268C File Offset: 0x0018088C
		public override string ToString()
		{
			string text = (this.Job != null) ? this.Job.ToString() : "null";
			string text2 = (this.SourceNode != null) ? this.SourceNode.ToString() : "null";
			return string.Concat(new string[]
			{
				"(job=",
				text,
				" sourceNode=",
				text2,
				")"
			});
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x00030258 File Offset: 0x0002E458
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<bool>(Gen.HashCombine<JobTag?>(Gen.HashCombine<ThinkNode>(Gen.HashCombine<Job>(0, this.jobInt), this.sourceNodeInt), this.tag), this.fromQueue);
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x00030287 File Offset: 0x0002E487
		public override bool Equals(object obj)
		{
			return obj is ThinkResult && this.Equals((ThinkResult)obj);
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x001826FC File Offset: 0x001808FC
		public bool Equals(ThinkResult other)
		{
			if (this.jobInt == other.jobInt && this.sourceNodeInt == other.sourceNodeInt)
			{
				JobTag? jobTag = this.tag;
				JobTag? jobTag2 = other.tag;
				if (jobTag.GetValueOrDefault() == jobTag2.GetValueOrDefault() & jobTag != null == (jobTag2 != null))
				{
					return this.fromQueue == other.fromQueue;
				}
			}
			return false;
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x0003029F File Offset: 0x0002E49F
		public static bool operator ==(ThinkResult lhs, ThinkResult rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x000302A9 File Offset: 0x0002E4A9
		public static bool operator !=(ThinkResult lhs, ThinkResult rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04002C4C RID: 11340
		private Job jobInt;

		// Token: 0x04002C4D RID: 11341
		private ThinkNode sourceNodeInt;

		// Token: 0x04002C4E RID: 11342
		private JobTag? tag;

		// Token: 0x04002C4F RID: 11343
		private bool fromQueue;
	}
}
