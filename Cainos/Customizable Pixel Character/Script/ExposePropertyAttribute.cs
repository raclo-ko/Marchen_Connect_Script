//https://wiki.unity3d.com/index.php/ExposePropertiesInInspector_SetOnlyWhenChanged

using System;

namespace Cainos.CustomizablePixelCharacter
{
    [AttributeUsage(AttributeTargets.Property)] public class ExposePropertyAttribute : Attribute { }
}
