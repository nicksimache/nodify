using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace Nodify.Playground
{
    /// <summary>Applies JSON overrides to the existing playground settings at startup.</summary>
    internal static class SettingsFile
    {
        public static void Load()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "playground.settings.json");
            if (!File.Exists(path)) return;

            var errors = new List<string>();
            try
            {
                using (var stream = File.OpenRead(path))
                using (var reader = JsonReaderWriterFactory.CreateJsonReader(stream, XmlDictionaryReaderQuotas.Max))
                {
                    var root = XElement.Load(reader);
                    if ((string?)root.Attribute("type") != "object")
                        throw new FormatException("The settings file must contain a JSON object.");
                    foreach (var section in root.Elements())
                    {
                        object target;
                        switch (section.Name.LocalName)
                        {
                            case "Playground": target = PlaygroundSettings.Instance; break;
                            case "Editor": target = EditorSettings.Instance; break;
                            default: errors.Add($"Unknown section: {section.Name.LocalName}"); continue;
                        }
                        if ((string?)section.Attribute("type") != "object")
                        {
                            errors.Add($"{section.Name.LocalName} must be an object.");
                            continue;
                        }
                        foreach (var setting in section.Elements())
                        {
                            string name = setting.Name.LocalName;
                            try
                            {
                                var property = target.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                                if (property?.SetMethod == null || name == nameof(PlaygroundSettings.SearchText))
                                    throw new FormatException("Unknown or read-only setting.");
                                var type = property.PropertyType;
                                object value;
                                if (type == typeof(PointEditor))
                                {
                                    var point = Point.Parse(setting.Value);
                                    if (!IsFinite(point.X) || !IsFinite(point.Y)) throw new FormatException("Coordinates must be finite.");
                                    // Keep the original labels and update existing point bindings.
                                    var editor = (PointEditor)property.GetValue(target)!;
                                    editor.X = point.X;
                                    editor.Y = point.Y;
                                    continue;
                                }
                                if (type.IsEnum)
                                {
                                    value = Enum.Parse(type, setting.Value, true);
                                    if (!Enum.IsDefined(type, value)) throw new FormatException("Unknown option.");
                                }
                                else if (type == typeof(bool) || type == typeof(uint) || type == typeof(double) || type == typeof(string))
                                    value = Convert.ChangeType(setting.Value, type, CultureInfo.InvariantCulture);
                                else throw new FormatException("Unsupported setting type.");
                                if (value is double number && !IsFinite(number)) throw new FormatException("Number must be finite.");
                                if ((name == "GridSpacing" || name == "MinZoom" || name == "MaxZoom" || name == "Zoom")
                                    && Convert.ToDouble(value, CultureInfo.InvariantCulture) <= 0)
                                    throw new FormatException("Value must be greater than zero.");
                                property.SetValue(target, value);
                            }
                            catch (Exception ex) when (ex is FormatException || ex is ArgumentException || ex is OverflowException || ex is TargetInvocationException)
                            {
                                errors.Add($"{section.Name.LocalName}.{name}: {ex.InnerException?.Message ?? ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is XmlException || ex is FormatException)
            {
                errors.Add(ex.Message);
            }
            if (errors.Count > 0)
                MessageBox.Show($"Some settings could not be loaded from {path}:\n\n" + string.Join("\n", errors),
                    "Playground settings", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private static bool IsFinite(double value) => !double.IsNaN(value) && !double.IsInfinity(value);
    }
}
