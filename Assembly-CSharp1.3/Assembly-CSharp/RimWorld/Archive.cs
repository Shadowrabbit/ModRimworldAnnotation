using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AF3 RID: 2803
	public class Archive : IExposable
	{
		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x060041F0 RID: 16880 RVA: 0x001616BA File Offset: 0x0015F8BA
		public List<IArchivable> ArchivablesListForReading
		{
			get
			{
				return this.archivables;
			}
		}

		// Token: 0x060041F1 RID: 16881 RVA: 0x001616C4 File Offset: 0x0015F8C4
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

		// Token: 0x060041F2 RID: 16882 RVA: 0x00161758 File Offset: 0x0015F958
		public bool Add(IArchivable archivable)
		{
			if (archivable == null)
			{
				Log.Error("Tried to add null archivable.");
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

		// Token: 0x060041F3 RID: 16883 RVA: 0x001617BC File Offset: 0x0015F9BC
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

		// Token: 0x060041F4 RID: 16884 RVA: 0x001617E4 File Offset: 0x0015F9E4
		public bool Contains(IArchivable archivable)
		{
			return this.archivables.Contains(archivable);
		}

		// Token: 0x060041F5 RID: 16885 RVA: 0x001617F2 File Offset: 0x0015F9F2
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

		// Token: 0x060041F6 RID: 16886 RVA: 0x00161815 File Offset: 0x0015FA15
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

		// Token: 0x060041F7 RID: 16887 RVA: 0x00161838 File Offset: 0x0015FA38
		public bool IsPinned(IArchivable archivable)
		{
			return this.pinnedArchivables.Contains(archivable);
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x00161848 File Offset: 0x0015FA48
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

		// Token: 0x060041F9 RID: 16889 RVA: 0x00161900 File Offset: 0x0015FB00
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

		// Token: 0x04002831 RID: 10289
		private List<IArchivable> archivables = new List<IArchivable>();

		// Token: 0x04002832 RID: 10290
		private HashSet<IArchivable> pinnedArchivables = new HashSet<IArchivable>();

		// Token: 0x04002833 RID: 10291
		public const int MaxNonPinnedArchivables = 200;
	}
}
