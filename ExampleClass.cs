using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestSerializationProblem
{
	[XmlType(Namespace = "testnamespace")]
	public class ExampleClass
	{

		[XmlElement(IsNullable = true, Order = 0)]
		public string? A { get; set; }

		[XmlElement(IsNullable = true, Order = 1)]
		public string? B { get; set; }

		[XmlElement(IsNullable = true, Order = 2)]
		public string? C { get; set; }

		[XmlElement(IsNullable = true, Order = 3)]
		public string? D { get; set; }
	}

	[XmlType(Namespace = "testnamespace")]
	public class ExampleClassWithoutOrder
	{

		[XmlElement(IsNullable = true)]
		public string? A { get; set; }

		[XmlElement(IsNullable = true)]
		public string? B { get; set; }

		[XmlElement(IsNullable = true)]
		public string? C { get; set; }

		[XmlElement(IsNullable = true)]
		public string? D { get; set; }
	}
}
