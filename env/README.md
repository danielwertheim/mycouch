The project `MyCouch.TestServer` is a small self-hosted Nancy server that is used to serve e.g. test environment configurations to the integration tests. Just ensure there's a folder called `env\data` and that it contains the JSON-file `normal.json`. There should be one checked in that is good for running tests against a default local CouchDb install.

```
env\data\normal.json
```

Some explanation of the `Supports` attribute in the JSON [is explained here](http://danielwertheim.se/2014/04/05/xunit-dynamically-skipping-tests-for-different-test-environments)

The `MyCouch.TestServer` project **is not being built** in `DEBUG` nor for `RELEASE`. Hence the first time you need to explicitly e.g rebuild it via right clicking on it in the Solution Explorer.