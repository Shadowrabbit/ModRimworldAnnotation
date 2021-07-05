using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013A0 RID: 5024
	public class PawnTable
	{
		// Token: 0x17001568 RID: 5480
		// (get) Token: 0x06007A45 RID: 31301 RVA: 0x002B21A8 File Offset: 0x002B03A8
		public List<PawnColumnDef> ColumnsListForReading
		{
			get
			{
				return this.def.columns;
			}
		}

		// Token: 0x17001569 RID: 5481
		// (get) Token: 0x06007A46 RID: 31302 RVA: 0x002B21B5 File Offset: 0x002B03B5
		public PawnColumnDef SortingBy
		{
			get
			{
				return this.sortByColumn;
			}
		}

		// Token: 0x1700156A RID: 5482
		// (get) Token: 0x06007A47 RID: 31303 RVA: 0x002B21BD File Offset: 0x002B03BD
		public bool SortingDescending
		{
			get
			{
				return this.SortingBy != null && this.sortDescending;
			}
		}

		// Token: 0x1700156B RID: 5483
		// (get) Token: 0x06007A48 RID: 31304 RVA: 0x002B21CF File Offset: 0x002B03CF
		public Vector2 Size
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedSize;
			}
		}

		// Token: 0x1700156C RID: 5484
		// (get) Token: 0x06007A49 RID: 31305 RVA: 0x002B21DD File Offset: 0x002B03DD
		public float HeightNoScrollbar
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeightNoScrollbar;
			}
		}

		// Token: 0x1700156D RID: 5485
		// (get) Token: 0x06007A4A RID: 31306 RVA: 0x002B21EB File Offset: 0x002B03EB
		public float HeaderHeight
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedHeaderHeight;
			}
		}

		// Token: 0x1700156E RID: 5486
		// (get) Token: 0x06007A4B RID: 31307 RVA: 0x002B21F9 File Offset: 0x002B03F9
		public List<Pawn> PawnsListForReading
		{
			get
			{
				this.RecacheIfDirty();
				return this.cachedPawns;
			}
		}

		// Token: 0x06007A4C RID: 31308 RVA: 0x002B2208 File Offset: 0x002B0408
		public PawnTable(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight)
		{
			this.def = def;
			this.pawnsGetter = pawnsGetter;
			this.SetMinMaxSize(def.minWidth, uiWidth, 0, uiHeight);
			this.SetDirty();
		}

		// Token: 0x06007A4D RID: 31309 RVA: 0x002B2284 File Offset: 0x002B0484
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

		// Token: 0x06007A4E RID: 31310 RVA: 0x002B2609 File Offset: 0x002B0809
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06007A4F RID: 31311 RVA: 0x002B2612 File Offset: 0x002B0812
		public void SetMinMaxSize(int minTableWidth, int maxTableWidth, int minTableHeight, int maxTableHeight)
		{
			this.minTableWidth = minTableWidth;
			this.maxTableWidth = maxTableWidth;
			this.minTableHeight = minTableHeight;
			this.maxTableHeight = maxTableHeight;
			this.hasFixedSize = false;
			this.SetDirty();
		}

		// Token: 0x06007A50 RID: 31312 RVA: 0x002B263E File Offset: 0x002B083E
		public void SetFixedSize(Vector2 size)
		{
			this.fixedSize = size;
			this.hasFixedSize = true;
			this.SetDirty();
		}

		// Token: 0x06007A51 RID: 31313 RVA: 0x002B2654 File Offset: 0x002B0854
		public void SortBy(PawnColumnDef column, bool descending)
		{
			this.sortByColumn = column;
			this.sortDescending = descending;
			this.SetDirty();
		}

		// Token: 0x06007A52 RID: 31314 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool CanAssignPawn(Pawn p)
		{
			return true;
		}

		// Token: 0x06007A53 RID: 31315 RVA: 0x002B266C File Offset: 0x002B086C
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

		// Token: 0x06007A54 RID: 31316 RVA: 0x002B26C0 File Offset: 0x002B08C0
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

		// Token: 0x06007A55 RID: 31317 RVA: 0x002B2767 File Offset: 0x002B0967
		protected virtual IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return from p in input
			orderby p.Label
			select p;
		}

		// Token: 0x06007A56 RID: 31318 RVA: 0x000210E7 File Offset: 0x0001F2E7
		protected virtual IEnumerable<Pawn> PrimarySortFunction(IEnumerable<Pawn> input)
		{
			return input;
		}

		// Token: 0x06007A57 RID: 31319 RVA: 0x002B2790 File Offset: 0x002B0990
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

		// Token: 0x06007A58 RID: 31320 RVA: 0x002B27EC File Offset: 0x002B09EC
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

		// Token: 0x06007A59 RID: 31321 RVA: 0x002B2850 File Offset: 0x002B0A50
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
					goto IL_242;
				}
			}
			Log.Error("Too many iterations.");
			goto IL_242;
			Block_13:
			noMoreFreeSpace = true;
			IL_242:
			noMoreFreeSpace = false;
		}

		// Token: 0x06007A5A RID: 31322 RVA: 0x002B2AA4 File Offset: 0x002B0CA4
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
			Log.Error("Too many iterations.");
			return;
			Block_10:
			this.DistributeRemainingWidthProportionallyAboveMax(totalAvailableSpaceForColumns - usedWidth);
		}

		// Token: 0x06007A5B RID: 31323 RVA: 0x002B2C78 File Offset: 0x002B0E78
		private void RecacheRowHeights()
		{
			this.cachedRowHeights.Clear();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				this.cachedRowHeights.Add(this.CalculateRowHeight(this.cachedPawns[i]));
			}
		}

		// Token: 0x06007A5C RID: 31324 RVA: 0x002B2CC4 File Offset: 0x002B0EC4
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

		// Token: 0x06007A5D RID: 31325 RVA: 0x002B2D98 File Offset: 0x002B0F98
		private void RecacheLookTargets()
		{
			this.cachedLookTargets.Clear();
			this.cachedLookTargets.AddRange(from p in this.cachedPawns
			select new LookTargets(p));
		}

		// Token: 0x06007A5E RID: 31326 RVA: 0x002B2DE8 File Offset: 0x002B0FE8
		private void SubtractProportionally(float toSubtract, float totalUsedWidth)
		{
			for (int i = 0; i < this.cachedColumnWidths.Count; i++)
			{
				List<float> list = this.cachedColumnWidths;
				int index = i;
				list[index] -= toSubtract * this.cachedColumnWidths[i] / totalUsedWidth;
			}
		}

		// Token: 0x06007A5F RID: 31327 RVA: 0x002B2E34 File Offset: 0x002B1034
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

		// Token: 0x06007A60 RID: 31328 RVA: 0x002B2EE1 File Offset: 0x002B10E1
		private float GetOptimalWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetOptimalWidth(this), 0f);
		}

		// Token: 0x06007A61 RID: 31329 RVA: 0x002B2EFA File Offset: 0x002B10FA
		private float GetMinWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMinWidth(this), 0f);
		}

		// Token: 0x06007A62 RID: 31330 RVA: 0x002B2F13 File Offset: 0x002B1113
		private float GetMaxWidth(PawnColumnDef column)
		{
			return Mathf.Max((float)column.Worker.GetMaxWidth(this), 0f);
		}

		// Token: 0x06007A63 RID: 31331 RVA: 0x002B2F2C File Offset: 0x002B112C
		private float CalculateRowHeight(Pawn pawn)
		{
			float num = 0f;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				num = Mathf.Max(num, (float)this.def.columns[i].Worker.GetMinCellHeight(pawn));
			}
			return num;
		}

		// Token: 0x06007A64 RID: 31332 RVA: 0x002B2F80 File Offset: 0x002B1180
		private float CalculateHeaderHeight()
		{
			float num = 0f;
			for (int i = 0; i < this.def.columns.Count; i++)
			{
				num = Mathf.Max(num, (float)this.def.columns[i].Worker.GetMinHeaderHeight(this));
			}
			return num;
		}

		// Token: 0x06007A65 RID: 31333 RVA: 0x002B2FD4 File Offset: 0x002B11D4
		private float CalculateTotalRequiredHeight()
		{
			float num = this.CalculateHeaderHeight();
			for (int i = 0; i < this.cachedPawns.Count; i++)
			{
				num += this.CalculateRowHeight(this.cachedPawns[i]);
			}
			return num;
		}

		// Token: 0x04004398 RID: 17304
		private PawnTableDef def;

		// Token: 0x04004399 RID: 17305
		private Func<IEnumerable<Pawn>> pawnsGetter;

		// Token: 0x0400439A RID: 17306
		private int minTableWidth;

		// Token: 0x0400439B RID: 17307
		private int maxTableWidth;

		// Token: 0x0400439C RID: 17308
		private int minTableHeight;

		// Token: 0x0400439D RID: 17309
		private int maxTableHeight;

		// Token: 0x0400439E RID: 17310
		private Vector2 fixedSize;

		// Token: 0x0400439F RID: 17311
		private bool hasFixedSize;

		// Token: 0x040043A0 RID: 17312
		private bool dirty;

		// Token: 0x040043A1 RID: 17313
		private List<bool> columnAtMaxWidth = new List<bool>();

		// Token: 0x040043A2 RID: 17314
		private List<bool> columnAtOptimalWidth = new List<bool>();

		// Token: 0x040043A3 RID: 17315
		private Vector2 scrollPosition;

		// Token: 0x040043A4 RID: 17316
		private PawnColumnDef sortByColumn;

		// Token: 0x040043A5 RID: 17317
		private bool sortDescending;

		// Token: 0x040043A6 RID: 17318
		private Vector2 cachedSize;

		// Token: 0x040043A7 RID: 17319
		private List<Pawn> cachedPawns = new List<Pawn>();

		// Token: 0x040043A8 RID: 17320
		private List<float> cachedColumnWidths = new List<float>();

		// Token: 0x040043A9 RID: 17321
		private List<float> cachedRowHeights = new List<float>();

		// Token: 0x040043AA RID: 17322
		private List<LookTargets> cachedLookTargets = new List<LookTargets>();

		// Token: 0x040043AB RID: 17323
		private float cachedHeaderHeight;

		// Token: 0x040043AC RID: 17324
		private float cachedHeightNoScrollbar;
	}
}
