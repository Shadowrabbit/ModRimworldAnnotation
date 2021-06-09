using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200034F RID: 847
	[StaticConstructorOnStartup]
	public abstract class ModRequirement
	{
		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060015CF RID: 5583
		public abstract bool IsSatisfied { get; }

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060015D0 RID: 5584
		public abstract string RequirementTypeLabel { get; }

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060015D1 RID: 5585 RVA: 0x0001586A File Offset: 0x00013A6A
		public virtual string Tooltip
		{
			get
			{
				return "ModPackageId".Translate() + ": " + this.packageId;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060015D2 RID: 5586 RVA: 0x00015890 File Offset: 0x00013A90
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

		// Token: 0x060015D3 RID: 5587
		public abstract void OnClicked(Page_ModsConfig window);

		// Token: 0x040010CD RID: 4301
		public string packageId;

		// Token: 0x040010CE RID: 4302
		public string displayName;

		// Token: 0x040010CF RID: 4303
		public static Texture2D NotResolved = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/NotResolved", true);

		// Token: 0x040010D0 RID: 4304
		public static Texture2D NotInstalled = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/NotInstalled", true);

		// Token: 0x040010D1 RID: 4305
		public static Texture2D Installed = ContentFinder<Texture2D>.Get("UI/Icons/ModRequirements/Installed", true);

		// Token: 0x040010D2 RID: 4306
		public static Texture2D Resolved = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOn", true);
	}
}
