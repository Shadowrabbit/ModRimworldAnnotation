using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001931 RID: 6449
	[StaticConstructorOnStartup]
	public abstract class Alert
	{
		// Token: 0x1700168F RID: 5775
		// (get) Token: 0x06008EEE RID: 36590 RVA: 0x0005FAD6 File Offset: 0x0005DCD6
		public virtual AlertPriority Priority
		{
			get
			{
				return this.defaultPriority;
			}
		}

		// Token: 0x17001690 RID: 5776
		// (get) Token: 0x06008EEF RID: 36591 RVA: 0x0005FADE File Offset: 0x0005DCDE
		protected virtual Color BGColor
		{
			get
			{
				return Color.clear;
			}
		}

		// Token: 0x17001691 RID: 5777
		// (get) Token: 0x06008EF0 RID: 36592 RVA: 0x0005FAE5 File Offset: 0x0005DCE5
		public bool Active
		{
			get
			{
				return this.cachedActive;
			}
		}

		// Token: 0x17001692 RID: 5778
		// (get) Token: 0x06008EF1 RID: 36593 RVA: 0x0005FAED File Offset: 0x0005DCED
		public string Label
		{
			get
			{
				if (!this.Active)
				{
					return "";
				}
				return this.cachedLabel;
			}
		}

		// Token: 0x17001693 RID: 5779
		// (get) Token: 0x06008EF2 RID: 36594 RVA: 0x0005FB03 File Offset: 0x0005DD03
		public float Height
		{
			get
			{
				Text.Font = GameFont.Small;
				return Text.CalcHeight(this.Label, 148f);
			}
		}

		// Token: 0x06008EF3 RID: 36595
		public abstract AlertReport GetReport();

		// Token: 0x06008EF4 RID: 36596 RVA: 0x0005FB1B File Offset: 0x0005DD1B
		public virtual TaggedString GetExplanation()
		{
			return this.defaultExplanation;
		}

		// Token: 0x06008EF5 RID: 36597 RVA: 0x0005FB28 File Offset: 0x0005DD28
		public virtual string GetLabel()
		{
			return this.defaultLabel;
		}

		// Token: 0x06008EF6 RID: 36598 RVA: 0x00292D24 File Offset: 0x00290F24
		public void Notify_Started()
		{
			if (this.Priority >= AlertPriority.High)
			{
				if (this.alertBounce == null)
				{
					this.alertBounce = new AlertBounce();
				}
				this.alertBounce.DoAlertStartEffect();
				if (Time.timeSinceLevelLoad > 1f && Time.realtimeSinceStartup > this.lastBellTime + 0.5f)
				{
					SoundDefOf.TinyBell.PlayOneShotOnCamera(null);
					this.lastBellTime = Time.realtimeSinceStartup;
				}
			}
		}

		// Token: 0x06008EF7 RID: 36599 RVA: 0x00292D90 File Offset: 0x00290F90
		public void Recalculate()
		{
			AlertReport report = this.GetReport();
			this.cachedActive = report.active;
			if (report.active)
			{
				this.cachedLabel = this.GetLabel();
			}
		}

		// Token: 0x06008EF8 RID: 36600 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void AlertActiveUpdate()
		{
		}

		// Token: 0x06008EF9 RID: 36601 RVA: 0x00292DC4 File Offset: 0x00290FC4
		public virtual Rect DrawAt(float topY, bool minimized)
		{
			Rect rect = new Rect((float)UI.screenWidth - 154f, topY, 154f, this.Height);
			if (this.alertBounce != null)
			{
				rect.x -= this.alertBounce.CalculateHorizontalOffset();
			}
			GUI.color = this.BGColor;
			GUI.DrawTexture(rect, Alert.AlertBGTex);
			GUI.color = Color.white;
			GUI.BeginGroup(rect);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(new Rect(0f, 0f, 148f, this.Height), this.Label);
			GUI.EndGroup();
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, Alert.AlertBGTexHighlight);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.OnClick();
			}
			Text.Anchor = TextAnchor.UpperLeft;
			return rect;
		}

		// Token: 0x06008EFA RID: 36602 RVA: 0x00292E90 File Offset: 0x00291090
		protected virtual void OnClick()
		{
			IEnumerable<GlobalTargetInfo> allCulprits = this.GetReport().AllCulprits;
			if (allCulprits != null)
			{
				Alert.tmpTargets.Clear();
				foreach (GlobalTargetInfo item in allCulprits)
				{
					if (item.IsValid)
					{
						Alert.tmpTargets.Add(item);
					}
				}
				if (Alert.tmpTargets.Any<GlobalTargetInfo>())
				{
					if (Event.current.button == 1)
					{
						this.jumpToTargetCycleIndex--;
					}
					else
					{
						this.jumpToTargetCycleIndex++;
					}
					CameraJumper.TryJumpAndSelect(Alert.tmpTargets[GenMath.PositiveMod(this.jumpToTargetCycleIndex, Alert.tmpTargets.Count)]);
					Alert.tmpTargets.Clear();
				}
			}
		}

		// Token: 0x06008EFB RID: 36603 RVA: 0x00292F6C File Offset: 0x0029116C
		public void DrawInfoPane()
		{
			Alert.<>c__DisplayClass33_0 CS$<>8__locals1 = new Alert.<>c__DisplayClass33_0();
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			this.Recalculate();
			if (!this.Active)
			{
				return;
			}
			CS$<>8__locals1.expString = this.GetExplanation();
			if (CS$<>8__locals1.expString.NullOrEmpty())
			{
				return;
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			if (this.GetReport().AnyCulpritValid)
			{
				CS$<>8__locals1.expString += "\n\n(" + "ClickToJumpToProblem".Translate() + ")";
			}
			float num = Text.CalcHeight(CS$<>8__locals1.expString, 310f);
			num += 20f;
			CS$<>8__locals1.infoRect = new Rect((float)UI.screenWidth - 154f - 330f - 8f, Mathf.Max(Mathf.Min(Event.current.mousePosition.y, (float)UI.screenHeight - num), 0f), 330f, num);
			if (CS$<>8__locals1.infoRect.yMax > (float)UI.screenHeight)
			{
				Alert.<>c__DisplayClass33_0 CS$<>8__locals2 = CS$<>8__locals1;
				CS$<>8__locals2.infoRect.y = CS$<>8__locals2.infoRect.y - ((float)UI.screenHeight - CS$<>8__locals1.infoRect.yMax);
			}
			if (CS$<>8__locals1.infoRect.y < 0f)
			{
				CS$<>8__locals1.infoRect.y = 0f;
			}
			Find.WindowStack.ImmediateWindow(138956, CS$<>8__locals1.infoRect, WindowLayer.GameUI, delegate
			{
				Text.Font = GameFont.Small;
				Rect rect = CS$<>8__locals1.infoRect.AtZero();
				Widgets.DrawWindowBackground(rect);
				Rect position = rect.ContractedBy(10f);
				GUI.BeginGroup(position);
				Widgets.Label(new Rect(0f, 0f, position.width, position.height), CS$<>8__locals1.expString);
				GUI.EndGroup();
			}, false, false, 1f);
		}

		// Token: 0x04005B25 RID: 23333
		protected AlertPriority defaultPriority;

		// Token: 0x04005B26 RID: 23334
		protected string defaultLabel;

		// Token: 0x04005B27 RID: 23335
		protected string defaultExplanation;

		// Token: 0x04005B28 RID: 23336
		protected float lastBellTime = -1000f;

		// Token: 0x04005B29 RID: 23337
		private int jumpToTargetCycleIndex;

		// Token: 0x04005B2A RID: 23338
		private bool cachedActive;

		// Token: 0x04005B2B RID: 23339
		private string cachedLabel;

		// Token: 0x04005B2C RID: 23340
		private AlertBounce alertBounce;

		// Token: 0x04005B2D RID: 23341
		public const float Width = 154f;

		// Token: 0x04005B2E RID: 23342
		private const float TextWidth = 148f;

		// Token: 0x04005B2F RID: 23343
		private const float ItemPeekWidth = 30f;

		// Token: 0x04005B30 RID: 23344
		public const float InfoRectWidth = 330f;

		// Token: 0x04005B31 RID: 23345
		private static readonly Texture2D AlertBGTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		// Token: 0x04005B32 RID: 23346
		private static readonly Texture2D AlertBGTexHighlight = TexUI.HighlightTex;

		// Token: 0x04005B33 RID: 23347
		private static List<GlobalTargetInfo> tmpTargets = new List<GlobalTargetInfo>();
	}
}
