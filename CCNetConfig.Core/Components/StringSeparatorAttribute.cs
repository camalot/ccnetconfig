using System;
using System.Collections.Generic;
using System.Text;

namespace CCNetConfig.Core.Components {
	public class StringSeparatorAttribute : Attribute {
		public StringSeparatorAttribute ()
			: this ( ";" ) {

		}

		public StringSeparatorAttribute ( string separator ) {
			this.Separator = separator;
		}

		public string Separator { get; set; }
	}
}
