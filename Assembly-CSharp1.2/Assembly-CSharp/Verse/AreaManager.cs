using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000236 RID: 566
	public class AreaManager : IExposable
	{
		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x00010EE9 File Offset: 0x0000F0E9
		public List<Area> AllAreas
		{
			get
			{
				return this.areas;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x00010EF1 File Offset: 0x0000F0F1
		public Area_Home Home
		{
			get
			{
				return this.Get<Area_Home>();
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000E8D RID: 3725 RVA: 0x00010EF9 File Offset: 0x0000F0F9
		public Area_BuildRoof BuildRoof
		{
			get
			{
				return this.Get<Area_BuildRoof>();
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x00010F01 File Offset: 0x0000F101
		public Area_NoRoof NoRoof
		{
			get
			{
				return this.Get<Area_NoRoof>();
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000E8F RID: 3727 RVA: 0x00010F09 File Offset: 0x0000F109
		public Area_SnowClear SnowClear
		{
			get
			{
				return this.Get<Area_SnowClear>();
			}
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00010F11 File Offset: 0x0000F111
		public AreaManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x000B3570 File Offset: 0x000B1770
		public void AddStartingAreas()
		{
			this.areas.Add(new Area_Home(this));
			this.areas.Add(new Area_BuildRoof(this));
			this.areas.Add(new Area_NoRoof(this));
			this.areas.Add(new Area_SnowClear(this));
			Area_Allowed area_Allowed;
			this.TryMakeNewAllowed(out area_Allowed);
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00010F2B File Offset: 0x0000F12B
		public void ExposeData()
		{
			Scribe_Collections.Look<Area>(ref this.areas, "areas", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateAllAreasLinks();
			}
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x000B35CC File Offset: 0x000B17CC
		public void AreaManagerUpdate()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].AreaUpdate();
			}
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00010F51 File Offset: 0x0000F151
		internal void Remove(Area area)
		{
			if (!area.Mutable)
			{
				Log.Error("Tried to delete non-Deletable area " + area, false);
				return;
			}
			this.areas.Remove(area);
			this.NotifyEveryoneAreaRemoved(area);
			if (Designator_AreaAllowed.SelectedArea == area)
			{
				Designator_AreaAllowed.ClearSelectedArea();
			}
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x000B3600 File Offset: 0x000B1800
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

		// Token: 0x06000E96 RID: 3734 RVA: 0x000B364C File Offset: 0x000B184C
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

		// Token: 0x06000E97 RID: 3735 RVA: 0x00010F8E File Offset: 0x0000F18E
		private void SortAreas()
		{
			this.areas.InsertionSort((Area a, Area b) => b.ListPriority.CompareTo(a.ListPriority));
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x000B369C File Offset: 0x000B189C
		private void UpdateAllAreasLinks()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].areaManager = this;
			}
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x000B36D4 File Offset: 0x000B18D4
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

		// Token: 0x06000E9A RID: 3738 RVA: 0x000B3734 File Offset: 0x000B1934
		public void Notify_MapRemoved()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.NotifyEveryoneAreaRemoved(this.areas[i]);
			}
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00010FBA File Offset: 0x0000F1BA
		public bool CanMakeNewAllowed()
		{
			return (from a in this.areas
			where a is Area_Allowed
			select a).Count<Area>() < 10;
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00010FEF File Offset: 0x0000F1EF
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

		// Token: 0x04000C06 RID: 3078
		public Map map;

		// Token: 0x04000C07 RID: 3079
		private List<Area> areas = new List<Area>();

		// Token: 0x04000C08 RID: 3080
		public const int MaxAllowedAreas = 10;
	}
}
