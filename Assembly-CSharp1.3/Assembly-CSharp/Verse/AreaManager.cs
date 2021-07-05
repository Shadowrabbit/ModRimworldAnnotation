using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x0200017E RID: 382
	public class AreaManager : IExposable
	{
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x0003B322 File Offset: 0x00039522
		public List<Area> AllAreas
		{
			get
			{
				return this.areas;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x0003B32A File Offset: 0x0003952A
		public Area_Home Home
		{
			get
			{
				return this.Get<Area_Home>();
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x0003B332 File Offset: 0x00039532
		public Area_BuildRoof BuildRoof
		{
			get
			{
				return this.Get<Area_BuildRoof>();
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x0003B33A File Offset: 0x0003953A
		public Area_NoRoof NoRoof
		{
			get
			{
				return this.Get<Area_NoRoof>();
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x0003B342 File Offset: 0x00039542
		public Area_SnowClear SnowClear
		{
			get
			{
				return this.Get<Area_SnowClear>();
			}
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0003B34A File Offset: 0x0003954A
		public AreaManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0003B364 File Offset: 0x00039564
		public void AddStartingAreas()
		{
			this.areas.Add(new Area_Home(this));
			this.areas.Add(new Area_BuildRoof(this));
			this.areas.Add(new Area_NoRoof(this));
			this.areas.Add(new Area_SnowClear(this));
			Area_Allowed area_Allowed;
			this.TryMakeNewAllowed(out area_Allowed);
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0003B3BE File Offset: 0x000395BE
		public void ExposeData()
		{
			Scribe_Collections.Look<Area>(ref this.areas, "areas", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateAllAreasLinks();
			}
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0003B3E4 File Offset: 0x000395E4
		public void AreaManagerUpdate()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].AreaUpdate();
			}
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x0003B418 File Offset: 0x00039618
		internal void Remove(Area area)
		{
			if (!area.Mutable)
			{
				Log.Error("Tried to delete non-Deletable area " + area);
				return;
			}
			this.areas.Remove(area);
			this.NotifyEveryoneAreaRemoved(area);
			if (Designator_AreaAllowed.SelectedArea == area)
			{
				Designator_AreaAllowed.ClearSelectedArea();
			}
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x0003B454 File Offset: 0x00039654
		public Area GetLabeled(string s)
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				if (this.areas[i].Label == s)
				{
					return this.areas[i];
				}
			}
			return null;
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0003B4A0 File Offset: 0x000396A0
		public T Get<T>() where T : Area
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				T t = this.areas[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0003B4ED File Offset: 0x000396ED
		private void SortAreas()
		{
			this.areas.InsertionSort((Area a, Area b) => b.ListPriority.CompareTo(a.ListPriority));
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0003B51C File Offset: 0x0003971C
		private void UpdateAllAreasLinks()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].areaManager = this;
			}
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0003B554 File Offset: 0x00039754
		private void NotifyEveryoneAreaRemoved(Area area)
		{
			foreach (Pawn pawn in PawnsFinder.All_AliveOrDead)
			{
				if (pawn.playerSettings != null)
				{
					pawn.playerSettings.Notify_AreaRemoved(area);
				}
			}
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0003B5B4 File Offset: 0x000397B4
		public void Notify_MapRemoved()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.NotifyEveryoneAreaRemoved(this.areas[i]);
			}
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0003B5E9 File Offset: 0x000397E9
		public bool CanMakeNewAllowed()
		{
			return (from a in this.areas
			where a is Area_Allowed
			select a).Count<Area>() < 10;
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0003B61E File Offset: 0x0003981E
		public bool TryMakeNewAllowed(out Area_Allowed area)
		{
			if (!this.CanMakeNewAllowed())
			{
				area = null;
				return false;
			}
			area = new Area_Allowed(this, null);
			this.areas.Add(area);
			this.SortAreas();
			return true;
		}

		// Token: 0x04000917 RID: 2327
		public Map map;

		// Token: 0x04000918 RID: 2328
		private List<Area> areas = new List<Area>();

		// Token: 0x04000919 RID: 2329
		public const int MaxAllowedAreas = 10;
	}
}
