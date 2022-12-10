using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using CommunityToolkit.Diagnostics;

namespace Mapping;

public class Mapper
{
	private readonly Methods _getter = Methods.Getter;
	private readonly Methods _setter = Methods.Setter;

	public Dictionary<Type, Dictionary<PropertyInfo, Dictionary<string, Dictionary<Methods, Delegate>>>> RegisteredTypes { get; set; } = new Dictionary<Type, Dictionary<PropertyInfo, Dictionary<string, Dictionary<Methods, Delegate>>>>();

	#region Public Methods

	public void RegisterType<TClass>()
	{
		RegisteredTypes.Add(typeof(TClass), CacheProperties<TClass>());
	}
	public TDestination Map<TSource, TDestination>(TSource source)
		where TDestination : new()
	{
		// Create new targert object.
		TDestination destination = new TDestination();

		// Get the source and target types.
		Type SourceType = typeof(TSource);
		Type DestinationType = typeof(TDestination);

		// Get property dictionary for source class and target class.
		RegisteredTypes.TryGetValue(SourceType, out Dictionary<PropertyInfo, Dictionary<string, Dictionary<Methods, Delegate>>>? sourcePropertyInfoDictionary);
		RegisteredTypes.TryGetValue(DestinationType, out Dictionary<PropertyInfo, Dictionary<string, Dictionary<Methods, Delegate>>>? destinationPropertyInfoDictionary);

		foreach (KeyValuePair<PropertyInfo, Dictionary<string, Dictionary<Methods, Delegate>>> sourcePropertyInfoItem in sourcePropertyInfoDictionary)
		{
			string sourcePropertyName = sourcePropertyInfoItem.Key.Name;
			KeyValuePair<PropertyInfo, Dictionary<string, Dictionary<Methods, Delegate>>> destinationPropertyInfoItem = destinationPropertyInfoDictionary.Where(predicate: (KeyValuePair<PropertyInfo, Dictionary<string, Dictionary<Methods, Delegate>>> kvp) => kvp.Key.Name == sourcePropertyName).SingleOrDefault();


			Delegate SetterDelegate;
			Delegate GetterDelegate;

			Dictionary<Methods, Delegate> sourcePropertyNameItem = new Dictionary<Methods, Delegate>();
			Dictionary<Methods, Delegate> destinationPropertyNameItem = new Dictionary<Methods, Delegate>();

			if (!sourcePropertyInfoItem.Value.TryGetValue(sourcePropertyName, out sourcePropertyNameItem))
			{
				ThrowHelper.ThrowArgumentNullException(nameof(sourcePropertyNameItem));
			}

			if(!destinationPropertyInfoItem.Value.TryGetValue(sourcePropertyName, out destinationPropertyNameItem))
			{
				ThrowHelper.ThrowArgumentNullException(nameof(destinationPropertyNameItem));
			}

			if (!sourcePropertyNameItem.TryGetValue(_getter, out GetterDelegate))
			{
				ThrowHelper.ThrowArgumentNullException(nameof(GetterDelegate));
			}

			if(!destinationPropertyNameItem.TryGetValue(_setter, out SetterDelegate))
			{
				ThrowHelper.ThrowArgumentNullException(nameof(SetterDelegate));
			}

			MapProperty<TSource, TDestination>(GetterDelegate, SetterDelegate, source, destination, sourcePropertyInfoItem.Key);
		}

		return destination;
	}

	#endregion

	#region Private Methods

	#region Mapping

