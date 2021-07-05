using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020011B0 RID: 4528
	public class CompProperties_Techprint : CompProperties
	{
		// Token: 0x06006D13 RID: 27923 RVA: 0x00249731 File Offset: 0x00247931
		public CompProperties_Techprint()
		{
			this.compClass = typeof(CompTechprint);
		}

		// Token: 0x06006D14 RID: 27924 RVA: 0x0024974C File Offset: 0x0024794C
		public override void ResolveReferences(ThingDef parentDef)
		{
			if (parentDef.descriptionHyperlinks == null)
			{
				parentDef.descriptionHyperlinks = new List<DefHyperlink>();
			}
			List<Def> unlockedDefs = this.project.UnlockedDefs;
			for (int i = 0; i < unlockedDefs.Count; i++)
			{
				ThingDef def;
				RecipeDef recipeDef;
				if ((def = (unlockedDefs[i] as ThingDef)) != null)
				{
					parentDef.descriptionHyperlinks.Add(def);
				}
				else if ((recipeDef = (unlockedDefs[i] as RecipeDef)) != null && !recipeDef.products.NullOrEmpty<ThingDefCountClass>())
				{
					for (int j = 0; j < recipeDef.products.Count; j++)
					{
						parentDef.descriptionHyperlinks.Add(recipeDef.products[j].thingDef);
					}
				}
			}
			parentDef.description += "\n\n" + "Unlocks".Translate() + ": " + (from x in this.project.UnlockedDefs
			select x.label).ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x04003CA4 RID: 15524
		public ResearchProjectDef project;
	}
}
