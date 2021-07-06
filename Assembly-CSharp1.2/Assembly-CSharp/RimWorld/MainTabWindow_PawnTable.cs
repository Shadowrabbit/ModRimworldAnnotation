using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B41 RID: 6977
	public abstract class MainTabWindow_PawnTable : MainTabWindow
	{
		// Token: 0x17001849 RID: 6217
		// (get) Token: 0x060099AF RID: 39343 RVA: 0x0006673A File Offset: 0x0006493A
		protected virtual float ExtraBottomSpace
		{
			get
			{
				return 53f;
			}
		}

		// Token: 0x1700184A RID: 6218
		// (get) Token: 0x060099B0 RID: 39344 RVA: 0x00016647 File Offset: 0x00014847
		protected virtual float ExtraTopSpace
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700184B RID: 6219
		// (get) Token: 0x060099B1 RID: 39345
		protected abstract PawnTableDef PawnTableDef { get; }

		// Token: 0x1700184C RID: 6220
		// (get) Token: 0x060099B2 RID: 39346 RVA: 0x00066741 File Offset: 0x00064941
		protected override float Margin
		{
			get
			{
				return 6f;
			}
		}

		// Token: 0x1700184D RID: 6221
		// (get) Token: 0x060099B3 RID: 39347 RVA: 0x002D2158 File Offset: 0x002D0358
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

		// Token: 0x1700184E RID: 6222
		// (get) Token: 0x060099B4 RID: 39348 RVA: 0x00066748 File Offset: 0x00064948
		protected virtual IEnumerable<Pawn> Pawns
		{
			get
			{
				return Find.CurrentMap.mapPawns.FreeColonists;
			}
		}

		// Token: 0x060099B5 RID: 39349 RVA: 0x00066759 File Offset: 0x00064959
		public override void PostOpen()
		{
			if (this.table == null)
			{
				this.table = this.CreateTable();
			}
			this.SetDirty();
		}

		// Token: 0x060099B6 RID: 39350 RVA: 0x00066775 File Offset: 0x00064975
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			this.table.PawnTableOnGUI(new Vector2(rect.x, rect.y + this.ExtraTopSpace));
		}

		// Token: 0x060099B7 RID: 39351 RVA: 0x000667A3 File Offset: 0x000649A3
		public void Notify_PawnsChanged()
		{
			this.SetDirty();
		}

		// Token: 0x060099B8 RID: 39352 RVA: 0x000667AB File Offset: 0x000649AB
		public override void Notify_ResolutionChanged()
		{
			this.table = this.CreateTable();
			base.Notify_ResolutionChanged();
		}

		// Token: 0x060099B9 RID: 39353 RVA: 0x002D21C0 File Offset: 0x002D03C0
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

		// Token: 0x060099BA RID: 39354 RVA: 0x000667BF File Offset: 0x000649BF
		protected void SetDirty()
		{
			this.table.SetDirty();
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x0400622A RID: 25130
		private PawnTable table;
	}
}
