using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B97 RID: 7063
	public class PawnTable
	{
		// Token: 0x17001873 RID: 6259
		// (get) Token: 0x06009BA2 RID: 39842 RVA: 0x00067882 File Offset: 0x00065A82
		public List<PawnColumnDef> ColumnsListForReading
		{
			get
			{
				return this.def.columns;
			}
		}

		// Token: 0x17001874 RID: 6260
		// (get) Token: 0x06009BA3 RID: 39843 RVA: 0x0006788F File Offset: 0x00065A8F
		public PawnColumnDef SortingBy
		{
			get
			{
				return this.sortByColumn;
			}
		}

		// Token: 0x17001875 RID: 6261
		// (get) Token: 0x06009BA4 RID: 39844 RVA: 0x00067897 File Offset: 0x00065A97
		public bool SortingDescending
		{
			get
			{
				return this.SortingBy != null && this.sortDescending;
			}
		}

		// Token: 0x17001876 RID: 6262
		// (get) Token: 0x06009BA5 RID: 39845 RVA: 0x000678A9 File Offset: 0x00065AA9
		public Vector2 Size
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedSize;
			}
		}

		// Token: 0x17001877 RID: 6263
		// (get) Token: 0x06009BA6 RID: 39846 RVA: 0x000678B7 File Offset: 0x00065AB7
		public float HeightNoScrollbar
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeightNoScrollbar;
			}
		}

		// Token: 0x17001878 RID: 6264
		// (get) Token: 0x06009BA7 RID: 39847 RVA: 0x000678C5 File Offset: 0x00065AC5
		public float HeaderHeight
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeaderHeight;
			}
		}

		// Token: 0x17001879 RID: 6265
		// (get) Token: 0x06009BA8 RID: 39848 RVA: 0x000678D3 File Offset: 0x00065AD3
		public List<Pawn> PawnsListForReading
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedPawns;
			}
		}

		// Token: 0x06009BA9 RID: 39849 RVA: 0x002D9DE4 File Offset: 0x002D7FE4
		public PawnTable(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight)
		{
			this.def = def;
			this.pawnsGetter = pawnsGetter;
			this.SetMinMaxSize(def.minWidth, uiWidth, 0, uiHeight);
			this.SetDirty();
		}

		// Token: 0x06009BAA RID: 39850 RVA: 0x002D9E60 File Offset: 0x002D8060
		public void PawnTableOnGUI(Vector2 position)
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			this.RecacheIfDirty();
			float num = this.cachedSize.x - 16f;
			int num2 = 0;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				int num3;
				if (i == this.def.columns.Count - 1)
				{
					num3 = (int)(num - (float)num2);
				}
				else
				{
					num3 = (int)this.cachedColumnWidths[i];
				}
				Rect rect = new Rect((float)((int)position.x + num2), (float)((int)position.y), (float)num3, (float)((int)this.cachedHeaderHeight));
				this.def.columns[i].Worker.DoHeader(rect, this);
				num2 += num3;
			}
			Rect outRect = new Rect((float)((int)position.x), (float)((int)position.y + (int)this.cachedHeaderHeight), (float)((int)this.cachedSize.x), (float)((int)this.cachedSize.y - (int)this.cachedHeaderHeight));
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, (float)((int)this.cachedHeightNoScrollbar - (int)this.cachedHeaderHeight));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			int num4 = 0;
			for (int j = 0; j < this.cachedPawns.Count; j++)
			{
				num2 = 0;
				if ((float)num4 - this.scrollPosition.y + (float)((int)this.cachedRowHeights[j]) >= 0f && (float)num4 - this.scrollPosition.y <= outRect.height)
				{
					GUI.color = new Color(1f, 1f, 1f, 0.2f);
					Widgets.DrawLineHorizontal(0f, (float)num4, viewRect.width);
					GUI.color = Color.white;
					if (!this.CanAssignPawn(this.cachedPawns[j]))
					{
						GUI.color = Color.gray;
					}
					Rect rect2 = new Rect(0f, (float)num4, viewRect.width, (float)((int)this.cachedRowHeights[j]));
					if (Mouse.IsOver(rect2))
					{
						GUI.DrawTexture(rect2, TexUI.HighlightTex);
						this.cachedLookTargets[j].Highlight(true, this.cachedPawns[j].IsColonist, false);
					}
					for (int k = 0; k < this.def.columns.Count; k++)
					{
						int num5;
						if (k == this.def.columns.Count - 1)
						{
							num5 = (int)(num - (float)num2);
						}
						else
						{
							num5 = (int)this.cachedColumnWidths[k];
						}
						Rect rect3 = new Rect((float)num2, (float)num4, (float)num5, (float)((int)this.cachedRowHeights[j]));
						this.def.columns[k].Worker.DoCell(rect3, this.cachedPawns[j], this);
						num2 += num5;
					}
					if (this.cachedPawns[j].Downed)
					{
						GUI.color = new Color(1f, 0f, 0f, 0.5f);
						Widgets.DrawLineHorizontal(0f, rect2.center.y, viewRect.width);
					}
					GUI.color = Color.white;
				}
				num4 += (int)this.cachedRowHeights[j];
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06009BAB RID: 39851 RVA: 0x000678E1 File Offset: 0x00065AE1
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06009BAC RID: 39852 RVA: 0x000678EA File Offset: 0x00065AEA
		public void SetMinMaxSize(int minTableWidth, int maxTableWidth, int minTableHeight, int maxTableHeight)
		{
			this.minTableWidth = minTableWidth;
			this.maxTableWidth = maxTableWidth;
			this.minTableHeight = minTableHeight;
			this.maxTableHeight = maxTableHeight;
			this.hasFixedSize = false;
			this.SetDirty();
		}

		// Token: 0x06009BAD RID: 39853 RVA: 0x00067916 File Offset: 0x00065B16
		public void SetFixedSize(Vector2 size)
		{
			this.fixedSize = size;
			this.hasFixedSize = true;
			this.SetDirty();
		}

		// Token: 0x06009BAE RID: 39854 RVA: 0x0006792C File Offset: 0x00065B2C
		public void SortBy(PawnColumnDef column, bool descending)
		{
			this.sortByColumn = column;
			this.sortDescending = descending;
			this.SetDirty();
		}

		// Token: 0x06009BAF RID: 39855 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool CanAssignPawn(Pawn p)
		{
			return true;
		}

		// Token: 0x06009BB0 RID: 39856 RVA: 0x002DA1E8 File Offset: 0x002D83E8
		private void RecacheIfDirty()
		{
			if (!this.dirty)
			{
				return;
			}
			this.dirty = false;
			this.RecachePawns();
			this.RecacheRowHeights();
			this.cachedHeaderHeight = this.CalculateHeaderHeight();
			this.cachedHeightNoScrollbar = this.CalculateTotalRequiredHeight();
			this.RecacheSize();
			this.RecacheColumnWidths();
			this.RecacheLookTargets();
		}

		// Token: 0x06009BB1 RID: 39857 RVA: 0x002DA23C File Offset: 0x002D843C
		private void RecachePawns()
		{
			this.cachedPawns.Clear();
			this.cachedPawns.AddRange(this.pawnsGetter());
			this.cachedPawns = this.LabelSortFunction(this.cachedPawns).ToList<Pawn>();
			if (this.sortByColumn != null)
			{
				if (this.sortDescending)
				{
					this.cachedPawns.SortStable(new Func<Pawn, Pawn, int>(this.sortByColumn.Worker.Compare));
				}
				else
				{
					this.cachedPawns.SortStable((Pawn a, Pawn b) => this.sortByColumn.Worker.Compare(b, a));
				}
			}
			this.cachedPawns = this.PrimarySortFunction(this.cachedPawns).ToList<Pawn>();
		}

		// Token: 0x06009BB2 RID: 39858 RVA: 0x00067942 File Offset: 0x00065B42
		protected virtual IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.Label
			select p;
		}

		// Token: 0x06009BB3 RID: 39859 RVA: 0x0001037D File Offset: 0x0000E57D
		protected virtual IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return input;
		}

		// Token: 0x06009BB4 RID: 39860 RVA: 0x002DA2E4 File Offset: 0x002D84E4
		private void RecacheColumnWidths()
		{
			float num = this.cachedSize.x - 16f;
			float num2 = 0f;
			this.RecacheColumnWidths_StartWithMinWidths(out num2);
			if (num2 == num)
			{
				return;
			}
			if (num2 > num)
			{
				this.SubtractProportionally(num2 - num, num2);
				return;
			}
			bool flag;
			this.RecacheColumnWidths_DistributeUntilOptimal(num, ref num2, out flag);
			if (flag)
			{
				return;
			}
			this.RecacheColumnWidths_DistributeAboveOptimal(num, ref num2);
		}

		// Token: 0x06009BB5 RID: 39861 RVA: 0x002DA340 File Offset: 0x002D8540
		private void RecacheColumnWidths_StartWithMinWidths(out float minWidthsSum)
		{
			minWidthsSum = 0f;
			this.cachedColumnWidths.Clear();
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				float minWidth = this.GetMinWidth(this.def.columns[i]);
				this.cachedColumnWidths.Add(minWidth);
				minWidthsSum += minWidth;
			}
		}

		// Token: 0x06009BB6 RID: 39862 RVA: 0x002DA3A4 File Offset: 0x002D85A4
		private void RecacheColumnWidths_DistributeUntilOptimal(float totalAvailableSpaceForColumns, ref float usedWidth, out bool noMoreFreeSpace)
		{
			this.columnAtOptimalWidth.Clear();
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				this.columnAtOptimalWidth.Add(this.cachedColumnWidths[i] >= this.GetOptimalWidth(this.def.columns[i]));
			}
			int num = 0;
			for (;;)
			{
				num++;
				if (num >= 10000)
				{
					break;
				}
				float num2 = float.MinValue;
				for (int j = 0; j < this.def.columns.Count; j++)
				{
					if (!this.columnAtOptimalWidth[j])
					{
						num2 = Mathf.Max(num2, (float)this.def.columns[j].widthPriority);
					}
				}
				float num3 = 0f;
				for (int k = 0; k < this.cachedColumnWidths.Count; k++)
				{
					if (!this.columnAtOptimalWidth[k] && (float)this.def.columns[k].widthPriority == num2)
					{
						num3 += this.GetOptimalWidth(this.def.columns[k]);
					}
				}
				float num4 = totalAvailableSpaceForColumns - usedWidth;
				bool flag = false;
				bool flag2 = false;
				for (int l = 0; l < this.cachedColumnWidths.Count; l++)
				{
					if (!this.columnAtOptimalWidth[l])
					{
						if ((float)this.def.columns[l].widthPriority != num2)
						{
							flag = true;
						}
						else
						{
							float num5 = num4 * this.GetOptimalWidth(this.def.columns[l]) / num3;
							float num6 = this.GetOptimalWidth(this.def.columns[l]) - this.cachedColumnWidths[l];
							if (num5 >= num6)
							{
								num5 = num6;
								this.columnAtOptimalWidth[l] = true;
								flag2 = true;
							}
							else
							{
								flag = true;
							}
							if (num5 > 0f)
							{
								List<float> list = this.cachedColumnWidths;
								int index = l;
								list[index] += num5;
								usedWidth += num5;
							}
						}
					}
				}
				if (usedWidth >= totalAvailableSpaceForColumns - 0.1f)
				{
					goto Block_13;
				}
				if (!flag || !flag2)
				{
					goto IL_243;
				}
			}
			Log.Error("Too many iterations.", false);
			goto IL_243;
			Block_13:
			noMoreFreeSpace = true;
			IL_243:
			noMoreFreeSpace = false;
		}

		// Token: 0x06009BB7 RID: 39863 RVA: 0x002DA5F8 File Offset: 0x002D87F8
		private void RecacheColumnWidths_DistributeAboveOptimal(float totalAvailableSpaceForColumns, ref float usedWidth)
		{
			this.columnAtMaxWidth.Clear();
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				this.columnAtMaxWidth.Add(this.cachedColumnWidths[i] >= this.GetMaxWidth(this.def.columns[i]));
			}
			int num = 0;
			for (;;)
			{
				num++;
				if (num >= 10000)
				{
					break;
				}
				float num2 = 0f;
				for (int j = 0; j < this.def.columns.Count; j++)
				{
					if (!this.columnAtMaxWidth[j])
					{
						num2 += Mathf.Max(this.GetOptimalWidth(this.def.columns[j]), 1f);
					}
				}
				float num3 = totalAvailableSpaceForColumns - usedWidth;
				bool flag = false;
				for (int k = 0; k < this.def.columns.Count; k++)
				{
					if (!this.columnAtMaxWidth[k])
					{
						float num4 = num3 * Mathf.Max(this.GetOptimalWidth(this.def.columns[k]), 1f) / num2;
						float num5 = this.GetMaxWidth(this.def.columns[k]) - this.cachedColumnWidths[k];
						if (num4 >= num5)
						{
							num4 = num5;
							this.columnAtMaxWidth[k] = true;
						}
						else
						{
							flag = true;
						}
						if (num4 > 0f)
						{
							List<float> list = this.cachedColumnWidths;
							int index = k;
							list[index] += num4;
							usedWidth += num4;
						}
					}
				}
				if (usedWidth >= totalAvailableSpaceForColumns - 0.1f)
				{
					return;
				}
				if (!flag)
				{
					goto Block_10;
				}
			}
			Log.Error("Too many iterations.", false);
			return;
			Block_10:
			this.DistributeRemainingWidthProportionallyAboveMax(totalAvailableSpaceForColumns - usedWidth);
		}

		// Token: 0x06009BB8 RID: 39864 RVA: 0x002DA7CC File Offset: 0x002D89CC
		private void RecacheRowHeights()
		{
			this.cachedRowHeights.Clear();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				this.cachedRowHeights.Add(this.CalculateRowHeight(this.cachedPawns[i]));
			}
		}

		// Token: 0x06009BB9 RID: 39865 RVA: 0x002DA818 File Offset: 0x002D8A18
		private void RecacheSize()
		{
			if (this.hasFixedSize)
			{
				this.cachedSize = this.fixedSize;
				return;
			}
			float num = 0f;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				if (!this.def.columns[i].ignoreWhenCalculatingOptimalTableSize)
				{
					num += this.GetOptimalWidth(this.def.columns[i]);
				}
			}
			float num2 = Mathf.Clamp(num + 16f, (float)this.minTableWidth, (float)this.maxTableWidth);
			float num3 = Mathf.Clamp(this.cachedHeightNoScrollbar, (float)this.minTableHeight, (float)this.maxTableHeight);
			num2 = Mathf.Min(num2, (float)UI.screenWidth);
			num3 = Mathf.Min(num3, (float)UI.screenHeight);
			this.cachedSize = new Vector2(num2, num3);
		}

		// Token: 0x06009BBA RID: 39866 RVA: 0x002DA8EC File Offset: 0x002D8AEC
		private void RecacheLookTargets()
		{
			this.cachedLookTargets.Clear();
			this.cachedLookTargets.AddRange(from p in this.cachedPawns
			select new LookTargets(p));
		}

		// Token: 0x06009BBB RID: 39867 RVA: 0x002DA93C File Offset: 0x002D8B3C
		private void SubtractProportionally(float toSubtract, float totalUsedWidth)
		{
			for (int i = 0; i < this.cachedColumnWidths.Count; i++)
			{
				List<float> list = this.cachedColumnWidths;
				int index = i;
				list[index] -= toSubtract * this.cachedColumnWidths[i] / totalUsedWidth;
			}
		}

		// Token: 0x06009BBC RID: 39868 RVA: 0x002DA988 File Offset: 0x002D8B88
		private void DistributeRemainingWidthProportionallyAboveMax(float toDistribute)
		{
			float num = 0f;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				num += Mathf.Max(this.GetOptimalWidth(this.def.columns[i]), 1f);
			}
			for (int j = 0; j < this.def.columns.Count; j++)
			{
				List<float> list = this.cachedColumnWidths;
				int index = j;
				list[index] += toDistribute * Mathf.Max(this.GetOptimalWidth(this.def.columns[j]), 1f) / num;
			}
		}

		// Token: 0x06009BBD RID: 39869 RVA: 0x00067969 File Offset: 0x00065B69
		private float GetOptimalWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetOptimalWidth(this), 0f);
		}

		// Token: 0x06009BBE RID: 39870 RVA: 0x00067982 File Offset: 0x00065B82
		private float GetMinWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMinWidth(this), 0f);
		}

		// Token: 0x06009BBF RID: 39871 RVA: 0x0006799B File Offset: 0x00065B9B
		private float GetMaxWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMaxWidth(this), 0f);
		}

		// Token: 0x06009BC0 RID: 39872 RVA: 0x002DAA38 File Offset: 0x002D8C38
		private float CalculateRowHeight(Pawn pawn)
		{
			float num = 0f;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				num = Mathf.Max(num, (float)this.def.columns[i].Worker.GetMinCellHeight(pawn));
			}
			return num;
		}

		// Token: 0x06009BC1 RID: 39873 RVA: 0x002DAA8C File Offset: 0x002D8C8C
		private float CalculateHeaderHeight()
		{
			float num = 0f;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				num = Mathf.Max(num, (float)this.def.columns[i].Worker.GetMinHeaderHeight(this));
			}
			return num;
		}

		// Token: 0x06009BC2 RID: 39874 RVA: 0x002DAAE0 File Offset: 0x002D8CE0
		private float CalculateTotalRequiredHeight()
		{
			float num = this.CalculateHeaderHeight();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				num += this.CalculateRowHeight(this.cachedPawns[i]);
			}
			return num;
		}

		// Token: 0x0400630D RID: 25357
		private PawnTableDef def;

		// Token: 0x0400630E RID: 25358
		private Func<IEnumerable<Pawn>> pawnsGetter;

		// Token: 0x0400630F RID: 25359
		private int minTableWidth;

		// Token: 0x04006310 RID: 25360
		private int maxTableWidth;

		// Token: 0x04006311 RID: 25361
		private int minTableHeight;

		// Token: 0x04006312 RID: 25362
		private int maxTableHeight;

		// Token: 0x04006313 RID: 25363
		private Vector2 fixedSize;

		// Token: 0x04006314 RID: 25364
		private bool hasFixedSize;

		// Token: 0x04006315 RID: 25365
		private bool dirty;

		// Token: 0x04006316 RID: 25366
		private List<bool> columnAtMaxWidth = new List<bool>();

		// Token: 0x04006317 RID: 25367
		private List<bool> columnAtOptimalWidth = new List<bool>();

		// Token: 0x04006318 RID: 25368
		private Vector2 scrollPosition;

		// Token: 0x04006319 RID: 25369
		private PawnColumnDef sortByColumn;

		// Token: 0x0400631A RID: 25370
		private bool sortDescending;

		// Token: 0x0400631B RID: 25371
		private Vector2 cachedSize;

		// Token: 0x0400631C RID: 25372
		private List<Pawn> cachedPawns = new List<Pawn>();

		// Token: 0x0400631D RID: 25373
		private List<float> cachedColumnWidths = new List<float>();

		// Token: 0x0400631E RID: 25374
		private List<float> cachedRowHeights = new List<float>();

		// Token: 0x0400631F RID: 25375
		private List<LookTargets> cachedLookTargets = new List<LookTargets>();

		// Token: 0x04006320 RID: 25376
		private float cachedHeaderHeight;

		// Token: 0x04006321 RID: 25377
		private float cachedHeightNoScrollbar;
	}
}
