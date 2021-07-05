using System;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019D9 RID: 6617
	public class Dialog_AssignBuildingOwner : Window
	{
		// Token: 0x17001736 RID: 5942
		// (get) Token: 0x0600923E RID: 37438 RVA: 0x00062003 File Offset: 0x00060203
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(620f, 500f);
			}
		}

		// Token: 0x0600923F RID: 37439 RVA: 0x00062014 File Offset: 0x00060214
		public Dialog_AssignBuildingOwner(CompAssignableToPawn assignable)
		{
			this.assignable = assignable;
			this.doCloseButton = true;
			this.doCloseX = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06009240 RID: 37440 RVA: 0x002A0218 File Offset: 0x0029E418
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
						else
						{
							GUI.color = Color.white;
							num += num3;
						}
					}
				}
			}
			finally
			{
				Widgets.EndScrollView();
			}
		}

		// Token: 0x04005C73 RID: 23667
		private CompAssignableToPawn assignable;

		// Token: 0x04005C74 RID: 23668
		private Vector2 scrollPosition;

		// Token: 0x04005C75 RID: 23669
		private const float EntryHeight = 35f;

		// Token: 0x04005C76 RID: 23670
		private const float LineSpacing = 8f;
	}
}
