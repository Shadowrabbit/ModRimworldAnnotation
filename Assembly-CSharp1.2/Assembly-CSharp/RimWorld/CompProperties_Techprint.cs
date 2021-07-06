using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200186B RID: 6251
	public class CompProperties_Techprint : CompProperties
	{
		// Token: 0x06008AB4 RID: 35508 RVA: 0x0005CFF5 File Offset: 0x0005B1F5
		public CompProperties_Techprint()
		{
			this.compClass = typeof(CompTechprint);
		}

		// Token: 0x06008AB5 RID: 35509 RVA: 0x00287898 File Offset: 0x00285A98
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
			select x.label).ToCommaList(false).CapitalizeFirst();
		}

		// Token: 0x040058F7 RID: 22775
		public ResearchProjectDef project;
	}
}
