using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200101C RID: 4124
	public class Archive : IExposable
	{
		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x060059F7 RID: 23031 RVA: 0x0003E7AF File Offset: 0x0003C9AF
		public List<IArchivable> ArchivablesListForReading
		{
			get
			{
				return this.archivables;
			}
		}

		// Token: 0x060059F8 RID: 23032 RVA: 0x001D4164 File Offset: 0x001D2364
		public void ExposeData()
		{
			Scribe_Collections.Look<IArchivable>(ref this.archivables, "archivables", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<IArchivable>(ref this.pinnedArchivables, "pinnedArchivables", LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.archivables.RemoveAll((IArchivable x) => x == null);
				this.pinnedArchivables.RemoveWhere((IArchivable x) => x == null);
			}
		}

		// Token: 0x060059F9 RID: 23033 RVA: 0x001D41F8 File Offset: 0x001D23F8
		public bool Add(IArchivable archivable)
		{
			if (archivable == null)
			{
				Log.Error("Tried to add null archivable.", false);
				return false;
			}
			if (this.Contains(archivable))
			{
				return false;
			}
			this.archivables.Add(archivable);
			this.archivables.SortBy((IArchivable x) => x.CreatedTicksGame);
			this.CheckCullArchivables();
			return true;
		}

		// Token: 0x060059FA RID: 23034 RVA: 0x0003E7B7 File Offset: 0x0003C9B7
		public bool Remove(IArchivable archivable)
		{
			if (!this.Contains(archivable))
			{
				return false;
			}
			this.archivables.Remove(archivable);
			this.pinnedArchivables.Remove(archivable);
			return true;
		}

		// Token: 0x060059FB RID: 23035 RVA: 0x0003E7DF File Offset: 0x0003C9DF
		public bool Contains(IArchivable archivable)
		{
			return this.archivables.Contains(archivable);
		}

		// Token: 0x060059FC RID: 23036 RVA: 0x0003E7ED File Offset: 0x0003C9ED
		public void Pin(IArchivable archivable)
		{
			if (!this.Contains(archivable))
			{
				return;
			}
			if (this.IsPinned(archivable))
			{
				return;
			}
			this.pinnedArchivables.Add(archivable);
		}

		// Token: 0x060059FD RID: 23037 RVA: 0x0003E810 File Offset: 0x0003CA10
		public void Unpin(IArchivable archivable)
		{
			if (!this.Contains(archivable))
			{
				return;
			}
			if (!this.IsPinned(archivable))
			{
				return;
			}
			this.pinnedArchivables.Remove(archivable);
		}

		// Token: 0x060059FE RID: 23038 RVA: 0x0003E833 File Offset: 0x0003CA33
		public bool IsPinned(IArchivable archivable)
		{
			return this.pinnedArchivables.Contains(archivable);
		}

		// Token: 0x060059FF RID: 23039 RVA: 0x001D4260 File Offset: 0x001D2460
		private void CheckCullArchivables()
		{
			int num = 0;
			for (int i = 0; i < this.archivables.Count; i++)
			{
				if (!this.IsPinned(this.archivables[i]) && this.archivables[i].CanCullArchivedNow)
				{
					num++;
				}
			}
			int num2 = num - 200;
			int num3 = 0;
			while (num3 < this.archivables.Count && num2 > 0)
			{
				if (!this.IsPinned(this.archivables[num3]) && this.archivables[num3].CanCullArchivedNow && this.Remove(this.archivables[num3]))
				{
					num2--;
					num3--;
				}
				num3++;
			}
		}

		// Token: 0x06005A00 RID: 23040 RVA: 0x001D4318 File Offset: 0x001D2518
		public void Notify_MapRemoved(Map map)
		{
			for (int i = 0; i < this.archivables.Count; i++)
			{
				LookTargets lookTargets = this.archivables[i].LookTargets;
				if (lookTargets != null)
				{
					lookTargets.Notify_MapRemoved(map);
				}
			}
		}

		// Token: 0x04003C92 RID: 15506
		private List<IArchivable> archivables = new List<IArchivable>();

		// Token: 0x04003C93 RID: 15507
		private HashSet<IArchivable> pinnedArchivables = new HashSet<IArchivable>();

		// Token: 0x04003C94 RID: 15508
		public const int MaxNonPinnedArchivables = 200;
	}
}
