using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012DC RID: 4828
	public class Dialog_ChangeDryadCaste : Window
	{
		// Token: 0x17001439 RID: 5177
		// (get) Token: 0x06007376 RID: 29558 RVA: 0x0026C1DB File Offset: 0x0026A3DB
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)Mathf.Min(900, UI.screenWidth), 650f);
			}
		}

		// Token: 0x1700143A RID: 5178
		// (get) Token: 0x06007377 RID: 29559 RVA: 0x0026C1F7 File Offset: 0x0026A3F7
		private PawnKindDef SelectedKind
		{
			get
			{
				return this.selectedMode.pawnKindDef;
			}
		}

		// Token: 0x06007378 RID: 29560 RVA: 0x0026C204 File Offset: 0x0026A404
		public Dialog_ChangeDryadCaste(Thing tree)
		{
			this.treeConnection = tree.TryGetComp<CompTreeConnection>();
			this.currentMode = this.treeConnection.desiredMode;
			this.selectedMode = this.currentMode;
			this.connectedPawn = this.treeConnection.ConnectedPawn;
			this.forcePause = true;
			this.closeOnAccept = false;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.allDryadModes = DefDatabase<GauranlenTreeModeDef>.AllDefs.ToList<GauranlenTreeModeDef>();
		}

		// Token: 0x06007379 RID: 29561 RVA: 0x0026C27D File Offset: 0x0026A47D
		public override void PreOpen()
		{
			if (!ModLister.CheckIdeology("Dryad upgrades"))
			{
				this.Close(true);
			}
			base.PreOpen();
			this.SetupView();
		}

		// Token: 0x0600737A RID: 29562 RVA: 0x0026C2A0 File Offset: 0x0026A4A0
		private void SetupView()
		{
			foreach (GauranlenTreeModeDef stage in this.allDryadModes)
			{
				this.rightViewWidth = Mathf.Max(this.rightViewWidth, this.GetPosition(stage, this.InitialSize.y).x + Dialog_ChangeDryadCaste.OptionSize.x);
			}
			this.rightViewWidth += 20f;
		}

		// Token: 0x0600737B RID: 29563 RVA: 0x0026C334 File Offset: 0x0026A534
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			string label = (this.selectedMode != null) ? this.selectedMode.LabelCap : "ChangeMode".Translate();
			Widgets.Label(new Rect(inRect.x, inRect.y, inRect.width, 35f), label);
			Text.Font = GameFont.Small;
			float num = inRect.y + 35f + 10f;
			float num2 = num;
			float num3 = inRect.height - num;
			num3 -= Dialog_ChangeDryadCaste.ButSize.y + 10f;
			this.DrawLeftRect(new Rect(inRect.xMin, num, 400f, num3), ref num2);
			this.DrawRightRect(new Rect(inRect.x + 400f + 17f, num, inRect.width - 400f - 17f, num3));
		}

		// Token: 0x0600737C RID: 29564 RVA: 0x0026C418 File Offset: 0x0026A618
		private void DrawLeftRect(Rect rect, ref float curY)
		{
			Rect rect2 = new Rect(rect.x, curY, rect.width, rect.height)
			{
				yMax = rect.yMax
			}.ContractedBy(4f);
			if (this.selectedMode == null)
			{
				Widgets.Label(rect2, "ChooseProductionModeInitialDesc".Translate(this.connectedPawn.Named("PAWN"), this.treeConnection.parent.Named("TREE"), ThingDefOf.DryadCocoon.GetCompProperties<CompProperties_DryadCocoon>().daysToComplete.Named("UPGRADEDURATION")));
				return;
			}
			Widgets.Label(rect2.x, ref curY, rect2.width, this.selectedMode.Description, default(TipSignal));
			curY += 10f;
			if (!this.selectedMode.requiredMemes.NullOrEmpty<MemeDef>())
			{
				Widgets.Label(rect2.x, ref curY, rect2.width, "RequiredMemes".Translate() + ":", default(TipSignal));
				string text = "";
				for (int i = 0; i < this.selectedMode.requiredMemes.Count; i++)
				{
					MemeDef memeDef = this.selectedMode.requiredMemes[i];
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text = text + "  - " + memeDef.LabelCap.ToString().Colorize(this.connectedPawn.Ideo.HasMeme(memeDef) ? Color.white : ColorLibrary.RedReadable);
				}
				Widgets.Label(rect2.x, ref curY, rect2.width, text, default(TipSignal));
				curY += 10f;
			}
			if (this.selectedMode.previousStage != null)
			{
				Widgets.Label(rect2.x, ref curY, rect2.width, "RequiredStage".Translate() + ": " + this.selectedMode.previousStage.pawnKindDef.LabelCap.ToString().Colorize(Color.white), default(TipSignal));
				curY += 10f;
			}
			if (this.selectedMode.displayedStats != null)
			{
				for (int j = 0; j < this.selectedMode.displayedStats.Count; j++)
				{
					StatDef statDef = this.selectedMode.displayedStats[j];
					Widgets.Label(rect2.x, ref curY, rect2.width, statDef.LabelCap + ": " + statDef.ValueToString(this.SelectedKind.race.GetStatValueAbstract(statDef, null), statDef.toStringNumberSense, true), default(TipSignal));
				}
				curY += 10f;
			}
			if (this.selectedMode.hyperlinks != null)
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in Dialog_InfoCard.DefsToHyperlinks(this.selectedMode.hyperlinks))
				{
					Widgets.HyperlinkWithIcon(new Rect(rect2.x, curY, rect2.width, Text.LineHeight), hyperlink, null, 2f, 6f, null, false, null);
					curY += Text.LineHeight;
				}
				curY += 10f;
			}
			Rect rect3 = new Rect(rect2.x, rect2.yMax - 55f, rect2.width, 55f);
			if (this.MeetsRequirements(this.selectedMode) && this.selectedMode != this.currentMode)
			{
				if (Widgets.ButtonText(rect3, "Accept".Translate(), true, true, true))
				{
					Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("GauranlenModeChangeDescFull".Translate(this.treeConnection.parent.Named("TREE"), this.connectedPawn.Named("CONNECTEDPAWN"), ThingDefOf.DryadCocoon.GetCompProperties<CompProperties_DryadCocoon>().daysToComplete.Named("DURATION")), delegate
					{
						this.StartChange();
					}, true, null);
					Find.WindowStack.Add(window);
					return;
				}
			}
			else
			{
				string label;
				if (this.selectedMode == this.currentMode)
				{
					label = "AlreadySelected".Translate();
				}
				else if (!this.MeetsMemeRequirements(this.selectedMode))
				{
					label = "MissingRequiredMemes".Translate();
				}
				else if (this.selectedMode.previousStage != null && this.currentMode != this.selectedMode.previousStage)
				{
					label = "Locked".Translate() + ": " + "MissingRequiredCaste".Translate();
				}
				else
				{
					label = "Locked".Translate();
				}
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.DrawHighlight(rect3);
				Widgets.Label(rect3.ContractedBy(5f), label);
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		// Token: 0x0600737D RID: 29565 RVA: 0x0026C960 File Offset: 0x0026AB60
		private void StartChange()
		{
			this.treeConnection.desiredMode = this.selectedMode;
			SoundDefOf.GauranlenProductionModeSet.PlayOneShotOnCamera(null);
			this.Close(false);
		}

		// Token: 0x0600737E RID: 29566 RVA: 0x0026C988 File Offset: 0x0026AB88
		private void DrawRightRect(Rect rect)
		{
			Widgets.DrawMenuSection(rect);
			Rect rect2 = new Rect(0f, 0f, this.rightViewWidth, rect.height - 16f);
			Rect rect3 = rect2.ContractedBy(10f);
			Widgets.ScrollHorizontal(rect, ref this.scrollPosition, rect2, 20f);
			Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, true);
			GUI.BeginGroup(rect3);
			this.DrawDependencyLines(rect3);
			foreach (GauranlenTreeModeDef stage in this.allDryadModes)
			{
				this.DrawDryadStage(rect3, stage);
			}
			GUI.EndGroup();
			Widgets.EndScrollView();
		}

		// Token: 0x0600737F RID: 29567 RVA: 0x0026CA4C File Offset: 0x0026AC4C
		private bool MeetsMemeRequirements(GauranlenTreeModeDef stage)
		{
			if (!stage.requiredMemes.NullOrEmpty<MemeDef>())
			{
				foreach (MemeDef meme in stage.requiredMemes)
				{
					if (!this.connectedPawn.Ideo.HasMeme(meme))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06007380 RID: 29568 RVA: 0x0026CAC0 File Offset: 0x0026ACC0
		private bool MeetsRequirements(GauranlenTreeModeDef mode)
		{
			return (mode.previousStage == null || this.currentMode == mode.previousStage) && this.MeetsMemeRequirements(mode);
		}

		// Token: 0x06007381 RID: 29569 RVA: 0x0026CAE4 File Offset: 0x0026ACE4
		private Color GetBoxColor(GauranlenTreeModeDef mode)
		{
			Color color = TexUI.AvailResearchColor;
			if (mode == this.currentMode)
			{
				color = TexUI.ActiveResearchColor;
			}
			else if (!this.MeetsRequirements(mode))
			{
				color = TexUI.LockedResearchColor;
			}
			if (this.selectedMode == mode)
			{
				color += TexUI.HighlightBgResearchColor;
			}
			return color;
		}

		// Token: 0x06007382 RID: 29570 RVA: 0x0026CB2D File Offset: 0x0026AD2D
		private Color GetBoxOutlineColor(GauranlenTreeModeDef mode)
		{
			if (this.selectedMode != null && this.selectedMode == mode)
			{
				return TexUI.HighlightBorderResearchColor;
			}
			return TexUI.DefaultBorderResearchColor;
		}

		// Token: 0x06007383 RID: 29571 RVA: 0x0026CB4B File Offset: 0x0026AD4B
		private Color GetTextColor(GauranlenTreeModeDef mode)
		{
			if (!this.MeetsRequirements(mode))
			{
				return ColorLibrary.RedReadable;
			}
			return Color.white;
		}

		// Token: 0x06007384 RID: 29572 RVA: 0x0026CB64 File Offset: 0x0026AD64
		private void DrawDependencyLines(Rect fullRect)
		{
			foreach (GauranlenTreeModeDef gauranlenTreeModeDef in this.allDryadModes)
			{
				if (gauranlenTreeModeDef.previousStage != null)
				{
					this.DrawLineBetween(gauranlenTreeModeDef, gauranlenTreeModeDef.previousStage, fullRect.height, TexUI.DefaultLineResearchColor, 2f);
				}
			}
			foreach (GauranlenTreeModeDef gauranlenTreeModeDef2 in this.allDryadModes)
			{
				if (gauranlenTreeModeDef2.previousStage != null && (gauranlenTreeModeDef2.previousStage == this.selectedMode || this.selectedMode == gauranlenTreeModeDef2))
				{
					this.DrawLineBetween(gauranlenTreeModeDef2, gauranlenTreeModeDef2.previousStage, fullRect.height, TexUI.HighlightLineResearchColor, 3f);
				}
			}
		}

		// Token: 0x06007385 RID: 29573 RVA: 0x0026CC50 File Offset: 0x0026AE50
		private void DrawDryadStage(Rect rect, GauranlenTreeModeDef stage)
		{
			Vector2 position = this.GetPosition(stage, rect.height);
			Rect rect2 = new Rect(position.x, position.y, Dialog_ChangeDryadCaste.OptionSize.x, Dialog_ChangeDryadCaste.OptionSize.y);
			Widgets.DrawBoxSolidWithOutline(rect2, this.GetBoxColor(stage), this.GetBoxOutlineColor(stage), 1);
			Rect rect3 = new Rect(rect2.x, rect2.y, rect2.height, rect2.height);
			Widgets.DefIcon(rect3.ContractedBy(4f), stage.pawnKindDef, null, 1f, null, false, null);
			GUI.color = this.GetTextColor(stage);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(new Rect(rect3.xMax, rect2.y, rect2.width - rect3.width, rect2.height).ContractedBy(4f), stage.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			if (Widgets.ButtonInvisible(rect2, true))
			{
				this.selectedMode = stage;
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06007386 RID: 29574 RVA: 0x0026CD70 File Offset: 0x0026AF70
		private void DrawLineBetween(GauranlenTreeModeDef left, GauranlenTreeModeDef right, float height, Color color, float width = 2f)
		{
			Vector2 start = this.GetPosition(left, height) + new Vector2(5f, Dialog_ChangeDryadCaste.OptionSize.y / 2f);
			Vector2 end = this.GetPosition(right, height) + Dialog_ChangeDryadCaste.OptionSize / 2f;
			Widgets.DrawLine(start, end, color, width);
		}

		// Token: 0x06007387 RID: 29575 RVA: 0x0026CDCC File Offset: 0x0026AFCC
		private Vector2 GetPosition(GauranlenTreeModeDef stage, float height)
		{
			return new Vector2(stage.drawPosition.x * Dialog_ChangeDryadCaste.OptionSize.x + stage.drawPosition.x * 52f, (height - Dialog_ChangeDryadCaste.OptionSize.y) * stage.drawPosition.y);
		}

		// Token: 0x04003F2B RID: 16171
		private CompTreeConnection treeConnection;

		// Token: 0x04003F2C RID: 16172
		private Pawn connectedPawn;

		// Token: 0x04003F2D RID: 16173
		private Vector2 scrollPosition;

		// Token: 0x04003F2E RID: 16174
		private GauranlenTreeModeDef selectedMode;

		// Token: 0x04003F2F RID: 16175
		private GauranlenTreeModeDef currentMode;

		// Token: 0x04003F30 RID: 16176
		private float rightViewWidth;

		// Token: 0x04003F31 RID: 16177
		private List<GauranlenTreeModeDef> allDryadModes;

		// Token: 0x04003F32 RID: 16178
		private const float HeaderHeight = 35f;

		// Token: 0x04003F33 RID: 16179
		private const float LeftRectWidth = 400f;

		// Token: 0x04003F34 RID: 16180
		private const float OptionSpacing = 52f;

		// Token: 0x04003F35 RID: 16181
		private const float ChangeFormButtonHeight = 55f;

		// Token: 0x04003F36 RID: 16182
		private static readonly Vector2 OptionSize = new Vector2(190f, 46f);

		// Token: 0x04003F37 RID: 16183
		private static readonly Vector2 ButSize = new Vector2(200f, 40f);
	}
}
