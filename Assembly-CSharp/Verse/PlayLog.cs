using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001D9 RID: 473
	public class PlayLog : IExposable
	{
		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x0000F755 File Offset: 0x0000D955
		public List<LogEntry> AllEntries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x0000F75D File Offset: 0x0000D95D
		public int LastTick
		{
			get
			{
				if (this.entries.Count == 0)
				{
					return 0;
				}
				return this.entries[0].Tick;
			}
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x0000F77F File Offset: 0x0000D97F
		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			this.ReduceToCapacity();
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x0000F794 File Offset: 0x0000D994
		private void ReduceToCapacity()
		{
			while (this.entries.Count > 150)
			{
				this.RemoveEntry(this.entries[this.entries.Count - 1]);
			}
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x0000F7C8 File Offset: 0x0000D9C8
		public void ExposeData()
		{
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x000A3BD4 File Offset: 0x000A1DD4
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int i = this.entries.Count - 1; i >= 0; i--)
			{
				if (this.entries[i].Concerns(p))
				{
					if (!silentlyRemoveReferences)
					{
						Log.Warning(string.Concat(new object[]
						{
							"Discarding pawn ",
							p,
							", but he is referenced by a play log entry ",
							this.entries[i],
							"."
						}), false);
					}
					this.RemoveEntry(this.entries[i]);
				}
			}
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0000F7E0 File Offset: 0x0000D9E0
		private void RemoveEntry(LogEntry entry)
		{
			this.entries.Remove(entry);
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x000A3C60 File Offset: 0x000A1E60
		public bool AnyEntryConcerns(Pawn p)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000AAC RID: 2732
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x04000AAD RID: 2733
		private const int Capacity = 150;
	}
}
