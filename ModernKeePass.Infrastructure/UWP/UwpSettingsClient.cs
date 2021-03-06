﻿using System;
using Windows.Foundation.Collections;
using Windows.Storage;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Infrastructure.UWP
{
    public class UwpSettingsClient : ISettingsProxy
    {
        private readonly IPropertySet _values = ApplicationData.Current.LocalSettings.Values;
        
        public T GetSetting<T>(string property, T defaultValue = default(T))
        {
            try
            {
                var value = (T)Convert.ChangeType(_values[property], typeof(T));
                return value == null ? defaultValue : value;
            }
            catch (InvalidCastException)
            {
                return defaultValue;
            }
        }

        public void PutSetting<T>(string property, T value)
        {
            if (_values.ContainsKey(property))
                _values[property] = value;
            else _values.Add(property, value);
        }
    }
}
