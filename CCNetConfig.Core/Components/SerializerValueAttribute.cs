using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace CCNetConfig.Core.Components {
	/// <summary>
	/// Overrides the value of an enum to the specified value.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field )]
	public class SerializerValueAttribute : Attribute {
		/// <summary>
		/// Initializes a new instance of the <see cref="SerializerValueAttribute"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public SerializerValueAttribute (string value) : base () {
			this.Value = value;
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value { get; set; }
	}
}
