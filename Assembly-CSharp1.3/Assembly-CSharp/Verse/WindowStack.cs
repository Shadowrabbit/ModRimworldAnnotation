using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200045E RID: 1118
	public class WindowStack
	{
		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x060021C7 RID: 8647 RVA: 0x000D3150 File Offset: 0x000D1350
		public int Count
		{
			get
			{
				return this.windows.Count;
			}
		}

		// Token: 0x17000651 RID: 1617
		public Window this[int index]
		{
			get
			{
				return this.windows[index];
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x060021C9 RID: 8649 RVA: 0x000D316B File Offset: 0x000D136B
		public IList<Window> Windows
		{
			get
			{
				return this.windows.AsReadOnly();
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x060021CA RID: 8650 RVA: 0x000D3178 File Offset: 0x000D1378
		public FloatMenu FloatMenu
		{
			get
			{
				return this.WindowOfType<FloatMenu>();
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x060021CB RID: 8651 RVA: 0x000D3180 File Offset: 0x000D1380
		public bool WindowsForcePause
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].forcePause)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x060021CC RID: 8652 RVA: 0x000D31BC File Offset: 0x000D13BC
		public bool WindowsPreventCameraMotion
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventCameraMotion)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x060021CD RID: 8653 RVA: 0x000D31F8 File Offset: 0x000D13F8
		public bool WindowsPreventDrawTutor
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventDrawTutor)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x060021CE RID: 8654 RVA: 0x000D3231 File Offset: 0x000D1431
		public float SecondsSinceClosedGameStartDialog
		{
			get
			{
				if (this.gameStartDialogOpen)
				{
					return 0f;
				}
				if (this.timeGameStartDialogClosed < 0f)
				{
					return 9999999f;
				}
				return Time.time - this.timeGameStartDialogClosed;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x060021CF RID: 8655 RVA: 0x000D3260 File Offset: 0x000D1460
		public bool MouseObscuredNow
		{
			get
			{
				return this.GetWindowAt(UI.MousePosUIInvertedUseEventIfCan) != this.currentlyDrawnWindow;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x060021D0 RID: 8656 RVA: 0x000D3278 File Offset: 0x000D1478
		public bool CurrentWindowGetsInput
		{
			get
			{
				return this.GetsInput(this.currentlyDrawnWindow);
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x060021D1 RID: 8657 RVA: 0x000D3288 File Offset: 0x000D1488
		public bool NonImmediateDialogWindowOpen
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (!(this.windows[i] is ImmediateWindow) && this.windows[i].layer == WindowLayer.Dialog)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x060021D2 RID: 8658 RVA: 0x000D32D8 File Offset: 0x000D14D8
		public bool WindowsPreventSave
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventSave)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000D3314 File Offset: 0x000D1514
		public void WindowsUpdate()
		{
			this.AdjustWindowsIfResolutionChanged();
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].WindowUpdate();
			}
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x000D3350 File Offset: 0x000D1550
		public void HandleEventsHighPriority()
		{
			if (Event.current.type == EventType.MouseDown && this.GetWindowAt(UI.GUIToScreenPoint(Event.current.mousePosition)) == null)
			{
				bool flag = this.CloseWindowsBecauseClicked(null);
				this.NotifyOutsideClicks(null);
				if (flag)
				{
					Event.current.Use();
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				this.Notify_PressedCancel();
			}
			if (KeyBindingDefOf.Accept.KeyDownEvent)
			{
				this.Notify_PressedAccept();
			}
			if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.KeyDown) && !this.GetsInput(null))
			{
				Event.current.Use();
			}
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x000D33EC File Offset: 0x000D15EC
		public void WindowStackOnGUI()
		{
			this.windowStackOnGUITmpList.Clear();
			this.windowStackOnGUITmpList.AddRange(this.windows);
			for (int i = this.windowStackOnGUITmpList.Count - 1; i >= 0; i--)
			{
				this.windowStackOnGUITmpList[i].ExtraOnGUI();
			}
			this.UpdateImmediateWindowsList();
			this.windowStackOnGUITmpList.Clear();
			this.windowStackOnGUITmpList.AddRange(this.windows);
			for (int j = 0; j < this.windowStackOnGUITmpList.Count; j++)
			{
				if (this.windowStackOnGUITmpList[j].drawShadow)
				{
					GUI.color = new Color(1f, 1f, 1f, this.windowStackOnGUITmpList[j].shadowAlpha);
					Widgets.DrawShadowAround(this.windowStackOnGUITmpList[j].windowRect);
					GUI.color = Color.white;
				}
				this.windowStackOnGUITmpList[j].WindowOnGUI();
			}
			if (this.updateInternalWindowsOrderLater)
			{
				this.updateInternalWindowsOrderLater = false;
				this.UpdateInternalWindowsOrder();
			}
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x000D34FC File Offset: 0x000D16FC
		public void Notify_ClickedInsideWindow(Window window)
		{
			if (this.GetsInput(window))
			{
				this.windows.Remove(window);
				this.InsertAtCorrectPositionInList(window);
				this.focusedWindow = window;
			}
			else
			{
				Event.current.Use();
			}
			this.CloseWindowsBecauseClicked(window);
			this.NotifyOutsideClicks(window);
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x000D354F File Offset: 0x000D174F
		public void Notify_ManuallySetFocus(Window window)
		{
			this.focusedWindow = window;
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x000D3560 File Offset: 0x000D1760
		public void Notify_PressedCancel()
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if ((this.windows[i].closeOnCancel || this.windows[i].forceCatchAcceptAndCancelEventEvenIfUnfocused) && this.GetsInput(this.windows[i]))
				{
					this.windows[i].OnCancelKeyPressed();
					return;
				}
			}
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x000D35D4 File Offset: 0x000D17D4
		public void Notify_PressedAccept()
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if ((this.windows[i].closeOnAccept || this.windows[i].forceCatchAcceptAndCancelEventEvenIfUnfocused) && this.GetsInput(this.windows[i]))
				{
					this.windows[i].OnAcceptKeyPressed();
					return;
				}
			}
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x000D3645 File Offset: 0x000D1845
		public void Notify_GameStartDialogOpened()
		{
			this.gameStartDialogOpen = true;
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x000D364E File Offset: 0x000D184E
		public void Notify_GameStartDialogClosed()
		{
			this.timeGameStartDialogClosed = Time.time;
			this.gameStartDialogOpen = false;
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x000D3664 File Offset: 0x000D1864
		public bool IsOpen<WindowType>()
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] is WindowType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x000D36A0 File Offset: 0x000D18A0
		public bool IsOpen(Type type)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].GetType() == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x000D36DF File Offset: 0x000D18DF
		public bool IsOpen(Window window)
		{
			return this.windows.Contains(window);
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x000D36F0 File Offset: 0x000D18F0
		public WindowType WindowOfType<WindowType>() where WindowType : class
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] is WindowType)
				{
					return this.windows[i] as WindowType;
				}
			}
			return default(WindowType);
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x000D3748 File Offset: 0x000D1948
		public bool GetsInput(Window window)
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i] == window)
				{
					return true;
				}
				if (this.windows[i].absorbInputAroundWindow)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x000D3794 File Offset: 0x000D1994
		public void Add(Window window)
		{
			this.RemoveWindowsOfType(window.GetType());
			window.ID = WindowStack.uniqueWindowID++;
			window.PreOpen();
			this.InsertAtCorrectPositionInList(window);
			this.FocusAfterInsertIfShould(window);
			this.updateInternalWindowsOrderLater = true;
			window.PostOpen();
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x000D37E4 File Offset: 0x000D19E4
		public void ImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground = true, bool absorbInputAroundWindow = false, float shadowAlpha = 1f, Action doClickOutsideFunc = null)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (ID == 0)
			{
				Log.Warning("Used 0 as immediate window ID.");
				return;
			}
			ID = -Math.Abs(ID);
			bool flag = false;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].ID == ID)
				{
					ImmediateWindow immediateWindow = (ImmediateWindow)this.windows[i];
					immediateWindow.windowRect = rect;
					immediateWindow.doWindowFunc = doWindowFunc;
					immediateWindow.doClickOutsideFunc = doClickOutsideFunc;
					immediateWindow.layer = layer;
					immediateWindow.doWindowBackground = doBackground;
					immediateWindow.absorbInputAroundWindow = absorbInputAroundWindow;
					immediateWindow.shadowAlpha = shadowAlpha;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.AddNewImmediateWindow(ID, rect, layer, doWindowFunc, doBackground, absorbInputAroundWindow, shadowAlpha, doClickOutsideFunc);
			}
			this.immediateWindowsRequests.Add(ID);
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x000D38B0 File Offset: 0x000D1AB0
		public bool TryRemove(Type windowType, bool doCloseSound = true)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].GetType() == windowType)
				{
					return this.TryRemove(this.windows[i], doCloseSound);
				}
			}
			return false;
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x000D3904 File Offset: 0x000D1B04
		public bool TryRemoveAssignableFromType(Type windowType, bool doCloseSound = true)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (windowType.IsAssignableFrom(this.windows[i].GetType()))
				{
					return this.TryRemove(this.windows[i], doCloseSound);
				}
			}
			return false;
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x000D3958 File Offset: 0x000D1B58
		public bool TryRemove(Window window, bool doCloseSound = true)
		{
			bool flag = false;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] == window)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (doCloseSound && window.soundClose != null)
			{
				window.soundClose.PlayOneShotOnCamera(null);
			}
			window.PreClose();
			this.windows.Remove(window);
			window.PostClose();
			if (this.focusedWindow == window)
			{
				if (this.windows.Count > 0)
				{
					this.focusedWindow = this.windows[this.windows.Count - 1];
				}
				else
				{
					this.focusedWindow = null;
				}
				this.updateInternalWindowsOrderLater = true;
			}
			return true;
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x000D3A0C File Offset: 0x000D1C0C
		public Window GetWindowAt(Vector2 pos)
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i].windowRect.Contains(pos))
				{
					return this.windows[i];
				}
			}
			return null;
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x000D3A58 File Offset: 0x000D1C58
		private void AddNewImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground, bool absorbInputAroundWindow, float shadowAlpha, Action doClickOutsideFunc)
		{
			if (ID >= 0)
			{
				Log.Error("Invalid immediate window ID.");
				return;
			}
			ImmediateWindow immediateWindow = new ImmediateWindow();
			immediateWindow.ID = ID;
			immediateWindow.layer = layer;
			immediateWindow.doWindowFunc = doWindowFunc;
			immediateWindow.doClickOutsideFunc = doClickOutsideFunc;
			immediateWindow.doWindowBackground = doBackground;
			immediateWindow.absorbInputAroundWindow = absorbInputAroundWindow;
			immediateWindow.shadowAlpha = shadowAlpha;
			immediateWindow.PreOpen();
			immediateWindow.windowRect = rect;
			this.InsertAtCorrectPositionInList(immediateWindow);
			this.FocusAfterInsertIfShould(immediateWindow);
			this.updateInternalWindowsOrderLater = true;
			immediateWindow.PostOpen();
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x000D3AD8 File Offset: 0x000D1CD8
		private void UpdateImmediateWindowsList()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			this.updateImmediateWindowsListTmpList.Clear();
			this.updateImmediateWindowsListTmpList.AddRange(this.windows);
			for (int i = 0; i < this.updateImmediateWindowsListTmpList.Count; i++)
			{
				if (this.IsImmediateWindow(this.updateImmediateWindowsListTmpList[i]))
				{
					bool flag = false;
					for (int j = 0; j < this.immediateWindowsRequests.Count; j++)
					{
						if (this.immediateWindowsRequests[j] == this.updateImmediateWindowsListTmpList[i].ID)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.TryRemove(this.updateImmediateWindowsListTmpList[i], true);
					}
				}
			}
			this.immediateWindowsRequests.Clear();
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x000D3B98 File Offset: 0x000D1D98
		private void InsertAtCorrectPositionInList(Window window)
		{
			int index = 0;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (window.layer >= this.windows[i].layer)
				{
					index = i + 1;
				}
			}
			this.windows.Insert(index, window);
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x000D3BF0 File Offset: 0x000D1DF0
		private void FocusAfterInsertIfShould(Window window)
		{
			if (!window.focusWhenOpened)
			{
				return;
			}
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i] == window)
				{
					this.focusedWindow = this.windows[i];
					this.updateInternalWindowsOrderLater = true;
					return;
				}
				if (this.windows[i] == this.focusedWindow)
				{
					break;
				}
			}
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x000D3C5C File Offset: 0x000D1E5C
		private void AdjustWindowsIfResolutionChanged()
		{
			IntVec2 a = new IntVec2(UI.screenWidth, UI.screenHeight);
			if (!UnityGUIBugsFixer.ResolutionsEqual(a, this.prevResolution))
			{
				this.prevResolution = a;
				for (int i = 0; i < this.windows.Count; i++)
				{
					this.windows[i].Notify_ResolutionChanged();
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.MarkColonistsDirty();
				}
			}
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x000D3CC8 File Offset: 0x000D1EC8
		private void RemoveWindowsOfType(Type type)
		{
			this.removeWindowsOfTypeTmpList.Clear();
			this.removeWindowsOfTypeTmpList.AddRange(this.windows);
			for (int i = 0; i < this.removeWindowsOfTypeTmpList.Count; i++)
			{
				if (this.removeWindowsOfTypeTmpList[i].onlyOneOfTypeAllowed && this.removeWindowsOfTypeTmpList[i].GetType() == type)
				{
					this.TryRemove(this.removeWindowsOfTypeTmpList[i], true);
				}
			}
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x000D3D48 File Offset: 0x000D1F48
		private void NotifyOutsideClicks(Window clickedWindow)
		{
			foreach (Window window in this.windows)
			{
				if (window != clickedWindow)
				{
					window.Notify_ClickOutsideWindow();
				}
			}
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x000D3DA0 File Offset: 0x000D1FA0
		private bool CloseWindowsBecauseClicked(Window clickedWindow)
		{
			this.closeWindowsTmpList.Clear();
			this.closeWindowsTmpList.AddRange(this.windows);
			bool result = false;
			int num = this.closeWindowsTmpList.Count - 1;
			while (num >= 0 && this.closeWindowsTmpList[num] != clickedWindow)
			{
				if (this.closeWindowsTmpList[num].closeOnClickedOutside)
				{
					result = true;
					this.TryRemove(this.closeWindowsTmpList[num], true);
				}
				num--;
			}
			return result;
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x000D3E1C File Offset: 0x000D201C
		private bool IsImmediateWindow(Window window)
		{
			return window.ID < 0;
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x000D3E28 File Offset: 0x000D2028
		private void UpdateInternalWindowsOrder()
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				GUI.BringWindowToFront(this.windows[i].ID);
			}
			if (this.focusedWindow != null)
			{
				GUI.FocusWindow(this.focusedWindow.ID);
			}
		}

		// Token: 0x0400153F RID: 5439
		public Window currentlyDrawnWindow;

		// Token: 0x04001540 RID: 5440
		private List<Window> windows = new List<Window>();

		// Token: 0x04001541 RID: 5441
		private List<int> immediateWindowsRequests = new List<int>();

		// Token: 0x04001542 RID: 5442
		private bool updateInternalWindowsOrderLater;

		// Token: 0x04001543 RID: 5443
		private Window focusedWindow;

		// Token: 0x04001544 RID: 5444
		private static int uniqueWindowID;

		// Token: 0x04001545 RID: 5445
		private bool gameStartDialogOpen;

		// Token: 0x04001546 RID: 5446
		private float timeGameStartDialogClosed = -1f;

		// Token: 0x04001547 RID: 5447
		private IntVec2 prevResolution = new IntVec2(UI.screenWidth, UI.screenHeight);

		// Token: 0x04001548 RID: 5448
		private List<Window> windowStackOnGUITmpList = new List<Window>();

		// Token: 0x04001549 RID: 5449
		private List<Window> updateImmediateWindowsListTmpList = new List<Window>();

		// Token: 0x0400154A RID: 5450
		private List<Window> removeWindowsOfTypeTmpList = new List<Window>();

		// Token: 0x0400154B RID: 5451
		private List<Window> closeWindowsTmpList = new List<Window>();
	}
}
