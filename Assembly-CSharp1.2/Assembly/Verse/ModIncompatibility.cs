using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000351 RID: 849
	public class ModIncompatibility : ModRequirement
	{
		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x060015DD RID: 5597 RVA: 0x000158EA File Offset: 0x00013AEA
		public override string RequirementTypeLabel
		{
			get
			{
				return "ModIncompatibleWith".Translate("");
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x060015DE RID: 5598 RVA: 0x00015905 File Offset: 0x00013B05
		public override bool IsSatisfied
		{
			get
			{
				return ModLister.GetActiveModWithIdentifier(this.packageId) == null;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x060015DF RID: 5599 RVA: 0x000D4218 File Offset: 0x000D2418
		public override string Tooltip
		{
			get
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
				if (modWithIdentifier != null && modWithIdentifier.Active)
				{
					return base.Tooltip + "\n" + "ContentActive".Translate() + "\n\n" + "ModClickToSelect".Translate();
				}
				return base.Tooltip;
			}
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x000D4284 File Offset: 0x000D2484
		public override void OnClicked(Page_ModsConfig window)
		{
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.packageId, true);
			if (modWithIdentifier != null && modWithIdentifier.Active)
			{
				window.SelectMod(modWithIdentifier);
			}
		}
	}
}
