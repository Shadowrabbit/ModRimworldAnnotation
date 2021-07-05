using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020007BF RID: 1983
	public class BackCompatibilityConverter_Universal : BackCompatibilityConverter
	{
		// Token: 0x060031F7 RID: 12791 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return true;
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x0014B42C File Offset: 0x0014962C
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (defType == typeof(ThingDef))
			{
				if (defName == "WoolYak" || defName == "WoolCamel")
				{
					return "WoolSheep";
				}
				if (defName == "Plant_TreeAnimus" || defName == "Plant_TreeAnimusSmall" || defName == "Plant_TreeAnimaSmall" || defName == "Plant_TreeAnimaNormal" || defName == "Plant_TreeAnimaHardy")
				{
					return "Plant_TreeAnima";
				}
				if (defName == "Psytrainer_EntropyLink")
				{
					return "Psytrainer_EntropyDump";
				}
				if (defName == "PsylinkNeuroformer")
				{
					return "PsychicAmplifier";
				}
				if (defName == "PsychicShockLance")
				{
					return "Apparel_PsychicShockLance";
				}
				if (defName == "PsychicInsanityLance")
				{
					return "Apparel_PsychicInsanityLance";
				}
			}
			if (defType == typeof(AbilityDef) && defName == "EntropyLink")
			{
				return "EntropyDump";
			}
			if (defType == typeof(HediffDef) && defName == "Psylink")
			{
				return "PsychicAmplifier";
			}
			return null;
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x0014B54C File Offset: 0x0014974C
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			if (providedClassName == "Hediff_PsychicAmplifier")
			{
				return typeof(Hediff_Psylink);
			}
			if (providedClassName == "ThingWithComps" || providedClassName == "Verse.ThingWithComps")
			{
				XmlElement xmlElement = node["def"];
				if (xmlElement != null)
				{
					if (xmlElement.InnerText == "PsychicShockLance")
					{
						return typeof(Apparel);
					}
					if (xmlElement.InnerText == "PsychicInsanityLance")
					{
						return typeof(Apparel);
					}
					if (xmlElement.InnerText == "OrbitalTargeterBombardment")
					{
						return typeof(Apparel);
					}
					if (xmlElement.InnerText == "OrbitalTargeterPowerBeam")
					{
						return typeof(Apparel);
					}
					if (xmlElement.InnerText == "OrbitalTargeterMechCluster")
					{
						return typeof(Apparel);
					}
					if (xmlElement.InnerText == "TornadoGenerator")
					{
						return typeof(Apparel);
					}
				}
			}
			return null;
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x0014B650 File Offset: 0x00149850
		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				int num = VersionControl.BuildFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
				Pawn_RoyaltyTracker pawn_RoyaltyTracker;
				if ((pawn_RoyaltyTracker = (obj as Pawn_RoyaltyTracker)) != null && num <= 2575)
				{
					foreach (RoyalTitle royalTitle in pawn_RoyaltyTracker.AllTitlesForReading)
					{
						royalTitle.conceited = RoyalTitleUtility.ShouldBecomeConceitedOnNewTitle(pawn_RoyaltyTracker.pawn);
					}
				}
				Pawn_NeedsTracker pawn_NeedsTracker;
				if ((pawn_NeedsTracker = (obj as Pawn_NeedsTracker)) != null)
				{
					pawn_NeedsTracker.AllNeeds.RemoveAll((Need n) => n.def.defName == "Authority");
				}
			}
			Pawn pawn;
			if ((pawn = (obj as Pawn)) != null)
			{
				if (pawn.abilities == null)
				{
					pawn.abilities = new Pawn_AbilityTracker(pawn);
				}
				if (pawn.health != null)
				{
					if (pawn.health.hediffSet.hediffs.RemoveAll((Hediff x) => x == null) != 0)
					{
						Log.Error(pawn.ToStringSafe<Pawn>() + " had some null hediffs.", false);
					}
					Pawn_HealthTracker health = pawn.health;
					Hediff hediff;
					if (health == null)
					{
						hediff = null;
					}
					else
					{
						HediffSet hediffSet = health.hediffSet;
						hediff = ((hediffSet != null) ? hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicHangover, false) : null);
					}
					Hediff hediff2 = hediff;
					if (hediff2 != null)
					{
						pawn.health.hediffSet.hediffs.Remove(hediff2);
					}
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.WakeUpTolerance, false);
					if (firstHediffOfDef != null)
					{
						pawn.health.hediffSet.hediffs.Remove(firstHediffOfDef);
					}
					Hediff firstHediffOfDef2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.GoJuiceTolerance, false);
					if (firstHediffOfDef2 != null)
					{
						pawn.health.hediffSet.hediffs.Remove(firstHediffOfDef2);
					}
				}
				if (pawn.equipment == null || pawn.apparel == null || pawn.inventory == null)
				{
					return;
				}
				List<ThingWithComps> list = null;
				for (int i = 0; i < pawn.equipment.AllEquipmentListForReading.Count; i++)
				{
					ThingWithComps thingWithComps = pawn.equipment.AllEquipmentListForReading[i];
					if (thingWithComps.def.defName == "OrbitalTargeterBombardment" || thingWithComps.def.defName == "OrbitalTargeterPowerBeam" || thingWithComps.def.defName == "OrbitalTargeterMechCluster" || thingWithComps.def.defName == "TornadoGenerator")
					{
						list = (list ?? new List<ThingWithComps>());
						list.Add(thingWithComps);
					}
				}
				if (list == null)
				{
					return;
				}
				using (List<ThingWithComps>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ThingWithComps thingWithComps2 = enumerator2.Current;
						Apparel apparel = (Apparel)thingWithComps2;
						pawn.equipment.Remove(apparel);
						this.ResetVerbs(apparel);
						if (pawn.apparel.CanWearWithoutDroppingAnything(apparel.def))
						{
							pawn.apparel.Wear(apparel, true, false);
						}
						else
						{
							pawn.inventory.innerContainer.TryAdd(apparel, true);
						}
					}
					return;
				}
			}
			Map map;
			if (Scribe.mode == LoadSaveMode.LoadingVars && (map = (obj as Map)) != null && map.temporaryThingDrawer == null)
			{
				map.temporaryThingDrawer = new TemporaryThingDrawer();
			}
		}

		// Token: 0x060031FB RID: 12795 RVA: 0x0014B9BC File Offset: 0x00149BBC
		private void ResetVerbs(ThingWithComps t)
		{
			IVerbOwner verbOwner = t as IVerbOwner;
			if (verbOwner != null)
			{
				VerbTracker verbTracker = verbOwner.VerbTracker;
				if (verbTracker != null)
				{
					verbTracker.VerbsNeedReinitOnLoad();
				}
			}
			foreach (ThingComp thingComp in t.AllComps)
			{
				IVerbOwner verbOwner2 = thingComp as IVerbOwner;
				if (verbOwner2 != null)
				{
					VerbTracker verbTracker2 = verbOwner2.VerbTracker;
					if (verbTracker2 != null)
					{
						verbTracker2.VerbsNeedReinitOnLoad();
					}
				}
			}
		}
	}
}
