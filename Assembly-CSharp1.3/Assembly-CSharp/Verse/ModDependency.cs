using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000241 RID: 577
	public class ModDependency : ModRequirement
	{
		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x0005E661 File Offset: 0x0005C861
		public override string RequirementTypeLabel
		{
			get
			{
				return "ModDependsOn".Translate("");
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060010B0 RID: 4272 RVA: 0x0005E67C File Offset: 0x0005C87C
		public override bool IsSatisfied
		{
			get
			{
				return ModLister.GetActiveModWithIdentifier(this.packageId) != null;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x0005E68C File Offset: 0x0005C88C
		public override Texture2D StatusIcon
		{
			get
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
				if (modWithIdentifier == null)
				{
					return ModRequirement.NotInstalled;
				}
				if (!modWithIdentifier.Active)
				{
					return ModRequirement.Installed;
				}
				return ModRequirement.Resolved;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060010B2 RID: 4274 RVA: 0x0005E6C4 File Offset: 0x0005C8C4
		public override string Tooltip
		{
			get
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
				if (modWithIdentifier == null)
				{
					return base.Tooltip + "\n" + "ContentNotInstalled".Translate() + "\n\n" + "ModClickToGoToWebsite".Translate();
				}
				if (!modWithIdentifier.Active)
				{
					return base.Tooltip + "\n" + "ContentInstalledButNotActive".Translate() + "\n\n" + "ModClickToSelect".Translate();
				}
				return base.Tooltip;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x0005E76B File Offset: 0x0005C96B
		public string Url
		{
			get
			{
				return this.steamWorkshopUrl ?? this.downloadUrl;
			}
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x0005E780 File Offset: 0x0005C980
		public override void OnClicked(Page_ModsConfig window)
		{
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
			if (modWithIdentifier == null)
			{
				if (!this.Url.NullOrEmpty())
				{
					SteamUtility.OpenUrl(this.Url);
					return;
				}
			}
			else if (!modWithIdentifier.Active)
			{
				window.SelectMod(modWithIdentifier);
			}
		}

		// Token: 0x04000CCC RID: 3276
		public string downloadUrl;

		// Token: 0x04000CCD RID: 3277
		public string steamWorkshopUrl;
	}
}
