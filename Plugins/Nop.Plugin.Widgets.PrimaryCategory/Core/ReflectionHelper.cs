namespace Nop.Plugin.Widgets.PrimaryCategory.Core
{
    public class ReflectionHelper
    {
        public static void SetValue<T>(T t, string name, object value)
        {
            var type = t.GetType();
            var prop = type.GetProperty(name);
            if (prop != null)
                prop.SetValue(t, value, null);
        }

        public static object GetValue<T>(T t, string name)
        {
            var type = t.GetType();
            var prop = type.GetProperty(name);
            if (prop != null)
                return prop.GetValue(t, null);
            return null;
        }
    }
}
