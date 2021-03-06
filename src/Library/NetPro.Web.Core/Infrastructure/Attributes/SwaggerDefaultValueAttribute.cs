﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NetPro.Web.Core
{
	/// <summary>
	/// Swagger默认值设置
	/// </summary>
	public class SwaggerDefaultValueAttribute : Attribute
	{
		public object Value { get; set; }

		public SwaggerDefaultValueAttribute(object value)
		{
			this.Value = value;
		}
	}
}
