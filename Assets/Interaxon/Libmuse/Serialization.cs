using System;
using Newtonsoft.Json;

using System.Collections.Generic;


/*
 * You may have noticed that there is a lot of commented code here. I am keeping it for future comparison in case the status quo changes.
 * While the original JSON containers work perfectly well with Windows, there are a few considerations that trip up JSON deserialization on Android and, potentially, iOS.
 * First of all, Android and iOS both use IL2CPP compiling protocol instead of Mono, which used to be a problem with JSON before 2022. 
 * Even if you do switch to Mono, JSON (both Newtonsoft, Utility and all other forks) has to contend with one problem on Android:
 * JSON does not deserialize properties, only fields... which MUST have the exact name of the field declared in the serialized string.
 * Meaning, everything MUST be [Serializable]. This also applies to PropertyDrawers.cs, by the way!
 * 
 * An example of the cleanup that must be done: the property name "packetType" does not coincide with the field name "DataPacketType".
 * Missing the correct field name will result in JSON not deserializing the data into that variable.
 * Making it a property will also have the same result.
 * Declaring it a property with [JsonProperty("name")] or simply adding { get ; set; } will make it a property instead of a field.
 * 
 * It is through trial and error that I found out the name of these fields, replacing them in the code for the convenience of the average game developer.
 * You're welcome: - Diego Saldivar (2024)
 */


namespace Interaxon.Libmuse
{
    [Serializable]
    public class MuseConnectionState
    {
        // See notice at the top of this script explaining these adaptations.

        /*
        public ConnectionState PreviousConnectionState { get; set; }

        public ConnectionState CurrentConnectionState { get; set; }

        */

        public ConnectionState PreviousConnectionState;

        public ConnectionState CurrentConnectionState;

