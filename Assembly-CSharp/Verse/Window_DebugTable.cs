using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020006F1 RID: 1777
	public class Window_DebugTable : Window
	{
		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06002D31 RID: 11569 RVA: 0x00023846 File Offset: 0x00021A46
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x00132550 File Offset: 0x00130750
		public Window_DebugTable(string[,] tables)
		{
			this.tableRaw = tables;
			this.colVisible = new bool[this.tableRaw.GetLength(0)];
			for (int i = 0; i < this.colVisible.Length; i++)
			{
				this.colVisible[i] = true;
			}
			this.doCloseButton = true;
			this.doCloseX = true;
			Text.Font = GameFont.Tiny;
			this.BuildTableSorted();
		}

		// Token: 0x06002D33 RID: 11571 RVA: 0x001325E0 File Offset: 0x001307E0
		private void BuildTableSorted()
		{
			if (this.sortMode == Window_DebugTable.SortMode.Off)
			{
				this.tableSorted = this.tableRaw;
			}
			else
			{
				List<List<string>> list = new List<List<string>>();
				for (int i = 1; i < this.tableRaw.GetLength(1); i++)
				{
					list.Add(new List<string>());
					for (int j = 0; j < this.tableRaw.GetLength(0); j++)
					{
						list[i - 1].Add(this.tableRaw[j, i]);
					}
				}
				NumericStringComparer comparer = new NumericStringComparer();
				switch (this.sortMode)
				{
				case Window_DebugTable.SortMode.Off:
					throw new Exception();
				case Window_DebugTable.SortMode.Ascending:
					list = list.OrderBy((List<string> x) => x[this.sortColumn], comparer).ToList<List<string>>();
					break;
				case Window_DebugTable.SortMode.Descending:
					list = list.OrderByDescending((List<string> x) => x[this.sortColumn], comparer).ToList<List<string>>();
					break;
				}
				this.tableSorted = new string[this.tableRaw.GetLength(0), this.tableRaw.GetLength(1)];
				for (int k = 0; k < this.tableRaw.GetLength(1); k++)
				{
					for (int l = 0; l < this.tableRaw.GetLength(0); l++)
					{
						if (k == 0)
						{
							this.tableSorted[l, k] = this.tableRaw[l, k];
						}
						else
						{
							this.tableSorted[l, k] = list[k - 1][l];
						}
					}
				}
			}
			this.colWidths.Clear();
			for (int m = 0; m < this.tableRaw.GetLength(0); m++)
			{
				float item;
				if (this.colVisible[m])
				{
					float num = 0f;
					for (int n = 0; n < this.tableRaw.GetLength(1); n++)
					{
						float x2 = Text.CalcSize(this.tableRaw[m, n]).x;
						if (x2 > num)
						{
							num = x2;
						}
					}
					item = num + 2f;
				}
				else
				{
					item = 10f;
				}
				this.colWidths.Add(item);
			}
			this.rowHeights.Clear();
			for (int num2 = 0; num2 < this.tableSorted.GetLength(1); num2++)
			{
				float num3 = 0f;
				for (int num4 = 0; num4 < this.tableSorted.GetLength(0); num4++)
				{
					float y = Text.CalcSize(this.tableSorted[num4, num2]).y;
					if (y > num3)
					{
						num3 = y;
					}
				}
				this.rowHeights.Add(num3 + 2f);
			}
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x00132878 File Offset: 0x00130A78
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Tiny;
			inRect.yMax -= 40f;
			Rect viewRect = new Rect(0f, 0f, this.colWidths.Sum(), this.rowHeights.Sum());
			Widgets.BeginScrollView(inRect, ref this.scrollPosition, viewRect, true);
			float num = 0f;
			for (int i = 0; i < this.tableSorted.GetLength(0); i++)
			{
				float num2 = 0f;
				for (int j = 0; j < this.tableSorted.GetLength(1); j++)
				{
					Rect rect = new Rect(num, num2, this.colWidths[i], this.rowHeights[j]);
					Rect rect2 = rect;
					rect2.xMin -= 999f;
					rect2.xMax += 999f;
					if (Mouse.IsOver(rect2) || i % 2 == 0)
					{
						Widgets.DrawHighlight(rect);
					}
					if (j == 0 && Mouse.IsOver(rect))
					{
						rect.x += 2f;
						rect.y += 2f;
					}
					if (i == 0 || this.colVisible[i])
					{
						Widgets.Label(rect, this.tableSorted[i, j]);
					}
					if (j == 0)
					{
						MouseoverSounds.DoRegion(rect);
						if (Mouse.IsOver(rect) && Event.current.type == EventType.MouseDown)
						{
							if (Event.current.button == 0)
							{
								if (i != this.sortColumn)
								{
									this.sortMode = Window_DebugTable.SortMode.Off;
								}
								switch (this.sortMode)
								{
								case Window_DebugTable.SortMode.Off:
									this.sortMode = Window_DebugTable.SortMode.Descending;
									this.sortColumn = i;
									SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
									break;
								case Window_DebugTable.SortMode.Ascending:
									this.sortMode = Window_DebugTable.SortMode.Off;
									this.sortColumn = -1;
									SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
									break;
								case Window_DebugTable.SortMode.Descending:
									this.sortMode = Window_DebugTable.SortMode.Ascending;
									this.sortColumn = i;
									SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
									break;
								}
								this.BuildTableSorted();
							}
							else if (Event.current.button == 1)
							{
								this.colVisible[i] = !this.colVisible[i];
								SoundDefOf.Crunch.PlayOneShotOnCamera(null);
								this.BuildTableSorted();
							}
							Event.current.Use();
						}
					}
					num2 += this.rowHeights[j];
				}
				num += this.colWidths[i];
			}
			Widgets.EndScrollView();
			if (Widgets.ButtonImage(new Rect(inRect.x + inRect.width - 44f, inRect.y + 4f, 18f, 24f), TexButton.Copy, true))
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int k = 0; k < this.tableSorted.GetLength(1); k++)
				{
					for (int l = 0; l < this.tableSorted.GetLength(0); l++)
					{
						if (l != 0)
						{
							stringBuilder.Append(",");
						}
						stringBuilder.Append(this.tableSorted[l, k]);
					}
					stringBuilder.Append("\n");
				}
				GUIUtility.systemCopyBuffer = stringBuilder.ToString();
			}
		}

		// Token: 0x04001EA1 RID: 7841
		private string[,] tableRaw;

		// Token: 0x04001EA2 RID: 7842
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04001EA3 RID: 7843
		private string[,] tableSorted;

		// Token: 0x04001EA4 RID: 7844
		private List<float> colWidths = new List<float>();

		// Token: 0x04001EA5 RID: 7845
		private List<float> rowHeights = new List<float>();

		// Token: 0x04001EA6 RID: 7846
		private int sortColumn = -1;

		// Token: 0x04001EA7 RID: 7847
		private Window_DebugTable.SortMode sortMode;

		// Token: 0x04001EA8 RID: 7848
		private bool[] colVisible;

		// Token: 0x04001EA9 RID: 7849
		private const float ColExtraWidth = 2f;

		// Token: 0x04001EAA RID: 7850
		private const float RowExtraHeight = 2f;

		// Token: 0x04001EAB RID: 7851
		private const float HiddenColumnWidth = 10f;

		// Token: 0x020006F2 RID: 1778
		private enum SortMode
		{
			// Token: 0x04001EAD RID: 7853
			Off,
			// Token: 0x04001EAE RID: 7854
			Ascending,
			// Token: 0x04001EAF RID: 7855
			Descending
		}
	}
}
