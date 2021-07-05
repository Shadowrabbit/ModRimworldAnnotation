using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200123B RID: 4667
	[StaticConstructorOnStartup]
	public abstract class Alert
	{
		// Token: 0x17001388 RID: 5000
		// (get) Token: 0x06007001 RID: 28673 RVA: 0x0025530B File Offset: 0x0025350B
		public virtual AlertPriority Priority
		{
			get
			{
				return this.defaultPriority;
			}
		}

		// Token: 0x17001389 RID: 5001
		// (get) Token: 0x06007002 RID: 28674 RVA: 0x00255313 File Offset: 0x00253513
		protected virtual Color BGColor
		{
			get
			{
				return Color.clear;
			}
		}

		// Token: 0x1700138A RID: 5002
		// (get) Token: 0x06007003 RID: 28675 RVA: 0x0025531A File Offset: 0x0025351A
		public bool Active
		{
			get
			{
				return this.cachedActive;
			}
		}

		// Token: 0x1700138B RID: 5003
		// (get) Token: 0x06007004 RID: 28676 RVA: 0x00255322 File Offset: 0x00253522
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

		// Token: 0x1700138C RID: 5004
		// (get) Token: 0x06007005 RID: 28677 RVA: 0x00255338 File Offset: 0x00253538
		public float Height
		{
			get
			{
				Text.Font = GameFont.Small;
				return Text.CalcHeight(this.Label, 148f);
			}
		}

		// Token: 0x06007006 RID: 28678
		public abstract AlertReport GetReport();

		// Token: 0x06007007 RID: 28679 RVA: 0x00255350 File Offset: 0x00253550
		public virtual TaggedString GetExplanation()
		{
			return this.defaultExplanation;
		}

		// Token: 0x06007008 RID: 28680 RVA: 0x0025535D File Offset: 0x0025355D
		public virtual string GetLabel()
		{
			return this.defaultLabel;
		}

		// Token: 0x1700138D RID: 5005
		// (get) Token: 0x06007009 RID: 28681 RVA: 0x00255365 File Offset: 0x00253565
		public virtual string GetJumpToTargetsText
		{
			get
			{
				return "ClickToJumpToProblem".Translate();
			}
		}

		// Token: 0x0600700A RID: 28682 RVA: 0x00255378 File Offset: 0x00253578
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

		// Token: 0x0600700B RID: 28683 RVA: 0x002553E4 File Offset: 0x002535E4
		public void Recalculate()
		{
			AlertReport report = this.GetReport();
			this.cachedActive = report.active;
			if (report.active)
			{
				this.cachedLabel = this.GetLabel();
			}
		}

		// Token: 0x0600700C RID: 28684 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void AlertActiveUpdate()
		{
		}

		// Token: 0x0600700D RID: 28685 RVA: 0x00255418 File Offset: 0x00253618
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

		// Token: 0x0600700E RID: 28686 RVA: 0x002554E4 File Offset: 0x002536E4
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

		// Token: 0x0600700F RID: 28687 RVA: 0x002555C0 File Offset: 0x002537C0
		public void DrawInfoPane()
		{
			Alert.<>c__DisplayClass35_0 CS$<>8__locals1 = new Alert.<>c__DisplayClass35_0();
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
				CS$<>8__locals1.expString += "\n\n(" + this.GetJumpToTargetsText + ")";
			}
			float num = Text.CalcHeight(CS$<>8__locals1.expString, 310f);
			num += 20f;
			CS$<>8__locals1.infoRect = new Rect((float)UI.screenWidth - 154f - 330f - 8f, Mathf.Max(Mathf.Min(Event.current.mousePosition.y, (float)UI.screenHeight - num), 0f), 330f, num);
			if (CS$<>8__locals1.infoRect.yMax > (float)UI.screenHeight)
			{
				Alert.<>c__DisplayClass35_0 CS$<>8__locals2 = CS$<>8__locals1;
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
			}, false, false, 1f, null);
		}

		// Token: 0x04003DD0 RID: 15824
		protected AlertPriority defaultPriority;

		// Token: 0x04003DD1 RID: 15825
		protected string defaultLabel;

		// Token: 0x04003DD2 RID: 15826
		protected string defaultExplanation;

		// Token: 0x04003DD3 RID: 15827
		protected float lastBellTime = -1000f;

		// Token: 0x04003DD4 RID: 15828
		private int jumpToTargetCycleIndex = -1;

		// Token: 0x04003DD5 RID: 15829
		private bool cachedActive;

		// Token: 0x04003DD6 RID: 15830
		private string cachedLabel;

		// Token: 0x04003DD7 RID: 15831
		private AlertBounce alertBounce;

		// Token: 0x04003DD8 RID: 15832
		public const float Width = 154f;

		// Token: 0x04003DD9 RID: 15833
		private const float TextWidth = 148f;

		// Token: 0x04003DDA RID: 15834
		private const float ItemPeekWidth = 30f;

		// Token: 0x04003DDB RID: 15835
		public const float InfoRectWidth = 330f;

		// Token: 0x04003DDC RID: 15836
		private static readonly Texture2D AlertBGTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		// Token: 0x04003DDD RID: 15837
		private static readonly Texture2D AlertBGTexHighlight = TexUI.HighlightTex;

		// Token: 0x04003DDE RID: 15838
		private static List<GlobalTargetInfo> tmpTargets = new List<GlobalTargetInfo>();
	}
}
