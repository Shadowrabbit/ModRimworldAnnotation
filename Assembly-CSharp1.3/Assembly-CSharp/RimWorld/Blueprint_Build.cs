using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001040 RID: 4160
	public class Blueprint_Build : Blueprint
	{
		// Token: 0x170010B4 RID: 4276
		// (get) Token: 0x0600624E RID: 25166 RVA: 0x00215764 File Offset: 0x00213964
		public override string Label
		{
			get
			{
				string text = this.def.entityDefToBuild.label;
				if (base.StyleSourcePrecept != null)
				{
					text = base.StyleSourcePrecept.TransformThingLabel(text);
				}
				if (this.stuffToUse != null)
				{
					return "ThingMadeOfStuffLabel".Translate(this.stuffToUse.LabelAsStuff, text) + "BlueprintLabelExtra".Translate();
				}
				return text + "BlueprintLabelExtra".Translate();
			}
		}

		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x0600624F RID: 25167 RVA: 0x002157E9 File Offset: 0x002139E9
		protected override float WorkTotal
		{
			get
			{
				return this.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, this.stuffToUse);
			}
		}

		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x06006250 RID: 25168 RVA: 0x00215808 File Offset: 0x00213A08
		public override Graphic Graphic
		{
			get
			{
				if (this.cachedGraphic == null)
				{
					this.cachedGraphic = base.Graphic.GetColoredVersion(this.def.graphic.Shader, this.def.graphic.Color, this.def.graphic.ColorTwo);
				}
				return this.cachedGraphic;
			}
		}

		// Token: 0x06006251 RID: 25169 RVA: 0x00215864 File Offset: 0x00213A64
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.stuffToUse, "stuffToUse");
		}

		// Token: 0x06006252 RID: 25170 RVA: 0x0021587C File Offset: 0x00213A7C
		public override ThingDef EntityToBuildStuff()
		{
			return this.stuffToUse;
		}

		// Token: 0x06006253 RID: 25171 RVA: 0x00215884 File Offset: 0x00213A84
		public override List<ThingDefCountClass> MaterialsNeeded()
		{
			return this.def.entityDefToBuild.CostListAdjusted(this.stuffToUse, true);
		}

		// Token: 0x06006254 RID: 25172 RVA: 0x0021589D File Offset: 0x00213A9D
		protected override Thing MakeSolidThing()
		{
			Frame frame = (Frame)ThingMaker.MakeThing(this.def.entityDefToBuild.frameDef, this.stuffToUse);
			frame.StyleSourcePrecept = base.StyleSourcePrecept;
			return frame;
		}

		// Token: 0x06006255 RID: 25173 RVA: 0x002158CB File Offset: 0x00213ACB
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Command command = BuildCopyCommandUtility.BuildCopyCommand(this.def.entityDefToBuild, this.stuffToUse);
			if (command != null)
			{
				yield return command;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				foreach (Command command2 in BuildRelatedCommandUtility.RelatedBuildCommands(this.def.entityDefToBuild))
				{
					yield return command2;
				}
				IEnumerator<Command> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06006256 RID: 25174 RVA: 0x002158DC File Offset: 0x00213ADC
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine("ContainedResources".Translate() + ":");
			bool flag = true;
			foreach (ThingDefCountClass thingDefCountClass in this.MaterialsNeeded())
			{
				if (!flag)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(thingDefCountClass.thingDef.LabelCap + ": 0 / " + thingDefCountClass.count);
				flag = false;
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x040037DC RID: 14300
		public ThingDef stuffToUse;

		// Token: 0x040037DD RID: 14301
		[Unsaved(false)]
		private Graphic cachedGraphic;
	}
}
