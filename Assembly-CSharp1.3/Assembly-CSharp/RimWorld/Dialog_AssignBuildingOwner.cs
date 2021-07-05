using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012D8 RID: 4824
	public class Dialog_AssignBuildingOwner : Window
	{
		// Token: 0x17001432 RID: 5170
		// (get) Token: 0x0600734A RID: 29514 RVA: 0x00268027 File Offset: 0x00266227
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(620f, 500f);
			}
		}

		// Token: 0x0600734B RID: 29515 RVA: 0x00268038 File Offset: 0x00266238
		public Dialog_AssignBuildingOwner(CompAssignableToPawn assignable)
		{
			this.assignable = assignable;
			this.doCloseButton = true;
			this.doCloseX = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x0600734C RID: 29516 RVA: 0x00268064 File Offset: 0x00266264
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(inRect);
			outRect.yMin += 20f;
			outRect.yMax -= 40f;
			outRect.width -= 16f;
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, (float)this.assignable.AssigningCandidates.Count<Pawn>() * 35f + 100f);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			try
			{
				float num = 0f;
				bool flag = false;
				foreach (Pawn pawn in this.assignable.AssignedPawns)
				{
					flag = true;
					Rect rect = new Rect(0f, num, viewRect.width * 0.7f, 32f);
					Widgets.Label(rect, pawn.LabelCap);
					rect.x = rect.xMax;
					rect.width = viewRect.width * 0.3f;
					if (Widgets.ButtonText(rect, "BuildingUnassign".Translate(), true, true, true))
					{
						this.assignable.TryUnassignPawn(pawn, true);
						SoundDefOf.Click.PlayOneShotOnCamera(null);
						return;
					}
					num += 35f;
				}
				if (flag)
				{
					num += 15f;
				}
				foreach (Pawn pawn2 in this.assignable.AssigningCandidates)
				{
					if (!this.assignable.AssignedPawns.Contains(pawn2))
					{
						AcceptanceReport acceptanceReport = this.assignable.CanAssignTo(pawn2);
						bool accepted = acceptanceReport.Accepted;
						string text = pawn2.LabelCap + (accepted ? "" : (" (" + acceptanceReport.Reason.StripTags() + ")"));
						float width = viewRect.width * 0.7f;
						float num2 = Text.CalcHeight(text, width);
						float num3 = (35f > num2) ? 35f : num2;
						Rect rect2 = new Rect(0f, num, width, num3);
						if (!accepted)
						{
							GUI.color = Color.gray;
						}
						Widgets.Label(rect2, text);
						rect2.x = rect2.xMax;
						rect2.width = viewRect.width * 0.3f;
						rect2.height = 35f;
						if (accepted && this.assignable.IdeoligionForbids(pawn2))
						{
							rect2.xMax -= 28f;
							Widgets.Label(rect2, "IdeoligionForbids".Translate());
							IdeoUIUtility.DoIdeoIcon(new Rect(rect2.xMax + 4f, rect2.y, 24f, 24f), pawn2.ideo.Ideo, true, delegate
							{
								this.Close(true);
							});
						}
						else
						{
							TaggedString taggedString = this.assignable.AssignedAnything(pawn2) ? "BuildingReassign".Translate() : "BuildingAssign".Translate();
							if (Widgets.ButtonText(rect2, taggedString, true, true, accepted))
							{
								this.assignable.TryAssignPawn(pawn2);
								if (this.assignable.MaxAssignedPawnsCount == 1)
								{
									this.Close(true);
									break;
								}
								SoundDefOf.Click.PlayOneShotOnCamera(null);
								break;
							}
						}
						GUI.color = Color.white;
						num += num3;
					}
				}
			}
			finally
			{
				Widgets.EndScrollView();
			}
		}

		// Token: 0x04003EE1 RID: 16097
		private CompAssignableToPawn assignable;

		// Token: 0x04003EE2 RID: 16098
		private Vector2 scrollPosition;

		// Token: 0x04003EE3 RID: 16099
		private const float EntryHeight = 35f;

		// Token: 0x04003EE4 RID: 16100
		private const float LineSpacing = 8f;

		// Token: 0x04003EE5 RID: 16101
		private const int IdeoIconSize = 24;
	}
}
