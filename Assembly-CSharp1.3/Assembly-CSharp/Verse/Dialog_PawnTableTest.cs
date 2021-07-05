using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003CA RID: 970
	public class Dialog_PawnTableTest : Window
	{
		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001DC1 RID: 7617 RVA: 0x000B9F29 File Offset: 0x000B8129
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001DC2 RID: 7618 RVA: 0x000B9F3C File Offset: 0x000B813C
		private List<Pawn> Pawns
		{
			get
			{
				return Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x000B9F52 File Offset: 0x000B8152
		public Dialog_PawnTableTest(PawnColumnDef singleColumn)
		{
			this.singleColumn = singleColumn;
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x000B9F64 File Offset: 0x000B8164
		public override void DoWindowContents(Rect inRect)
		{
			int num = ((int)inRect.height - 90) / 3;
			PawnTableDef pawnTableDef = new PawnTableDef();
			pawnTableDef.columns = new List<PawnColumnDef>
			{
				this.singleColumn
			};
			pawnTableDef.minWidth = 0;
			if (this.pawnTableMin == null)
			{
				this.pawnTableMin = new PawnTable(pawnTableDef, () => this.Pawns, 0, 0);
				this.pawnTableMin.SetMinMaxSize(Mathf.Min(this.singleColumn.Worker.GetMinWidth(this.pawnTableMin) + 16, (int)inRect.width), Mathf.Min(this.singleColumn.Worker.GetMinWidth(this.pawnTableMin) + 16, (int)inRect.width), 0, num);
			}
			if (this.pawnTableOptimal == null)
			{
				this.pawnTableOptimal = new PawnTable(pawnTableDef, () => this.Pawns, 0, 0);
				this.pawnTableOptimal.SetMinMaxSize(Mathf.Min(this.singleColumn.Worker.GetOptimalWidth(this.pawnTableOptimal) + 16, (int)inRect.width), Mathf.Min(this.singleColumn.Worker.GetOptimalWidth(this.pawnTableOptimal) + 16, (int)inRect.width), 0, num);
			}
			if (this.pawnTableMax == null)
			{
				this.pawnTableMax = new PawnTable(pawnTableDef, () => this.Pawns, 0, 0);
				this.pawnTableMax.SetMinMaxSize(Mathf.Min(this.singleColumn.Worker.GetMaxWidth(this.pawnTableMax) + 16, (int)inRect.width), Mathf.Min(this.singleColumn.Worker.GetMaxWidth(this.pawnTableMax) + 16, (int)inRect.width), 0, num);
			}
			int num2 = 0;
			Text.Font = GameFont.Small;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(0f, (float)num2, inRect.width, 30f), "Min size");
			GUI.color = Color.white;
			num2 += 30;
			this.pawnTableMin.PawnTableOnGUI(new Vector2(0f, (float)num2));
			num2 += num;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(0f, (float)num2, inRect.width, 30f), "Optimal size");
			GUI.color = Color.white;
			num2 += 30;
			this.pawnTableOptimal.PawnTableOnGUI(new Vector2(0f, (float)num2));
			num2 += num;
			GUI.color = Color.gray;
			Widgets.Label(new Rect(0f, (float)num2, inRect.width, 30f), "Max size");
			GUI.color = Color.white;
			num2 += 30;
			this.pawnTableMax.PawnTableOnGUI(new Vector2(0f, (float)num2));
			num2 += num;
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x000BA220 File Offset: 0x000B8420
		[DebugOutput("UI", false)]
		private static void PawnColumnTest()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			List<PawnColumnDef> allDefsListForReading = DefDatabase<PawnColumnDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				PawnColumnDef localDef = allDefsListForReading[i];
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					Find.WindowStack.Add(new Dialog_PawnTableTest(localDef));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x040011DC RID: 4572
		private PawnColumnDef singleColumn;

		// Token: 0x040011DD RID: 4573
		private PawnTable pawnTableMin;

		// Token: 0x040011DE RID: 4574
		private PawnTable pawnTableOptimal;

		// Token: 0x040011DF RID: 4575
		private PawnTable pawnTableMax;

		// Token: 0x040011E0 RID: 4576
		private const int TableTitleHeight = 30;
	}
}
