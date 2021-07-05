using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200136D RID: 4973
	public abstract class MainTabWindow_PawnTable : MainTabWindow
	{
		// Token: 0x17001547 RID: 5447
		// (get) Token: 0x060078B3 RID: 30899 RVA: 0x002A8E67 File Offset: 0x002A7067
		protected virtual float ExtraBottomSpace
		{
			get
			{
				return 53f;
			}
		}

		// Token: 0x17001548 RID: 5448
		// (get) Token: 0x060078B4 RID: 30900 RVA: 0x000682C5 File Offset: 0x000664C5
		protected virtual float ExtraTopSpace
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001549 RID: 5449
		// (get) Token: 0x060078B5 RID: 30901
		protected abstract PawnTableDef PawnTableDef { get; }

		// Token: 0x1700154A RID: 5450
		// (get) Token: 0x060078B6 RID: 30902 RVA: 0x002A8E6E File Offset: 0x002A706E
		protected override float Margin
		{
			get
			{
				return 6f;
			}
		}

		// Token: 0x1700154B RID: 5451
		// (get) Token: 0x060078B7 RID: 30903 RVA: 0x002A8E78 File Offset: 0x002A7078
		public override Vector2 RequestedTabSize
		{
			get
			{
				if (this.table == null)
				{
					return Vector2.zero;
				}
				return new Vector2(this.table.Size.x + this.Margin * 2f, this.table.Size.y + this.ExtraBottomSpace + this.ExtraTopSpace + this.Margin * 2f);
			}
		}

		// Token: 0x1700154C RID: 5452
		// (get) Token: 0x060078B8 RID: 30904 RVA: 0x002A8EE0 File Offset: 0x002A70E0
		protected virtual IEnumerable<Pawn> Pawns
		{
			get
			{
				return Find.CurrentMap.mapPawns.FreeColonists_NoHusks;
			}
		}

		// Token: 0x060078B9 RID: 30905 RVA: 0x002A8EF1 File Offset: 0x002A70F1
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.table == null)
			{
				this.table = this.CreateTable();
			}
			this.SetDirty();
		}

		// Token: 0x060078BA RID: 30906 RVA: 0x002A8F13 File Offset: 0x002A7113
		public override void DoWindowContents(Rect rect)
		{
			this.table.PawnTableOnGUI(new Vector2(rect.x, rect.y + this.ExtraTopSpace));
		}

		// Token: 0x060078BB RID: 30907 RVA: 0x002A8F3A File Offset: 0x002A713A
		public void Notify_PawnsChanged()
		{
			this.SetDirty();
		}

		// Token: 0x060078BC RID: 30908 RVA: 0x002A8F42 File Offset: 0x002A7142
		public override void Notify_ResolutionChanged()
		{
			this.table = this.CreateTable();
			base.Notify_ResolutionChanged();
		}

		// Token: 0x060078BD RID: 30909 RVA: 0x002A8F58 File Offset: 0x002A7158
		private PawnTable CreateTable()
		{
			return (PawnTable)Activator.CreateInstance(this.PawnTableDef.workerClass, new object[]
			{
				this.PawnTableDef,
				new Func<IEnumerable<Pawn>>(() => this.Pawns),
				UI.screenWidth - (int)(this.Margin * 2f),
				(int)((float)(UI.screenHeight - 35) - this.ExtraBottomSpace - this.ExtraTopSpace - this.Margin * 2f)
			});
		}

		// Token: 0x060078BE RID: 30910 RVA: 0x002A8FE0 File Offset: 0x002A71E0
		protected void SetDirty()
		{
			this.table.SetDirty();
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x04004312 RID: 17170
		private PawnTable table;
	}
}
