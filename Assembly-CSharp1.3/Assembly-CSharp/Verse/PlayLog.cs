using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000138 RID: 312
	public class PlayLog : IExposable
	{
		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x00028089 File Offset: 0x00026289
		public List<LogEntry> AllEntries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x00028091 File Offset: 0x00026291
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

		// Token: 0x06000899 RID: 2201 RVA: 0x000280B3 File Offset: 0x000262B3
		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			this.ReduceToCapacity();
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x000280C8 File Offset: 0x000262C8
		private void ReduceToCapacity()
		{
			while (this.entries.Count > 150)
			{
				this.RemoveEntry(this.entries[this.entries.Count - 1]);
			}
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x000280FC File Offset: 0x000262FC
		public void ExposeData()
		{
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00028114 File Offset: 0x00026314
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
						}));
					}
					this.RemoveEntry(this.entries[i]);
				}
			}
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x000281A0 File Offset: 0x000263A0
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.entries[i].Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x000281D8 File Offset: 0x000263D8
		public void Notify_IdeoRemoved(Ideo ideo)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.entries[i].Notify_IdeoRemoved(ideo);
			}
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0002820D File Offset: 0x0002640D
		private void RemoveEntry(LogEntry entry)
		{
			this.entries.Remove(entry);
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0002821C File Offset: 0x0002641C
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

		// Token: 0x04000802 RID: 2050
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x04000803 RID: 2051
		private const int Capacity = 150;
	}
}
