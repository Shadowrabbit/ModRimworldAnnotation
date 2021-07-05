using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F6 RID: 1014
	[StaticConstructorOnStartup]
	public class Command_SetBedOwnerType : Command
	{
		// Token: 0x06001E87 RID: 7815 RVA: 0x000BEC10 File Offset: 0x000BCE10
		public Command_SetBedOwnerType(Building_Bed bed)
		{
			this.bed = bed;
			switch (bed.ForOwnerType)
			{
			case BedOwnerType.Colonist:
				this.defaultLabel = "CommandBedSetForColonistsLabel".Translate();
				this.icon = Command_SetBedOwnerType.ForColonistsTex;
				break;
			case BedOwnerType.Prisoner:
				this.defaultLabel = "CommandBedSetForPrisonersLabel".Translate();
				this.icon = Command_SetBedOwnerType.ForPrisonersTex;
				break;
			case BedOwnerType.Slave:
				this.defaultLabel = "CommandBedSetForSlavesLabel".Translate();
				this.icon = Command_SetBedOwnerType.ForSlavesTex;
				break;
			default:
				Log.Error(string.Format("Unknown owner type selected for bed: {0}", bed.ForOwnerType));
				break;
			}
			this.defaultDesc = "CommandBedSetForOwnerTypeDesc".Translate();
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x000BECDC File Offset: 0x000BCEDC
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("CommandBedSetForColonistsLabel".Translate(), delegate()
			{
				this.bed.SetBedOwnerTypeByInterface(BedOwnerType.Colonist);
			}, Command_SetBedOwnerType.ForColonistsTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			list.Add(new FloatMenuOption("CommandBedSetForPrisonersLabel".Translate(), delegate()
			{
				if (!Building_Bed.RoomCanBePrisonCell(this.bed.GetRoom(RegionType.Set_All)) && !this.bed.ForPrisoners)
				{
					Messages.Message("CommandBedSetForPrisonersFailOutdoors".Translate(), this.bed, MessageTypeDefOf.RejectInput, false);
					return;
				}
				this.bed.SetBedOwnerTypeByInterface(BedOwnerType.Prisoner);
			}, Command_SetBedOwnerType.ForPrisonersTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			list.Add(new FloatMenuOption("CommandBedSetForSlavesLabel".Translate(), delegate()
			{
				this.bed.SetBedOwnerTypeByInterface(BedOwnerType.Slave);
			}, Command_SetBedOwnerType.ForSlavesTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x04001295 RID: 4757
		private Building_Bed bed;

		// Token: 0x04001296 RID: 4758
		private static readonly Texture2D ForColonistsTex = ContentFinder<Texture2D>.Get("UI/Commands/ForColonists", true);

		// Token: 0x04001297 RID: 4759
		private static readonly Texture2D ForSlavesTex = ContentFinder<Texture2D>.Get("UI/Commands/ForSlaves", true);

		// Token: 0x04001298 RID: 4760
		private static readonly Texture2D ForPrisonersTex = ContentFinder<Texture2D>.Get("UI/Commands/ForPrisoners", true);
	}
}
