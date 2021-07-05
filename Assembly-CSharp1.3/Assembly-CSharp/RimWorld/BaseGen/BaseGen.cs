using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001595 RID: 5525
	public static class BaseGen
	{
		// Token: 0x17001603 RID: 5635
		// (get) Token: 0x0600827C RID: 33404 RVA: 0x002E4CB8 File Offset: 0x002E2EB8
		public static string CurrentSymbolPath
		{
			get
			{
				return BaseGen.currentSymbolPath;
			}
		}

		// Token: 0x0600827D RID: 33405 RVA: 0x002E4CC0 File Offset: 0x002E2EC0
		public static void Reset()
		{
			BaseGen.rulesBySymbol.Clear();
			List<RuleDef> allDefsListForReading = DefDatabase<RuleDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				List<RuleDef> list;
				if (!BaseGen.rulesBySymbol.TryGetValue(allDefsListForReading[i].symbol, out list))
				{
					list = new List<RuleDef>();
					BaseGen.rulesBySymbol.Add(allDefsListForReading[i].symbol, list);
				}
				list.Add(allDefsListForReading[i]);
			}
		}

		// Token: 0x0600827E RID: 33406 RVA: 0x002E4D34 File Offset: 0x002E2F34
		public static void Generate()
		{
			if (BaseGen.working)
			{
				Log.Error("Cannot call Generate() while already generating. Nested calls are not allowed.");
				return;
			}
			BaseGen.working = true;
			BaseGen.currentSymbolPath = "";
			BaseGen.globalSettings.ClearResult();
			try
			{
				if (BaseGen.symbolStack.Empty)
				{
					Log.Warning("Symbol stack is empty.");
				}
				else if (BaseGen.globalSettings.map == null)
				{
					Log.Error("Called BaseGen.Resolve() with null map.");
				}
				else
				{
					int num = BaseGen.symbolStack.Count - 1;
					int num2 = 0;
					while (!BaseGen.symbolStack.Empty)
					{
						num2++;
						if (num2 > 100000)
						{
							Log.Error("Error in BaseGen: Too many iterations. Infinite loop?");
							break;
						}
						SymbolStack.Element element = BaseGen.symbolStack.Pop();
						BaseGen.currentSymbolPath = element.symbolPath;
						if (BaseGen.symbolStack.Count == num)
						{
							BaseGen.globalSettings.mainRect = element.resolveParams.rect;
							num--;
						}
						try
						{
							BaseGen.Resolve(element);
						}
						catch (Exception ex)
						{
							Log.Error(string.Concat(new object[]
							{
								"Error while resolving symbol \"",
								element.symbol,
								"\" with params=",
								element.resolveParams,
								"\n\nException: ",
								ex
							}));
						}
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Error in BaseGen: " + arg);
			}
			finally
			{
				BaseGen.globalSettings.landingPadsGenerated = BaseGen.globalSettings.basePart_landingPadsResolved;
				BaseGen.working = false;
				BaseGen.symbolStack.Clear();
				BaseGen.globalSettings.Clear();
			}
		}

		// Token: 0x0600827F RID: 33407 RVA: 0x002E4F00 File Offset: 0x002E3100
		private static void Resolve(SymbolStack.Element toResolve)
		{
			string symbol = toResolve.symbol;
			ResolveParams resolveParams = toResolve.resolveParams;
			BaseGen.tmpResolvers.Clear();
			List<RuleDef> list;
			if (BaseGen.rulesBySymbol.TryGetValue(symbol, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					RuleDef ruleDef = list[i];
					for (int j = 0; j < ruleDef.resolvers.Count; j++)
					{
						SymbolResolver symbolResolver = ruleDef.resolvers[j];
						if (symbolResolver.CanResolve(resolveParams))
						{
							BaseGen.tmpResolvers.Add(symbolResolver);
						}
					}
				}
			}
			if (!BaseGen.tmpResolvers.Any<SymbolResolver>())
			{
				Log.Warning(string.Concat(new object[]
				{
					"Could not find any RuleDef for symbol \"",
					symbol,
					"\" with any resolver that could resolve ",
					resolveParams
				}));
				return;
			}
			BaseGen.tmpResolvers.RandomElementByWeight((SymbolResolver x) => x.selectionWeight).Resolve(resolveParams);
		}

		// Token: 0x04005130 RID: 20784
		public static GlobalSettings globalSettings = new GlobalSettings();

		// Token: 0x04005131 RID: 20785
		public static SymbolStack symbolStack = new SymbolStack();

		// Token: 0x04005132 RID: 20786
		private static Dictionary<string, List<RuleDef>> rulesBySymbol = new Dictionary<string, List<RuleDef>>();

		// Token: 0x04005133 RID: 20787
		private static bool working;

		// Token: 0x04005134 RID: 20788
		private static string currentSymbolPath;

		// Token: 0x04005135 RID: 20789
		private const int MaxResolvedSymbols = 100000;

		// Token: 0x04005136 RID: 20790
		private static List<SymbolResolver> tmpResolvers = new List<SymbolResolver>();
	}
}
