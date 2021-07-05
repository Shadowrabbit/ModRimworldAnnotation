using System;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200113D RID: 4413
	public class CompGeneratedNames : ThingComp
	{
		// Token: 0x17001224 RID: 4644
		// (get) Token: 0x06006A03 RID: 27139 RVA: 0x0023B073 File Offset: 0x00239273
		public CompProperties_GeneratedName Props
		{
			get
			{
				return (CompProperties_GeneratedName)this.props;
			}
		}

		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x06006A04 RID: 27140 RVA: 0x0023B080 File Offset: 0x00239280
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06006A05 RID: 27141 RVA: 0x0023B088 File Offset: 0x00239288
		public static string GenerateName(CompProperties_GeneratedName props)
		{
			return GenText.CapitalizeAsTitle(GrammarResolver.Resolve("r_weapon_name", new GrammarRequest
			{
				Includes = 
				{
					props.nameMaker
				}
			}, null, false, null, null, null, true));
		}

		// Token: 0x06006A06 RID: 27142 RVA: 0x0023B0C8 File Offset: 0x002392C8
		public override string TransformLabel(string label)
		{
			if (this.parent.StyleSourcePrecept != null)
			{
				return label;
			}
			if (this.parent.GetComp<CompBladelinkWeapon>() != null)
			{
				return this.name + ", " + label;
			}
			return this.name + " (" + label + ")";
		}

		// Token: 0x06006A07 RID: 27143 RVA: 0x0023B119 File Offset: 0x00239319
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.name = CompGeneratedNames.GenerateName(this.Props);
		}

		// Token: 0x06006A08 RID: 27144 RVA: 0x0023B133 File Offset: 0x00239333
		public override void PostExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
		}

		// Token: 0x04003B25 RID: 15141
		private string name;
	}
}
