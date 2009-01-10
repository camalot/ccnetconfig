using System;
using System.Collections.Generic;
using System.Text;

namespace CCNetConfig.Core.Exceptions {
	public class UnsupportedPropertyForVersionException : Exception {
		public UnsupportedPropertyForVersionException () : base ( "The property specified is not valid for this version CC.NET") {

		}

		public UnsupportedPropertyForVersionException ( string propertyName, Version version ) : base (string.Format("The property '{0}' is not supported in version {1} of CC.NET",propertyName, version)) {
			this.PropertyName = propertyName;
			this.Version = version;
		}

		public Version Version { get; set; }
		public string PropertyName { get; set; }
	}
}
