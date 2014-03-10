The project `MyCouch.TestServer` is a small self-hosted Nancy server that is used to serve e.g. test environment configurations to the integration tests. Just ensure there's a folder called `data` and that it contains three JSON-files (*Cloudant is only needed if you want to run thoose tests*):

```
data\
     cloudant.json
     normal.json
     temp.json
```

`normal.json` and `temp.json` should exist from the beginning and point to local CouchDb databases. **Any other files are not checked in** to source control since it can contain sensitive data (e.g. Cloudant account info).

`cloudant.json` is only needed if you would like to execute tests against Cloudant.

The `MyCouch.TestServer` project **is not being built** in `DEBUG` nor for `RELEASE`. Hence the first time you need to explicitly e.g rebuild it via right clicking on it in the Solution Explorer.