        public static MuseConnectionState FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MuseConnectionState>(json);
        }
    }

    [Serializable]
    public class LogPacket
    {
        public static LogPacket FromJson(string json)
        {
            return JsonConvert.DeserializeObject<LogPacket>(json);
        }


        public Severity Severity;
        public bool Raw;
        public string Tag;
        public double Timestamp;
        public string Message;

        // See notice at the top of this script explaining these adaptations.

        /*
        [JsonProperty("severity")]
        public Severity Severity { get; private set; }

        [JsonProperty("raw")]
        public bool Raw { get; private set; }

        [JsonProperty("tag")]
        public string Tag { get; private set; }

        [JsonProperty("timestamp")]
        public double Timestamp { get; private set; }

        [JsonProperty("message")]
        public string Message { get; private set; }
        */
    }

    [Serializable]
    public class MuseConnectionPacket
    {
        public static MuseConnectionPacket FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MuseConnectionPacket>(json);
        }

        public ConnectionState CurrentConnectionState;
        public ConnectionState PreviousConnectionState;
        public string BluetoothMac;

        // See notice at the top of this script explaining these adaptations.

        /*
        [JsonProperty("currentConnectionState")]
        public ConnectionState CurrentConnectionState { get; private set; }

        [JsonProperty("previousConnectionState")]
        public ConnectionState PreviousConnectionState { get; private set; }

        [JsonProperty("bluetoothMac")]
        public string BluetoothMac { get; private set; }
        */
    }

    [Serializable]
    public class MuseDataPacket
    {

        // See notice at the top of this script explaining these adaptations.

        /*
        public static MuseDataPacket FromNative(MuseDataPacketType packetType, double[] values, long timestamp, string macAddress)
        {
            return new MuseDataPacket
            {
                PacketType = packetType,
                Values = values,
                ValuesSize = values.Length,
                BluetoothMac = macAddress,
                Timestamp = timestamp
            };
        }

        [JsonProperty("packetType")]
        public MuseDataPacketType PacketType { get; private set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; private set; }

        [JsonProperty("values")]
        public double[] Values { get; private set; }

        [JsonProperty("valuesSize")]
        public long ValuesSize { get; private set; }

        [JsonProperty("bluetoothMac")]
        public string BluetoothMac { get; private set; }

        */

        public static MuseDataPacket FromNative(MuseDataPacketType packetType, double[] values, long timestamp, string macAddress)
        {
            return new MuseDataPacket
            {
                DataPacketType = packetType,
                DataPacketValue = values,
                TimeStamp = timestamp,
                ValuesSize = values.Length,
                BluetoothMac = macAddress
            };
        }

        public MuseDataPacketType DataPacketType;
        public double[] DataPacketValue;
        public long TimeStamp;
        public long ValuesSize; //Is this streamed at all?
        public string BluetoothMac; //Is this streamed at all?

    }

    [Serializable]
    public class MuseArtifactPacket
    {
        public static MuseArtifactPacket FromNative(double[] values, long timestamp, string macAddress)
        {
            return new MuseArtifactPacket
            {
                HeadbandOn = !values[0].Equals(0),
                Blink = !values[1].Equals(0),
                JawClench = !values[2].Equals(0),
                BluetoothMac = macAddress,
                Timestamp = timestamp
            };
        }

        public bool HeadbandOn;
        public bool Blink;
        public bool JawClench;
        public long Timestamp; //Is this streamed at all?
        public string BluetoothMac; //Is this streamed at all?


        // See notice at the top of this script explaining these adaptations.

        /*
        [JsonProperty("headbandOn")]
        public bool HeadbandOn { get; private set; }

        [JsonProperty("blink")]
        public bool Blink { get; private set; }

        [JsonProperty("jawClench")]
        public bool JawClench { get; private set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; private set; }

        [JsonProperty("bluetoothMac")]
        public string BluetoothMac { get; private set; }
        */
    }

    [Serializable]
    public class MuseConfiguration
    {
        public static MuseConfiguration FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MuseConfiguration>(json);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public MusePreset Preset;
        public string HeadbandName;
        public string MicrocontrollerId;
        public int EegChannelCount;
        public int AfeGain;
        public int DownsampleRate;
        public int SeroutMode;
        public int OutputFrequency;
        public int AdcFrequency;
        public bool NotchFilterEnabled;
        public NotchFrequency NotchFilter;
        public int AccelerometerSampleFrequency;
        public bool BatteryDataEnabled;
        public bool DrlRefEnabled;
        public int DrlRefFrequency;
        public double BatteryPercentRemaining;
        public string BluetoothMac;
        public string SerialNumber;
        public string HeadsetSerialNumber;
        public MuseModel Model;
        public string LicenseNonce;

        // See notice at the top of this script explaining these adaptations.

        /*
        [JsonProperty("preset")]
        public MusePreset Preset { get; private set; }

        [JsonProperty("headbandName")]
        public string HeadbandName { get; private set; }

        [JsonProperty("microcontrollerId")]
        public string MicrocontrollerId { get; private set; }

        [JsonProperty("eegChannelCount")]
        public int EegChannelCount { get; private set; }

        [JsonProperty("afeGain")]
        public int AfeGain { get; private set; }

        [JsonProperty("downsampleRate")]
        public int DownsampleRate { get; private set; }

        [JsonProperty("seroutMode")]
        public int SeroutMode { get; private set; }

        [JsonProperty("outputFrequency")]
        public int OutputFrequency { get; private set; }

        [JsonProperty("adcFrequency")]
        public int AdcFrequency { get; private set; }

        [JsonProperty("notchFilterEnabled")]
        public bool NotchFilterEnabled { get; private set; }

        [JsonProperty("notchFilter")]
        public NotchFrequency NotchFilter { get; private set; }

        [JsonProperty("accelerometerSampleFrequency")]
        public int AccelerometerSampleFrequency { get; private set; }

        [JsonProperty("batteryDataEnabled")]
        public bool BatteryDataEnabled { get; private set; }

        [JsonProperty("drlRefEnabled")]
        public bool DrlRefEnabled { get; private set; }

        [JsonProperty("drlRefFrequency")]
        public int DrlRefFrequency { get; set; }

        [JsonProperty("batteryPercentRemaining")]
        public double BatteryPercentRemaining { get; private set; }

        [JsonProperty("bluetoothMac")]
        public string BluetoothMac { get; private set; }

        [JsonProperty("serialNumber")]
        public string SerialNumber { get; private set; }

        [JsonProperty("headsetSerialNumber")]
        public string HeadsetSerialNumber { get; set; }

        [JsonProperty("model")]
        public MuseModel Model { get; private set; }

        [JsonProperty("nonce")]
        public string LicenseNonce { get; set; }
        */
    }

    [Serializable]
    public class MuseVersion
    {
        public static MuseVersion FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MuseVersion>(json);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string RunningState;
        public string HardwareVersion;
        public string BspVersion;
        public string FirmwareVersion;
        public string BootloaderVersion;
        public string FirmwareBuildNumber;
        public string FirmwareType;
        public int ProtocolVersion;

        // See notice at the top of this script explaining these adaptations.

        /*
        [JsonProperty("runningState")]
        public string RunningState { get; private set; }

        [JsonProperty("hardwareVersion")]
        public string HardwareVersion { get; private set; }

        [JsonProperty("bspVersion")]
        public string BspVersion { get; private set; }

        [JsonProperty("firmwareVersion")]
        public string FirmwareVersion { get; private set; }

        [JsonProperty("bootloaderVersion")]
        public string BootloaderVersion { get; private set; }

        [JsonProperty("firmwareBuildNumber")]
        public string FirmwareBuildNumber { get; private set; }

        [JsonProperty("firmwareType")]
        public string FirmwareType { get; private set; }

        [JsonProperty("protocolVersion")]
        public int ProtocolVersion { get; private set; }
        */
    }

    [Serializable]
    public class MuseError
    {
        public static MuseError FromJson(string json)
        {
            return JsonConvert.DeserializeObject<MuseError>(json);
        }

        public ErrorType Type;
        public int Code;
        public string Info;
        public string BluetoothMac;

        // See notice at the top of this script explaining these adaptations.

        /*
        [JsonProperty("type")]
        public ErrorType Type { get; private set; }

        [JsonProperty("code")]
        public int Code { get; private set; }

        [JsonProperty("info")]
        public string Info { get; private set; }

        [JsonProperty("bluetoothMac")]
        public string BluetoothMac { get; private set; }
        */
    }

    [Serializable]
    public class AnnotationData
    {

        // See notice at the top of this script explaining these adaptations.

        /*
        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("format")]
        public AnnotationFormat Format { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("eventId")]
        public string EventId { get; set; }

        [JsonProperty("parentId")]
        public string ParentId { get; set; }
        */

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string Data;
        public AnnotationFormat Format;
        public string EventType;
        public string EventId;
        public string ParentId;
    }

    [Serializable]
    public class DspData
    {

        // See notice at the top of this script explaining these adaptations.
        /*
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("floatArray")]
        public double[] FloatArray { get; set; }

        [JsonProperty("intArray")]
        public long[] IntArray { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
        */

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string Type;
        public double[] FloatArray;
        public long[] IntArray;
        public string Version;
    }

    [Serializable]
    public class ComputingDeviceConfiguration
    {
        // See notice at the top of this script explaining these adaptations.
        /*
        [JsonProperty("osType")]
        public string OsType { get; set; }

        [JsonProperty("osVersion")]
        public string OsVersion { get; set; }

        [JsonProperty("hardwareModelName")]
        public string HardwareModelName { get; set; }

        [JsonProperty("hardwareModelId")]
        public string HardwareModelId { get; set; }

        [JsonProperty("processorName")]
        public string ProcessorName { get; set; }

        [JsonProperty("processorSpeed")]
        public string ProcessorSpeed { get; set; }

        [JsonProperty("numberOfProcessors")]
        public int ProcessorCount { get; set; }

        [JsonProperty("memorySize")]
        public string MemorySize { get; set; }

        [JsonProperty("bluetoothVersion")]
        public string BluetoothVersion { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("timeZoneOffsetSeconds")]
        public int TimeZoneOffsetSeconds { get; set; }
        */

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string OsType;
        public string OsVersion;
        public string HardwareModelName;
        public string HardwareModelId;
        public string ProcessorName;
        public string ProcessorSpeed;
        public int ProcessorCount;
        public string MemorySize;
        public string BluetoothVersion;
        public string TimeZone;
        public int TimeZoneOffsetSeconds;
    }
}
