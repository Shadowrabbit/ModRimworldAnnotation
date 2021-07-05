using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017F4 RID: 6132
	[StaticConstructorOnStartup]
	public class FormCaravanComp : WorldObjectComp
	{
		// Token: 0x17001765 RID: 5989
		// (get) Token: 0x06008F06 RID: 36614 RVA: 0x00334889 File Offset: 0x00332A89
		public WorldObjectCompProperties_FormCaravan Props
		{
			get
			{
				return (WorldObjectCompProperties_FormCaravan)this.props;
			}
		}

		// Token: 0x17001766 RID: 5990
		// (get) Token: 0x06008F07 RID: 36615 RVA: 0x00334896 File Offset: 0x00332A96
		private MapParent MapParent
		{
			get
			{
				return (MapParent)this.parent;
			}
		}

		// Token: 0x17001767 RID: 5991
		// (get) Token: 0x06008F08 RID: 36616 RVA: 0x003348A3 File Offset: 0x00332AA3
		public bool Reform
		{
			get
			{
				return !this.MapParent.HasMap || !this.MapParent.Map.IsPlayerHome;
			}
		}

		// Token: 0x17001768 RID: 5992
		// (get) Token: 0x06008F09 RID: 36617 RVA: 0x003348C8 File Offset: 0x00332AC8
		public bool CanFormOrReformCaravanNow
		{
			get
			{
				MapParent mapParent = this.MapParent;
				return mapParent.HasMap && (!this.Reform || (!this.AnyActiveThreatNow && mapParent.Map.mapPawns.FreeColonistsSpawnedCount != 0));
			}
		}

		// Token: 0x17001769 RID: 5993
		// (get) Token: 0x06008F0A RID: 36618 RVA: 0x0033490B File Offset: 0x00332B0B
		public bool AnyActiveThreatNow
		{
			get
			{
				return this.MapParent.HasMap && GenHostility.AnyHostileActiveThreatToPlayer(this.MapParent.Map, true, true);
			}
		}

		// Token: 0x1700176A RID: 5994
		// (get) Token: 0x06008F0B RID: 36619 RVA: 0x00334930 File Offset: 0x00332B30
		public bool AnyUnexploredFoggedRooms
		{
			get
			{
				if (this.foggedRoomsCheckRect == null)
				{
					return false;
				}
				MapParent mapParent = this.MapParent;
				if (!mapParent.HasMap)
				{
					return false;
				}
				List<Room> allRooms = mapParent.Map.regionGrid.allRooms;
				CellRect value = this.foggedRoomsCheckRect.Value;
				for (int i = 0; i < allRooms.Count; i++)
				{
					if (allRooms[i].Fogged && allRooms[i].ProperRoom)
					{
						foreach (IntVec3 c in allRooms[i].Cells)
						{
							if (value.Contains(c))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06008F0C RID: 36620 RVA: 0x00334A00 File Offset: 0x00332C00
		public override void CompTick()
		{
			base.CompTick();
			bool anyActiveThreatNow = this.AnyActiveThreatNow;
			if (!anyActiveThreatNow && this.anyActiveThreatLastTick && this.Reform && this.CanFormOrReformCaravanNow)
			{
				if (this.AnyUnexploredFoggedRooms)
				{
					Messages.Message("MessageCanReformCaravanNowNoMoreEnemiesButUnexploredAreas".Translate(), new LookTargets(this.parent), MessageTypeDefOf.SituationResolved, false);
				}
				else
				{
					Messages.Message("MessageCanReformCaravanNowNoMoreEnemies".Translate(), new LookTargets(this.parent), MessageTypeDefOf.NeutralEvent, false);
				}
			}
			this.anyActiveThreatLastTick = anyActiveThreatNow;
		}

		// Token: 0x06008F0D RID: 36621 RVA: 0x00334A90 File Offset: 0x00332C90
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.anyActiveThreatLastTick, "anyActiveThreatLastTick", false, false);
			Scribe_Values.Look<CellRect?>(ref this.foggedRoomsCheckRect, "foggedRoomsCheckRect", null, false);
		}

		// Token: 0x06008F0E RID: 36622 RVA: 0x00334AC9 File Offset: 0x00332CC9
		public override IEnumerable<Gizmo> GetGizmos()
		{
			MapParent mapParent = (MapParent)this.parent;
			if (mapParent.HasMap)
			{
				if (!this.Reform)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandFormCaravan".Translate(),
						defaultDesc = "CommandFormCaravanDesc".Translate(),
						icon = FormCaravanComp.FormCaravanCommand,
						hotKey = KeyBindingDefOf.Misc2,
						tutorTag = "FormCaravan",
						action = delegate()
						{
							Find.WindowStack.Add(new Dialog_FormCaravan(mapParent.Map, false, null, false));
						}
					};
				}
				else if (mapParent.Map.mapPawns.FreeColonistsSpawnedCount != 0)
				{
					Command_Action command_Action = new Command_Action();
					command_Action.defaultLabel = "CommandReformCaravan".Translate();
					command_Action.defaultDesc = "CommandReformCaravanDesc".Translate();
					command_Action.icon = FormCaravanComp.FormCaravanCommand;
					command_Action.hotKey = KeyBindingDefOf.Misc2;
					command_Action.tutorTag = "ReformCaravan";
					command_Action.action = delegate()
					{
						Find.WindowStack.Add(new Dialog_FormCaravan(mapParent.Map, true, null, false));
					};
					if (GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map, true, true))
					{
						command_Action.Disable("CommandReformCaravanFailHostilePawns".Translate());
					}
					yield return command_Action;
				}
				if (Prefs.DevMode)
				{
					yield return new Command_Action
					{
						defaultLabel = "Dev: Show available exits",
						action = delegate()
						{
							foreach (int tile in CaravanExitMapUtility.AvailableExitTilesAt(mapParent.Map))
							{
								Find.WorldDebugDrawer.FlashTile(tile, 0f, null, 10);
							}
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x04005A08 RID: 23048
		public CellRect? foggedRoomsCheckRect;

		// Token: 0x04005A09 RID: 23049
		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		// Token: 0x04005A0A RID: 23050
		private bool anyActiveThreatLastTick;
	}
}
