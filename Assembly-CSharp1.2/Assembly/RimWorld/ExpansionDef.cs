using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F84 RID: 3972
	public class ExpansionDef : Def
	{
		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x0600571C RID: 22300 RVA: 0x0003C62F File Offset: 0x0003A82F
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

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x0600571D RID: 22301 RVA: 0x0003C657 File Offset: 0x0003A857
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

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x0600571E RID: 22302 RVA: 0x0003C67F File Offset: 0x0003A87F
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

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x0600571F RID: 22303 RVA: 0x0003C6B9 File Offset: 0x0003A8B9
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

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06005720 RID: 22304 RVA: 0x0003C6D5 File Offset: 0x0003A8D5
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

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06005721 RID: 22305 RVA: 0x001CC8B4 File Offset: 0x001CAAB4
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

		// Token: 0x06005722 RID: 22306 RVA: 0x0003C701 File Offset: 0x0003A901
		public override void PostLoad()
		{
			base.PostLoad();
			this.linkedMod = this.linkedMod.ToLower();
		}

		// Token: 0x06005723 RID: 22307 RVA: 0x0003C71A File Offset: 0x0003A91A
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

		// Token: 0x0400389B RID: 14491
		[NoTranslate]
		public string iconPath;

		// Token: 0x0400389C RID: 14492
		[NoTranslate]
		public string backgroundPath;

		// Token: 0x0400389D RID: 14493
		[NoTranslate]
		public string linkedMod;

		// Token: 0x0400389E RID: 14494
		[NoTranslate]
		public string steamUrl;

		// Token: 0x0400389F RID: 14495
		[NoTranslate]
		public string siteUrl;

		// Token: 0x040038A0 RID: 14496
		[NoTranslate]
		public string previewImagesFolderPath;

		// Token: 0x040038A1 RID: 14497
		public bool isCore;

		// Token: 0x040038A2 RID: 14498
		private Texture2D cachedIcon;

		// Token: 0x040038A3 RID: 14499
		private Texture2D cachedBG;

		// Token: 0x040038A4 RID: 14500
		private List<Texture2D> cachedPreviewImages;
	}
}
