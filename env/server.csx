using Nancy.Responses;

Require<NancyPack>().At(8991).Host();

public class TestEnvironmentsModule
  : NancyModule
{
  public TestEnvironmentsModule() 
    : base("testenvironments")
  {
    Get["/{config}"] = p => {
      var kv = (IDictionary<string, object>)p;
      var config = kv.ContainsKey("config")
        ? kv["config"].ToString()
        : null;
        
      if (config == null) return config;
      
      return new GenericFileResponse(string.Concat("data/", config, ".json"));
    };
  }
}