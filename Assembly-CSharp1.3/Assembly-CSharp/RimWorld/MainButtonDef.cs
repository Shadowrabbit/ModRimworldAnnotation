using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A89 RID: 2697
	public class MainButtonDef : Def
	{
		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06004061 RID: 16481 RVA: 0x0015C569 File Offset: 0x0015A769
		public MainButtonWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (MainButtonWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06004062 RID: 16482 RVA: 0x0015C59B File Offset: 0x0015A79B
		public MainTabWindow TabWindow
		{
			get
			{
				if (this.tabWindowInt == null && this.tabWindowClass != null)
				{
					this.tabWindowInt = (MainTabWindow)Activator.CreateInstance(this.tabWindowClass);
					this.tabWindowInt.def = this;
				}
				return this.tabWindowInt;
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06004063 RID: 16483 RVA: 0x0015C5DC File Offset: 0x0015A7DC
		public string ShortenedLabelCap
		{
			get
			{
				if (this.cachedShortenedLabelCap == null)
				{
					this.cachedShortenedLabelCap = this.LabelCap.Shorten();
				}
				return this.cachedShortenedLabelCap;
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06004064 RID: 16484 RVA: 0x0015C610 File Offset: 0x0015A810
		public float LabelCapWidth
		{
			get
			{
				if (this.cachedLabelCapWidth < 0f)
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					this.cachedLabelCapWidth = Text.CalcSize(this.LabelCap).x;
					Text.Font = font;
				}
				return this.cachedLabelCapWidth;
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06004065 RID: 16485 RVA: 0x0015C650 File Offset: 0x0015A850
		public float ShortenedLabelCapWidth
		{
			get
			{
				if (this.cachedShortenedLabelCapWidth < 0f)
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					this.cachedShortenedLabelCapWidth = Text.CalcSize(this.ShortenedLabelCap).x;
					Text.Font = font;
				}
				return this.cachedShortenedLabelCapWidth;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x06004066 RID: 16486 RVA: 0x0015C68B File Offset: 0x0015A88B
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null && this.iconPath != null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x0015C6BB File Offset: 0x0015A8BB
		public override void PostLoad()
		{
			base.PostLoad();
			this.cachedHighlightTagClosed = "MainTab-" + this.defName + "-Closed";
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x0015C6DE File Offset: 0x0015A8DE
		public void Notify_SwitchedMap()
		{
			if (this.tabWindowInt != null)
			{
				Find.WindowStack.TryRemove(this.tabWindowInt, true);
				this.tabWindowInt = null;
			}
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x0015C701 File Offset: 0x0015A901
		public void Notify_ClearingAllMapsMemory()
		{
			this.tabWindowInt = null;
		}

		// Token: 0x040024FA RID: 9466
		public Type workerClass = typeof(MainButtonWorker_ToggleTab);

		// Token: 0x040024FB RID: 9467
		public Type tabWindowClass;

		// Token: 0x040024FC RID: 9468
		public bool buttonVisible = true;

		// Token: 0x040024FD RID: 9469
		public int order;

		// Token: 0x040024FE RID: 9470
		public KeyCode defaultHotKey;

		// Token: 0x040024FF RID: 9471
		public bool canBeTutorDenied = true;

		// Token: 0x04002500 RID: 9472
		public bool validWithoutMap;

		// Token: 0x04002501 RID: 9473
		public bool minimized;

		// Token: 0x04002502 RID: 9474
		public string iconPath;

		// Token: 0x04002503 RID: 9475
		public bool closesWorldView;

		// Token: 0x04002504 RID: 9476
		[Unsaved(false)]
		public KeyBindingDef hotKey;

		// Token: 0x04002505 RID: 9477
		[Unsaved(false)]
		public string cachedTutorTag;

		// Token: 0x04002506 RID: 9478
		[Unsaved(false)]
		public string cachedHighlightTagClosed;

		// Token: 0x04002507 RID: 9479
		[Unsaved(false)]
		private MainButtonWorker workerInt;

		// Token: 0x04002508 RID: 9480
		[Unsaved(false)]
		private MainTabWindow tabWindowInt;

		// Token: 0x04002509 RID: 9481
		[Unsaved(false)]
		private string cachedShortenedLabelCap;

		// Token: 0x0400250A RID: 9482
		[Unsaved(false)]
		private float cachedLabelCapWidth = -1f;

		// Token: 0x0400250B RID: 9483
		[Unsaved(false)]
		private float cachedShortenedLabelCapWidth = -1f;

		// Token: 0x0400250C RID: 9484
		[Unsaved(false)]
		private Texture2D icon;

		// Token: 0x0400250D RID: 9485
		public const int ButtonHeight = 35;
	}
}
