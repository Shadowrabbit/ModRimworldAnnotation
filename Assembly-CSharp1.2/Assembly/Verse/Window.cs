using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020007AE RID: 1966
	public abstract class Window
	{
		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06003167 RID: 12647 RVA: 0x00026EF7 File Offset: 0x000250F7
		public virtual Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06003168 RID: 12648 RVA: 0x00026F40 File Offset: 0x00025140
		protected virtual float Margin
		{
			get
			{
				return 18f;
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06003169 RID: 12649 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool IsDebug
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x0600316A RID: 12650 RVA: 0x00026F47 File Offset: 0x00025147
		public bool IsOpen
		{
			get
			{
				return Find.WindowStack.IsOpen(this);
			}
		}

		// Token: 0x0600316B RID: 12651 RVA: 0x00145C18 File Offset: 0x00143E18
		public Window()
		{
			this.soundAppear = SoundDefOf.DialogBoxAppear;
			this.soundClose = SoundDefOf.Click;
			this.onGUIProfilerLabelCached = "WindowOnGUI: " + base.GetType().Name;
			this.extraOnGUIProfilerLabelCached = "ExtraOnGUI: " + base.GetType().Name;
			this.innerWindowOnGUICached = new GUI.WindowFunction(this.InnerWindowOnGUI);
		}

		// Token: 0x0600316C RID: 12652 RVA: 0x00026F54 File Offset: 0x00025154
		public virtual void WindowUpdate()
		{
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.Maintain();
			}
		}

		// Token: 0x0600316D RID: 12653
		public abstract void DoWindowContents(Rect inRect);

		// Token: 0x0600316E RID: 12654 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ExtraOnGUI()
		{
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x00145CE4 File Offset: 0x00143EE4
		public virtual void PreOpen()
		{
			this.SetInitialSizeAndPosition();
			if (this.layer == WindowLayer.Dialog)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.DesignatorManager.Dragger.EndDrag();
					Find.DesignatorManager.Deselect();
					Find.Selector.Notify_DialogOpened();
				}
				if (Find.World != null)
				{
					Find.WorldSelector.Notify_DialogOpened();
				}
			}
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x00026F69 File Offset: 0x00025169
		public virtual void PostOpen()
		{
			if (this.soundAppear != null)
			{
				this.soundAppear.PlayOneShotOnCamera(null);
			}
			if (this.soundAmbient != null)
			{
				this.sustainerAmbient = this.soundAmbient.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerFrame));
			}
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PreClose()
		{
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostClose()
		{
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x00145D3C File Offset: 0x00143F3C
		public virtual void WindowOnGUI()
		{
			if (this.resizeable)
			{
				if (this.resizer == null)
				{
					this.resizer = new WindowResizer();
				}
				if (this.resizeLater)
				{
					this.resizeLater = false;
					this.windowRect = this.resizeLaterRect;
				}
			}
			this.windowRect = this.windowRect.Rounded();
			this.windowRect = GUI.Window(this.ID, this.windowRect, this.innerWindowOnGUICached, "", Widgets.EmptyStyle);
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x00145DB8 File Offset: 0x00143FB8
		private void InnerWindowOnGUI(int x)
		{
			Rect rect = this.windowRect.AtZero();
			UnityGUIBugsFixer.OnGUI();
			Find.WindowStack.currentlyDrawnWindow = this;
			if (this.doWindowBackground)
			{
				Widgets.DrawWindowBackground(rect);
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Find.WindowStack.Notify_PressedCancel();
			}
			if (KeyBindingDefOf.Accept.KeyDownEvent)
			{
				Find.WindowStack.Notify_PressedAccept();
			}
			if (Event.current.type == EventType.MouseDown)
			{
				Find.WindowStack.Notify_ClickedInsideWindow(this);
			}
			if (Event.current.type == EventType.KeyDown && !Find.WindowStack.GetsInput(this))
			{
				Event.current.Use();
			}
			if (!this.optionalTitle.NullOrEmpty())
			{
				GUI.Label(new Rect(this.Margin, this.Margin, this.windowRect.width, 25f), this.optionalTitle);
			}
			if (this.doCloseX && Widgets.CloseButtonFor(rect))
			{
				this.Close(true);
			}
			if (this.resizeable && Event.current.type != EventType.Repaint)
			{
				Rect lhs = this.resizer.DoResizeControl(this.windowRect);
				if (lhs != this.windowRect)
				{
					this.resizeLater = true;
					this.resizeLaterRect = lhs;
				}
			}
			Rect rect2 = rect.ContractedBy(this.Margin);
			if (!this.optionalTitle.NullOrEmpty())
			{
				rect2.yMin += this.Margin + 25f;
			}
			GUI.BeginGroup(rect2);
			try
			{
				this.DoWindowContents(rect2.AtZero());
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception filling window for ",
					base.GetType(),
					": ",
					ex
				}), false);
			}
			GUI.EndGroup();
			if (this.resizeable && Event.current.type == EventType.Repaint)
			{
				this.resizer.DoResizeControl(this.windowRect);
			}
			if (this.doCloseButton)
			{
				Text.Font = GameFont.Small;
				if (Widgets.ButtonText(new Rect(rect.width / 2f - this.CloseButSize.x / 2f, rect.height - 55f, this.CloseButSize.x, this.CloseButSize.y), "CloseButton".Translate(), true, true, true))
				{
					this.Close(true);
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.IsOpen)
			{
				this.OnCancelKeyPressed();
			}
			if (this.draggable)
			{
				GUI.DragWindow();
			}
			else if (Event.current.type == EventType.MouseDown)
			{
				Event.current.Use();
			}
			ScreenFader.OverlayOnGUI(rect.size);
			Find.WindowStack.currentlyDrawnWindow = null;
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x00026F9E File Offset: 0x0002519E
		public virtual void Close(bool doCloseSound = true)
		{
			Find.WindowStack.TryRemove(this, doCloseSound);
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool CausesMessageBackground()
		{
			return false;
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x0014606C File Offset: 0x0014426C
		protected virtual void SetInitialSizeAndPosition()
		{
			this.windowRect = new Rect(((float)UI.screenWidth - this.InitialSize.x) / 2f, ((float)UI.screenHeight - this.InitialSize.y) / 2f, this.InitialSize.x, this.InitialSize.y);
			this.windowRect = this.windowRect.Rounded();
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x00026FAD File Offset: 0x000251AD
		public virtual void OnCancelKeyPressed()
		{
			if (this.closeOnCancel)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x00026FC8 File Offset: 0x000251C8
		public virtual void OnAcceptKeyPressed()
		{
			if (this.closeOnAccept)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x00026FE3 File Offset: 0x000251E3
		public virtual void Notify_ResolutionChanged()
		{
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x0400222C RID: 8748
		public WindowLayer layer = WindowLayer.Dialog;

		// Token: 0x0400222D RID: 8749
		public string optionalTitle;

		// Token: 0x0400222E RID: 8750
		public bool doCloseX;

		// Token: 0x0400222F RID: 8751
		public bool doCloseButton;

		// Token: 0x04002230 RID: 8752
		public bool closeOnAccept = true;

		// Token: 0x04002231 RID: 8753
		public bool closeOnCancel = true;

		// Token: 0x04002232 RID: 8754
		public bool forceCatchAcceptAndCancelEventEvenIfUnfocused;

		// Token: 0x04002233 RID: 8755
		public bool closeOnClickedOutside;

		// Token: 0x04002234 RID: 8756
		public bool forcePause;

		// Token: 0x04002235 RID: 8757
		public bool preventCameraMotion = true;

		// Token: 0x04002236 RID: 8758
		public bool preventDrawTutor;

		// Token: 0x04002237 RID: 8759
		public bool doWindowBackground = true;

		// Token: 0x04002238 RID: 8760
		public bool onlyOneOfTypeAllowed = true;

		// Token: 0x04002239 RID: 8761
		public bool absorbInputAroundWindow;

		// Token: 0x0400223A RID: 8762
		public bool resizeable;

		// Token: 0x0400223B RID: 8763
		public bool draggable;

		// Token: 0x0400223C RID: 8764
		public bool drawShadow = true;

		// Token: 0x0400223D RID: 8765
		public bool focusWhenOpened = true;

		// Token: 0x0400223E RID: 8766
		public float shadowAlpha = 1f;

		// Token: 0x0400223F RID: 8767
		public SoundDef soundAppear;

		// Token: 0x04002240 RID: 8768
		public SoundDef soundClose;

		// Token: 0x04002241 RID: 8769
		public SoundDef soundAmbient;

		// Token: 0x04002242 RID: 8770
		public bool silenceAmbientSound;

		// Token: 0x04002243 RID: 8771
		public const float StandardMargin = 18f;

		// Token: 0x04002244 RID: 8772
		protected readonly Vector2 CloseButSize = new Vector2(120f, 40f);

		// Token: 0x04002245 RID: 8773
		public int ID;

		// Token: 0x04002246 RID: 8774
		public Rect windowRect;

		// Token: 0x04002247 RID: 8775
		private Sustainer sustainerAmbient;

		// Token: 0x04002248 RID: 8776
		private WindowResizer resizer;

		// Token: 0x04002249 RID: 8777
		private bool resizeLater;

		// Token: 0x0400224A RID: 8778
		private Rect resizeLaterRect;

		// Token: 0x0400224B RID: 8779
		private string onGUIProfilerLabelCached;

		// Token: 0x0400224C RID: 8780
		public string extraOnGUIProfilerLabelCached;

		// Token: 0x0400224D RID: 8781
		private GUI.WindowFunction innerWindowOnGUICached;
	}
}
