using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TestSerializationProblem;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var xmlString = """
			<ExampleClass xmlns="testnamespace">
				<A>ValueA</A>
				<B>ValueB</B>
				<C>ValueC</C>
				<D>ValueD</D>
			</ExampleClass>
			""";

		var xmlWithProperyInTheMiddleString = """
			<ExampleClass xmlns="testnamespace">
				<A>ValueA</A>
				<B>ValueB</B>
				<PropertyNew>This can by ignored</PropertyNew>
				<C>ValueC</C>
				<D>ValueD</D>
			</ExampleClass>
			""";


		var serializer = new XmlSerializer(typeof(ExampleClass), "testnamespace");

		serializer.UnknownAttribute += (s, e) => Debug.WriteLine($"UnknownAttribute: {e.Attr.Name}");
		serializer.UnknownElement += (s, e) => Debug.WriteLine($"UnknownElement: {e.Element.Name}");
		serializer.UnknownNode += (s, e) => Debug.WriteLine($"UnknownNode: {e.Name}");
		serializer.UnreferencedObject += (s, e) => Debug.WriteLine($"UnreferencedObject: {e.UnreferencedId}");

		var objOk = serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString))) as ExampleClass;
		var objBad = serializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlWithProperyInTheMiddleString))) as ExampleClass;

		Debug.WriteLine(objOk?.C);
		Debug.WriteLine(objBad?.C); // Must be "ValueC" but load Null


		//Fix the root node name for the other test class
		xmlString = xmlString.Replace(nameof(ExampleClass), nameof(ExampleClassWithoutOrder));
		xmlWithProperyInTheMiddleString = xmlWithProperyInTheMiddleString.Replace(nameof(ExampleClass), nameof(ExampleClassWithoutOrder));

		var serializerWithoutOrder = new XmlSerializer(typeof(ExampleClassWithoutOrder), "testnamespace");

		serializerWithoutOrder.UnknownAttribute += (s, e) => Debug.WriteLine($"UnknownAttribute: {e.Attr.Name}");
		serializerWithoutOrder.UnknownElement += (s, e) => Debug.WriteLine($"UnknownElement: {e.Element.Name}");
		serializerWithoutOrder.UnknownNode += (s, e) => Debug.WriteLine($"UnknownNode: {e.Name}");
		serializerWithoutOrder.UnreferencedObject += (s, e) => Debug.WriteLine($"UnreferencedObject: {e.UnreferencedId}");

		var objOkWithoutOrder = serializerWithoutOrder.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlString))) as ExampleClassWithoutOrder;
		var objBadWithoutOrder = serializerWithoutOrder.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(xmlWithProperyInTheMiddleString))) as ExampleClassWithoutOrder;

		Debug.WriteLine(objOk?.C);
		Debug.WriteLine(objBad?.C); // The load it's Ok!

		Environment.Exit(0);
		
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}


}


