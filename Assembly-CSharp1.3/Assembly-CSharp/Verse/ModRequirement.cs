using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000240 RID: 576
	[StaticConstructorOnStartup]
	public abstract class ModRequirement
	{
		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060010A8 RID: 4264
		public abstract bool IsSatisfied { get; }

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060010A9 RID: 4265
		public abstract string RequirementTypeLabel { get; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060010AA RID: 4266 RVA: 0x0005E5D7 File Offset: 0x0005C7D7
		public virtual string Tooltip
		{
			get
			{
				return "ModPackageId".Translate() + ": " + this.packageId;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060010AB RID: 4267 RVA: 0x0005E5FD File Offset: 0x0005C7FD
		public virtual Texture2D StatusIcon
		{
			get
			{
				if (!this.IsSatisfied)
				{
					return ModRequirement.NotResolved;
				}
				return ModRequirement.Resolved;
			}
		}

		// Token: 0x060010AC RID: 4268
		public abstract void OnClicked(Page_ModsConfig window);

		// Token: 0x04000CC6 RID: 3270
		public string packageId;

		// Token: 0x04000CC7 RID: 3271
		public string displayName;

		// Token: 0x04000CC8 RID: 3272
		public static Texture2D NotResolved = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/NotResolved", true);

		// Token: 0x04000CC9 RID: 3273
		public static Texture2D NotInstalled = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/NotInstalled", true);

		// Token: 0x04000CCA RID: 3274
		public static Texture2D Installed = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/Installed", true);

		// Token: 0x04000CCB RID: 3275
		public static Texture2D Resolved = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOn", true);
	}
}
