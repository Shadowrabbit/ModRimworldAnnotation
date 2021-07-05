using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000242 RID: 578
	public class ModIncompatibility : ModRequirement
	{
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060010B6 RID: 4278 RVA: 0x0005E7CD File Offset: 0x0005C9CD
		public override string RequirementTypeLabel
		{
			get
			{
				return "ModIncompatibleWith".Translate("");
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x0005E7E8 File Offset: 0x0005C9E8
		public override bool IsSatisfied
		{
			get
			{
				return ModLister.GetActiveModWithIdentifier(this.packageId) == null;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060010B8 RID: 4280 RVA: 0x0005E7F8 File Offset: 0x0005C9F8
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

		// Token: 0x060010B9 RID: 4281 RVA: 0x0005E864 File Offset: 0x0005CA64
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
