using System;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017C9 RID: 6089
	public class CompGeneratedNames : ThingComp
	{
		// Token: 0x170014DE RID: 5342
		// (get) Token: 0x060086AE RID: 34478 RVA: 0x0005A58F File Offset: 0x0005878F
		public CompProperties_GeneratedName Props
		{
			get
			{
				return (CompProperties_GeneratedName)this.props;
			}
		}

		// Token: 0x170014DF RID: 5343
		// (get) Token: 0x060086AF RID: 34479 RVA: 0x0005A59C File Offset: 0x0005879C
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x060086B0 RID: 34480 RVA: 0x0005A5A4 File Offset: 0x000587A4
		public override string TransformLabel(string label)
		{
			if (this.parent.GetComp<CompBladelinkWeapon>() != null)
			{
				return this.name + ", " + label;
			}
			return this.name + " (" + label + ")";
		}

		// Token: 0x060086B1 RID: 34481 RVA: 0x002797F0 File Offset: 0x002779F0
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.name = GenText.CapitalizeAsTitle(GrammarResolver.Resolve("r_weapon_name", new GrammarRequest
			{
				Includes = 
				{
					this.Props.nameMaker
				}
			}, null, false, null, null, null, true));
		}

		// Token: 0x060086B2 RID: 34482 RVA: 0x0005A5DB File Offset: 0x000587DB
		public override void PostExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
		}

		// Token: 0x040056A0 RID: 22176
		private string name;
	}
}
