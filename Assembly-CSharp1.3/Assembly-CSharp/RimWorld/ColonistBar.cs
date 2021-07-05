using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001294 RID: 4756
	[StaticConstructorOnStartup]
	public class ColonistBar
	{
		// Token: 0x170013CF RID: 5071
		// (get) Token: 0x06007186 RID: 29062 RVA: 0x0025E396 File Offset: 0x0025C596
		public List<ColonistBar.Entry> Entries
		{
			get
			{
				this.CheckRecacheEntries();
				return this.cachedEntries;
			}
		}

		// Token: 0x170013D0 RID: 5072
		// (get) Token: 0x06007187 RID: 29063 RVA: 0x0025E3A4 File Offset: 0x0025C5A4
		private bool ShowGroupFrames
		{
			get
			{
				List<ColonistBar.Entry> entries = this.Entries;
				int num = -1;
				for (int i = 0; i < entries.Count; i++)
				{
					num = Mathf.Max(num, entries[i].group);
				}
				return num >= 1;
			}
		}

		// Token: 0x170013D1 RID: 5073
		// (get) Token: 0x06007188 RID: 29064 RVA: 0x0025E3E5 File Offset: 0x0025C5E5
		public float Scale
		{
			get
			{
				return this.cachedScale;
			}
		}

		// Token: 0x170013D2 RID: 5074
		// (get) Token: 0x06007189 RID: 29065 RVA: 0x0025E3ED File Offset: 0x0025C5ED
		public List<Vector2> DrawLocs
		{
			get
			{
				return this.cachedDrawLocs;
			}
		}

		// Token: 0x170013D3 RID: 5075
		// (get) Token: 0x0600718A RID: 29066 RVA: 0x0025E3F5 File Offset: 0x0025C5F5
		public Vector2 Size
		{
			get
			{
				return ColonistBar.BaseSize * this.Scale;
			}
		}

		// Token: 0x170013D4 RID: 5076
		// (get) Token: 0x0600718B RID: 29067 RVA: 0x0025E407 File Offset: 0x0025C607
		public float SpaceBetweenColonistsHorizontal
		{
			get
			{
				return 24f * this.Scale;
			}
		}

		// Token: 0x170013D5 RID: 5077
		// (get) Token: 0x0600718C RID: 29068 RVA: 0x0025E415 File Offset: 0x0025C615
		private bool Visible
		{
			get
			{
				return UI.screenWidth >= 800 && UI.screenHeight >= 500 && !Find.TilePicker.Active;
			}
		}

		// Token: 0x0600718D RID: 29069 RVA: 0x0025E440 File Offset: 0x0025C640
		public void MarkColonistsDirty()
		{
			this.entriesDirty = true;
		}

		// Token: 0x0600718E RID: 29070 RVA: 0x0025E44C File Offset: 0x0025C64C
		public void ColonistBarOnGUI()
		{
			if (!this.Visible)
			{
				return;
			}
			if (Event.current.type != EventType.Layout)
			{
				List<ColonistBar.Entry> entries = this.Entries;
				int num = -1;
				bool showGroupFrames = this.ShowGroupFrames;
				int reorderableGroup = -1;
				for (int i = 0; i < this.cachedDrawLocs.Count; i++)
				{
					Rect rect = new Rect(this.cachedDrawLocs[i].x, this.cachedDrawLocs[i].y, this.Size.x, this.Size.y);
					ColonistBar.Entry entry = entries[i];
					bool flag = num != entry.group;
					num = entry.group;
					if (flag)
					{
						reorderableGroup = ReorderableWidget.NewGroup(entry.reorderAction, ReorderableDirection.Horizontal, this.SpaceBetweenColonistsHorizontal, entry.extraDraggedItemOnGUI);
					}
					bool reordering;
					if (entry.pawn != null)
					{
						this.drawer.HandleClicks(rect, entry.pawn, reorderableGroup, out reordering);
					}
					else
					{
						reordering = false;
					}
					if (Event.current.type == EventType.Repaint)
					{
						if (flag && showGroupFrames)
						{
							this.drawer.DrawGroupFrame(entry.group);
						}
						if (entry.pawn != null)
						{
							this.drawer.DrawColonist(rect, entry.pawn, entry.map, this.colonistsToHighlight.Contains(entry.pawn), reordering);
						}
					}
				}
				num = -1;
				if (showGroupFrames)
				{
					for (int j = 0; j < this.cachedDrawLocs.Count; j++)
					{
						ColonistBar.Entry entry2 = entries[j];
						bool flag2 = num != entry2.group;
						num = entry2.group;
						if (flag2)
						{
							this.drawer.HandleGroupFrameClicks(entry2.group);
						}
					}
				}
			}
			if (Event.current.type == EventType.Repaint)
			{
				this.colonistsToHighlight.Clear();
			}
		}

		// Token: 0x0600718F RID: 29071 RVA: 0x0025E614 File Offset: 0x0025C814
		private void CheckRecacheEntries()
		{
			if (!this.entriesDirty)
			{
				return;
			}
			this.entriesDirty = false;
			this.cachedEntries.Clear();
			if (Find.PlaySettings.showColonistBar)
			{
				ColonistBar.tmpMaps.Clear();
				ColonistBar.tmpMaps.AddRange(Find.Maps);
				ColonistBar.tmpMaps.SortBy((Map x) => !x.IsPlayerHome, (Map x) => x.uniqueID);
				int num = 0;
				for (int i = 0; i < ColonistBar.tmpMaps.Count; i++)
				{
					ColonistBar.tmpPawns.Clear();
					ColonistBar.tmpPawns.AddRange(ColonistBar.tmpMaps[i].mapPawns.FreeColonists);
					List<Thing> list = ColonistBar.tmpMaps[i].listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
					for (int j = 0; j < list.Count; j++)
					{
						if (!list[j].IsDessicated())
						{
							Pawn innerPawn = ((Corpse)list[j]).InnerPawn;
							if (innerPawn != null && innerPawn.IsColonist)
							{
								ColonistBar.tmpPawns.Add(innerPawn);
							}
						}
					}
					List<Pawn> allPawnsSpawned = ColonistBar.tmpMaps[i].mapPawns.AllPawnsSpawned;
					for (int k = 0; k < allPawnsSpawned.Count; k++)
					{
						Corpse corpse = allPawnsSpawned[k].carryTracker.CarriedThing as Corpse;
						if (corpse != null && !corpse.IsDessicated() && corpse.InnerPawn.IsColonist)
						{
							ColonistBar.tmpPawns.Add(corpse.InnerPawn);
						}
					}
					PlayerPawnsDisplayOrderUtility.Sort(ColonistBar.tmpPawns);
					for (int l = 0; l < ColonistBar.tmpPawns.Count; l++)
					{
						this.cachedEntries.Add(new ColonistBar.Entry(ColonistBar.tmpPawns[l], ColonistBar.tmpMaps[i], num));
					}
					if (!ColonistBar.tmpPawns.Any<Pawn>())
					{
						this.cachedEntries.Add(new ColonistBar.Entry(null, ColonistBar.tmpMaps[i], num));
					}
					num++;
				}
				ColonistBar.tmpCaravans.Clear();
				ColonistBar.tmpCaravans.AddRange(Find.WorldObjects.Caravans);
				ColonistBar.tmpCaravans.SortBy((Caravan x) => x.ID);
				for (int m = 0; m < ColonistBar.tmpCaravans.Count; m++)
				{
					if (ColonistBar.tmpCaravans[m].IsPlayerControlled)
					{
						ColonistBar.tmpPawns.Clear();
						ColonistBar.tmpPawns.AddRange(ColonistBar.tmpCaravans[m].PawnsListForReading);
						PlayerPawnsDisplayOrderUtility.Sort(ColonistBar.tmpPawns);
						for (int n = 0; n < ColonistBar.tmpPawns.Count; n++)
						{
							if (ColonistBar.tmpPawns[n].IsColonist)
							{
								this.cachedEntries.Add(new ColonistBar.Entry(ColonistBar.tmpPawns[n], null, num));
							}
						}
						num++;
					}
				}
			}
			this.drawer.Notify_RecachedEntries();
			ColonistBar.tmpPawns.Clear();
			ColonistBar.tmpMaps.Clear();
			ColonistBar.tmpCaravans.Clear();
			this.drawLocsFinder.CalculateDrawLocs(this.cachedDrawLocs, out this.cachedScale);
		}

		// Token: 0x06007190 RID: 29072 RVA: 0x0025E978 File Offset: 0x0025CB78
		public float GetEntryRectAlpha(Rect rect)
		{
			float t;
			if (Messages.CollidesWithAnyMessage(rect, out t))
			{
				return Mathf.Lerp(1f, 0.2f, t);
			}
			return 1f;
		}

		// Token: 0x06007191 RID: 29073 RVA: 0x0025E9A5 File Offset: 0x0025CBA5
		public void Highlight(Pawn pawn)
		{
			if (!this.Visible)
			{
				return;
			}
			if (!this.colonistsToHighlight.Contains(pawn))
			{
				this.colonistsToHighlight.Add(pawn);
			}
		}

		// Token: 0x06007192 RID: 29074 RVA: 0x0025E9CC File Offset: 0x0025CBCC
		public void Reorder(int from, int to, int entryGroup)
		{
			int num = 0;
			Pawn pawn = null;
			Pawn pawn2 = null;
			Pawn pawn3 = null;
			for (int i = 0; i < this.cachedEntries.Count; i++)
			{
				if (this.cachedEntries[i].group == entryGroup && this.cachedEntries[i].pawn != null)
				{
					if (num == from)
					{
						pawn = this.cachedEntries[i].pawn;
					}
					if (num == to)
					{
						pawn2 = this.cachedEntries[i].pawn;
					}
					pawn3 = this.cachedEntries[i].pawn;
					num++;
				}
			}
			if (pawn == null)
			{
				return;
			}
			int num2 = (pawn2 != null) ? pawn2.playerSettings.displayOrder : (pawn3.playerSettings.displayOrder + 1);
			for (int j = 0; j < this.cachedEntries.Count; j++)
			{
				Pawn pawn4 = this.cachedEntries[j].pawn;
				if (pawn4 != null)
				{
					if (pawn4.playerSettings.displayOrder == num2)
					{
						if (pawn2 != null && this.cachedEntries[j].group == entryGroup)
						{
							if (pawn4.thingIDNumber < pawn2.thingIDNumber)
							{
								pawn4.playerSettings.displayOrder--;
							}
							else
							{
								pawn4.playerSettings.displayOrder++;
							}
						}
					}
					else if (pawn4.playerSettings.displayOrder > num2)
					{
						pawn4.playerSettings.displayOrder++;
					}
					else
					{
						pawn4.playerSettings.displayOrder--;
					}
				}
			}
			pawn.playerSettings.displayOrder = num2;
			this.MarkColonistsDirty();
			MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
		}

		// Token: 0x06007193 RID: 29075 RVA: 0x0025EB84 File Offset: 0x0025CD84
		public void DrawColonistMouseAttachment(int index, Vector2 dragStartPos, int entryGroup)
		{
			Pawn pawn = null;
			Vector2 vector = default(Vector2);
			int num = 0;
			for (int i = 0; i < this.cachedEntries.Count; i++)
			{
				if (this.cachedEntries[i].group == entryGroup && this.cachedEntries[i].pawn != null)
				{
					if (num == index)
					{
						pawn = this.cachedEntries[i].pawn;
						vector = this.cachedDrawLocs[i];
						break;
					}
					num++;
				}
			}
			if (pawn != null)
			{
				Texture iconTex = PortraitsCache.Get(pawn, ColonistBarColonistDrawer.PawnTextureSize, Rot4.South, ColonistBarColonistDrawer.PawnTextureCameraOffset, 1.28205f, true, true, true, true, null, false);
				Rect rect = new Rect(vector.x, vector.y, this.Size.x, this.Size.y);
				Rect pawnTextureRect = this.drawer.GetPawnTextureRect(rect.position);
				pawnTextureRect.position += Event.current.mousePosition - dragStartPos;
				GenUI.DrawMouseAttachment(iconTex, "", 0f, default(Vector2), new Rect?(pawnTextureRect), false, default(Color));
			}
		}

		// Token: 0x06007194 RID: 29076 RVA: 0x0025ECB4 File Offset: 0x0025CEB4
		public bool AnyColonistOrCorpseAt(Vector2 pos)
		{
			ColonistBar.Entry entry;
			return this.TryGetEntryAt(pos, out entry) && entry.pawn != null;
		}

		// Token: 0x06007195 RID: 29077 RVA: 0x0025ECD8 File Offset: 0x0025CED8
		public bool TryGetEntryAt(Vector2 pos, out ColonistBar.Entry entry)
		{
			List<Vector2> drawLocs = this.DrawLocs;
			List<ColonistBar.Entry> entries = this.Entries;
			Vector2 size = this.Size;
			for (int i = 0; i < drawLocs.Count; i++)
			{
				if (new Rect(drawLocs[i].x, drawLocs[i].y, size.x, size.y).Contains(pos))
				{
					entry = entries[i];
					return true;
				}
			}
			entry = default(ColonistBar.Entry);
			return false;
		}

		// Token: 0x06007196 RID: 29078 RVA: 0x0025ED58 File Offset: 0x0025CF58
		public List<Pawn> GetColonistsInOrder()
		{
			List<ColonistBar.Entry> entries = this.Entries;
			ColonistBar.tmpColonistsInOrder.Clear();
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].pawn != null)
				{
					ColonistBar.tmpColonistsInOrder.Add(entries[i].pawn);
				}
			}
			return ColonistBar.tmpColonistsInOrder;
		}

		// Token: 0x06007197 RID: 29079 RVA: 0x0025EDB0 File Offset: 0x0025CFB0
		public List<Thing> ColonistsOrCorpsesInScreenRect(Rect rect)
		{
			List<Vector2> drawLocs = this.DrawLocs;
			List<ColonistBar.Entry> entries = this.Entries;
			Vector2 size = this.Size;
			ColonistBar.tmpColonistsWithMap.Clear();
			for (int i = 0; i < drawLocs.Count; i++)
			{
				if (rect.Overlaps(new Rect(drawLocs[i].x, drawLocs[i].y, size.x, size.y)))
				{
					Pawn pawn = entries[i].pawn;
					if (pawn != null)
					{
						Thing first;
						if (pawn.Dead && pawn.Corpse != null && pawn.Corpse.SpawnedOrAnyParentSpawned)
						{
							first = pawn.Corpse;
						}
						else
						{
							first = pawn;
						}
						ColonistBar.tmpColonistsWithMap.Add(new Pair<Thing, Map>(first, entries[i].map));
					}
				}
			}
			if (WorldRendererUtility.WorldRenderedNow)
			{
				if (ColonistBar.tmpColonistsWithMap.Any((Pair<Thing, Map> x) => x.Second == null))
				{
					ColonistBar.tmpColonistsWithMap.RemoveAll((Pair<Thing, Map> x) => x.Second != null);
					goto IL_179;
				}
			}
			if (ColonistBar.tmpColonistsWithMap.Any((Pair<Thing, Map> x) => x.Second == Find.CurrentMap))
			{
				ColonistBar.tmpColonistsWithMap.RemoveAll((Pair<Thing, Map> x) => x.Second != Find.CurrentMap);
			}
			IL_179:
			ColonistBar.tmpColonists.Clear();
			for (int j = 0; j < ColonistBar.tmpColonistsWithMap.Count; j++)
			{
				ColonistBar.tmpColonists.Add(ColonistBar.tmpColonistsWithMap[j].First);
			}
			ColonistBar.tmpColonistsWithMap.Clear();
			return ColonistBar.tmpColonists;
		}

		// Token: 0x06007198 RID: 29080 RVA: 0x0025EF88 File Offset: 0x0025D188
		public List<Thing> MapColonistsOrCorpsesInScreenRect(Rect rect)
		{
			ColonistBar.tmpMapColonistsOrCorpsesInScreenRect.Clear();
			if (!this.Visible)
			{
				return ColonistBar.tmpMapColonistsOrCorpsesInScreenRect;
			}
			List<Thing> list = this.ColonistsOrCorpsesInScreenRect(rect);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Spawned)
				{
					ColonistBar.tmpMapColonistsOrCorpsesInScreenRect.Add(list[i]);
				}
			}
			return ColonistBar.tmpMapColonistsOrCorpsesInScreenRect;
		}

		// Token: 0x06007199 RID: 29081 RVA: 0x0025EFEC File Offset: 0x0025D1EC
		public List<Pawn> CaravanMembersInScreenRect(Rect rect)
		{
			ColonistBar.tmpCaravanPawns.Clear();
			if (!this.Visible)
			{
				return ColonistBar.tmpCaravanPawns;
			}
			List<Thing> list = this.ColonistsOrCorpsesInScreenRect(rect);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i] as Pawn;
				if (pawn != null && pawn.IsCaravanMember())
				{
					ColonistBar.tmpCaravanPawns.Add(pawn);
				}
			}
			return ColonistBar.tmpCaravanPawns;
		}

		// Token: 0x0600719A RID: 29082 RVA: 0x0025F054 File Offset: 0x0025D254
		public List<Caravan> CaravanMembersCaravansInScreenRect(Rect rect)
		{
			ColonistBar.tmpCaravans.Clear();
			if (!this.Visible)
			{
				return ColonistBar.tmpCaravans;
			}
			List<Pawn> list = this.CaravanMembersInScreenRect(rect);
			for (int i = 0; i < list.Count; i++)
			{
				ColonistBar.tmpCaravans.Add(list[i].GetCaravan());
			}
			return ColonistBar.tmpCaravans;
		}

		// Token: 0x0600719B RID: 29083 RVA: 0x0025F0B0 File Offset: 0x0025D2B0
		public Caravan CaravanMemberCaravanAt(Vector2 at)
		{
			if (!this.Visible)
			{
				return null;
			}
			Pawn pawn = this.ColonistOrCorpseAt(at) as Pawn;
			if (pawn != null && pawn.IsCaravanMember())
			{
				return pawn.GetCaravan();
			}
			return null;
		}

		// Token: 0x0600719C RID: 29084 RVA: 0x0025F0E8 File Offset: 0x0025D2E8
		public Thing ColonistOrCorpseAt(Vector2 pos)
		{
			if (!this.Visible)
			{
				return null;
			}
			ColonistBar.Entry entry;
			if (!this.TryGetEntryAt(pos, out entry))
			{
				return null;
			}
			Pawn pawn = entry.pawn;
			Thing result;
			if (pawn != null && pawn.Dead && pawn.Corpse != null && pawn.Corpse.SpawnedOrAnyParentSpawned)
			{
				result = pawn.Corpse;
			}
			else
			{
				result = pawn;
			}
			return result;
		}

		// Token: 0x04003E75 RID: 15989
		public ColonistBarColonistDrawer drawer = new ColonistBarColonistDrawer();

		// Token: 0x04003E76 RID: 15990
		private ColonistBarDrawLocsFinder drawLocsFinder = new ColonistBarDrawLocsFinder();

		// Token: 0x04003E77 RID: 15991
		private List<ColonistBar.Entry> cachedEntries = new List<ColonistBar.Entry>();

		// Token: 0x04003E78 RID: 15992
		private List<Vector2> cachedDrawLocs = new List<Vector2>();

		// Token: 0x04003E79 RID: 15993
		private float cachedScale = 1f;

		// Token: 0x04003E7A RID: 15994
		private bool entriesDirty = true;

		// Token: 0x04003E7B RID: 15995
		private List<Pawn> colonistsToHighlight = new List<Pawn>();

		// Token: 0x04003E7C RID: 15996
		public static readonly Texture2D BGTex = Command.BGTex;

		// Token: 0x04003E7D RID: 15997
		public static readonly Vector2 BaseSize = new Vector2(48f, 48f);

		// Token: 0x04003E7E RID: 15998
		public const float BaseSelectedTexJump = 20f;

		// Token: 0x04003E7F RID: 15999
		public const float BaseSelectedTexScale = 0.4f;

		// Token: 0x04003E80 RID: 16000
		public const float EntryInAnotherMapAlpha = 0.4f;

		// Token: 0x04003E81 RID: 16001
		public const float BaseSpaceBetweenGroups = 25f;

		// Token: 0x04003E82 RID: 16002
		public const float BaseSpaceBetweenColonistsHorizontal = 24f;

		// Token: 0x04003E83 RID: 16003
		public const float BaseSpaceBetweenColonistsVertical = 32f;

		// Token: 0x04003E84 RID: 16004
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04003E85 RID: 16005
		private static List<Map> tmpMaps = new List<Map>();

		// Token: 0x04003E86 RID: 16006
		private static List<Caravan> tmpCaravans = new List<Caravan>();

		// Token: 0x04003E87 RID: 16007
		private static List<Pawn> tmpColonistsInOrder = new List<Pawn>();

		// Token: 0x04003E88 RID: 16008
		private static List<Pair<Thing, Map>> tmpColonistsWithMap = new List<Pair<Thing, Map>>();

		// Token: 0x04003E89 RID: 16009
		private static List<Thing> tmpColonists = new List<Thing>();

		// Token: 0x04003E8A RID: 16010
		private static List<Thing> tmpMapColonistsOrCorpsesInScreenRect = new List<Thing>();

		// Token: 0x04003E8B RID: 16011
		private static List<Pawn> tmpCaravanPawns = new List<Pawn>();

		// Token: 0x02002603 RID: 9731
		public struct Entry
		{
			// Token: 0x0600D4CF RID: 54479 RVA: 0x00405F04 File Offset: 0x00404104
			public Entry(Pawn pawn, Map map, int group)
			{
				this.pawn = pawn;
				this.map = map;
				this.group = group;
				this.reorderAction = delegate(int from, int to)
				{
					Find.ColonistBar.Reorder(from, to, group);
				};
				this.extraDraggedItemOnGUI = delegate(int index, Vector2 dragStartPos)
				{
					Find.ColonistBar.DrawColonistMouseAttachment(index, dragStartPos, group);
				};
			}

			// Token: 0x040090EC RID: 37100
			public Pawn pawn;

			// Token: 0x040090ED RID: 37101
			public Map map;

			// Token: 0x040090EE RID: 37102
			public int group;

			// Token: 0x040090EF RID: 37103
			public Action<int, int> reorderAction;

			// Token: 0x040090F0 RID: 37104
			public Action<int, Vector2> extraDraggedItemOnGUI;
		}
	}
}
