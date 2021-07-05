using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200045C RID: 1116
	[StaticConstructorOnStartup]
	public class Pawn_InventoryTracker : IThingHolder, IExposable
	{
		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001C48 RID: 7240 RVA: 0x00019999 File Offset: 0x00017B99
		// (set) Token: 0x06001C49 RID: 7241 RVA: 0x000199AB File Offset: 0x00017BAB
		public bool UnloadEverything
		{
			get
			{
				return this.unloadEverything && this.HasAnyUnloadableThing;
			}
			set
			{
				if (value && this.HasAnyUnloadableThing)
				{
					this.unloadEverything = true;
					return;
				}
				this.unloadEverything = false;
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001C4A RID: 7242 RVA: 0x000F05D8 File Offset: 0x000EE7D8
		private bool HasAnyUnloadableThing
		{
			get
			{
				return this.FirstUnloadableThing != default(ThingCount);
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001C4B RID: 7243 RVA: 0x000F05FC File Offset: 0x000EE7FC
		public ThingCount FirstUnloadableThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return default(ThingCount);
				}
				if (this.pawn.drugs != null && this.pawn.drugs.CurrentPolicy != null)
				{
					DrugPolicy currentPolicy = this.pawn.drugs.CurrentPolicy;
					Pawn_InventoryTracker.tmpDrugsToKeep.Clear();
					for (int i = 0; i < currentPolicy.Count; i++)
					{
						if (currentPolicy[i].takeToInventory > 0)
						{
							Pawn_InventoryTracker.tmpDrugsToKeep.Add(new ThingDefCount(currentPolicy[i].drug, currentPolicy[i].takeToInventory));
						}
					}
					for (int j = 0; j < this.innerContainer.Count; j++)
					{
						if (!this.innerContainer[j].def.IsDrug)
						{
							return new ThingCount(this.innerContainer[j], this.innerContainer[j].stackCount);
						}
						int num = -1;
						for (int k = 0; k < Pawn_InventoryTracker.tmpDrugsToKeep.Count; k++)
						{
							if (this.innerContainer[j].def == Pawn_InventoryTracker.tmpDrugsToKeep[k].ThingDef)
							{
								num = k;
								break;
							}
						}
						if (num < 0)
						{
							return new ThingCount(this.innerContainer[j], this.innerContainer[j].stackCount);
						}
						if (this.innerContainer[j].stackCount > Pawn_InventoryTracker.tmpDrugsToKeep[num].Count)
						{
							return new ThingCount(this.innerContainer[j], this.innerContainer[j].stackCount - Pawn_InventoryTracker.tmpDrugsToKeep[num].Count);
						}
						Pawn_InventoryTracker.tmpDrugsToKeep[num] = new ThingDefCount(Pawn_InventoryTracker.tmpDrugsToKeep[num].ThingDef, Pawn_InventoryTracker.tmpDrugsToKeep[num].Count - this.innerContainer[j].stackCount);
					}
					return default(ThingCount);
				}
				return new ThingCount(this.innerContainer[0], this.innerContainer[0].stackCount);
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001C4C RID: 7244 RVA: 0x000199C7 File Offset: 0x00017BC7
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x000199CF File Offset: 0x00017BCF
		public Pawn_InventoryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x000F084C File Offset: 0x000EEA4C
		public void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.itemsNotForSale, "itemsNotForSale", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.unloadEverything, "unloadEverything", false, false);
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x00019A02 File Offset: 0x00017C02
		public void InventoryTrackerTick()
		{
			this.innerContainer.ThingOwnerTick(true);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x00019A27 File Offset: 0x00017C27
		public void InventoryTrackerTickRare()
		{
			this.innerContainer.ThingOwnerTickRare(true);
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x000F089C File Offset: 0x000EEA9C
		public void DropAllNearPawn(IntVec3 pos, bool forbid = false, bool unforbid = false)
		{
			if (this.pawn.MapHeld == null)
			{
				Log.Error("Tried to drop all inventory near pawn but the pawn is unspawned. pawn=" + this.pawn, false);
				return;
			}
			Pawn_InventoryTracker.tmpThingList.Clear();
			Pawn_InventoryTracker.tmpThingList.AddRange(this.innerContainer);
			Action<Thing, int> <>9__0;
			for (int i = 0; i < Pawn_InventoryTracker.tmpThingList.Count; i++)
			{
				ThingOwner<Thing> thingOwner = this.innerContainer;
				Thing thing = Pawn_InventoryTracker.tmpThingList[i];
				Map mapHeld = this.pawn.MapHeld;
				ThingPlaceMode mode = ThingPlaceMode.Near;
				Action<Thing, int> placedAction;
				if ((placedAction = <>9__0) == null)
				{
					placedAction = (<>9__0 = delegate(Thing t, int unused)
					{
						if (forbid)
						{
							t.SetForbiddenIfOutsideHomeArea();
						}
						if (unforbid)
						{
							t.SetForbidden(false, false);
						}
						if (t.def.IsPleasureDrug)
						{
							LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugBurning, OpportunityType.Important);
						}
					});
				}
				Thing thing2;
				thingOwner.TryDrop(thing, pos, mapHeld, mode, out thing2, placedAction, null);
			}
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x00019A35 File Offset: 0x00017C35
		public void DestroyAll(DestroyMode mode = DestroyMode.Vanish)
		{
			this.innerContainer.ClearAndDestroyContents(mode);
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x00019A43 File Offset: 0x00017C43
		public bool Contains(Thing item)
		{
			return this.innerContainer.Contains(item);
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x00019A51 File Offset: 0x00017C51
		public bool NotForSale(Thing item)
		{
			return this.itemsNotForSale.Contains(item);
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x00019A5F File Offset: 0x00017C5F
		public void TryAddItemNotForSale(Thing item)
		{
			if (this.innerContainer.TryAdd(item, false))
			{
				this.itemsNotForSale.Add(item);
			}
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x00019A7C File Offset: 0x00017C7C
		public void Notify_ItemRemoved(Thing item)
		{
			this.itemsNotForSale.Remove(item);
			if (this.unloadEverything && !this.HasAnyUnloadableThing)
			{
				this.unloadEverything = false;
			}
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x00019AA2 File Offset: 0x00017CA2
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x00019AAA File Offset: 0x00017CAA
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x00019AB8 File Offset: 0x00017CB8
		public IEnumerable<Thing> GetDrugs()
		{
			foreach (Thing thing in this.innerContainer)
			{
				if (thing.TryGetComp<CompDrug>() != null)
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x00019AC8 File Offset: 0x00017CC8
		public IEnumerable<Thing> GetCombatEnhancingDrugs()
		{
			foreach (Thing thing in this.innerContainer)
			{
				CompDrug compDrug = thing.TryGetComp<CompDrug>();
				if (compDrug != null && compDrug.Props.isCombatEnhancingDrug)
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x00019AD8 File Offset: 0x00017CD8
		public Thing FindCombatEnhancingDrug()
		{
			return this.GetCombatEnhancingDrugs().FirstOrDefault<Thing>();
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x00019AE5 File Offset: 0x00017CE5
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.pawn.IsColonistPlayerControlled && this.pawn.Drafted && Find.Selector.SingleSelectedThing == this.pawn && !this.pawn.IsTeetotaler())
			{
				this.usableDrugsTmp.Clear();
				foreach (Thing thing in this.GetDrugs())
				{
					if (FoodUtility.WillIngestFromInventoryNow(this.pawn, thing))
					{
						this.usableDrugsTmp.Add(thing);
					}
				}
				if (this.usableDrugsTmp.Count == 0)
				{
					yield break;
				}
				if (this.usableDrugsTmp.Count == 1)
				{
					Thing drug = this.usableDrugsTmp[0];
					yield return new Command_Action
					{
						defaultLabel = "ConsumeThing".Translate(drug.LabelNoCount, drug),
						defaultDesc = drug.LabelCapNoCount + ": " + drug.def.description.CapitalizeFirst(),
						icon = drug.def.uiIcon,
						iconAngle = drug.def.uiIconAngle,
						iconOffset = drug.def.uiIconOffset,
						action = delegate()
						{
							FoodUtility.IngestFromInventoryNow(this.pawn, drug);
						}
					};
				}
				else
				{
					yield return new Command_Action
					{
						defaultLabel = "TakeDrug".Translate(),
						defaultDesc = "TakeDrugDesc".Translate(),
						icon = Pawn_InventoryTracker.DrugTex,
						action = delegate()
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							using (List<Thing>.Enumerator enumerator2 = this.usableDrugsTmp.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									Thing drug = enumerator2.Current;
									list.Add(new FloatMenuOption("ConsumeThing".Translate(drug.LabelNoCount, drug), delegate()
									{
										FoodUtility.IngestFromInventoryNow(this.pawn, drug);
									}, MenuOptionPriority.Default, null, null, 0f, null, null));
								}
							}
							Find.WindowStack.Add(new FloatMenu(list));
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x04001451 RID: 5201
		public Pawn pawn;

		// Token: 0x04001452 RID: 5202
		public ThingOwner<Thing> innerContainer;

		// Token: 0x04001453 RID: 5203
		private bool unloadEverything;

		// Token: 0x04001454 RID: 5204
		private List<Thing> itemsNotForSale = new List<Thing>();

		// Token: 0x04001455 RID: 5205
		public static readonly Texture2D DrugTex = ContentFinder<Texture2D>.Get("UI/Commands/TakeDrug", true);

		// Token: 0x04001456 RID: 5206
		private static List<ThingDefCount> tmpDrugsToKeep = new List<ThingDefCount>();

		// Token: 0x04001457 RID: 5207
		private static List<Thing> tmpThingList = new List<Thing>();

		// Token: 0x04001458 RID: 5208
		private List<Thing> usableDrugsTmp = new List<Thing>();
	}
}
