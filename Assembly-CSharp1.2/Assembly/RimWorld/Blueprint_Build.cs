using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001667 RID: 5735
	public class Blueprint_Build : Blueprint
	{
		// Token: 0x1700133A RID: 4922
		// (get) Token: 0x06007CFB RID: 31995 RVA: 0x002557AC File Offset: 0x002539AC
		public override string Label
		{
			get
			{
				string label = this.def.entityDefToBuild.label;
				if (this.stuffToUse != null)
				{
					return "ThingMadeOfStuffLabel".Translate(this.stuffToUse.LabelAsStuff, label) + "BlueprintLabelExtra".Translate();
				}
				return label + "BlueprintLabelExtra".Translate();
			}
		}

		// Token: 0x1700133B RID: 4923
		// (get) Token: 0x06007CFC RID: 31996 RVA: 0x00053F27 File Offset: 0x00052127
		protected override float WorkTotal
		{
			get
			{
				return this.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, this.stuffToUse);
			}
		}

		// Token: 0x06007CFD RID: 31997 RVA: 0x00053F44 File Offset: 0x00052144
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.stuffToUse, "stuffToUse");
		}

		// Token: 0x06007CFE RID: 31998 RVA: 0x00053F5C File Offset: 0x0005215C
		public override ThingDef EntityToBuildStuff()
		{
			return this.stuffToUse;
		}

		// Token: 0x06007CFF RID: 31999 RVA: 0x00053F64 File Offset: 0x00052164
		public override List<ThingDefCountClass> MaterialsNeeded()
		{
			return this.def.entityDefToBuild.CostListAdjusted(this.stuffToUse, true);
		}

		// Token: 0x06007D00 RID: 32000 RVA: 0x00053F7D File Offset: 0x0005217D
		protected override Thing MakeSolidThing()
		{
			return ThingMaker.MakeThing(this.def.entityDefToBuild.frameDef, this.stuffToUse);
		}

		// Token: 0x06007D01 RID: 32001 RVA: 0x00053F9A File Offset: 0x0005219A
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
				foreach (Command command2 in BuildFacilityCommandUtility.BuildFacilityCommands(this.def.entityDefToBuild))
				{
					yield return command2;
				}
				IEnumerator<Command> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06007D02 RID: 32002 RVA: 0x0025581C File Offset: 0x00253A1C
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

		// Token: 0x0400519A RID: 20890
		public ThingDef stuffToUse;
	}
}
