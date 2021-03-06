﻿using System;
using System.Globalization;

namespace IllidanS4.SharpUtils.Reflection.CSharp
{
	partial class LanguageType
	{
		private class FieldTypeDescription : LanguageType
		{
			public override string LocalizedName{
				get{ return DynamicResources.GetString("SK_FIELD", CultureInfo.CurrentCulture); }
			}
			
			public override string Name{
				get{ return DynamicResources.GetString("SK_FIELD", CultureInfo.InvariantCulture); }
			}
		}
	}
}
