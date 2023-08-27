﻿using System.Linq;
using System.Net.NetworkInformation;
using DeviceId.Internal;

namespace DeviceId.Components;

/// <summary>
/// An implementation of <see cref="IDeviceIdComponent"/> that uses the MAC Address of the PC.
/// </summary>
public class MacAddressDeviceIdComponent : IDeviceIdComponent
{
    /// <summary>
    /// A value determining whether wireless devices should be excluded.
    /// </summary>
    private readonly bool _excludeWireless;

    /// <summary>
    /// Initializes a new instance of the <see cref="MacAddressDeviceIdComponent"/> class.
    /// </summary>
    /// <param name="excludeWireless">A value determining whether wireless devices should be excluded.</param>
    public MacAddressDeviceIdComponent(bool excludeWireless)
    {
        _excludeWireless = excludeWireless;
    }

    /// <summary>
    /// Gets the component value.
    /// </summary>
    /// <returns>The component value.</returns>
    public string GetValue()
    {
        var values = NetworkInterface.GetAllNetworkInterfaces()
            .Where(x => x.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            .Select(x => x.GetPhysicalAddress().ToString())
            .Where(x => x != "000000000000")
            .Select(x => MacAddressFormatter.FormatMacAddress(x))
            .ToList();

        values.Sort();

        return values.Count > 0
            ? string.Join(",", values.ToArray())
            : null;
    }
}
