using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000332 RID: 818
	public class Scribe_Deep
	{
		// Token: 0x0600173F RID: 5951 RVA: 0x0008A824 File Offset: 0x00088A24
		public static void Look<T>(ref T target, string label, params object[] ctorArgs)
		{
			Scribe_Deep.Look<T>(ref target, false, label, ctorArgs);
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x0008A830 File Offset: 0x00088A30
		public static void Look<T>(ref T target, bool saveDestroyedThings, string label, params object[] ctorArgs)
		{
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					try
					{
						target = ScribeExtractor.SaveableFromNode<T>(Scribe.loader.curXmlParent[label], ctorArgs);
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception while loading ",
							Scribe.loader.curXmlParent[label].ToStringSafe<XmlElement>(),
							": ",
							ex
						}));
						target = default(T);
					}
				}
				return;
			}
			Thing thing = target as Thing;
			if (thing != null && thing.Destroyed)
			{
				if (!saveDestroyedThings)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Deep-saving destroyed thing ",
						thing,
						" with saveDestroyedThings==false. label=",
						label
					}));
				}
				else if (thing.Discarded)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Deep-saving discarded thing ",
						thing,
						". This mode means that the thing is no longer managed by anything in the code and should not be deep-saved anywhere. (even with saveDestroyedThings==true) , label=",
						label
					}));
				}
			}
			IExposable exposable = target as IExposable;
			if (target != null && exposable == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"Cannot use LookDeep to save non-IExposable non-null ",
					label,
					" of type ",
					typeof(T)
				}));
				return;
			}
			if (target == null)
			{
				if (!Scribe.EnterNode(label))
				{
					goto IL_1A8;
				}
				try
				{
					Scribe.saver.WriteAttribute("IsNull", "True");
					goto IL_1A8;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			if (Scribe.EnterNode(label))
			{
				try
				{
					if (target.GetType() != typeof(T) || typeof(T).IsGenericTypeDefinition)
					{
						Scribe.saver.WriteAttribute("Class", GenTypes.GetTypeNameWithoutIgnoredNamespaces(target.GetType()));
					}
					exposable.ExposeData();
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception while saving ",
						exposable.ToStringSafe<IExposable>(),
						": ",
						ex2
					}));
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			IL_1A8:
			Scribe.saver.loadIDsErrorsChecker.RegisterDeepSaved(target, label);
		}
	}
}
