using System;

namespace Verse.AI
{
	// Token: 0x0200063A RID: 1594
	public struct ThinkResult : IEquatable<ThinkResult>
	{
		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06002D6C RID: 11628 RVA: 0x0010FEF5 File Offset: 0x0010E0F5
		public Job Job
		{
			get
			{
				return this.jobInt;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06002D6D RID: 11629 RVA: 0x0010FEFD File Offset: 0x0010E0FD
		public ThinkNode SourceNode
		{
			get
			{
				return this.sourceNodeInt;
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06002D6E RID: 11630 RVA: 0x0010FF05 File Offset: 0x0010E105
		public JobTag? Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06002D6F RID: 11631 RVA: 0x0010FF0D File Offset: 0x0010E10D
		public bool FromQueue
		{
			get
			{
				return this.fromQueue;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06002D70 RID: 11632 RVA: 0x0010FF15 File Offset: 0x0010E115
		public bool IsValid
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002D71 RID: 11633 RVA: 0x0010FF20 File Offset: 0x0010E120
		public static ThinkResult NoJob
		{
			get
			{
				return new ThinkResult(null, null, null, false);
			}
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x0010FF3E File Offset: 0x0010E13E
		public ThinkResult(Job job, ThinkNode sourceNode, JobTag? tag = null, bool fromQueue = false)
		{
			this.jobInt = job;
			this.sourceNodeInt = sourceNode;
			this.tag = tag;
			this.fromQueue = fromQueue;
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x0010FF60 File Offset: 0x0010E160
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

		// Token: 0x06002D74 RID: 11636 RVA: 0x0010FFCE File Offset: 0x0010E1CE
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<bool>(Gen.HashCombine<JobTag?>(Gen.HashCombine<ThinkNode>(Gen.HashCombine<Job>(0, this.jobInt), this.sourceNodeInt), this.tag), this.fromQueue);
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x0010FFFD File Offset: 0x0010E1FD
		public override bool Equals(object obj)
		{
			return obj is ThinkResult && this.Equals((ThinkResult)obj);
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x00110018 File Offset: 0x0010E218
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

		// Token: 0x06002D77 RID: 11639 RVA: 0x00110082 File Offset: 0x0010E282
		public static bool operator ==(ThinkResult lhs, ThinkResult rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x0011008C File Offset: 0x0010E28C
		public static bool operator !=(ThinkResult lhs, ThinkResult rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04001BD4 RID: 7124
		private Job jobInt;

		// Token: 0x04001BD5 RID: 7125
		private ThinkNode sourceNodeInt;

		// Token: 0x04001BD6 RID: 7126
		private JobTag? tag;

		// Token: 0x04001BD7 RID: 7127
		private bool fromQueue;
	}
}
