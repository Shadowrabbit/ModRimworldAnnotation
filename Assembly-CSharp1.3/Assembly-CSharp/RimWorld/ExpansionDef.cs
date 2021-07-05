using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A60 RID: 2656
	public class ExpansionDef : Def
	{
		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06003FCB RID: 16331 RVA: 0x0015A2FA File Offset: 0x001584FA
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06003FCC RID: 16332 RVA: 0x0015A322 File Offset: 0x00158522
		public Texture2D BackgroundImage
		{
			get
			{
				if (this.cachedBG == null)
				{
					this.cachedBG = ContentFinder<Texture2D>.Get(this.backgroundPath, true);
				}
				return this.cachedBG;
			}
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06003FCD RID: 16333 RVA: 0x0015A34A File Offset: 0x0015854A
		public List<Texture2D> PreviewImages
		{
			get
			{
				if (this.cachedPreviewImages.NullOrEmpty<Texture2D>())
				{
					if (this.previewImagesFolderPath.NullOrEmpty())
					{
						return null;
					}
					this.cachedPreviewImages = new List<Texture2D>(ContentFinder<Texture2D>.GetAllInFolder(this.previewImagesFolderPath));
				}
				return this.cachedPreviewImages;
			}
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06003FCE RID: 16334 RVA: 0x0015A384 File Offset: 0x00158584
		public string StoreURL
		{
			get
			{
				if (!this.steamUrl.NullOrEmpty())
				{
					return this.steamUrl;
				}
				return this.siteUrl;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06003FCF RID: 16335 RVA: 0x0015A3A0 File Offset: 0x001585A0
		public ExpansionStatus Status
		{
			get
			{
				if (ModsConfig.IsActive(this.linkedMod))
				{
					return ExpansionStatus.Active;
				}
				if (ModLister.AllInstalledMods.Any((ModMetaData m) => m.SamePackageId(this.linkedMod, false)))
				{
					return ExpansionStatus.Installed;
				}
				return ExpansionStatus.NotInstalled;
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06003FD0 RID: 16336 RVA: 0x0015A3CC File Offset: 0x001585CC
		public string StatusDescription
		{
			get
			{
				ExpansionStatus status = this.Status;
				if (status == ExpansionStatus.Active)
				{
					return "ContentActive".Translate();
				}
				if (status != ExpansionStatus.Installed)
				{
					return "ContentNotInstalled".Translate();
				}
				return "ContentInstalledButNotActive".Translate();
			}
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x0015A418 File Offset: 0x00158618
		public override void PostLoad()
		{
			base.PostLoad();
			this.linkedMod = this.linkedMod.ToLower();
		}

		// Token: 0x06003FD2 RID: 16338 RVA: 0x0015A431 File Offset: 0x00158631
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.linkedMod, false);
			if (modWithIdentifier != null && !modWithIdentifier.Official)
			{
				yield return modWithIdentifier.Name + " - ExpansionDefs are used for official content. For mods, you should define ModMetaData in About.xml.";
			}
			yield break;
			yield break;
		}

		// Token: 0x040023BD RID: 9149
		[NoTranslate]
		public string iconPath;

		// Token: 0x040023BE RID: 9150
		[NoTranslate]
		public string backgroundPath;

		// Token: 0x040023BF RID: 9151
		[NoTranslate]
		public string linkedMod;

		// Token: 0x040023C0 RID: 9152
		[NoTranslate]
		public string steamUrl;

		// Token: 0x040023C1 RID: 9153
		[NoTranslate]
		public string siteUrl;

		// Token: 0x040023C2 RID: 9154
		[NoTranslate]
		public string previewImagesFolderPath;

		// Token: 0x040023C3 RID: 9155
		public bool isCore;

		// Token: 0x040023C4 RID: 9156
		private Texture2D cachedIcon;

		// Token: 0x040023C5 RID: 9157
		private Texture2D cachedBG;

		// Token: 0x040023C6 RID: 9158
		private List<Texture2D> cachedPreviewImages;
	}
}
