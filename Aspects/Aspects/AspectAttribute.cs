using System;

namespace Aspects
{
    // TODO is `AttributeUsage` overwritten or combined? When defining abstract attributes, we may want to restrict usage
    //      to specific `AttributeTargets`. When defining concrete implementations, we may want to further restrict usage,
    //      e.g., `AllowMultiple=false`, but we wouldn't want to mistakenly change the `AttributeTargets` restriction of
    //      the base type.

    /// <summary>
    ///
    /// </summary>
    public abstract class AspectAttribute : Attribute
    {
    }

    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ClassAspectAttribute : Attribute
    {
    }

    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class MethodAspectAttribute : Attribute
    {
    }

    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class PropertyAspectAttribute : Attribute
    {

    }
}