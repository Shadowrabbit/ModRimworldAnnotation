using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000350 RID: 848
	public class ModDependency : ModRequirement
	{
		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060015D6 RID: 5590 RVA: 0x000158A5 File Offset: 0x00013AA5
		public override string RequirementTypeLabel
		{
			get
			{
				return "ModDependsOn".Translate("");
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x060015D7 RID: 5591 RVA: 0x000158C0 File Offset: 0x00013AC0
		public override bool IsSatisfied
		{
			get
			{
				return ModLister.GetActiveModWithIdentifier(this.packageId) != null;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x060015D8 RID: 5592 RVA: 0x000D40F0 File Offset: 0x000D22F0
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

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x060015D9 RID: 5593 RVA: 0x000D4128 File Offset: 0x000D2328
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

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x060015DA RID: 5594 RVA: 0x000158D0 File Offset: 0x00013AD0
		public string Url
		{
			get
			{
				return this.steamWorkshopUrl ?? this.downloadUrl;
			}
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x000D41D0 File Offset: 0x000D23D0
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

		// Token: 0x040010D3 RID: 4307
		public string downloadUrl;

		// Token: 0x040010D4 RID: 4308
		public string steamWorkshopUrl;
	}
}
