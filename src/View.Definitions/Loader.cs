namespace View.Definitions;

public delegate Task Loader(ILoadContext ctx);
public delegate Task<T> Loader<T>(ILoadContext ctx);