	private void MapProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination, PropertyInfo property)
	{
		switch (property.PropertyType.Name)
		{
			case "SByte"       : MapSByteProperty<TSource, TDestination>(getter, setter, source, destination); break;
			case "Byte"        : MapByteProperty<TSource, TDestination>(getter, setter, source, destination); break;
			case "Int16"       : MapShortProperty<TSource, TDestination>(getter, setter, source, destination); break;
			case "UInt16"      : MapUnsignedShortProperty<TSource, TDestination>(getter, setter, source, destination); break;
			case "Int32"       : MapIntegerProperty<TSource, TDestination>(getter, setter, source, destination); break;
			case "UInt32"      : MapUnsignedIntegerProperty<TSource, TDestination>(getter, setter, source, destination); break;
			case "Int64"       : MapLongProperty<TSource, TDestination>(getter, setter, source, destination); break;
			case "UInt64"      : MapUnsignedLongProperty<TSource, TDestination>(getter, setter, source, destination); break;
			case "Char"        : MapCharacterProperty<TSource, TDestination>(getter, setter, source, destination); break;
			case "Single"      : MapFloatProperty<TSource, TDestination>(getter, setter, source, destination); break;
			case "Double"      : MapDoubleProperty<TSource, TDestination>(getter, setter, source, destination); break;
			case "Decimal"     : MapDecimalProperty<TSource, TDestination>(getter, setter, source, destination); break;
			case "Boolean"     : MapBooleanProperty<TSource, TDestination>(getter, setter, source, destination); break;
			case "String"      : MapStringProperty<TSource, TDestination>(getter, setter, source, destination); break;
			case "DateTime"    : MapDateTimeProperty<TSource, TDestination>(getter, setter, source, destination); break;
			case "Guid"    	   : MapGuidProperty<TSource, TDestination>(getter, setter, source, destination); break;
			default            : throw new NotImplementedException();
		};
	}
	private void MapSByteProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)		=> (setter as Action<TDestination, sbyte>)!(destination, (getter as Func<TSource, sbyte>)!(source));
	private void MapByteProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)			=> (setter as Action<TDestination, byte>)!(destination, (getter as Func<TSource, byte>)!(source));
	private void MapShortProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)		=> (setter as Action<TDestination, short>)!(destination, (getter as Func<TSource, short>)!(source));
	private void MapUnsignedShortProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)	=> (setter as Action<TDestination, ushort>)!(destination, (getter as Func<TSource, ushort>)!(source));
	private void MapIntegerProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)		=> (setter as Action<TDestination, int>)!(destination, (getter as Func<TSource, int>)!(source));
	private void MapUnsignedIntegerProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)	=> (setter as Action<TDestination, uint>)!(destination, (getter as Func<TSource, uint>)!(source));
	private void MapLongProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)			=> (setter as Action<TDestination, long>)!(destination, (getter as Func<TSource, long>)!(source));
	private void MapUnsignedLongProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)		=> (setter as Action<TDestination, ulong>)!(destination, (getter as Func<TSource, ulong>)!(source));
	private void MapCharacterProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)		=> (setter as Action<TDestination, char>)!(destination, (getter as Func<TSource, char>)!(source));
	private void MapFloatProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)		=> (setter as Action<TDestination, float>)!(destination, (getter as Func<TSource, float>)!(source));
	private void MapDoubleProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)		=> (setter as Action<TDestination, double>)!(destination, (getter as Func<TSource, double>)!(source));
	private void MapDecimalProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)		=> (setter as Action<TDestination, decimal>)!(destination, (getter as Func<TSource, decimal>)!(source));
	private void MapBooleanProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)		=> (setter as Action<TDestination, bool>)!(destination, (getter as Func<TSource, bool>)!(source));
	private void MapStringProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)		=> (setter as Action<TDestination, string>)!(destination, (getter as Func<TSource, string>)!(source));
	private void MapDateTimeProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)		=> (setter as Action<TDestination, DateTime>)!(destination, (getter as Func<TSource, DateTime>)!(source));
	private void MapGuidProperty<TSource, TDestination>(Delegate getter, Delegate setter, TSource source, TDestination destination)			=> (setter as Action<TDestination, Guid>)!(destination, (getter as Func<TSource, Guid>)!(source));

	#endregion

	#region Caching Methods

	private Dictionary<PropertyInfo, Dictionary<string, Dictionary<Methods, Delegate>>> CacheProperties<TClass>()
	{
		Dictionary<PropertyInfo, Dictionary<string, Dictionary<Methods, Delegate>>> propertyInfoDictionary = new Dictionary<PropertyInfo, Dictionary<string, Dictionary<Methods, Delegate>>>();
		PropertyInfo[] properties = typeof(TClass).GetProperties()!;

		foreach (PropertyInfo property in properties)
		{
			Dictionary<string, Dictionary<Methods, Delegate>>? propertyNameDictionary = CacheAccessors<TClass>(property);
			propertyInfoDictionary.Add(property, propertyNameDictionary);
		}

		return propertyInfoDictionary;
	}

	#region Cache Accessors

	private Dictionary<string, Dictionary<Methods, Delegate>> CacheAccessors<TClass>(PropertyInfo property) => property.PropertyType.Name switch
	{
		"SByte" 	=> CacheSByteAccessors<TClass>(property),
		"Byte" 		=> CacheByteAccessors<TClass>(property),
		"Int16" 	=> CacheShortAccessors<TClass>(property),
		"UInt16" 	=> CacheUnsignedShortAccessors<TClass>(property),
		"Int32" 	=> CacheIntegerAccessors<TClass>(property),
		"UInt32" 	=> CacheUnsignedIntegerAccessors<TClass>(property),
		"Int64" 	=> CacheLongAccessors<TClass>(property),
		"UInt64" 	=> CacheUnsignedLongAccessors<TClass>(property),
		"Char" 		=> CacheCharacterAccessors<TClass>(property),
		"Single" 	=> CacheFloatAccessors<TClass>(property),
		"Double" 	=> CacheDoubleAccessors<TClass>(property),
		"Decimal" 	=> CacheDecimalAccessors<TClass>(property),
		"Boolean" 	=> CacheBooleanAccessors<TClass>(property),
		"String" 	=> CacheStringAccessors<TClass>(property),
		"DateTime" 	=> CacheDateTimeAccessors<TClass>(property),
		"Guid" 		=> CacheGuidAccessors<TClass>(property),
		_ 			=> throw new NotImplementedException($"A cache property method does not exist for {property.PropertyType.Name} type." ),
	};
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheSByteAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();

		delegateDictionary.Add(_getter, CacheSByteGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheSByteSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheByteAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();
		delegateDictionary.Add(_getter, CacheByteGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheByteSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheShortAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();
		delegateDictionary.Add(_getter, CacheShortGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheShortSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheUnsignedShortAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();
		delegateDictionary.Add(_getter, CacheUnsignedShortGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheUnsignedShortSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheIntegerAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();
		delegateDictionary.Add(_getter, CacheIntegerGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheIntegerSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheUnsignedIntegerAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();
		delegateDictionary.Add(_getter, CacheUnsignedIntegerGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheUnsignedIntegerSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheLongAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();
		delegateDictionary.Add(_getter, CacheLongGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheLongSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheUnsignedLongAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();
		delegateDictionary.Add(_getter, CacheUnsignedLongGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheUnsignedLongSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheCharacterAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();
		delegateDictionary.Add(_getter, CacheCharacterGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheCharacterSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheFloatAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();
		delegateDictionary.Add(_getter, CacheFloatGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheFloatSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheDoubleAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();
		delegateDictionary.Add(_getter, CacheDoubleGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheDoubleSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheDecimalAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();
		delegateDictionary.Add(_getter, CacheDecimalGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheDecimalSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheBooleanAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();
		delegateDictionary.Add(_getter, CacheBooleanGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheBooleanSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheStringAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();
		delegateDictionary.Add(_getter, CacheStringGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheStringSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheDateTimeAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();
		delegateDictionary.Add(_getter, CacheDateTimeGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheDateTimeSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}
	private Dictionary<string, Dictionary<Methods, Delegate>> CacheGuidAccessors<TClass>(PropertyInfo property)
	{
		Dictionary<string, Dictionary<Methods, Delegate>> propertyNameDictionary = new Dictionary<string, Dictionary<Methods, Delegate>>();
		Dictionary<Methods, Delegate> delegateDictionary = new Dictionary<Methods, Delegate>();
		delegateDictionary.Add(_getter, CacheGuidGetter<TClass>(property));
		delegateDictionary.Add(_setter, CacheGuidSetter<TClass>(property));
		propertyNameDictionary.Add(property.Name, delegateDictionary);
		return propertyNameDictionary;
	}

	#endregion
		
	#region Cache Getters

	private Func<TClass, TProperty> CacheGetter<TClass, TProperty>(PropertyInfo property) => (Func<TClass, TProperty>)Delegate.CreateDelegate(typeof(Func<TClass, TProperty>), property.GetGetMethod()!);
	private Func<TClass, sbyte> CacheSByteGetter<TClass>(PropertyInfo property)           => CacheGetter<TClass, sbyte>(property);
	private Func<TClass, byte> CacheByteGetter<TClass>(PropertyInfo property)             => CacheGetter<TClass, byte>(property);
	private Func<TClass, short> CacheShortGetter<TClass>(PropertyInfo property)           => CacheGetter<TClass, short>(property);
	private Func<TClass, ushort> CacheUnsignedShortGetter<TClass>(PropertyInfo property)  => CacheGetter<TClass, ushort>(property);
	private Func<TClass, int> CacheIntegerGetter<TClass>(PropertyInfo property)           => CacheGetter<TClass, int>(property);
	private Func<TClass, uint> CacheUnsignedIntegerGetter<TClass>(PropertyInfo property)  => CacheGetter<TClass, uint>(property);
	private Func<TClass, long> CacheLongGetter<TClass>(PropertyInfo property)             => CacheGetter<TClass, long>(property);
	private Func<TClass, ulong> CacheUnsignedLongGetter<TClass>(PropertyInfo property)    => CacheGetter<TClass, ulong>(property);
	private Func<TClass, char> CacheCharacterGetter<TClass>(PropertyInfo property)        => CacheGetter<TClass, char>(property);
	private Func<TClass, float> CacheFloatGetter<TClass>(PropertyInfo property)           => CacheGetter<TClass, float>(property);
	private Func<TClass, double> CacheDoubleGetter<TClass>(PropertyInfo property)         => CacheGetter<TClass, double>(property);
	private Func<TClass, decimal> CacheDecimalGetter<TClass>(PropertyInfo property)       => CacheGetter<TClass, decimal>(property);
	private Func<TClass, bool> CacheBooleanGetter<TClass>(PropertyInfo property)          => CacheGetter<TClass, bool>(property);
	private Func<TClass, string> CacheStringGetter<TClass>(PropertyInfo property)         => CacheGetter<TClass, string>(property);
	private Func<TClass, DateTime> CacheDateTimeGetter<TClass>(PropertyInfo property)     => CacheGetter<TClass, DateTime>(property);
	private Func<TClass, Guid> CacheGuidGetter<TClass>(PropertyInfo property)             => CacheGetter<TClass, Guid>(property);

	#endregion
		
	#region Cache Setters

	private Action<TClass, TProperty> CacheSetter<TClass, TProperty>(PropertyInfo property) => (Action<TClass, TProperty>)Delegate.CreateDelegate(typeof(Action<TClass, TProperty>), property.GetSetMethod()!);
	private Action<TClass, sbyte> CacheSByteSetter<TClass>(PropertyInfo property)           => CacheSetter<TClass, sbyte>(property);
	private Action<TClass, byte> CacheByteSetter<TClass>(PropertyInfo property)             => CacheSetter<TClass, byte>(property);
	private Action<TClass, short> CacheShortSetter<TClass>(PropertyInfo property)           => CacheSetter<TClass, short>(property);
	private Action<TClass, ushort> CacheUnsignedShortSetter<TClass>(PropertyInfo property)  => CacheSetter<TClass, ushort>(property);
	private Action<TClass, int> CacheIntegerSetter<TClass>(PropertyInfo property)           => CacheSetter<TClass, int>(property);
	private Action<TClass, uint> CacheUnsignedIntegerSetter<TClass>(PropertyInfo property)  => CacheSetter<TClass, uint>(property);
	private Action<TClass, long> CacheLongSetter<TClass>(PropertyInfo property)             => CacheSetter<TClass, long>(property);
	private Action<TClass, ulong> CacheUnsignedLongSetter<TClass>(PropertyInfo property)    => CacheSetter<TClass, ulong>(property);
	private Action<TClass, char> CacheCharacterSetter<TClass>(PropertyInfo property)        => CacheSetter<TClass, char>(property);
	private Action<TClass, float> CacheFloatSetter<TClass>(PropertyInfo property)           => CacheSetter<TClass, float>(property);
	private Action<TClass, double> CacheDoubleSetter<TClass>(PropertyInfo property)         => CacheSetter<TClass, double>(property);
	private Action<TClass, decimal> CacheDecimalSetter<TClass>(PropertyInfo property)       => CacheSetter<TClass, decimal>(property);
	private Action<TClass, bool> CacheBooleanSetter<TClass>(PropertyInfo property)          => CacheSetter<TClass, bool>(property);
	private Action<TClass, string> CacheStringSetter<TClass>(PropertyInfo property)         => CacheSetter<TClass, string>(property);
	private Action<TClass, DateTime> CacheDateTimeSetter<TClass>(PropertyInfo property)     => CacheSetter<TClass, DateTime>(property);
	private Action<TClass, Guid> CacheGuidSetter<TClass>(PropertyInfo property)             => CacheSetter<TClass, Guid>(property);

	#endregion

	#endregion

	#endregion
}
