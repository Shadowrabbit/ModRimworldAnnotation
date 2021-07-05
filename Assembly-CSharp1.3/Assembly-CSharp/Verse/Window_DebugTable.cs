using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003E1 RID: 993
	public class Window_DebugTable : Window
	{
		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001DFB RID: 7675 RVA: 0x000B9F29 File Offset: 0x000B8129
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x000BB71C File Offset: 0x000B991C
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

		// Token: 0x06001DFD RID: 7677 RVA: 0x000BB7AC File Offset: 0x000B99AC
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

		// Token: 0x06001DFE RID: 7678 RVA: 0x000BBA44 File Offset: 0x000B9C44
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Tiny;
			Rect outRect = inRect;
			outRect.yMax -= 40f;
			Rect viewRect = new Rect(0f, 0f, this.colWidths.Sum(), this.rowHeights.Sum());
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
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
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(new Rect(inRect.xMax - 120f, inRect.yMax - 30f, 120f, 30f), "Copy CSV", true, true, true))
			{
				this.CopyCSVToClipboard();
				Messages.Message("Copied table data to clipboard in CSV format.", MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x000BBD14 File Offset: 0x000B9F14
		private void CopyCSVToClipboard()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.tableSorted.GetLength(1); i++)
			{
				for (int j = 0; j < this.tableSorted.GetLength(0); j++)
				{
					if (j != 0)
					{
						stringBuilder.Append(",");
					}
					string text = this.tableSorted[j, i];
					stringBuilder.Append("\"" + text.Replace("\n", " ") + "\"");
				}
				stringBuilder.Append("\n");
			}
			GUIUtility.systemCopyBuffer = stringBuilder.ToString();
		}

		// Token: 0x0400120A RID: 4618
		private string[,] tableRaw;

		// Token: 0x0400120B RID: 4619
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x0400120C RID: 4620
		private string[,] tableSorted;

		// Token: 0x0400120D RID: 4621
		private List<float> colWidths = new List<float>();

		// Token: 0x0400120E RID: 4622
		private List<float> rowHeights = new List<float>();

		// Token: 0x0400120F RID: 4623
		private int sortColumn = -1;

		// Token: 0x04001210 RID: 4624
		private Window_DebugTable.SortMode sortMode;

		// Token: 0x04001211 RID: 4625
		private bool[] colVisible;

		// Token: 0x04001212 RID: 4626
		private const float ColExtraWidth = 2f;

		// Token: 0x04001213 RID: 4627
		private const float RowExtraHeight = 2f;

		// Token: 0x04001214 RID: 4628
		private const float HiddenColumnWidth = 10f;

		// Token: 0x02001C2F RID: 7215
		private enum SortMode
		{
			// Token: 0x04006D36 RID: 27958
			Off,
			// Token: 0x04006D37 RID: 27959
			Ascending,
			// Token: 0x04006D38 RID: 27960
			Descending
		}
	}
}
