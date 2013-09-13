The `server.csx` is used to serve test environment configurations to the integration tests. Just ensure there's a folder called `data` and that it contains two JSON-files:

```
data\
     local.json
     cloudant.json
```

`local.json` should exist from the beginning. **The other files are not checked in** to source control since it can contain sensitive data.

You will need to have [ScriptCs installed](http://scriptcs.net/).

Run the server using the `start.bat` file or in an elevated command prompt, using:

```
cmd:\\> scriptcs server.csx
```

The first time you need to install [the Nancy Script-pack](https://github.com/adamralph/scriptcs-nancy) and then delete `packages.config`:

```
cmd:\\> scriptcs -install ScriptCs-Nancy
```

All this has been covered in a blog-post: [Using ScriptCS and NancyFx to bootstrap my test environment](http://danielwertheim.se/2013/09/02/using-scriptcs-and-nancyfx-to-bootstrap-my-test-environment/).