using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200045C RID: 1116
	public abstract class Window
	{
		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060021AD RID: 8621 RVA: 0x000D2698 File Offset: 0x000D0898
		public virtual Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060021AE RID: 8622 RVA: 0x000D28BB File Offset: 0x000D0ABB
		protected virtual float Margin
		{
			get
			{
				return 18f;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool IsDebug
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x060021B0 RID: 8624 RVA: 0x000D28C2 File Offset: 0x000D0AC2
		public bool IsOpen
		{
			get
			{
				return Find.WindowStack.IsOpen(this);
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x060021B1 RID: 8625 RVA: 0x00002688 File Offset: 0x00000888
		public virtual QuickSearchWidget CommonSearchWidget
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x000D28D0 File Offset: 0x000D0AD0
		public Window()
		{
			this.soundAppear = SoundDefOf.DialogBoxAppear;
			this.soundClose = SoundDefOf.Click;
			this.onGUIProfilerLabelCached = "WindowOnGUI: " + base.GetType().Name;
			this.extraOnGUIProfilerLabelCached = "ExtraOnGUI: " + base.GetType().Name;
			this.innerWindowOnGUICached = new GUI.WindowFunction(this.InnerWindowOnGUI);
			this.notify_CommonSearchChangedCached = new Action(this.Notify_CommonSearchChanged);
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x000D29C6 File Offset: 0x000D0BC6
		public virtual void WindowUpdate()
		{
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.Maintain();
			}
		}

		// Token: 0x060021B4 RID: 8628
		public abstract void DoWindowContents(Rect inRect);

		// Token: 0x060021B5 RID: 8629 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ExtraOnGUI()
		{
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x000D29DC File Offset: 0x000D0BDC
		public virtual void PreOpen()
		{
			this.SetInitialSizeAndPosition();
			QuickSearchWidget commonSearchWidget = this.CommonSearchWidget;
			if (commonSearchWidget != null)
			{
				commonSearchWidget.Reset();
			}
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

		// Token: 0x060021B7 RID: 8631 RVA: 0x000D2A45 File Offset: 0x000D0C45
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

		// Token: 0x060021B8 RID: 8632 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PreClose()
		{
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostClose()
		{
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x000D2A7C File Offset: 0x000D0C7C
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

		// Token: 0x060021BB RID: 8635 RVA: 0x000D2AF8 File Offset: 0x000D0CF8
		private void InnerWindowOnGUI(int x)
		{
			OriginalEventUtility.RecordOriginalEvent(Event.current);
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
			QuickSearchWidget commonSearchWidget = this.CommonSearchWidget;
			if (commonSearchWidget != null)
			{
				Rect rect3 = new Rect(rect.x + this.commonSearchWidgetOffset.x, rect.height - 55f + this.commonSearchWidgetOffset.y, Window.QuickSearchSize.x, Window.QuickSearchSize.y);
				commonSearchWidget.OnGUI(rect3, this.notify_CommonSearchChangedCached);
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
				}));
			}
			GUI.EndGroup();
			if (this.grayOutIfOtherDialogOpen)
			{
				IList<Window> windows = Find.WindowStack.Windows;
				for (int i = 0; i < windows.Count; i++)
				{
					if (windows[i].layer == WindowLayer.Dialog && !(windows[i] is Page) && windows[i] != this)
					{
						Widgets.DrawRectFast(rect, new Color(0f, 0f, 0f, 0.5f), null);
						break;
					}
				}
			}
			if (this.resizeable && Event.current.type == EventType.Repaint)
			{
				this.resizer.DoResizeControl(this.windowRect);
			}
			if (this.doCloseButton)
			{
				Text.Font = GameFont.Small;
				if (Widgets.ButtonText(new Rect(rect.width / 2f - Window.CloseButSize.x / 2f, rect.height - 55f, Window.CloseButSize.x, Window.CloseButSize.y), "CloseButton".Translate(), true, true, true))
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
			OriginalEventUtility.Reset();
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x000D2E90 File Offset: 0x000D1090
		public virtual void Close(bool doCloseSound = true)
		{
			Find.WindowStack.TryRemove(this, doCloseSound);
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool CausesMessageBackground()
		{
			return false;
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x000D2EA0 File Offset: 0x000D10A0
		protected virtual void SetInitialSizeAndPosition()
		{
			Vector2 initialSize = this.InitialSize;
			this.windowRect = new Rect(((float)UI.screenWidth - initialSize.x) / 2f, ((float)UI.screenHeight - initialSize.y) / 2f, initialSize.x, initialSize.y);
			this.windowRect = this.windowRect.Rounded();
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x000D2F02 File Offset: 0x000D1102
		public virtual void OnCancelKeyPressed()
		{
			if (this.closeOnCancel)
			{
				this.Close(true);
				Event.current.Use();
			}
			if (this.openMenuOnCancel)
			{
				Find.MainTabsRoot.ToggleTab(MainButtonDefOf.Menu, true);
			}
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x000D2F35 File Offset: 0x000D1135
		public virtual void OnAcceptKeyPressed()
		{
			if (this.closeOnAccept)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x000D2F50 File Offset: 0x000D1150
		public virtual void Notify_ResolutionChanged()
		{
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_CommonSearchChanged()
		{
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x000D2F58 File Offset: 0x000D1158
		public virtual void Notify_ClickOutsideWindow()
		{
			QuickSearchWidget commonSearchWidget = this.CommonSearchWidget;
			if (commonSearchWidget == null)
			{
				return;
			}
			commonSearchWidget.Unfocus();
		}

		// Token: 0x04001512 RID: 5394
		public WindowLayer layer = WindowLayer.Dialog;

		// Token: 0x04001513 RID: 5395
		public string optionalTitle;

		// Token: 0x04001514 RID: 5396
		public bool doCloseX;

		// Token: 0x04001515 RID: 5397
		public bool doCloseButton;

		// Token: 0x04001516 RID: 5398
		public bool closeOnAccept = true;

		// Token: 0x04001517 RID: 5399
		public bool closeOnCancel = true;

		// Token: 0x04001518 RID: 5400
		public bool forceCatchAcceptAndCancelEventEvenIfUnfocused;

		// Token: 0x04001519 RID: 5401
		public bool closeOnClickedOutside;

		// Token: 0x0400151A RID: 5402
		public bool forcePause;

		// Token: 0x0400151B RID: 5403
		public bool preventCameraMotion = true;

		// Token: 0x0400151C RID: 5404
		public bool preventDrawTutor;

		// Token: 0x0400151D RID: 5405
		public bool doWindowBackground = true;

		// Token: 0x0400151E RID: 5406
		public bool onlyOneOfTypeAllowed = true;

		// Token: 0x0400151F RID: 5407
		public bool absorbInputAroundWindow;

		// Token: 0x04001520 RID: 5408
		public bool resizeable;

		// Token: 0x04001521 RID: 5409
		public bool draggable;

		// Token: 0x04001522 RID: 5410
		public bool drawShadow = true;

		// Token: 0x04001523 RID: 5411
		public bool focusWhenOpened = true;

		// Token: 0x04001524 RID: 5412
		public float shadowAlpha = 1f;

		// Token: 0x04001525 RID: 5413
		public SoundDef soundAppear;

		// Token: 0x04001526 RID: 5414
		public SoundDef soundClose;

		// Token: 0x04001527 RID: 5415
		public SoundDef soundAmbient;

		// Token: 0x04001528 RID: 5416
		public bool silenceAmbientSound;

		// Token: 0x04001529 RID: 5417
		public bool grayOutIfOtherDialogOpen;

		// Token: 0x0400152A RID: 5418
		public Vector2 commonSearchWidgetOffset = new Vector2(0f, Window.CloseButSize.y - Window.QuickSearchSize.y) / 2f;

		// Token: 0x0400152B RID: 5419
		public bool openMenuOnCancel;

		// Token: 0x0400152C RID: 5420
		public bool preventSave;

		// Token: 0x0400152D RID: 5421
		public const float StandardMargin = 18f;

		// Token: 0x0400152E RID: 5422
		public const float FooterRowHeight = 55f;

		// Token: 0x0400152F RID: 5423
		public static readonly Vector2 CloseButSize = new Vector2(120f, 40f);

		// Token: 0x04001530 RID: 5424
		public static readonly Vector2 QuickSearchSize = new Vector2(240f, 24f);

		// Token: 0x04001531 RID: 5425
		public int ID;

		// Token: 0x04001532 RID: 5426
		public Rect windowRect;

		// Token: 0x04001533 RID: 5427
		private Sustainer sustainerAmbient;

		// Token: 0x04001534 RID: 5428
		private WindowResizer resizer;

		// Token: 0x04001535 RID: 5429
		private bool resizeLater;

		// Token: 0x04001536 RID: 5430
		private Rect resizeLaterRect;

		// Token: 0x04001537 RID: 5431
		private string onGUIProfilerLabelCached;

		// Token: 0x04001538 RID: 5432
		public string extraOnGUIProfilerLabelCached;

		// Token: 0x04001539 RID: 5433
		private GUI.WindowFunction innerWindowOnGUICached;

		// Token: 0x0400153A RID: 5434
		private Action notify_CommonSearchChangedCached;
	}
}
