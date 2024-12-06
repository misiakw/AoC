using AoC.Common.Maps.StaticMap;

namespace AoC.Common.Maps
{
    public static class MapBuilder<T>
    {
        public static IMap<T> GetEmpty(MapBuilderParams par, T? def = default(T))
        {
            if(par.IsInfinite)
                return new InifiniteMap<T>(def);

            return new StaticMap<T>(par.Width ?? 0, par.Height ?? 0, def);
        }
        public static IMap<T> GetEmpty(MapBuilderParams<T> par)
        {
            return MapBuilder<T>.GetEmpty((MapBuilderParams)par, par.DefaultValue);
        }
    }


    public class MapBuilderParams
    {
        public bool IsInfinite = false;
        public long? Width, Height;
    }

    public class MapBuilderParams<T>: MapBuilderParams
    {
        public T? DefaultValue;
    }
}
