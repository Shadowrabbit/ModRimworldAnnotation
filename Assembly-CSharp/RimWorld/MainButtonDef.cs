using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB3 RID: 4019
	public class MainButtonDef : Def
	{
		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x060057E9 RID: 22505 RVA: 0x0003CFC0 File Offset: 0x0003B1C0
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

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x060057EA RID: 22506 RVA: 0x0003CFF2 File Offset: 0x0003B1F2
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

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x060057EB RID: 22507 RVA: 0x001CEBBC File Offset: 0x001CCDBC
		public string ShortenedLabelCap
		{
			get
			{
				if (this.cachedShortenedLabelCap == null)
				{
					this.cachedShortenedLabelCap = base.LabelCap.Shorten();
				}
				return this.cachedShortenedLabelCap;
			}
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x060057EC RID: 22508 RVA: 0x0003D032 File Offset: 0x0003B232
		public float LabelCapWidth
		{
			get
			{
				if (this.cachedLabelCapWidth < 0f)
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					this.cachedLabelCapWidth = Text.CalcSize(base.LabelCap).x;
					Text.Font = font;
				}
				return this.cachedLabelCapWidth;
			}
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x060057ED RID: 22509 RVA: 0x0003D072 File Offset: 0x0003B272
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

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x060057EE RID: 22510 RVA: 0x0003D0AD File Offset: 0x0003B2AD
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

		// Token: 0x060057EF RID: 22511 RVA: 0x0003D0DD File Offset: 0x0003B2DD
		public override void PostLoad()
		{
			base.PostLoad();
			this.cachedHighlightTagClosed = "MainTab-" + this.defName + "-Closed";
		}

		// Token: 0x060057F0 RID: 22512 RVA: 0x0003D100 File Offset: 0x0003B300
		public void Notify_SwitchedMap()
		{
			if (this.tabWindowInt != null)
			{
				Find.WindowStack.TryRemove(this.tabWindowInt, true);
				this.tabWindowInt = null;
			}
		}

		// Token: 0x060057F1 RID: 22513 RVA: 0x0003D123 File Offset: 0x0003B323
		public void Notify_ClearingAllMapsMemory()
		{
			this.tabWindowInt = null;
		}

		// Token: 0x040039E0 RID: 14816
		public Type workerClass = typeof(MainButtonWorker_ToggleTab);

		// Token: 0x040039E1 RID: 14817
		public Type tabWindowClass;

		// Token: 0x040039E2 RID: 14818
		public bool buttonVisible = true;

		// Token: 0x040039E3 RID: 14819
		public int order;

		// Token: 0x040039E4 RID: 14820
		public KeyCode defaultHotKey;

		// Token: 0x040039E5 RID: 14821
		public bool canBeTutorDenied = true;

		// Token: 0x040039E6 RID: 14822
		public bool validWithoutMap;

		// Token: 0x040039E7 RID: 14823
		public bool minimized;

		// Token: 0x040039E8 RID: 14824
		public string iconPath;

		// Token: 0x040039E9 RID: 14825
		[Unsaved(false)]
		public KeyBindingDef hotKey;

		// Token: 0x040039EA RID: 14826
		[Unsaved(false)]
		public string cachedTutorTag;

		// Token: 0x040039EB RID: 14827
		[Unsaved(false)]
		public string cachedHighlightTagClosed;

		// Token: 0x040039EC RID: 14828
		[Unsaved(false)]
		private MainButtonWorker workerInt;

		// Token: 0x040039ED RID: 14829
		[Unsaved(false)]
		private MainTabWindow tabWindowInt;

		// Token: 0x040039EE RID: 14830
		[Unsaved(false)]
		private string cachedShortenedLabelCap;

		// Token: 0x040039EF RID: 14831
		[Unsaved(false)]
		private float cachedLabelCapWidth = -1f;

		// Token: 0x040039F0 RID: 14832
		[Unsaved(false)]
		private float cachedShortenedLabelCapWidth = -1f;

		// Token: 0x040039F1 RID: 14833
		[Unsaved(false)]
		private Texture2D icon;

		// Token: 0x040039F2 RID: 14834
		public const int ButtonHeight = 35;
	}
}
