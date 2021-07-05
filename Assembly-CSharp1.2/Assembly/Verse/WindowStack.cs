using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020007B0 RID: 1968
	public class WindowStack
	{
		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x0600317D RID: 12669 RVA: 0x00027008 File Offset: 0x00025208
		public int Count
		{
			get
			{
				return this.windows.Count;
			}
		}

		// Token: 0x17000769 RID: 1897
		public Window this[int index]
		{
			get
			{
				return this.windows[index];
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x0600317F RID: 12671 RVA: 0x00027023 File Offset: 0x00025223
		public IList<Window> Windows
		{
			get
			{
				return this.windows.AsReadOnly();
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06003180 RID: 12672 RVA: 0x00027030 File Offset: 0x00025230
		public FloatMenu FloatMenu
		{
			get
			{
				return this.WindowOfType<FloatMenu>();
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06003181 RID: 12673 RVA: 0x00146278 File Offset: 0x00144478
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

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06003182 RID: 12674 RVA: 0x001462B4 File Offset: 0x001444B4
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

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06003183 RID: 12675 RVA: 0x001462F0 File Offset: 0x001444F0
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

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06003184 RID: 12676 RVA: 0x00027038 File Offset: 0x00025238
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

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06003185 RID: 12677 RVA: 0x00027067 File Offset: 0x00025267
		public bool MouseObscuredNow
		{
			get
			{
				return this.GetWindowAt(UI.MousePosUIInvertedUseEventIfCan) != this.currentlyDrawnWindow;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06003186 RID: 12678 RVA: 0x0002707F File Offset: 0x0002527F
		public bool CurrentWindowGetsInput
		{
			get
			{
				return this.GetsInput(this.currentlyDrawnWindow);
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06003187 RID: 12679 RVA: 0x0014632C File Offset: 0x0014452C
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

		// Token: 0x06003188 RID: 12680 RVA: 0x0014637C File Offset: 0x0014457C
		public void WindowsUpdate()
		{
			this.AdjustWindowsIfResolutionChanged();
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].WindowUpdate();
			}
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x001463B8 File Offset: 0x001445B8
		public void HandleEventsHighPriority()
		{
			if (Event.current.type == EventType.MouseDown && this.GetWindowAt(UI.GUIToScreenPoint(Event.current.mousePosition)) == null && this.CloseWindowsBecauseClicked(null))
			{
				Event.current.Use();
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

		// Token: 0x0600318A RID: 12682 RVA: 0x0014644C File Offset: 0x0014464C
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

		// Token: 0x0600318B RID: 12683 RVA: 0x0014655C File Offset: 0x0014475C
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
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x0002708D File Offset: 0x0002528D
		public void Notify_ManuallySetFocus(Window window)
		{
			this.focusedWindow = window;
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x001465A8 File Offset: 0x001447A8
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

		// Token: 0x0600318E RID: 12686 RVA: 0x0014661C File Offset: 0x0014481C
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

		// Token: 0x0600318F RID: 12687 RVA: 0x0002709D File Offset: 0x0002529D
		public void Notify_GameStartDialogOpened()
		{
			this.gameStartDialogOpen = true;
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x000270A6 File Offset: 0x000252A6
		public void Notify_GameStartDialogClosed()
		{
			this.timeGameStartDialogClosed = Time.time;
			this.gameStartDialogOpen = false;
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x00146690 File Offset: 0x00144890
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

		// Token: 0x06003192 RID: 12690 RVA: 0x001466CC File Offset: 0x001448CC
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

		// Token: 0x06003193 RID: 12691 RVA: 0x000270BA File Offset: 0x000252BA
		public bool IsOpen(Window window)
		{
			return this.windows.Contains(window);
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x0014670C File Offset: 0x0014490C
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

		// Token: 0x06003195 RID: 12693 RVA: 0x00146764 File Offset: 0x00144964
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

		// Token: 0x06003196 RID: 12694 RVA: 0x001467B0 File Offset: 0x001449B0
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

		// Token: 0x06003197 RID: 12695 RVA: 0x00146800 File Offset: 0x00144A00
		public void ImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground = true, bool absorbInputAroundWindow = false, float shadowAlpha = 1f)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (ID == 0)
			{
				Log.Warning("Used 0 as immediate window ID.", false);
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
				this.AddNewImmediateWindow(ID, rect, layer, doWindowFunc, doBackground, absorbInputAroundWindow, shadowAlpha);
			}
			this.immediateWindowsRequests.Add(ID);
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x001468C4 File Offset: 0x00144AC4
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

		// Token: 0x06003199 RID: 12697 RVA: 0x00146918 File Offset: 0x00144B18
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

		// Token: 0x0600319A RID: 12698 RVA: 0x0014696C File Offset: 0x00144B6C
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

		// Token: 0x0600319B RID: 12699 RVA: 0x00146A20 File Offset: 0x00144C20
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

		// Token: 0x0600319C RID: 12700 RVA: 0x00146A6C File Offset: 0x00144C6C
		private void AddNewImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground, bool absorbInputAroundWindow, float shadowAlpha)
		{
			if (ID >= 0)
			{
				Log.Error("Invalid immediate window ID.", false);
				return;
			}
			ImmediateWindow immediateWindow = new ImmediateWindow();
			immediateWindow.ID = ID;
			immediateWindow.layer = layer;
			immediateWindow.doWindowFunc = doWindowFunc;
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

		// Token: 0x0600319D RID: 12701 RVA: 0x00146AE8 File Offset: 0x00144CE8
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

		// Token: 0x0600319E RID: 12702 RVA: 0x00146BA8 File Offset: 0x00144DA8
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

		// Token: 0x0600319F RID: 12703 RVA: 0x00146C00 File Offset: 0x00144E00
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

		// Token: 0x060031A0 RID: 12704 RVA: 0x00146C6C File Offset: 0x00144E6C
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

		// Token: 0x060031A1 RID: 12705 RVA: 0x00146CD8 File Offset: 0x00144ED8
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

		// Token: 0x060031A2 RID: 12706 RVA: 0x00146D58 File Offset: 0x00144F58
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

		// Token: 0x060031A3 RID: 12707 RVA: 0x000270C8 File Offset: 0x000252C8
		private bool IsImmediateWindow(Window window)
		{
			return window.ID < 0;
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x00146DD4 File Offset: 0x00144FD4
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

		// Token: 0x04002252 RID: 8786
		public Window currentlyDrawnWindow;

		// Token: 0x04002253 RID: 8787
		private List<Window> windows = new List<Window>();

		// Token: 0x04002254 RID: 8788
		private List<int> immediateWindowsRequests = new List<int>();

		// Token: 0x04002255 RID: 8789
		private bool updateInternalWindowsOrderLater;

		// Token: 0x04002256 RID: 8790
		private Window focusedWindow;

		// Token: 0x04002257 RID: 8791
		private static int uniqueWindowID;

		// Token: 0x04002258 RID: 8792
		private bool gameStartDialogOpen;

		// Token: 0x04002259 RID: 8793
		private float timeGameStartDialogClosed = -1f;

		// Token: 0x0400225A RID: 8794
		private IntVec2 prevResolution = new IntVec2(UI.screenWidth, UI.screenHeight);

		// Token: 0x0400225B RID: 8795
		private List<Window> windowStackOnGUITmpList = new List<Window>();

		// Token: 0x0400225C RID: 8796
		private List<Window> updateImmediateWindowsListTmpList = new List<Window>();

		// Token: 0x0400225D RID: 8797
		private List<Window> removeWindowsOfTypeTmpList = new List<Window>();

		// Token: 0x0400225E RID: 8798
		private List<Window> closeWindowsTmpList = new List<Window>();
	}
}
